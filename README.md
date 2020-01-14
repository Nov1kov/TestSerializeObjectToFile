# Save C# object to file.

## Test methodology

### 1. Update [model](https://github.com/Nov1kov/TestSerializeObjectToFile/blob/master/testserializeObjectToFile/Models/Model.cs)

Save model version 1 to file, and load this file to model version 2.

* Model has one object which has link to Model
* Model version 1 has MissingField and MissingList which doesn't have model version 2.
* Model version 2 has NewField which didn't have model version 1

### 2. Save [model with private fields](https://github.com/Nov1kov/TestSerializeObjectToFile/blob/master/testserializeObjectToFile/Models/ModelWithReadOnly.cs)

* Model has private readonly field
* Model has auto-property with private setter
* Model has auto-property without setter
* Model has propery with only getter. (this field shouldn't serialize) 

## Serializer's features

### Protobuf

* required attributes for classes and fields `[ProtoContract]` `[ProtoMember]`
* strong order in fields, so attribute required for every field
* required mark link against recusrsion `[ProtoMember(1, AsReference = true)]`
* required attribute for class with constructors `[ProtoContract(SkipConstructor = true)]`f
* additional setup for sub classes `ProtoInclude`

### Binary 
* required attributes for classes `[Serializable]`
* required callbacks for new objects `[OnDeserialized]`


### Newtonsoft.Json
* optional attributes for classes and fields
* for recursion options `ReferenceLoopHandling = ReferenceLoopHandling.Serialize`, `PreserveReferencesHandling = PreserveReferencesHandling.Objects`
* for save only in private fields need custom `IContractResolver`


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
* :heavy_minus_sign: may dependency of .net version

### Newtonsoft.Json
* :heavy_plus_sign: easiest implementation 

## Todo:
* [ ] test with big files
* [ ] test with different namespaces and classes

#### also read:
* [Serialization Performance comparison (C#/.NET)](https://maxondev.com/serialization-performance-comparison-c-net-formats-frameworks-xmldatacontractserializer-xmlserializer-binaryformatter-json-newtonsoft-servicestack-text/)
* [Microsoft docs: Version tolerant serialization](https://docs.microsoft.com/en-us/dotnet/standard/serialization/version-tolerant-serialization)
