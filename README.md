# Save C# object to file.

### Protobuf

features:
* attributes in model `[ProtoContract]` `[ProtoMember]`
* strong order in fields, so attribute required for every field
* for recursion `[ProtoMember(1, AsReference = true)]`
* follow class with constructors `[ProtoContract(SkipConstructor = true)]`

### Binary 
* attributes in model `[Serializable]`
* 


### Newtonsoft.Json
* Can without attributes
* for recursion options `ReferenceLoopHandling = ReferenceLoopHandling.Serialize`, `PreserveReferencesHandling = PreserveReferencesHandling.Objects`


also read:
[Serialization Performance comparison (C#/.NET)](https://maxondev.com/serialization-performance-comparison-c-net-formats-frameworks-xmldatacontractserializer-xmlserializer-binaryformatter-json-newtonsoft-servicestack-text/)