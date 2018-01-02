using InvertedTomato.IO.Messages;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LibraryTests {
    public class ProtoBufMessageTests {
        [Fact]
        public void Initialize() {
            var msg = new ProtoBufMessage<Message>(new Message() {
                Value = 1
            });

            Assert.Equal(new Byte[] { 8, 1 }, msg.Export());
        }

        [Fact]
        public void Import() {
            var msg = new ProtoBufMessage<Message>();
            msg.Import(new Byte[] { 8, 1 });
            Assert.Equal((UInt32)1, msg.Deserialize().Value);
        }

        [Fact]
        public void Export() {
            var msg = new ProtoBufMessage<Message>();
            msg.Serialize(new Message() {
                Value = 1
            });
            Assert.Equal(new Byte[] { 8, 1 }, msg.Export());
        }
    }

    [ProtoContract]
    public class Message {
        public Message() { }
        public Message(UInt32 value) { Value = value; }

        [ProtoMember(1)]
        public UInt32 Value { get; set; }
    }
}
