using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
            DirectoryInfo di = Directory.CreateDirectory(directory);
            
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
                var props = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                    .Select(f => CreateProperty(f, memberSerialization))
                    .ToList();
                props.ForEach(p => { p.Writable = true; p.Readable = true; });
                return props;
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