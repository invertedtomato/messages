using System;

namespace InvertedTomato.IO.Messages {
    public class BinaryMessage : IMessage {
        public Byte[] Value { get; set; }

        public BinaryMessage() { }
        public BinaryMessage(Byte[] value) {
            if(null == value) {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
        }

        public ArraySegment<Byte> Export() {
            if(null == Value) {
                throw new InvalidOperationException("No value set.");
            }

            return new ArraySegment<Byte>(Value);
        }

        public void Import(ArraySegment<Byte> payload) {
            if(null == payload) {
                throw new ArgumentNullException(nameof(payload));
            }

            Value = new Byte[payload.Count];
            Array.Copy(payload.Array, payload.Offset, Value, 0, payload.Count);
        }
    }
}
