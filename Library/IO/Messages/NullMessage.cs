using System;

namespace InvertedTomato.IO.Messages {
    public class NullMessage : IImportableMessage, IExportableMessage {
        public UInt32 TypeCode { get { return 0; } }

        public NullMessage() { }

        public ArraySegment<Byte> Export() {
            return new ArraySegment<Byte>(new Byte[] { });
        }

        public void Import(ArraySegment<Byte> payload) { }
    }
}
