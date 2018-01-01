using InvertedTomato.IO.Messages;
using System;
using Xunit;

namespace LibraryTests {
    public class StringMessageTests {
        [Fact]
        public void Initialize() {
            var msg = new StringMessage("test");
            Assert.Equal("test", msg.Value);
        }

        [Fact]
        public void Import() {
            var msg = new StringMessage();
            msg.Import(new Byte[] { 116, 101, 115, 116 });
            Assert.Equal("test", msg.Value);
        }

        [Fact]
        public void Export() {
            var msg = new StringMessage();
            msg.Value = "test";
            Assert.Equal(new Byte[] { 116, 101, 115, 116 }, msg.Export());
        }
    }
}
