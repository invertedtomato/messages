using System;
using System.Text;

namespace InvertedTomato.IO.Messages {
	public class StringMessage : IImportableMessage, IExportableMessage {
		public StringMessage() { }

		public StringMessage(String value) {
			if (null == value) {
				throw new ArgumentNullException(nameof(value));
			}

			Value = value;
		}

		public String Value { get; set; }

		public ArraySegment<Byte> Export() {
			if (null == Value) {
				throw new InvalidOperationException("No value set.");
			}

			return new ArraySegment<Byte>(Encoding.UTF8.GetBytes(Value));
		}

		public UInt32 TypeCode => 2;

		public void Import(ArraySegment<Byte> payload) {
#if DEBUG
			if (null == payload) {
				throw new ArgumentNullException(nameof(payload));
			}
#endif

			Value = Encoding.UTF8.GetString(payload.Array, payload.Offset, payload.Count);
		}

		public override String ToString() {
			return Value;
		}
	}
}