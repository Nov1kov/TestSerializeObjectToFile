using System.Collections.Generic;
using System.Linq;
using TestSerializeObjectToFile.CacheControllers;
using TestSerializeObjectToFile.Models;
using Xunit;

namespace TestSerializeObjectToFile
{
    public class SubclassModelTest
    {
        private ModelWithSubclass _model;
        private const string CacheFileDirectory = "CacheFiles";
        private const string ProtobufFile = "protobuf_subclass.bin";
        private const string BinaryFile = "binary_subclass.data";
        private const string JsonFile = "json_subclass.json";
        
        public SubclassModelTest()
        {
            _model = new ModelWithSubclass("inner string")
            {
                IntId1 = 1,
                IntId2 = 2,
                StrField1 = "Str1",
                StrField2 = "Str2",
            };
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
            var model = cacheController.Load<ModelWithSubclass>();
            ValidateModel(model);
        }

        private void ValidateModel(ModelWithSubclass model)
        {
            Assert.Equal("inner string", model.InnerField.InnerStr);
            Assert.Equal(1, model.IntId1);
            Assert.Equal(2, model.IntId2);
            Assert.Equal("Str1", model.StrField1);
            Assert.Equal("Str2", model.StrField2);
        }
    }
}