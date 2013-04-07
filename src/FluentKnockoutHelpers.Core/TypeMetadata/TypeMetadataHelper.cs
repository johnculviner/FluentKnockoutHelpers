using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FluentKnockoutHelpers.Core.TypeMetadata
{
    public static class TypeMetadataHelper
    {
        private static string _metadataModule;

        /// <summary>
        /// <para>Automatically builds Metadata for all complex types returned by controller actions of TControllerBaseType in the calling assembly</para>
        /// <para>Call in Application_Start</para>
        /// </summary>
        /// <typeparam name="TControllerBaseType">The base type of controllers to scan</typeparam>
        /// <returns>A builder to add or remove additional types</returns>
        public static void BuildFor<TControllerBaseType>()
        {
            BuildFor<TControllerBaseType>(null, Assembly.GetCallingAssembly());
        }

        public static void BuildFor<TControllerBaseType>(Action<TypeMetadataBuilder> additionalConfiguration)
        {
            BuildFor<TControllerBaseType>(additionalConfiguration, Assembly.GetCallingAssembly());
        }

        private static void BuildFor<TControllerBaseType>(Action<TypeMetadataBuilder> additionalConfiguration, Assembly endpointContainingAssembly)
        {
            ValidateSetup();
            var metadataModule = new TypeMetadataBuilder(typeof(TControllerBaseType), endpointContainingAssembly);

            if(additionalConfiguration != null)
                additionalConfiguration(metadataModule);
            BuildMetadataModule(metadataModule);
        }

        public static void BuildFor(Action<TypeMetadataBuilder> configuration)
        {
            if(configuration == null)
                throw new ArgumentNullException("configuration", "You must specify a configuration or use a different overload");

            ValidateSetup();
            var metadataModule = new TypeMetadataBuilder();
            configuration(metadataModule);
            BuildMetadataModule(metadataModule);
        }

        private static void ValidateSetup()
        {
            if (_metadataModule != null)
                throw new InvalidOperationException("TypeMetadata.Build must only be called once per AppDomain in Application_Start");
        }

        private static void BuildMetadataModule(TypeMetadataBuilder builder)
        {
            var sb = new StringBuilder();
            sb.AppendLine(GlobalSettings.JsonSerializer.ToJsonString(builder.Build()));
            _metadataModule = sb.ToString();
        }

        public static IHtmlString EmitTypeMetadataModule()
        {
            return new MvcHtmlString(_metadataModule);
        }
    }

    public class TypeMetadataBuilder
    {
        private readonly List<Type> _allAppDomainTypes = 
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .ToList();

        private readonly HashSet<Type> _topLevelTypes = new HashSet<Type>();
        private readonly HashSet<Type> _notTypes = new HashSet<Type>();

        internal TypeMetadataBuilder()
        {
        }

        internal TypeMetadataBuilder(Type controllerBaseType, Assembly endpointContainingAssembly)
        {
            var endpointReturnTypes =
                endpointContainingAssembly
                    .GetTypes()
                    .Where(t => t.IsSubclassOf(controllerBaseType))
                    //these probably are actions...
                    .SelectMany(
                        x => x.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                                 .Where(m => m.ReturnType != typeof (void) && !m.Name.StartsWith("get_"))
                    )
                    .Select(x => x.ReturnType)
                    .Where(t => !t.IsPrimitive)
                    .SelectMany(ProcessPossibleGenerics);

            foreach (var unprocessedType in endpointReturnTypes)
                _topLevelTypes.Add(unprocessedType);
        }

        private static IEnumerable<Type> ProcessPossibleGenerics(Type type)
        {
            if (!type.IsGenericType)
                return new [] {type};

            //List<T>, IEnumerable<T>, IQueryable<T> etc...
            if(typeof(IEnumerable).IsAssignableFrom(type))
                return type.GetGenericArguments();

            //probably some user-created generic type (but may need to change!)
            return new[] {type}.Union(type.GetGenericArguments());
        }

        public TypeMetadataBuilder And<TType>()
        {
            _topLevelTypes.Add(typeof(TType));
            return this;
        }

        //private void And(Type type, bool includeSubclasses)
        //{
        //    if (includeSubclasses)
        //    {
        //        var subClasses = _allAppDomainTypes.Where(t => t.IsSubclassOf(type));

        //        foreach (var subClass in subClasses)
        //             _topLevelTypes.Add(subClass);
        //    }

        //    _topLevelTypes.Add(type);
        //}

        public TypeMetadataBuilder Not<TType>()
        {
            _notTypes.Add(typeof(TType));
            return this;
        }

        internal IEnumerable<TypeMetadata> Build()
        {
            var finalTypes = new HashSet<Type>();

            //recursively add types that need metadata
            foreach (var type in _topLevelTypes)
                AddMemberTypes(type, finalTypes);

            //remove types explicitly disallowed by caller
            foreach (var not in _notTypes)
                finalTypes.Remove(not);

            return 
                finalTypes.Select(finalType => new TypeMetadata
                {
                    TypeName = finalType.FullName,
                    Instance = GetInstanceFromDefaultCtor(finalType),
                    FieldValidationRules = GetValidationRules(finalType)
                });
        }

        private static IEnumerable<FieldValidationRules> GetValidationRules(Type type)
        {
            var typeRules = new List<FieldValidationRules>();
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var pi in props.Where(p => p.PropertyType.IsPrimitive))
            {
                var fieldRules = new FieldValidationRules();
                WriteRuleIfHasAttribute<CreditCardAttribute>(pi, fieldRules, attr => new CreditCardValidationRule(attr));
                WriteRuleIfHasAttribute<EmailAddressAttribute>(pi, fieldRules, attr => new EmailAddressValidationRule(attr));
                WriteRuleIfHasAttribute<MaxLengthAttribute>(pi, fieldRules, attr => new MaxLengthValidationRule(attr));
                WriteRuleIfHasAttribute<MinLengthAttribute>(pi, fieldRules, attr => new MinLengthValidationRule(attr));
                WriteRuleIfHasAttribute<PhoneAttribute>(pi, fieldRules, attr => new PhoneValidationRule(attr));
                WriteRuleIfHasAttribute<RangeAttribute>(pi, fieldRules, attr => new RangeValidationRule(attr));
                WriteRuleIfHasAttribute<RegularExpressionAttribute>(pi, fieldRules, attr => new RegexValidationRule(attr));
                WriteRuleIfHasAttribute<RequiredAttribute>(pi, fieldRules, attr => new RequiredValidationRule(attr));
                WriteRuleIfHasAttribute<UrlAttribute>(pi, fieldRules, attr => new UrlValidationRule(attr));
                WriteRuleIfHasAttribute<System.ComponentModel.DataAnnotations.CompareAttribute>(pi, fieldRules, attr => new CompareValidationRule(attr));
                WriteRuleIfHasAttribute<StringLengthAttribute>(pi, fieldRules, attr => new MaxLengthValidationRule(attr));
                WriteRuleIfHasAttribute<StringLengthAttribute>(pi, fieldRules, attr => new MinLengthValidationRule(attr));

                WriteRuleIfIsType<short>(pi, fieldRules, () => new ShortValidationRule(false));
                WriteRuleIfIsType<short?>(pi, fieldRules, () => new ShortValidationRule(true));
                WriteRuleIfIsType<int>(pi, fieldRules, () => new IntValidationRule(false));
                WriteRuleIfIsType<int?>(pi, fieldRules, () => new IntValidationRule(true));
                WriteRuleIfIsType<long>(pi, fieldRules, () => new LongValidationRule(false));
                WriteRuleIfIsType<long?>(pi, fieldRules, () => new LongValidationRule(true));
                WriteRuleIfIsType<decimal>(pi, fieldRules, () => new DecimalValidationRule(false));
                WriteRuleIfIsType<decimal?>(pi, fieldRules, () => new DecimalValidationRule(true));
                WriteRuleIfIsType<DateTime>(pi, fieldRules, () => new DateTimeValidationRule(false));
                WriteRuleIfIsType<DateTime?>(pi, fieldRules, () => new DateTimeValidationRule(true));

                if (fieldRules.Rules.Count == 0) 
                    continue;
                
                fieldRules.FieldName = pi.Name;
                typeRules.Add(fieldRules);
            }

            return typeRules;
        }

        //write a validation rule for a data annotations attributes if it exists
        private static void WriteRuleIfHasAttribute<TAttribute>(PropertyInfo pi, FieldValidationRules fieldValidationRules, Func<TAttribute, ValidationRule> factory)
            where TAttribute : Attribute
        {
            var attr = pi.GetCustomAttribute<TAttribute>();

            if (attr != null)
                fieldValidationRules.Rules.Add(factory(attr));
        }

        private static void WriteRuleIfIsType<TType>(PropertyInfo pi, FieldValidationRules fieldValidationRules, Func<ValidationRule> factory)
        {
            if (pi.PropertyType == typeof(TType))
                fieldValidationRules.Rules.Add(factory());
        }

        //get an instance of a type if it has a parameterless constructor
        private static object GetInstanceFromDefaultCtor(Type t)
        {
            var defaultCtor = t.GetConstructor(Type.EmptyTypes);
            return defaultCtor == null ? null : defaultCtor.Invoke(null);
        }

        private static void AddMemberTypes(Type subject, HashSet<Type> finalTypes)
        {
            finalTypes.Add(subject);

            //look for nested properties that are complex types (not part of .NET see below) that need
            //metadata information sent to the client for validation and templating
            var propsTypesToRecurse = subject.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead)
                .Select(p => p.PropertyType)
                .Where(t => !t.IsPrimitive)
                .SelectMany(ProcessPossibleGenerics)
                .Where(t => !finalTypes.Contains(t))
                //really this should being used on user DTO types
                //cant think of a reason to be sending validation or creating template instances
                //of .NET framework types (other than primitives) on the client.
                //May change if someone comes up with a reason
                //but this is a bit of a safety precaution to possible craziness...
                .Where(t => !t.Assembly.FullName.Contains("PublicKeyToken=b77a5c561934e089"));

            foreach (var type in propsTypesToRecurse)
                AddMemberTypes(type, finalTypes);
        }
    }
}
