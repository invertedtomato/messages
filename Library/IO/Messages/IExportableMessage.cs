using System;

namespace InvertedTomato.IO.Messages {
    public interface IExportableMessage : IMessage {
        ArraySegment<Byte> Export();
    }
}
