using System;
using System.Collections;
using System.Collections.Generic;

namespace InvertedTomato.IO.Messages {
    public class CallbackRegister:IEnumerable {
        private readonly Dictionary<UInt32, HandlerRecord> Handlers = new Dictionary<UInt32, HandlerRecord>();
        private class HandlerRecord {
            public Type ObjectType { get; set; }
            public Delegate Callback { get; set; }
        }

        public event Action<UInt32, ArraySegment<Byte>, UInt32> OnUnknownMessage; // typecode, payload, topic

        public void Add<T>(Action<T, UInt32> callback) where T : IImportableMessage, new() { // message, topic
            // Extract typeCode
            var sample = new T();
            var typeCode = sample.TypeCode;

            if (null == callback) { // Handle removals
                Handlers.Remove(typeCode);
            } else { // Handle additions
                // Raise issue if the key already exists
                if (Handlers.ContainsKey(typeCode)) {
                    throw new InvalidOperationException($"Message has typecode of {typeCode} which has already been registered on a previous message. Perhaps you added the same message twice?");
                }

                // Add handler
                Handlers[sample.TypeCode] = new HandlerRecord() {
                    ObjectType = typeof(T),
                    Callback = callback
                };
            }
        }

        public void Invoke(UInt32 typeCode, ArraySegment<Byte> payload, UInt32 topic) {
            // Get handler
            if(!Handlers.TryGetValue(typeCode, out var handler)) {
                // No handler, raise unknown event
                OnUnknownMessage(typeCode, payload, topic);
                return;
            }

            // Create message
            var msg = (IImportableMessage)Activator.CreateInstance(handler.ObjectType);
            
            // Import payload
            msg.Import(payload);

            // Invoke handler with message
            handler.Callback.DynamicInvoke(msg, topic);
        }

        public IEnumerator GetEnumerator() {
            throw new NotSupportedException();
        }
    }
}
