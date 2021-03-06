﻿using InvertedTomato.Compression.Integers;
using InvertedTomato.IO.Bits;
using InvertedTomato.IO.Messages;
using Xunit;

namespace LibraryTests {
	public class GenericMessageTests {
		[Fact]
		public void Boolean_False() {
			var msg = new GenericMessage();
			msg.WriteBoolean(false);

			var payload = msg.Export();
			Assert.Equal("00000000", payload.ToArray().ToBinaryString());

			msg = new GenericMessage();
			msg.Import(payload);
			Assert.False(msg.ReadBoolean());
		}

		[Fact]
		public void Boolean_True() {
			var msg = new GenericMessage();
			msg.WriteBoolean(true);

			var payload = msg.Export();
			Assert.Equal("00000001", payload.ToArray().ToBinaryString());

			msg = new GenericMessage();
			msg.Import(payload);
			Assert.True(msg.ReadBoolean());
		}

		[Fact]
		public void SignedInteger_Minus100() {
			var msg = new GenericMessage();
			msg.WriteSignedInteger(-100);

			var payload = msg.Export();
			Assert.Equal("01000111 10000000", payload.ToArray().ToBinaryString());

			msg = new GenericMessage();
			msg.Import(payload);
			Assert.Equal(-100, msg.ReadSignedInteger());
		}

		[Fact]
		public void SignedInteger_Plus100() {
			var msg = new GenericMessage();
			msg.WriteSignedInteger(+100);

			var payload = msg.Export();
			Assert.Equal("01001000 10000000", payload.ToArray().ToBinaryString());

			msg = new GenericMessage();
			msg.Import(payload);
			Assert.Equal(+100, msg.ReadSignedInteger());
		}

		[Fact]
		public void UnsignedInteger_Max() {
			var msg = new GenericMessage();
			msg.WriteUnsignedInteger(VLQCodec.MaxValue);

			var payload = msg.Export();
			Assert.Equal("01111110 01111110 01111110 01111110 01111110 01111110 01111110 01111110 01111110 10000000", payload.ToArray().ToBinaryString());

			msg = new GenericMessage();
			msg.Import(payload);
			Assert.Equal(VLQCodec.MaxValue, msg.ReadUnsignedInteger());
		}

		[Fact]
		public void UnsignedInteger_Min() {
			var msg = new GenericMessage();
			msg.WriteUnsignedInteger(VLQCodec.MinValue);

			var payload = msg.Export();
			Assert.Equal("10000000", payload.ToArray().ToBinaryString());

			msg = new GenericMessage();
			msg.Import(payload);
			Assert.Equal(VLQCodec.MinValue, msg.ReadUnsignedInteger());
		}

		// TODO: Min&max of all other data types
	}
}