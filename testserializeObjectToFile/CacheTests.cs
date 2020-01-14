using System.Collections.Generic;
using TestSerializeObjectToFile.CacheControllers;
using TestSerializeObjectToFile.Models;
using Xunit;

namespace TestSerializeObjectToFile
{
    public class CacheTests
    {
        private Model _model;
        private const string CacheFileDirectory = "CacheFiles";
        private const string ProtobufFile = "protobuf.bin";
        private const string BinaryFile = "binaryformatter.bin";
        private const string JsonFile = "model.json";
        
        private List<ICacheController> _cacheControllers = new List<ICacheController>();

        public CacheTests()
        {
            _model = new Model
            {
                Id = 111,
                Field = "some string",
                List = { new Item(1), new Item(2) },
                //MissingField = "missing str field",
                //MissingList = { "item 1", "item 2" },
            };
            _model.Recursive = new ClassWithLink(_model, 14);
            _cacheControllers.Add(new JsonController(CacheFileDirectory, JsonFile));
            _cacheControllers.Add(new BinaryController(CacheFileDirectory, BinaryFile));
            _cacheControllers.Add(new ProtobufController(CacheFileDirectory, ProtobufFile));
        }
        
        [Fact]
        public void Serialize_Save_Model()
        {
            foreach (var cacheController in _cacheControllers)
            {
                cacheController.Save(_model);
            }
        }
                
        [Fact]
        public void Deserialize_Load_Model()
        {
            foreach (var cacheController in _cacheControllers)
            {
                var model = cacheController.Load<Model>();
                ValidateModel(model);
            }
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