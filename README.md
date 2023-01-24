# JsonParser
A simple JSON parser written in C# without external dependencies.

This JSON parser does not allocate strings while parsing, and reads data from a BinaryReader.
The values used to do fields lookup must be hashed in forehand.
The hash lookup is not collision-free, but it peformed well enough for my use cases.
Some files from the project uses "unsafe" methods.

Usage:

test.json contents:
```json
{
  "TestString": "Hello",
  "TestInt" : 1,
  "TestFloat": 2.0,
  "TestArray": ["I","am","an","array"]
}
```

```csharp
using System;
using System.IO;
using JsonParser;
...
//Pre-hashed keys
private static long testStringHash = HashUtils.GetHash("TestString");
private static long testIntHash = HashUtils.GetHash("TestInt");
private static long testFloatHash = HashUtils.GetHash("TestFloat");
private static long testArrayHash = HashUtils.GetHash("TestArray");
...
using (var binaryReader = new BinaryReader(File.Open("test.json", FileMode.Open)) {
    var jsonParser = new JsonParser(binaryReader);
    
    var rootValue = jsonParser.ParseRootValue();
    if (rootValue.Valid) {
      Console.WriteLine(rootValue.GetChildValueAsString(testStringHash));
      
      //A fixed string is used to avoid strings allocation when doing string-to-number conversion
      var temporaryString = new TemporaryString(1024);
      Console.WriteLine(rootValue.GetChildValueAsInt(testIntHash, temporaryString));
      Console.WriteLine(rootValue.GetChildValueAsFloat(testFloatHash, temporaryString));

      var array = rootValue.GetChildWithKey(testArrayHash);
      if (array.Valid) {
        Console.WriteLine(array.GetArrayValueAtIndex(0).ToString());
        Console.WriteLine(array.GetArrayValueAtIndex(1).ToString());
        Console.WriteLine(array.GetArrayValueAtIndex(2).ToString());
        Console.WriteLine(array.GetArrayValueAtIndex(3).ToString());
      }
    }
  }
}
```
