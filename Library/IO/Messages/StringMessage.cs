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

        public Byte[] Export() {
            if(null == Value) {
                throw new InvalidOperationException("No value set.");
            }

            return Encoding.UTF8.GetBytes(Value);
        }

        public void Import(Byte[] payload) {
            if(null == payload) {
                throw new ArgumentNullException(nameof(payload));
            }

            Value = Encoding.UTF8.GetString(payload, 0, payload.Length);
        }

        public override String ToString() {
            return Value;
        }
    }
}
