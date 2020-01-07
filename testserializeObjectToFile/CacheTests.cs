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
        }
        
        [Fact]
        public void Json_Save()
        {
            var jsonController = new JsonController(CacheFileDirectory);
            jsonController.Save(_model, JsonFile);
        }
                
        [Fact]
        public void Json_Load()
        {
            var jsonController = new JsonController(CacheFileDirectory);
            var model = jsonController.Load<Model>(JsonFile);
            ValidateModel(model);
        }  
        
        [Fact]
        public void Binary_Save()
        {
            var jsonController = new BinaryController(CacheFileDirectory);
            jsonController.Save(_model, BinaryFile);
        }
                
        [Fact]
        public void Binary_Load()
        {
            var jsonController = new BinaryController(CacheFileDirectory);
            var model = jsonController.Load<Model>(BinaryFile);
            ValidateModel(model);
        }
        
        [Fact]
        public void Proto_Save()
        {
            var protobuf = new ProtobufController(CacheFileDirectory);
            protobuf.Save(_model, ProtobufFile);
        }
        
        [Fact]
        public void Proto_Load()
        {
            var protobuf = new ProtobufController(CacheFileDirectory);
            var model = protobuf.Load<Model>(ProtobufFile);
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