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

        public Byte[] Export() {
            if(null == Value) {
                throw new InvalidOperationException("No value set.");
            }

            return Value;
        }

        public void Import(Byte[] payload) {
            if(null == payload) {
                throw new ArgumentNullException(nameof(payload));
            }

            Value = payload;
        }
    }
}
