using System;

namespace InvertedTomato.IO.Messages {
	public class NullMessage : IImportableMessage, IExportableMessage {
		public ArraySegment<Byte> Export() {
			return new ArraySegment<Byte>(new Byte[] { });
		}

		public UInt32 TypeCode => 0;

		public void Import(ArraySegment<Byte> payload) { }
	}
}