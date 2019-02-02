using System;

namespace InvertedTomato.IO.Messages {
	public class BinaryMessage : IImportableMessage, IExportableMessage {
		public BinaryMessage() { }

		public BinaryMessage(Byte[] value) {
			if (null == value) {
				throw new ArgumentNullException(nameof(value));
			}

			Value = value;
		}

		public Byte[] Value { get; set; }

		public ArraySegment<Byte> Export() {
			if (null == Value) {
				throw new InvalidOperationException("No value set.");
			}

			return new ArraySegment<Byte>(Value);
		}

		public UInt32 TypeCode => 1;

		public void Import(ArraySegment<Byte> payload) {
#if DEBUG
			if (null == payload) {
				throw new ArgumentNullException(nameof(payload));
			}
#endif

			Value = new Byte[payload.Count];
			Array.Copy(payload.Array, payload.Offset, Value, 0, payload.Count);
		}
	}
}