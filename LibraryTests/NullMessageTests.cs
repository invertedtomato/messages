using InvertedTomato.IO.Messages;
using System;
using Xunit;

namespace LibraryTests {
    public class NullMessageTests {
        [Fact]
        public void Import() {
            var msg = new NullMessage();
            msg.Import(new Byte[] { 1, 2, 3, });
        }

        [Fact]
        public void Export() {
            var msg = new NullMessage();
            Assert.Equal(new Byte[] { }, msg.Export());
        }
    }
}
