using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using Microsoft.CSharp;

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
        public static void BuildForAllEndpointSubclassesOf<TControllerBaseType>()
        {
            BuildForAllEndpointSubclassesOf<TControllerBaseType>(null, Assembly.GetCallingAssembly());
        }

        public static void BuildForAllEndpointSubclassesOf<TControllerBaseType>(Action<TypeMetadataBuilder> additionalConfiguration)
        {
            BuildForAllEndpointSubclassesOf<TControllerBaseType>(additionalConfiguration, Assembly.GetCallingAssembly());
        }

        private static void BuildForAllEndpointSubclassesOf<TControllerBaseType>(Action<TypeMetadataBuilder> additionalConfiguration, Assembly endpointContainingAssembly)
        {
            ValidateSetup();
            var metadataModule = new TypeMetadataBuilder(typeof(TControllerBaseType), endpointContainingAssembly);

            if(additionalConfiguration != null)
                additionalConfiguration(metadataModule);
            BuildMetadata(metadataModule);
        }

        public static void BuildForConfiguration(Action<TypeMetadataBuilder> configuration)
        {
            if(configuration == null)
                throw new ArgumentNullException("configuration", "You must specify a configuration or use a different overload");

            ValidateSetup();
            var metadataModule = new TypeMetadataBuilder();
            configuration(metadataModule);
            BuildMetadata(metadataModule);
        }

        private static void ValidateSetup()
        {
            if (_metadataModule != null)
                throw new InvalidOperationException("TypeMetadata.Build must only be called once per AppDomain in Application_Start");
        }

        private static void BuildMetadata(TypeMetadataBuilder builder)
        {
            _metadataModule = GlobalSettings.JsonSerializer.ToJsonString(builder.Build());
        }

        public static IHtmlString EmitTypeMetadataArray()
        {
            return new HtmlString(_metadataModule);
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
                    .Where(t => t.IsSubclassOf(controllerBaseType) && !Attribute.IsDefined(t, typeof(ExcludeMetadata)))
                    //these probably are actions...
                    .SelectMany(
                        x => x.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                                 .Where(m => m.ReturnType != typeof (void) && !m.Name.StartsWith("get_"))
                    )
                    .Where(method => !Attribute.IsDefined(method, typeof(ExcludeMetadata)))
                    .Select(method => method.ReturnType)
                    .SelectMany(ProcessPossibleGenerics)
                    .Where(t => !t.IsPrimitive && !t.IsAbstract);

            foreach (var unprocessedType in endpointReturnTypes)
                _topLevelTypes.Add(unprocessedType);
        }

        private static IEnumerable<Type> ProcessPossibleGenerics(Type type)
        {
            if (!type.IsGenericType)
                return new [] {type};

            //List<T>, IEnumerable<T>, IQueryable<T> etc...
            if(typeof(IEnumerable).IsAssignableFrom(type))
                return type.GenericTypeArguments;

            //probably some user-created generic type (but may need to change!)
            return new[] {type}.Union(type.GetGenericArguments());
        }

        public TypeMetadataBuilder And<TType>()
        {
            _topLevelTypes.Add(typeof(TType));
            return this;
        }

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
                    //provides compatiblity with both JSON.net and ServiceStack serializers on the method in which they serialize type information..
                    Type = GetTypeName(finalType),
                    Instance = GetInstanceFromDefaultCtor(finalType),
                    FieldMetadata = GetFieldMetadata(finalType)
                });
        }

        private static string GetTypeName(Type type)
        {
            return GlobalSettings.JsonSerializer.SerializerRequiresAssembly
                       ? string.Format("{0}, {1}", type.FullName, type.Assembly.GetName().Name)
                       : type.FullName;
        }

        private static IEnumerable<FieldMetadata> GetFieldMetadata(Type type)
        {
            var typeRules = new List<FieldMetadata>();
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => !Attribute.IsDefined(p, typeof(ExcludeMetadata)));

            foreach (var pi in props)
            {
                var metadata = new FieldMetadata();

                //data annotation attributes
                WriteRuleIfHasAttribute<RequiredAttribute>(pi, metadata, attr => new RequiredValidationRule(attr));
                WriteRuleIfHasAttribute<RangeAttribute>(pi, metadata, attr => new RangeValidationRule(attr));
                WriteRuleIfHasAttribute<MinLengthAttribute>(pi, metadata, attr => new MinLengthValidationRule(attr));
                WriteRuleIfHasAttribute<MaxLengthAttribute>(pi, metadata, attr => new MaxLengthValidationRule(attr));
                WriteRuleIfHasAttribute<RegularExpressionAttribute>(pi, metadata, attr => new RegexValidationRule(attr));
                WriteRuleIfHasAttribute<EmailAddressAttribute>(pi, metadata, attr => new EmailAddressValidationRule(attr));
                WriteRuleIfHasAttribute<CompareAttribute>(pi, metadata, attr => new CompareValidationRule(attr));
                WriteRuleIfHasAttribute<PhoneAttribute>(pi, metadata, attr => new PhoneValidationRule(attr));
                WriteRuleIfHasAttribute<UrlAttribute>(pi, metadata, attr => new UrlValidationRule(attr));
                WriteRuleIfHasAttribute<StringLengthAttribute>(pi, metadata, attr => new MaxLengthValidationRule(attr));
                WriteRuleIfHasAttribute<StringLengthAttribute>(pi, metadata, attr => new MinLengthValidationRule(attr));

                metadata.Name = pi.Name;
                metadata.Type = GetSimpleTypeName(pi.PropertyType);

                typeRules.Add(metadata);
            }

            return typeRules;
        }


        public static string GetSimpleTypeName(Type type)
        {
            //ints
            if (type == typeof(short))
                return "short";
            if (type == typeof(short?))
                return "short?";
            if (type == typeof(int))
                return "int";
            if (type == typeof(int?))
                return "int?";
            if (type == typeof(long))
                return "long";
            if (type == typeof(long?))
                return "long?";


            //floats
            if (type == typeof(float))
                return "float";
            if (type == typeof(float?))
                return "float?";
            if (type == typeof(double))
                return "double";
            if (type == typeof(double?))
                return "double?";
            if (type == typeof(decimal))
                return "decimal";
            if (type == typeof(decimal?))
                return "decimal?";


            //dates
            if (type == typeof(DateTime))
                return "DateTime";
            if (type == typeof(DateTime?))
                return "DateTime?";


            //string
            if (type == typeof(string))
                return "string";


            //bools
            if (type == typeof(bool))
                return "bool";
            if (type == typeof(bool?))
                return "bool?";


            //arrays
            if (typeof(IEnumerable).IsAssignableFrom(type))
                return GetSimpleIEnumerableTypeName(type);

            
            //other
            return GetTypeName(type);
        }

        private static string GetSimpleIEnumerableTypeName(Type type)
        {
            if (type.IsArray && type.HasElementType)
                return GetSimpleTypeName(type.GetElementType()) + "[]";

            if (type.GenericTypeArguments.Length == 1)
                return GetTypeName(type.GenericTypeArguments[0]) + "[]";

            return GetTypeName(type);
        }

        //write a validation rule for a data annotations attributes if it exists
        private static void WriteRuleIfHasAttribute<TAttribute>(PropertyInfo pi, FieldMetadata fieldMetadata, Func<TAttribute, ValidationRule> factory)
            where TAttribute : ValidationAttribute
        {
            var attr = pi.GetCustomAttribute<TAttribute>();

            if (attr != null)
                fieldMetadata.Rules.Add(factory(attr));
        }

        private static void WriteRuleIfIsType<TType>(PropertyInfo pi, FieldMetadata fieldMetadata, Func<ValidationRule> factory)
        {
            if (pi.PropertyType == typeof(TType))
                fieldMetadata.Rules.Add(factory());
        }

        //get an instance of a type if it has a parameterless constructor
        private static object GetInstanceFromDefaultCtor(Type t)
        {
            var defaultCtor = t.GetConstructor(Type.EmptyTypes);
            return defaultCtor == null ? null : defaultCtor.Invoke(null);
        }

        private void AddMemberTypes(Type subject, HashSet<Type> finalTypes)
        {
            finalTypes.Add(subject);

            //look for nested properties that are complex types (not part of .NET see below) that need
            //metadata information sent to the client for validation and templating
            var propsTypesToRecurse = subject.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead)
                .Select(p => p.PropertyType)
                .SelectMany(ProcessPossibleGenerics)
                //all subclasses of T to account for object hierarchies in the client
                .SelectMany(t => _allAppDomainTypes.Where(x => x.IsSubclassOf(t)).Union(new []{ t }))
                .Where(t => !Attribute.IsDefined(t, typeof (ExcludeMetadata)))
                .Where(t => !finalTypes.Contains(t))
                .Where(t => !t.IsPrimitive && !t.IsAbstract)
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
