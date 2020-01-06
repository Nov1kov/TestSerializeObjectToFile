using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using TestSerializeObjectToFile.CacheControllers;
using TestSerializeObjectToFile.Models;
using Xunit;

namespace TestSerializeObjectToFile
{
    public class CacheTests
    {
        private ModelProto _model;
        private const string CacheFileDirectory = "CacheFiles";
        private const string ProtobufFile = "protobuf.bin";

        public CacheTests()
        {
            _model = new ModelProto
            {
                Id = 111,
                Field = "some string",
                List = { new Item(1), new Item(2) },
                //MissingField = "missing str field",
                //MissingList = { "item 1", "item 2" },
            };
            _model.Recursive = new ClassWithParentLink(_model, 14);
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
            var model = protobuf.Load<ModelProto>(ProtobufFile);
            ValidateModel(model);
        }

        private void ValidateModel(ModelProto model)
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