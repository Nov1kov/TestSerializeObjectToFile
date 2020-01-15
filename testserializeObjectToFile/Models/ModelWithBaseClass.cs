using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using ProtoBuf;

namespace TestSerializeObjectToFile.Models
{
    // model 2 version
    [ProtoContract(SkipConstructor = true), Serializable]
    public class ModelWithSubclass : BaseClass
    {
        [ProtoMember(1)]
        public int IntId2 { get; set; }
        
        [ProtoMember(2)]
        public string StrField2 { get; set; }

        [ProtoMember(3)] 
        public InnerClass InnerField { get; }

        public ModelWithSubclass(string innerStr)
        {
            InnerField = new InnerClass
            {
                InnerStr = innerStr,
            };
        }

        [ProtoContract, Serializable]
        public class InnerClass : BaseClass
        {
            [ProtoMember(1)] 
            public string InnerStr { get; set; }
        }
    }

    [ProtoContract, Serializable]
    [ProtoInclude(100, typeof(ModelWithSubclass))]
    [ProtoInclude(200, typeof(ModelWithSubclass.InnerClass))]
    public class BaseClass
    {
        [ProtoMember(1)]
        public int IntId1 { get; set; }
        
        [ProtoMember(2)]
        public string StrField1 { get; set; }
    }
}