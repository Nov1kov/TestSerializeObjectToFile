using System;
using System.Collections.Generic;
using System.Data.Common;
using ProtoBuf;

namespace TestSerializeObjectToFile.Models
{
    [ProtoContract, Serializable]
    public class AbstractList
    {
        [ProtoMember(1)] public List<IItem> Items { get; } = new List<IItem>();
    }

    [ProtoContract, Serializable]
    public class ItemImplementation : IItem
    {
        [ProtoMember(1)] public int Id { get; set; }
    }

    [ProtoContract]
    [ProtoInclude(100, typeof(ItemImplementation))]
    public interface IItem
    {
        [ProtoMember(1)] int Id { get; set; }
    }
}