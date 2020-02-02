using System.Collections.Generic;
using System.Linq;
using TestSerializeObjectToFile.CacheControllers;
using TestSerializeObjectToFile.Models;
using Xunit;

namespace TestSerializeObjectToFile
{
    public class AbstractListTest
    {
        private const string CacheFileDirectory = "CacheFiles";
        private const string ProtobufFile = "protobuf_abstract_list.bin";
        private const string BinaryFile = "binary_abstract_list.data";
        private const string JsonFile = "json_abstract_list.json";

        public static IEnumerable<object[]> GetCacheController()
        {
            yield return new object[] { new JsonController(CacheFileDirectory, JsonFile) };
            yield return new object[] { new BinaryController(CacheFileDirectory, BinaryFile) };
            yield return new object[] { new ProtobufController(CacheFileDirectory, ProtobufFile) };
        }
        
        private AbstractList CreateModel()
        {
            var model = new AbstractList();
            
            for (int i = 0; i < 10; i++)
            {
                model.Items.Add(new ItemImplementation
                {
                    Id = i,
                });
            }

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
            var model = cacheController.Load<AbstractList>();
            ValidateModel(model);
        }

        private void ValidateModel(AbstractList model)
        {
            foreach (var item in model.Items.Select((value, i) => ( value, i )))
            {
                var value = item.value;
                var index = item.i;
                
                Assert.Equal(index, value.Id);
            }
        }
    }
}