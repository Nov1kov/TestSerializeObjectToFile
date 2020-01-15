using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TestSerializeObjectToFile.CacheControllers
{
    class JsonController : ICacheController
    {
        private readonly string _cacheFilePath;
        private JsonSerializer _serializer;
        private readonly string _fileName;

        public JsonController(string directory, string fileName)
        {
            _cacheFilePath = Path.Combine(directory, fileName);
            _fileName = fileName;
            var di = Directory.CreateDirectory(directory);
            
            _serializer = new JsonSerializer
            {
                TypeNameHandling = TypeNameHandling.Auto,
                ContractResolver = new PrivateFieldsContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };
        }
        
        public class PrivateFieldsContractResolver : DefaultContractResolver
        {
            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                var props = GetFields(type, BindingFlags.NonPublic | BindingFlags.Instance)
                    .Select(f => CreateProperty(f, memberSerialization))
                    .ToList();
                props.ForEach(p => { 
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
                GetChildPrivateFields(source, targetType, bindingAttr);
                return source.Cast<FieldInfo>();
            }
            
            private static void GetChildPrivateFields(
                IList<MemberInfo> initialFields,
                Type targetType,
                BindingFlags bindingAttr)
            {
                if ((bindingAttr & BindingFlags.NonPublic) == BindingFlags.Default)
                    return;
                BindingFlags bindingAttr1 = bindingAttr;
                while ((targetType = targetType.BaseType) != null)
                {
                    var fieldInfos = targetType.GetFields(bindingAttr1).Where(f => f.IsPrivate);
                    foreach (var fieldInfo in fieldInfos)
                    {
                        initialFields.Add(fieldInfo);
                    }
                }
            }
        }

        public bool Save<T>(T model)
        {
            using (StreamWriter sw = new StreamWriter(_fileName))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                _serializer.Serialize(writer, model);
                return true;
            }
        }

        public T Load<T>()
        {
            using (StreamReader file = File.OpenText(_cacheFilePath))
            {
                return (T) _serializer.Deserialize(file, typeof(T));
            }
        }
    }
}