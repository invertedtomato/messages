using System;
using System.IO;
using System.Linq;
using InvertedTomato.Compression.Integers;
using InvertedTomato.IO.Messages;
using Xunit;

namespace LibraryTests {
	public class MessageCoderTests {
		private readonly VLQCodec VLQ = new VLQCodec();

		private UInt64 VLQDecompress(Stream stream) {
			return VLQ.DecompressUnsigned(stream, 1).First();
		}

		private void VLQCompress(Stream stream, UInt64 value) {
			VLQ.CompressUnsigned(stream, value);
		}

		[Fact]
		public void Dispose_NotOwned() {
			var stream = new MemoryStream();
			var coder = new MessageCoder(stream, false);
			Assert.False(coder.IsDisposed);
			coder.Dispose();
			Assert.True(coder.IsDisposed);
			stream.WriteByte(0);
		}

		[Fact]
		public void Dispose_Owned() {
			var stream = new MemoryStream();
			var coder = new MessageCoder(stream, true);
			Assert.False(coder.IsDisposed);
			coder.Dispose();
			Assert.True(coder.IsDisposed);

			Assert.Throws<ObjectDisposedException>(() => { stream.WriteByte(0); });
		}


		[Fact]
		public void Dispose_UnspecifiedOwned() {
			var stream = new MemoryStream();
			var coder = new MessageCoder(stream, false);
			Assert.False(coder.IsDisposed);
			coder.Dispose();
			Assert.True(coder.IsDisposed);
			stream.WriteByte(0);
		}

		[Fact]
		public void Read_Register() {
			using (var stream = new MemoryStream()) {
				VLQCompress(stream, 2); // typecode
				VLQCompress(stream, 4); // length
				stream.WriteByte((Byte) 't'); // payload
				stream.WriteByte((Byte) 'e');
				stream.WriteByte((Byte) 's');
				stream.WriteByte((Byte) 't');

				stream.Position = 0;

				using (var coder = new MessageCoder(stream, false)) {
					var complete = false;
					var register = new CallbackRegister {
						(StringMessage message) => {
							Assert.Equal("test", message.Value);
							complete = true;
						}
					};
					coder.Read(register);
					Assert.True(complete);
				}
			}
		}


		[Fact]
		public void Read_Register_Mismatch() {
			using (var stream = new MemoryStream()) {
				VLQCompress(stream, 2); // typecode
				VLQCompress(stream, 4); // length
				stream.WriteByte((Byte) 't'); // payload
				stream.WriteByte((Byte) 'e');
				stream.WriteByte((Byte) 's');
				stream.WriteByte((Byte) 't');

				stream.Position = 0;

				using (var coder = new MessageCoder(stream, false)) {
					var register = new CallbackRegister {
						(BinaryMessage message) => { Assert.False(true); }
					};
					var complete = false;
					register.OnUnknownMessage += (typeCode, payload) => { complete = true; };
					coder.Read(register);
					Assert.True(complete);
				}
			}
		}

		[Fact]
		public void Read_Simple() {
			using (var stream = new MemoryStream()) {
				VLQCompress(stream, 2); // typecode
				VLQCompress(stream, 4); // length
				stream.WriteByte((Byte) 't'); // payload
				stream.WriteByte((Byte) 'e');
				stream.WriteByte((Byte) 's');
				stream.WriteByte((Byte) 't');

				stream.Position = 0;

				using (var coder = new MessageCoder(stream, false)) {
					var message = coder.Read<StringMessage>();
					Assert.Equal("test", message.Value);
				}
			}
		}

		[Fact]
		public void Read_Simple_Mismatch() {
			using (var stream = new MemoryStream()) {
				VLQCompress(stream, 2); // typecode
				VLQCompress(stream, 4); // length
				stream.WriteByte((Byte) 't'); // payload
				stream.WriteByte((Byte) 'e');
				stream.WriteByte((Byte) 's');
				stream.WriteByte((Byte) 't');

				stream.Position = 0;

				using (var coder = new MessageCoder(stream, false)) {
					Assert.Throws<MismatchTypeCode>(() => { coder.Read<BinaryMessage>(); });
				}
			}
		}

		[Fact]
		public void Write() {
			using (var stream = new MemoryStream()) {
				using (var coder = new MessageCoder(stream, false)) {
					coder.Write(new StringMessage("test"));
				}

				stream.Position = 0;

				Assert.Equal(6, stream.Length);
				Assert.Equal((UInt32) 2, VLQDecompress(stream)); // typecode
				Assert.Equal((UInt32) 4, VLQDecompress(stream)); // length
				Assert.Equal('t', stream.ReadByte());
				Assert.Equal('e', stream.ReadByte());
				Assert.Equal('s', stream.ReadByte());
				Assert.Equal('t', stream.ReadByte());
			}
		}
	}
}