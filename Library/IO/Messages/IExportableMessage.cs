using System;

namespace InvertedTomato.IO.Messages {
    public interface IExportableMessage {
        ArraySegment<Byte> Export();
    }
}
