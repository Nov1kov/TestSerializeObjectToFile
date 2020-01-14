using System.IO;

namespace TestSerializeObjectToFile.CacheControllers
{
    class BinaryController : ICacheController
    {
        private readonly string _cacheFilePath;

        public BinaryController(string directory, string fileName)
        {
            _cacheFilePath = Path.Combine(directory, fileName);
            DirectoryInfo di = Directory.CreateDirectory(directory);
        }

        public bool Save<T>(T model)
        {
            using (Stream stream = File.Open(_cacheFilePath, FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, model);
                return true;
            }
        }

        public T Load<T>()
        {
            using (Stream stream = File.Open(_cacheFilePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (T) binaryFormatter.Deserialize(stream);
            }
        }
    }
}