using System.IO;
using ProtoBuf;

namespace TestSerializeObjectToFile.CacheControllers
{
    class ProtobufController : ICacheController
    {
        private readonly string _directory;

        public ProtobufController(string directory)
        {
            _directory = directory;
            DirectoryInfo di = Directory.CreateDirectory(directory);
        }
        
        public bool Save<T>(T model, string fileName)
        {
            var cacheFilePath = Path.Combine(_directory, fileName);
            using (var file = File.Create(cacheFilePath)) {
                Serializer.Serialize(file, model);
                return true;
            }
        }

        public T Load<T>(string fileName)
        {
            var cacheFilePath = Path.Combine(_directory, fileName);
            using (var file = File.OpenRead(cacheFilePath)) {
                return Serializer.Deserialize<T>(file);
            }        }
    }
}