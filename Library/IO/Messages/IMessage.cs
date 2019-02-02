using System;

namespace InvertedTomato.IO.Messages {
	public interface IMessage {
		UInt32 TypeCode { get; }
	}
}