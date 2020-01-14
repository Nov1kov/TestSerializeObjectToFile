using System.IO;
using Newtonsoft.Json;

namespace TestSerializeObjectToFile.CacheControllers
{
    class JsonController : ICacheController
    {
        private readonly string _cacheFilePath;

        public JsonController(string directory, string fileName)
        {
            _cacheFilePath = Path.Combine(directory, fileName);
            DirectoryInfo di = Directory.CreateDirectory(directory);
        }

        public bool Save<T>(T model)
        {
            JsonSerializer serializer = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };

            using (StreamWriter sw = new StreamWriter(_cacheFilePath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, model);
                return true;
            }
        }

        public T Load<T>()
        {
            using (StreamReader file = File.OpenText(_cacheFilePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                return (T) serializer.Deserialize(file, typeof(T));
            }
        }
    }
}