using System;
using InvertedTomato.IO.Messages;
using Xunit;

namespace LibraryTests {
	public class BinaryMessageTests {
		[Fact]
		public void Export() {
			var msg = new BinaryMessage();
			msg.Value = new Byte[] {1, 2, 3};
			Assert.Equal(new Byte[] {1, 2, 3}, msg.Export());
		}

		[Fact]
		public void Import() {
			var msg = new BinaryMessage();
			msg.Import(new Byte[] {1, 2, 3});
			Assert.Equal(new Byte[] {1, 2, 3}, msg.Value);
		}

		[Fact]
		public void Initialize() {
			var msg = new BinaryMessage(new Byte[] {1, 2, 3});
			Assert.Equal(new Byte[] {1, 2, 3}, msg.Value);
		}
	}
}