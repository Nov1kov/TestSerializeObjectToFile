using System.IO;

namespace TestSerializeObjectToFile.CacheControllers
{
    class BinaryController : ICacheController
    {
        private readonly string _cacheFilePath;
        private readonly string _fileName;

        public BinaryController(string directory, string fileName)
        {
            _cacheFilePath = Path.Combine(directory, fileName);
            _fileName = fileName;
            DirectoryInfo di = Directory.CreateDirectory(directory);
        }

        public bool Save<T>(T model)
        {
            using (Stream stream = File.Open(_fileName, FileMode.Create))
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