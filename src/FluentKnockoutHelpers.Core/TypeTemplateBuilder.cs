using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FluentKnockoutHelpers.Core
{
    public class TypeTemplateBuilder : IHtmlString
    {
        private static readonly ConcurrentDictionary<Type, string> TypeTemplateJson = new ConcurrentDictionary<Type, string>();
        private static readonly ConcurrentDictionary<Type, Type[]> DerivedTypes = new ConcurrentDictionary<Type, Type[]>();

        private readonly List<string> _templateJsonToWrite = new List<string>();

        public TypeTemplateBuilder And<TType>(bool includeDerivedTypes = false)
        {
            var typesToAdd = new List<Type>();

            if (includeDerivedTypes)
            {
                var derivedTypes = DerivedTypes.GetOrAdd(typeof(TType), x =>
                                                                 AppDomain.CurrentDomain.GetAssemblies()
                                                                     .SelectMany(a => a.GetTypes())
                                                                     .Where(t => t.IsSubclassOf(x))
                                                                     .ToArray());
                typesToAdd.AddRange(derivedTypes);
            }

            //add json for each type determined to be needed. currently there must be a
            //default constructor on the type to do this
            _templateJsonToWrite.AddRange(
                typesToAdd
                    .Select(type => TypeTemplateJson.GetOrAdd(type, t =>
                    {
                        var defaultCtor = type.GetConstructor(Type.EmptyTypes);

                        if(defaultCtor == null)
                            return null; //can't create the type, excluded below

                        return GlobalSettings.JsonSerializer.ToJsonString(new
                            {
                                TypeName = t.FullName,
                                TemplateInstance = defaultCtor.Invoke(null)
                            });
                    }))
                    .Where(json => json != null) //no default CTOR, couldn't create
                );

            return this;
        }

        public string ToHtmlString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("{");
            sb.AppendLine("types: [");
            for (int i = 0; i < _templateJsonToWrite.Count; i++)
            {
                sb.Append(_templateJsonToWrite[i]);
                if (i != _templateJsonToWrite.Count - 1)
                    sb.AppendLine(",");
            }
            sb.AppendLine("]");
            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}
