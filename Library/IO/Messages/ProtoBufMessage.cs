using ProtoBuf;
using System;
using System.IO;

namespace InvertedTomato.IO.Messages {
    public class ProtoBufMessage<T> : IMessage, IImportableMessage, IExportableMessage {
        public UInt32 TypeCode { get { return 3; } }

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

        public ArraySegment<Byte> Export() {
            if(null == Payload) {
                throw new InvalidOperationException("No value set.");
            }

            return new ArraySegment<Byte>(Payload);
        }

        public void Import(ArraySegment<Byte> payload) {
            if(null == payload) {
                throw new ArgumentNullException(nameof(payload));
            }

            Payload = new Byte[payload.Count];
            Array.Copy(payload.Array, payload.Offset, Payload, 0, payload.Count);
        }
    }
}
