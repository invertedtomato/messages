# Messages
Message formats for sending over networks or storing in files.

## BinaryMessage
The most basic message format. It literally does nothing - just wraps a binary message ready for transmission or storage.

## StringMessage
Encodes a string as UTF8 binary.

## ProtoBufMessage
When provided with an appropraitely-decorated object, this serializes the object using [protobuf-net](https://github.com/mgravell/protobuf-net).