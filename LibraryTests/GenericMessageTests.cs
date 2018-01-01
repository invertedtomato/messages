﻿using InvertedTomato.IO.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using InvertedTomato.IO.Bits;

namespace LibraryTests {
    public class GenericMessageTests {
        [Fact]
        public void UnsignedInteger_Min() {
            var msg = new GenericMessage();
            msg.WriteUnsignedInteger(0);

            var payload = msg.Export();
            Assert.Equal("10000000", payload.ToBinaryString());

            msg = new GenericMessage();
            msg.Import(payload);
            Assert.Equal((UInt64)0, msg.ReadUnsignedInteger());
        }
        [Fact]
        public void UnsignedInteger_Max() {
            var msg = new GenericMessage();
            msg.WriteUnsignedInteger(UInt64.MaxValue);

            var payload = msg.Export();
            Assert.Equal("01111111 01111110 01111110 01111110 01111110 01111110 01111110 01111110 01111110 10000000", payload.ToBinaryString());

            msg = new GenericMessage();
            msg.Import(payload);
            Assert.Equal(UInt64.MaxValue, msg.ReadUnsignedInteger());
        }

        [Fact]
        public void Boolean_True() {
            var msg = new GenericMessage();
            msg.WriteBoolean(true);

            var payload = msg.Export();
            Assert.Equal("00000001", payload.ToBinaryString());

            msg = new GenericMessage();
            msg.Import(payload);
            Assert.True(msg.ReadBoolean());
        }


        [Fact]
        public void Boolean_False() {
            var msg = new GenericMessage();
            msg.WriteBoolean(false);

            var payload = msg.Export();
            Assert.Equal("00000000", payload.ToBinaryString());

            msg = new GenericMessage();
            msg.Import(payload);
            Assert.False(msg.ReadBoolean());
        }

        // TODO: Min&max of all other data types
    }
}