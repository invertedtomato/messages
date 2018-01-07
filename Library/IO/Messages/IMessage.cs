using System;

namespace InvertedTomato.IO.Messages {
    public interface IMessage {
        ArraySegment<Byte> Export();
        void Import(ArraySegment<Byte> payload);
    }
}
