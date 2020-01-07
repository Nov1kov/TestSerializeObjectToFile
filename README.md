# Save C# object to file.

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


also read:
[Serialization Performance comparison (C#/.NET)](https://maxondev.com/serialization-performance-comparison-c-net-formats-frameworks-xmldatacontractserializer-xmlserializer-binaryformatter-json-newtonsoft-servicestack-text/)