using System.Collections.Generic;
using TestSerializeObjectToFile.CacheControllers;
using Xunit;

namespace TestSerializeObjectToFile
{
    public class UpdateRenamedModelTest
    {
        private const string CacheFileDirectory = "CacheFiles";
        private const string ProtobufFile = "protobuf_renamed_model.bin";
        private const string BinaryFile = "binary_renamed_model.data";
        private const string JsonFile = "json_renamed_model.json";

        public static IEnumerable<object[]> GetCacheController()
        {
            yield return new object[] { new JsonController(CacheFileDirectory, JsonFile) };
            yield return new object[] { new BinaryController(CacheFileDirectory, BinaryFile) };
            yield return new object[] { new ProtobufController(CacheFileDirectory, ProtobufFile) };
        }
        
        private Models.V1.Model CreateModel()
        {
            return new Models.V1.Model
            {
                Id = 14,
                Field1 = "string",
                Integers = { 1, 2, 3 }
            };
        }
        
        [Theory]
        [MemberData(nameof(GetCacheController))]
        public void Serialize_Save_Model(ICacheController cacheController)
        {
            var model = CreateModel();
            cacheController.Save(model);
        }

        [Theory]
        [MemberData(nameof(GetCacheController))]
        public void Deserialize_Load_Model(ICacheController cacheController)
        {
            var model = cacheController.Load<Models.V2.ModelV2>();
            ValidateModel(model);
        }

        private void ValidateModel(Models.V2.ModelV2 model)
        {
            Assert.Equal("string", model.Field1);
            Assert.Equal(new []{1, 2, 3}, model.Integers);
            Assert.Equal(14, model.Id);
        }
    }
}