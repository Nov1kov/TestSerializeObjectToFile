using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ProtoBuf;

namespace TestSerializeObjectToFile.Models
{
    // model 2 version
    [ProtoContract, Serializable]
    public class Model
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        
        [ProtoMember(2)]
        public string Field { get; set; }
        
        [ProtoMember(3)]
        public string NewField { get; set; }
        
        [ProtoMember(4)]
        public List<Item> List { get; } = new List<Item>();
        
        [ProtoMember(7)]
        public ClassWithLink Recursive { get; set; }
        
        // field new in version 2
        [ProtoMember(8)] public List<Item> NewList { get; set; } = new List<Item>();

        // fields from 1 version
        //[ProtoMember(5)] public string MissingField { get; set; }
        
        //[ProtoMember(6)] public List<string> MissingList { get; } = new List<string>();
        
        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            if (NewList == null) NewList = new List<Item>();
        }
    }

    [ProtoContract(SkipConstructor = true), Serializable]
    public class ClassWithLink
    {
        [ProtoMember(1, AsReference = true)]
        public Model Model { get; }
        
        [ProtoMember(2)]
        public int Id { get; }

        public ClassWithLink(Model model, int id)
        {
            Model = model;
            Id = id;
        }
    }

    [ProtoContract(SkipConstructor = true), Serializable]
    public class Item
    {
        [ProtoMember(1)]
        public int Id { get; }

        public Item(int id)
        {
            Id = id;
        }
        
        protected bool Equals(Item other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Item) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}