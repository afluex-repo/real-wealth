 *** IMPORTANT CHANGE IN RESTSHARP VERSION 103 ***

In 103.0, JSON.NET was removed as a dependency. 

If this is still installed in your project and no other libraries depend on 
it you may remove it from your installed packages.

There is one breaking change: the default Json*Serializer* is no longer 
compatible with Json.NET. To use Json.NET for serialization, copy the code 
from https://github.com/restsharp/RestSharp/blob/86b31f9adf049d7fb821de8279154f41a17b36f7/RestSharp/Serializers/JsonSerializer.cs 
and register it with your client:

var client = new RestClient();
client.JsonSerializer = new YourCustomSerializer();

The default Json*Deserializer* should be 100% compatible.

If you run into any compatibility issues with deserialization, 
please report it to http://groups.google.com/group/restsharp