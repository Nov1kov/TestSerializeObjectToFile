# Save C# object to file.

## Test methodology

Save model version 1 to file, and load this file to model version 2.

### model version 1

```c#
public class Model
{
    public int Id { get; set; }
    public string Field { get; set; }
    public string NewField { get; set; }
    public List<Item> List { get; } = new List<Item>();
    public ClassWithLink Recursive { get; set; }
    
    // field which will hide in future version
    public string MissingField { get; set; }
    public List<string> MissingList { get; } = new List<string>();
}
```

### model version 2

```c#
public class Model
{
    public int Id { get; set; }
    public string Field { get; set; }
    public string NewField { get; set; }
    public List<Item> List { get; } = new List<Item>();
    public ClassWithLink Recursive { get; set; }
    
    // new field, must be empty list
    public List<Item> NewList { get; set; } = new List<Item>();
}
```

## Serializer's features

### Protobuf

* required attributes for classes and fields `[ProtoContract]` `[ProtoMember]`
* strong order in fields, so attribute required for every field
* for recursion `[ProtoMember(1, AsReference = true)]`
* follow class with constructors `[ProtoContract(SkipConstructor = true)]`


### Binary 
* required attributes for classes `[Serializable]`
* required callbacks for new objects `[OnDeserialized]`


### Newtonsoft.Json
* optional attributes for classes and fields
* for recursion options `ReferenceLoopHandling = ReferenceLoopHandling.Serialize`, `PreserveReferencesHandling = PreserveReferencesHandling.Objects`


## Model's size (bytes)
|           | Binary | Json |  Protobuf  |
|:----------|:-------|:-----|:---|
| version 1 |   1 518     |  211    |  66  |
| version 2 |   1 357     |  155    |  31  |

## Conclusion
### Protobuf-net
* :heavy_plus_sign: smallest size of model
* :heavy_minus_sign: hardest setup

### Binary
* :heavy_plus_sign: no third party libraries
* :heavy_plus_sign: easiest support recursion
* :heavy_minus_sign: don't support rename class, fields

### Newtonsoft.Json
* :heavy_plus_sign: easiest implementation 

#### also read:
* [Serialization Performance comparison (C#/.NET)](https://maxondev.com/serialization-performance-comparison-c-net-formats-frameworks-xmldatacontractserializer-xmlserializer-binaryformatter-json-newtonsoft-servicestack-text/)
* [Microsoft docs: Version tolerant serialization](https://docs.microsoft.com/en-us/dotnet/standard/serialization/version-tolerant-serialization)