using System;

namespace InvertedTomato.IO.Messages {
    public class MismatchTypeCode : Exception {
        public MismatchTypeCode() { }
        public MismatchTypeCode(String message) : base(message) { }
        public MismatchTypeCode(String message, Exception innerException) : base(message, innerException) { }
    }
}