using System;
using System.Text;

namespace InvertedTomato.IO.Messages {
    public class StringMessage : IMessage {
        public String Value { get; set; }

        public StringMessage() { }
        public StringMessage(String value) {
            if(null == value) {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
        }

        public ArraySegment<Byte> Export() {
            if(null == Value) {
                throw new InvalidOperationException("No value set.");
            }

            return new ArraySegment<Byte>(Encoding.UTF8.GetBytes(Value));
        }

        public void Import(ArraySegment<Byte> payload) {
            if(null == payload) {
                throw new ArgumentNullException(nameof(payload));
            }

            Value = Encoding.UTF8.GetString(payload.Array, payload.Offset, payload.Count);
        }

        public override String ToString() {
            return Value;
        }
    }
}
