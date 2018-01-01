using System;

namespace InvertedTomato.IO.Messages {
    public interface IMessage {
        Byte[] Export();
        void Import(Byte[] payload);
    }
}
