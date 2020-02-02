using System;
using System.Collections.Generic;
using ProtoBuf;

namespace TestSerializeObjectToFile.Models
{
    [ProtoContract(SkipConstructor=true, ImplicitFields = ImplicitFields.AllFields)]
    [Serializable]
    public class BaseModel
    {
        public int Id { get; set; }
    }
    
    
    namespace V1
    {
        // model 1 version
        [ProtoContract(SkipConstructor=true)]
        [Serializable]
        public class Model : BaseModel
        {
            [ProtoMember(1)]
            public string Field1 { get; set; }
            [ProtoMember(2)]
            public List<int> Integers { get; } = new List<int>();
        }
    }
   
    namespace V2
    {
        // model 2 version
        [ProtoContract(SkipConstructor=true)]
        [Serializable]
        public class ModelV2 : BaseModel
        {
            [ProtoMember(1)]
            public string Field1 { get; set; }
            [ProtoMember(2)]
            public List<int> Integers { get; } = new List<int>();
        }
    }
}