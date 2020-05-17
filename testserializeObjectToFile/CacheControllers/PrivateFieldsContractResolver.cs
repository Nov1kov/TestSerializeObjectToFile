using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TestSerializeObjectToFile.CacheControllers
{
    public class PrivateFieldsContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var props = GetFields(type, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Select(f => CreateProperty(f, memberSerialization))
                .ToList();
            props.ForEach(p =>
            {
                p.Writable = true;
                p.Readable = true;
                p.PropertyName = FixPropertyName(p.PropertyName);
            });
            return props;
        }
    
        private static string FixPropertyName(string name)
        {
            if (name.Contains("<"))
            {
                return name.Replace("<", "").Replace(">k__BackingField", "");
            }
            return name;
        }
                
        private static IEnumerable<FieldInfo> GetFields(
            Type targetType,
            BindingFlags bindingAttr)
        {
            List<MemberInfo> source = new List<MemberInfo>(targetType.GetFields(bindingAttr));
            GetChildFields(source, targetType, bindingAttr);
            return source.Cast<FieldInfo>();
        }
    
        private static void GetChildFields(
            IList<MemberInfo> initialFields,
            Type targetType,
            BindingFlags bindingAttr)
        {
            while ((targetType = targetType.BaseType) != null)
            {
                var fieldInfos = targetType.GetFields(bindingAttr);
                foreach (var fieldInfo in fieldInfos)
                {
                    initialFields.Add(fieldInfo);
                }
            }
        }
    }
}