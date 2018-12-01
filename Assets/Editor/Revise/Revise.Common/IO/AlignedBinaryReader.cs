#region License

/**
 * Copyright (C) 2012 Jack Wakefield
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

#endregion

using System.IO;
using System.Text;

namespace Revise.IO {
    public class AlignedBinaryReader : BinaryReader {
        #region Properties

        /// <summary>
        /// Gets the total number of bytes read.
        /// </summary>
        public int TotalRead {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether alignment is disabled.
        /// </summary>
        public bool DisableAlignment {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="AlignedBinaryReader"/> class.
        /// </summary>
        /// <param name="input">The supplied stream.</param>
        public AlignedBinaryReader(Stream input) :
            base(input) {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlignedBinaryReader"/> class.
        /// </summary>
        /// <param name="input">The supplied stream.</param>
        /// <param name="encoding">The character encoding.</param>
        /// <exception cref="T:System.ArgumentException">The stream does not support reading, the stream is null, or the stream is already closed. </exception>
        ///   
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="encoding"/> is null. </exception>
        public AlignedBinaryReader(Stream input, Encoding encoding) :
            base(input, encoding) {

        }

        /// <summary>
        /// Reads the specified number of bytes from the stream, starting from a specified point in the byte array.
        /// </summary>
        /// <param name="buffer">The buffer to read data into.</param>
        /// <param name="index">The starting point in the buffer at which to begin reading into the buffer.</param>
        /// <param name="count">The number of bytes to read.</param>
        /// <returns>
        /// The number of bytes read into <paramref name="buffer"/>. This might be less than the number of bytes requested if that many bytes are not available, or it might be zero if the end of the stream is reached.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">The buffer length minus <paramref name="index"/> is less than <paramref name="count"/>. -or-The number of decoded characters to read is greater than <paramref name="count"/>. This can happen if a Unicode decoder returns fallback characters or a surrogate pair.</exception>
        ///   
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="buffer"/> is null. </exception>
        ///   
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="index"/> or <paramref name="count"/> is negative. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        ///   
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override int Read(byte[] buffer, int index, int count) {
            int value = base.Read(buffer, index, count);
            TotalRead += count;
            return value;
        }

        /// <summary>
        /// Reads the specified number of characters from the stream, starting from a specified point in the character array.
        /// </summary>
        /// <param name="buffer">The buffer to read data into.</param>
        /// <param name="index">The starting point in the buffer at which to begin reading into the buffer.</param>
        /// <param name="count">The number of characters to read.</param>
        /// <returns>
        /// The total number of characters read into the buffer. This might be less than the number of characters requested if that many characters are not currently available, or it might be zero if the end of the stream is reached.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">The buffer length minus <paramref name="index"/> is less than <paramref name="count"/>. -or-The number of decoded characters to read is greater than <paramref name="count"/>. This can happen if a Unicode decoder returns fallback characters or a surrogate pair.</exception>
        ///   
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="buffer"/> is null. </exception>
        ///   
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="index"/> or <paramref name="count"/> is negative. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        ///   
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override int Read(char[] buffer, int index, int count) {
            int value = base.Read(buffer, index, count);
            TotalRead += count;
            return value;
        }

        /// <summary>
        /// Reads the specified number of bytes from the current stream into a byte array and advances the current position by that number of bytes.
        /// </summary>
        /// <param name="count">The number of bytes to read.</param>
        /// <returns>
        /// A byte array containing data read from the underlying stream. This might be less than the number of bytes requested if the end of the stream is reached.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">The number of decoded characters to read is greater than <paramref name="count"/>. This can happen if a Unicode decoder returns fallback characters or a surrogate pair.</exception>
        ///   
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        ///   
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="count"/> is negative. </exception>
        public override byte[] ReadBytes(int count) {
            byte[] value = base.ReadBytes(count);
            TotalRead += count;
            return value;
        }

        /// <summary>
        /// Reads the specified number of characters from the current stream, returns the data in a character array, and advances the current position in accordance with the Encoding used and the specific character being read from the stream.
        /// </summary>
        /// <param name="count">The number of characters to read.</param>
        /// <returns>
        /// A character array containing data read from the underlying stream. This might be less than the number of characters requested if the end of the stream is reached.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">The number of decoded characters to read is greater than <paramref name="count"/>. This can happen if a Unicode decoder returns fallback characters or a surrogate pair.</exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        ///   
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        ///   
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="count"/> is negative. </exception>
        public override char[] ReadChars(int count) {
            char[] value = base.ReadChars(count);
            TotalRead += count;
            return value;
        }

        /// <summary>
        /// Reads a string from the current stream. The string is prefixed with the length, encoded as an integer seven bits at a time.
        /// </summary>
        /// <returns>
        /// The string being read.
        /// </returns>
        /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        ///   
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override string ReadString() {
            long position = BaseStream.Position;

            string value = base.ReadString();
            TotalRead += (int)(BaseStream.Position - position);
            return value;
        }

        /// <summary>
        /// Reads the next character from the current stream and advances the current position of the stream in accordance with the Encoding used and the specific character being read from the stream.
        /// </summary>
        /// <returns>
        /// A character read from the current stream.
        /// </returns>
        /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        ///   
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        ///   
        /// <exception cref="T:System.ArgumentException">A surrogate character was read. </exception>
        public override char ReadChar() {
            char value = base.ReadChar();
            TotalRead++;
            return value;
        }

        /// <summary>
        /// Reads a signed byte from this stream and advances the current position of the stream by one byte.
        /// </summary>
        /// <returns>
        /// A signed byte read from the current stream.
        /// </returns>
        /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        ///   
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override sbyte ReadSByte() {
            sbyte value = base.ReadSByte();
            TotalRead++;
            return value;
        }

        /// <summary>
        /// Reads the next byte from the current stream and advances the current position of the stream by one byte.
        /// </summary>
        /// <returns>
        /// The next byte read from the current stream.
        /// </returns>
        /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        ///   
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override byte ReadByte() {
            byte value = base.ReadByte();
            TotalRead++;
            return value;
        }

        /// <summary>
        /// Reads a Boolean value from the current stream and advances the current position of the stream by one byte.
        /// </summary>
        /// <returns>
        /// true if the byte is nonzero; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        ///   
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override bool ReadBoolean() {
            bool value = base.ReadBoolean();
            TotalRead++;
            return value;
        }

        /// <summary>
        /// Reads a 2-byte signed integer from the current stream and advances the current position of the stream by two bytes.
        /// </summary>
        /// <returns>
        /// A 2-byte signed integer read from the current stream.
        /// </returns>
        /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        ///   
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override short ReadInt16() {
            Align(2);

            short value = base.ReadInt16();
            TotalRead += sizeof(short);
            return value;
        }

        /// <summary>
        /// Reads a 2-byte unsigned integer from the current stream using little-endian encoding and advances the position of the stream by two bytes.
        /// </summary>
        /// <returns>
        /// A 2-byte unsigned integer read from this stream.
        /// </returns>
        /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        ///   
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override ushort ReadUInt16() {
            Align(2);

            ushort value = base.ReadUInt16();
            TotalRead += sizeof(ushort);
            return value;
        }

        /// <summary>
        /// Reads a 4-byte signed integer from the current stream and advances the current position of the stream by four bytes.
        /// </summary>
        /// <returns>
        /// A 4-byte signed integer read from the current stream.
        /// </returns>
        /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        ///   
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override int ReadInt32() {
            Align(4);

            int value = base.ReadInt32();
            TotalRead += sizeof(int);
            return value;
        }

        /// <summary>
        /// Reads a 4-byte unsigned integer from the current stream and advances the position of the stream by four bytes.
        /// </summary>
        /// <returns>
        /// A 4-byte unsigned integer read from this stream.
        /// </returns>
        /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        ///   
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override uint ReadUInt32() {
            Align(4);

            uint value = base.ReadUInt32();
            TotalRead += sizeof(uint);
            return value;
        }

        /// <summary>
        /// Reads an 8-byte signed integer from the current stream and advances the current position of the stream by eight bytes.
        /// </summary>
        /// <returns>
        /// An 8-byte signed integer read from the current stream.
        /// </returns>
        /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        ///   
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override long ReadInt64() {
            Align(8);

            long value = base.ReadInt64();
            TotalRead += sizeof(long);
            return value;
        }

        /// <summary>
        /// Reads an 8-byte unsigned integer from the current stream and advances the position of the stream by eight bytes.
        /// </summary>
        /// <returns>
        /// An 8-byte unsigned integer read from this stream.
        /// </returns>
        /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
        ///   
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        public override ulong ReadUInt64() {
            Align(8);

            ulong value = base.ReadUInt64();
            TotalRead += sizeof(ulong);
            return value;
        }

        /// <summary>
        /// Reads a 4-byte floating point value from the current stream and advances the current position of the stream by four bytes.
        /// </summary>
        /// <returns>
        /// A 4-byte floating point value read from the current stream.
        /// </returns>
        /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        ///   
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override float ReadSingle() {
            Align(4);

            float value = base.ReadSingle();
            TotalRead += sizeof(float);
            return value;
        }

        /// <summary>
        /// Reads an 8-byte floating point value from the current stream and advances the current position of the stream by eight bytes.
        /// </summary>
        /// <returns>
        /// An 8-byte floating point value read from the current stream.
        /// </returns>
        /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        ///   
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override double ReadDouble() {
            Align(8);

            double value = base.ReadDouble();
            TotalRead += sizeof(double);
            return value;
        }

        /// <summary>
        /// Reads a decimal value from the current stream and advances the current position of the stream by sixteen bytes.
        /// </summary>
        /// <returns>
        /// A decimal value read from the current stream.
        /// </returns>
        /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        ///   
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override decimal ReadDecimal() {
            Align(8);

            decimal value = base.ReadDecimal();
            TotalRead += sizeof(decimal);
            return value;
        }

        /// <summary>
        /// Resets the total read count.
        /// </summary>
        public void Reset() {
            TotalRead = 0;
        }

        /// <summary>
        /// Aligns the data read.
        /// </summary>
        public void Align() {
            if (!DisableAlignment) {
                Align(4);
            }
        }

        /// <summary>
        /// Aligns the data written using the specified size.
        /// </summary>
        /// <param name="size">The size.</param>
        private void Align(int size) {
            int remainder = TotalRead % size;

            if (remainder != 0) {
                int skip = size - remainder;
                BaseStream.Seek(skip, SeekOrigin.Current);
                TotalRead += skip;
            }
        }
    }
}