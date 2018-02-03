using InvertedTomato.Compression.Integers;
using System;
using System.IO;
using System.Linq;

namespace InvertedTomato.IO.Messages {
    /// <summary>
    /// Reads and write messages on a stream in the following format: C+L+P*
    ///   C+  VLQ-encoded typecode used to distinguish the message type needed for decoding
    ///   L+  VLQ-encoded payload length
    ///   P*  Payload
    /// </summary>
    public class MessageCoder : IDisposable {
        public Boolean IsDisposed { get; private set; }

        private readonly Stream Underlying;
        private readonly Boolean IsOwned;
        private readonly VLQCodec VLQ = new VLQCodec();

        public MessageCoder(Stream underlying) : this(underlying, false) { }
        public MessageCoder(Stream underlying, Boolean isOwned) {
#if DEBUG
            if (null == underlying) {
                throw new ArgumentNullException(nameof(underlying));
            }
#endif
            Underlying = underlying;
            IsOwned = isOwned;
        }

        /// <summary>
        /// Read a message with an expected type. Use this when you know what message type you're receiving.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Read<T>() where T : IImportableMessage, new() { // C+L+P*
            // Read typecode
            var typeCode = VLQ.DecompressUnsigned(Underlying, 1).First();

            // Read payload length
            var length = VLQ.DecompressUnsigned(Underlying, 1).First();

            // Read payload
            var payload = new Byte[length];
            UInt32 pos = 0;
            do {
                var read = Underlying.Read(payload, 0, (Int32)(length - pos));
                if (read < 1) {
                    throw new EndOfStreamException();
                }
                pos += (UInt32)read;
            } while (pos < length);

            // Create message
            var message = new T();

            // Check typecode
            if (message.TypeCode != typeCode) {
                throw new MismatchTypeCode($"Read payload has typecode of {typeCode}, however trying to read to message with typecode of {message.TypeCode}.");
            }

            // Import payload into message
            message.Import(new ArraySegment<Byte>(payload));

            // Return message
            return message;
        }

        /// <summary>
        /// Read a message when the type is unknown.
        /// </summary>
        /// <param name="register">Register of all possible message types, and a callback for each one.</param>
        public void Read(CallbackRegister register) {
#if DEBUG
            if (null == register) {
                throw new ArgumentNullException(nameof(register));
            }
#endif

            // Read typecode
            var typeCode = (UInt32)VLQ.DecompressUnsigned(Underlying, 1).First();

            // Read payload length
            var length = VLQ.DecompressUnsigned(Underlying, 1).First();

            // Read payload
            var payload = new Byte[length];
            UInt32 pos = 0;
            do {
                var read = Underlying.Read(payload, 0, (Int32)(length - pos));
                if (read < 1) {
                    throw new EndOfStreamException();
                }
                pos += (UInt32)read;
            } while (pos < length);

            // Invoke callback
            register.Invoke(typeCode, new ArraySegment<Byte>(payload));
        }
        
        /// <summary>
        /// Write a message.
        /// </summary>
        /// <param name="message"></param>
        public void Write(IExportableMessage message) {
#if DEBUG
            if (null == message) {
                throw new ArgumentNullException(nameof(message));
            }
#endif

            // Extract payload
            var payload = message.Export();

            // Write typecode and length
            VLQ.CompressUnsigned(Underlying, message.TypeCode, payload.Count);

            // Write payload
            Underlying.Write(payload.Array, payload.Offset, payload.Count);
        }

        protected virtual void Dispose(Boolean disposing) {
            if (IsDisposed) {
                return;
            }
            IsDisposed = true;

            if (disposing) {
                // Dispose managed state (managed objects)
                if (IsOwned) {
                    Underlying?.Dispose();
                }
            }
        }
        public void Dispose() {
            Dispose(true);
        }
    }
}
