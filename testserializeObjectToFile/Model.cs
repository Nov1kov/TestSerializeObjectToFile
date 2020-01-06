using System.Collections.Generic;

namespace TestSerializeObjectToFile
{
    // model 2 version
    public class Model
    {
        public int Id { get; set; }
        public string Field { get; set; }
        public string NewField { get; set; }
        public List<Item> List { get; } = new List<Item>();
        public List<Item> NewList { get; } = new List<Item>();
        public ClassWithParentLink Recursive { get; set; }
        // fields from 1 version
        public string MissingField { get; set; }
        public List<string> MissingList { get; } = new List<string>();
    }

    public class ClassWithParentLink
    {
        public Model Model { get; }
        public int Id { get; }

        public ClassWithParentLink(Model model, int id)
        {
            Model = model;
            Id = id;
        }
    }

    public class Item
    {
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