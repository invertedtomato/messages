using ProtoBuf;
using System;
using System.IO;

namespace InvertedTomato.IO.Messages {
    public class ProtoBufMessage<T> : IMessage {
        public Byte[] Payload { get; set; }

        public ProtoBufMessage() { }
        public ProtoBufMessage(T value) {
            Serialize(value);
        }

        public void Serialize(T value) {
            if(null == value) {
                throw new ArgumentNullException(nameof(value));
            }

            using(var stream = new MemoryStream()) {
                Serializer.Serialize(stream, value);
                Payload = stream.ToArray();
            }
        }

        public T Deserialize() {
            if(null == Payload) {
                throw new InvalidOperationException("No value set.");
            }

            using(var stream = new MemoryStream(Payload)) {
                return Serializer.Deserialize<T>(stream);
            }
        }

        public Byte[] Export() {
            if(null == Payload) {
                throw new InvalidOperationException("No value set.");
            }

            return Payload;
        }

        public void Import(Byte[] payload) {
            if(null == payload) {
                throw new ArgumentNullException(nameof(payload));
            }

            Payload = payload;
        }
    }
}
