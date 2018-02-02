using System;

namespace InvertedTomato.IO.Messages {
    public interface IImportableMessage : IMessage {
        void Import(ArraySegment<Byte> payload);
    }
}
