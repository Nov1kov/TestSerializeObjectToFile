# Save C# object to file.

![XUnit tests](https://github.com/Nov1kov/TestSerializeObjectToFile/workflows/XUnit%20tests/badge.svg?event=push)

## Test methods

### 1. Update [model](https://github.com/Nov1kov/TestSerializeObjectToFile/blob/master/testserializeObjectToFile/Models/Model.cs)

Save model version 1 to file, and load this file to model version 2.

* Model has one object which has link to Model
* Model version 1 has MissingField and MissingList which doesn't have model version 2.
* Model version 2 has NewField which didn't have model version 1

### 2. Save and load [model with private fields](https://github.com/Nov1kov/TestSerializeObjectToFile/blob/master/testserializeObjectToFile/Models/ModelWithReadOnly.cs)

* Model has private readonly field
* Model has auto-property with private setter
* Model has auto-property without setter
* Model has propery with only getter. (this field shouldn't serialize) 

### 3. Save and load [list of abstract items](https://github.com/Nov1kov/TestSerializeObjectToFile/blob/master/testserializeObjectToFile/Models/AbstractList.cs)

* list with specific items, save and load like as list of abstract items

### 4. Save and load [model with subclasses](https://github.com/Nov1kov/TestSerializeObjectToFile/blob/master/testserializeObjectToFile/Models/ModelWithBaseClass.cs)

* model derived from base class
* model has public inner class 

### 5. Model's size (byte)

List of abstract items `public List<IItem> Items { get; } = new List<IItem>();`  which contain items with different int and strings

```c#
    public class ItemImpl : IItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
```


|           | Json TypeNameHandling.Auto** | Json  | Binary |  Protobuf  |
|:----------|:-------|:-----|:---|:---|
| 10 items |   1 872     | 393 |  1 085  |  186  |
| 50 items |  6 443     |  1 993 | 2 405   |  986  |
| 200 items |   26 095     | 8 295 | 7 455 |  4 230  |
| 1000 items |   131 697     | 42 697 | 34 655  |  22 630  |
| 1000 items * |   42 697     | 42 697 | 34 707  |  16 760   |

*items - without abstraction `public List<ItemImpl> Items { get; } = new List<ItemImpl>();`

** TypeNameHandling.Auto - using for desirialize objects from list with abstract objects 


## Serializer's features

### Protobuf

* required attributes for classes and interfaces `[ProtoContract]`
* required for every field `[ProtoMember]`
* strong order in fields, so attribute required for every field
* required mark link against recusrsion `[ProtoMember(1, AsReference = true)]`
* required attribute for class with constructors `[ProtoContract(SkipConstructor = true)]`f
* additional setup for sub classes `ProtoInclude`
* for serialize list of abstract items `public List<IItem> Items { get; }` need to wrap in class the list.

### Binary 
* required attributes for classes `[Serializable]`
* optional callbacks for initialize new objects `[OnDeserialized]`


### Newtonsoft.Json
* optional attributes for classes and fields
* for recursion options `ReferenceLoopHandling = ReferenceLoopHandling.Serialize`, `PreserveReferencesHandling = PreserveReferencesHandling.Objects`
* for save only in private fields need custom `IContractResolver`
* for serialize abstract items need `TypeNameHandling = TypeNameHandling.Auto`


## Conclusion
### Protobuf-net
* :heavy_plus_sign: smallest size of model
* :heavy_minus_sign: hardest setup

### Binary
* :heavy_plus_sign: no third party libraries
* :heavy_plus_sign: easiest support recursion
* :heavy_plus_sign: easiest support interface and abstract class implementations
* :heavy_minus_sign: don't support rename class, fields
* :heavy_minus_sign: may dependency of .net version

### Newtonsoft.Json
* :heavy_plus_sign: easiest implementation 
* :heavy_minus_sign: hard work with private fields

## Todo:
* [ ] test with different namespaces and classes

#### also read:
* [Serialization Performance comparison (C#/.NET)](https://maxondev.com/serialization-performance-comparison-c-net-formats-frameworks-xmldatacontractserializer-xmlserializer-binaryformatter-json-newtonsoft-servicestack-text/)
* [Microsoft docs: Version tolerant serialization](https://docs.microsoft.com/en-us/dotnet/standard/serialization/version-tolerant-serialization)
