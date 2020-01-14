using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ProtoBuf;

namespace TestSerializeObjectToFile.Models
{
    // model 2 version
    [ProtoContract(SkipConstructor = true), Serializable]
    public class ModelWithReadOnly
    {
        [ProtoMember(1)] 
        private readonly int _id;
        
        [ProtoMember(2)]
        public string Field1 { get; private set; }
        
        [ProtoMember(3)]
        public string Field2 { get; }
        
        [ProtoMember(4)]
        public Dictionary<int, string> Dictionary { get; } = new Dictionary<int, string>();

        public string Field3 => Field1 + Field2;

        public ModelWithReadOnly(int id, string field2)
        {
            _id = id;
            Field2 = field2;
        }

        public void ChangeField(string field)
        {
            Field1 = field;
        }

        public int GetId()
        {
            return _id;
        }
    }
}