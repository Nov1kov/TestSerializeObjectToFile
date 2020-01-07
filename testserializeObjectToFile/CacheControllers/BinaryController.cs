using System.IO;

namespace TestSerializeObjectToFile.CacheControllers
{
    class BinaryController : ICacheController
    {
        private string _directory;

        public BinaryController(string directory)
        {
            _directory = directory;
        }

        public bool Save<T>(T model, string fileName)
        {
            var cacheFilePath = Path.Combine(_directory, fileName);

            using (Stream stream = File.Open(cacheFilePath, FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, model);
                return true;
            }
        }

        public T Load<T>(string fileName)
        {
            var cacheFilePath = Path.Combine(_directory, fileName);
            using (Stream stream = File.Open(cacheFilePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (T) binaryFormatter.Deserialize(stream);
            }
        }
    }
}