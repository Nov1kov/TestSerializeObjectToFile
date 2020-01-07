using System.IO;
using Newtonsoft.Json;

namespace TestSerializeObjectToFile.CacheControllers
{
    class JsonController : ICacheController
    {
        private string _directory;

        public JsonController(string directory)
        {
            _directory = directory;
        }

        public bool Save<T>(T model, string fileName)
        {
            var cacheFilePath = Path.Combine(_directory, fileName);

            JsonSerializer serializer = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };

            using (StreamWriter sw = new StreamWriter(cacheFilePath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, model);
                return true;
            }
        }

        public T Load<T>(string fileName)
        {
            var cacheFilePath = Path.Combine(_directory, fileName);
            using (StreamReader file = File.OpenText(cacheFilePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                return (T) serializer.Deserialize(file, typeof(T));
            }
        }
    }
}