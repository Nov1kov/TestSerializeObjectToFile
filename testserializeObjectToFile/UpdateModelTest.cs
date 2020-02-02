using System.Collections.Generic;
using TestSerializeObjectToFile.CacheControllers;
using TestSerializeObjectToFile.Models;
using Xunit;

namespace TestSerializeObjectToFile
{
    public class UpdateModelTest
    {
        private const string CacheFileDirectory = "CacheFiles";
        private const string ProtobufFile = "protobuf.bin";
        private const string BinaryFile = "binary.data";
        private const string JsonFile = "json.json";

        public static IEnumerable<object[]> GetCacheController()
        {
            yield return new object[] { new JsonController(CacheFileDirectory, JsonFile) };
            yield return new object[] { new BinaryController(CacheFileDirectory, BinaryFile) };
            yield return new object[] { new ProtobufController(CacheFileDirectory, ProtobufFile) };
        }
        
        private Model CreateModel()
        {
            var model = new Model
            {
                Id = 111,
                Field = "some string",
                List = { new Item(1), new Item(2) },
                //MissingField = "missing str field",
                //MissingList = { "item 1", "item 2" },
            };
            model.Recursive = new ClassWithLink(model, 14);
            return model;
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
            var model = cacheController.Load<Model>();
            ValidateModel(model);
        }

        private void ValidateModel(Model model)
        {
            Assert.Equal(111, model.Id);
            Assert.Equal("some string", model.Field);
            Assert.True(string.IsNullOrEmpty(model.NewField));
            Assert.NotNull(model.NewList);
            Assert.Empty(model.NewList);
            var exceptedList = new List<Item>
            {
                new Item(1), 
                new Item(2)
            };
            Assert.Equal(exceptedList, model.List);
            Assert.Equal(model, model.Recursive.Model);
            Assert.Equal(14, model.Recursive.Id);
        }
    }
}