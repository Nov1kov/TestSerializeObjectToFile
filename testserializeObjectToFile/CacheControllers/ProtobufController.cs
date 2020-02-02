using System.IO;
using ProtoBuf.Meta;

namespace TestSerializeObjectToFile.CacheControllers
{
    // https://www.codeproject.com/Articles/642677/Protobuf-net-the-unofficial-manual#forms
    class ProtobufController : ICacheController
    {
        private readonly string _cacheFilePath;
        private readonly string _fileName;

        public ProtobufController(string directory, string fileName)
        {
            _cacheFilePath = Path.Combine(directory, fileName);
            _fileName = fileName;
            DirectoryInfo di = Directory.CreateDirectory(directory);
        }

        public bool Save<T>(T model)
        {
            var serializeModel = TypeModel.Create();
            serializeModel.Add(typeof(Models.BaseModel), true)
                .AddSubType(100, typeof(Models.V1.Model));

            using (var file = File.Create(_fileName)) 
            {
                serializeModel.Serialize(file, model);
                return true;
            }
        }

        public T Load<T>()
        {
            var serializeModel = TypeModel.Create();
            serializeModel.Add(typeof(Models.BaseModel), true)
                .AddSubType(100, typeof(Models.V2.ModelV2));

            using (var file = File.OpenRead(_cacheFilePath)) {
                return Deserialize<T>(serializeModel, file);
            }        
        }
        
        public static T Deserialize<T>(RuntimeTypeModel model, Stream source)
        {
            return (T) model.Deserialize(source, (object) null, typeof (T));
        }

    }
    
    
}