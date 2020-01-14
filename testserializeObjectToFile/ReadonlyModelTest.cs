using System.Collections.Generic;
using System.IO;
using TestSerializeObjectToFile.CacheControllers;
using TestSerializeObjectToFile.Models;
using Xunit;

namespace TestSerializeObjectToFile
{
    public class ReadonlyModelTest
    {
        private ModelWithReadOnly _model;
        private const string CacheFileDirectory = "CacheFiles";
        private const string ProtobufFile = "protobuf_read_only.bin";
        private const string BinaryFile = "binary_read_only.data";
        private const string JsonFile = "newtonsoft_read_only.json";
        
        public ReadonlyModelTest()
        {
            _model = new ModelWithReadOnly(96, "field2 string");
            _model.ChangeField("field1 string");
            _model.Dictionary.Add(14, "14");
            _model.Dictionary.Add(11, "11");
        }
        
        public static IEnumerable<object[]> GetCacheController()
        {
            yield return new object[] { new JsonController(CacheFileDirectory, JsonFile) };
            yield return new object[] { new BinaryController(CacheFileDirectory, BinaryFile) };
            yield return new object[] { new ProtobufController(CacheFileDirectory, ProtobufFile) };
        }
        
        [Theory]
        [MemberData(nameof(GetCacheController))]
        public void Serialize_Save_Model(ICacheController cacheController)
        {
            cacheController.Save(_model);
        }

        [Theory]
        [MemberData(nameof(GetCacheController))]
        public void Deserialize_Load_Model(ICacheController cacheController)
        {
            var model = cacheController.Load<ModelWithReadOnly>();
            ValidateModel(model);
        }

        private void ValidateModel(ModelWithReadOnly model)
        {
            Assert.Equal(96, model.GetId());
            Assert.Equal("field1 string", model.Field1);
            Assert.Equal("field2 string", model.Field2);
            Assert.Equal("field1 stringfield2 string", model.Field3);
            
            Assert.Equal("14", model.Dictionary[14]);
            Assert.Equal("11", model.Dictionary[11]);
        }
    }
}