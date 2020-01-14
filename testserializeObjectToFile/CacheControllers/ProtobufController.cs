using System.IO;
using ProtoBuf;

namespace TestSerializeObjectToFile.CacheControllers
{
    class ProtobufController : ICacheController
    {
        private readonly string _cacheFilePath;

        public ProtobufController(string directory, string fileName)
        {
            _cacheFilePath = Path.Combine(directory, fileName);
            DirectoryInfo di = Directory.CreateDirectory(directory);
        }

        public bool Save<T>(T model)
        {
            using (var file = File.Create(_cacheFilePath)) {
                Serializer.Serialize(file, model);
                return true;
            }
        }

        public T Load<T>()
        {
            using (var file = File.OpenRead(_cacheFilePath)) {
                return Serializer.Deserialize<T>(file);
            }        }
    }
}