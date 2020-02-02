using System;
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
                binaryFormatter.Binder = new ModelV1ToModelV2Binder();
                return (T) binaryFormatter.Deserialize(stream);
            }
        }
        
        // http://www.diranieh.com/NETSerialization/BinarySerialization.htm
        // https://stackoverflow.com/questions/3545544/how-to-refactor-a-class-that-is-serialized-in-net
        public class ModelV1ToModelV2Binder : System.Runtime.Serialization.SerializationBinder
        {
            public override Type BindToType(string assemblyName, string typeName)
            {
                const string strAssemblyXType = "TestSerializeObjectToFile.Models.V1.Model";
                const string strAssemblyYType = "TestSerializeObjectToFile.Models.V2.ModelV2";

                // Return different type if the assembly name passed in as a parameter is AssemblyX
                Type obTypeToDeserialize = null;
                if (typeName == strAssemblyXType)
                    obTypeToDeserialize = Type.GetType( strAssemblyYType );

                return obTypeToDeserialize;
            }
        }
    }
}