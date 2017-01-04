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
    public class AlignedBinaryWriter : BinaryWriter {
        #region Properties

        /// <summary>
        /// Gets the total number of bytes wrote.
        /// </summary>
        public int TotalWrote {
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
        /// Initializes a new instance of the <see cref="AlignedBinaryWriter"/> class.
        /// </summary>
        /// <param name="output">The supplied output.</param>
        public AlignedBinaryWriter(Stream output) :
            base(output) {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlignedBinaryWriter"/> class.
        /// </summary>
        /// <param name="output">The supplied stream.</param>
        /// <param name="encoding">The character encoding.</param>
        /// <exception cref="T:System.ArgumentException">The stream does not support writing, or the stream is already closed. </exception>
        ///   
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="output"/> or <paramref name="encoding"/> is null. </exception>
        public AlignedBinaryWriter(Stream output, Encoding encoding) :
            base(output, encoding) {

        }
        
        /// <summary>
        /// Writes a byte array to the underlying stream.
        /// </summary>
        /// <param name="buffer">A byte array containing the data to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        ///   
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="buffer"/> is null. </exception>
        public override void Write(byte[] buffer) {
            base.Write(buffer);
            TotalWrote += buffer.Length;
        }

        /// <summary>
        /// Writes a character array to the current stream and advances the current position of the stream in accordance with the Encoding used and the specific characters being written to the stream.
        /// </summary>
        /// <param name="chars">A character array containing the data to write.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="chars"/> is null. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        ///   
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override void Write(char[] chars) {
            base.Write(chars);
            TotalWrote += chars.Length;
        }

        /// <summary>
        /// Writes a region of a byte array to the current stream.
        /// </summary>
        /// <param name="buffer">A byte array containing the data to write.</param>
        /// <param name="index">The starting point in <paramref name="buffer"/> at which to begin writing.</param>
        /// <param name="count">The number of bytes to write.</param>
        /// <exception cref="T:System.ArgumentException">The buffer length minus <paramref name="index"/> is less than <paramref name="count"/>. </exception>
        ///   
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="buffer"/> is null. </exception>
        ///   
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="index"/> or <paramref name="count"/> is negative. </exception>
        ///   
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        public override void Write(byte[] buffer, int index, int count) {
            base.Write(buffer, index, count);
            TotalWrote += count;
        }

        /// <summary>
        /// Writes a section of a character array to the current stream, and advances the current position of the stream in accordance with the Encoding used and perhaps the specific characters being written to the stream.
        /// </summary>
        /// <param name="chars">A character array containing the data to write.</param>
        /// <param name="index">The starting point in <paramref name="chars"/> from which to begin writing.</param>
        /// <param name="count">The number of characters to write.</param>
        /// <exception cref="T:System.ArgumentException">The buffer length minus <paramref name="index"/> is less than <paramref name="count"/>. </exception>
        ///   
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="chars"/> is null. </exception>
        ///   
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="index"/> or <paramref name="count"/> is negative. </exception>
        ///   
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        public override void Write(char[] chars, int index, int count) {
            base.Write(chars, index, count);
            TotalWrote += count;
        }

        /// <summary>
        /// Writes a one-byte Boolean value to the current stream, with 0 representing false and 1 representing true.
        /// </summary>
        /// <param name="value">The Boolean value to write (0 or 1).</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        public override void Write(bool value) {
            base.Write(value);
            TotalWrote++;
        }

        /// <summary>
        /// Writes an unsigned byte to the current stream and advances the stream position by one byte.
        /// </summary>
        /// <param name="value">The unsigned byte to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        public override void Write(byte value) {
            base.Write(value);
            TotalWrote++;
        }

        /// <summary>
        /// Writes a signed byte to the current stream and advances the stream position by one byte.
        /// </summary>
        /// <param name="value">The signed byte to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        public override void Write(sbyte value) {
            base.Write(value);
            TotalWrote++;
        }

        /// <summary>
        /// Writes a Unicode character to the current stream and advances the current position of the stream in accordance with the Encoding used and the specific characters being written to the stream.
        /// </summary>
        /// <param name="ch">The non-surrogate, Unicode character to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        ///   
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="ch"/> is a single surrogate character.</exception>
        public override void Write(char ch) {
            base.Write(ch);
            TotalWrote++;
        }

        /// <summary>
        /// Writes a two-byte signed integer to the current stream and advances the stream position by two bytes.
        /// </summary>
        /// <param name="value">The two-byte signed integer to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        public override void Write(short value) {
            Align(2);

            base.Write(value);
            TotalWrote += sizeof(short);
        }

        /// <summary>
        /// Writes a two-byte unsigned integer to the current stream and advances the stream position by two bytes.
        /// </summary>
        /// <param name="value">The two-byte unsigned integer to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        public override void Write(ushort value) {
            Align(2);

            base.Write(value);
            TotalWrote += sizeof(ushort);
        }

        /// <summary>
        /// Writes a four-byte signed integer to the current stream and advances the stream position by four bytes.
        /// </summary>
        /// <param name="value">The four-byte signed integer to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        public override void Write(int value) {
            Align(4);

            base.Write(value);
            TotalWrote += sizeof(int);
        }

        /// <summary>
        /// Writes a four-byte unsigned integer to the current stream and advances the stream position by four bytes.
        /// </summary>
        /// <param name="value">The four-byte unsigned integer to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        public override void Write(uint value) {
            Align(4);

            base.Write(value);
            TotalWrote += sizeof(uint);
        }

        /// <summary>
        /// Writes an eight-byte signed integer to the current stream and advances the stream position by eight bytes.
        /// </summary>
        /// <param name="value">The eight-byte signed integer to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        public override void Write(long value) {
            Align(8);

            base.Write(value);
            TotalWrote += sizeof(long);
        }

        /// <summary>
        /// Writes an eight-byte unsigned integer to the current stream and advances the stream position by eight bytes.
        /// </summary>
        /// <param name="value">The eight-byte unsigned integer to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        public override void Write(ulong value) {
            Align(8);

            base.Write(value);
            TotalWrote += sizeof(ulong);
        }

        /// <summary>
        /// Writes a four-byte floating-point value to the current stream and advances the stream position by four bytes.
        /// </summary>
        /// <param name="value">The four-byte floating-point value to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        public override void Write(float value) {
            Align(4);

            base.Write(value);
            TotalWrote += sizeof(float);
        }

        /// <summary>
        /// Writes an eight-byte floating-point value to the current stream and advances the stream position by eight bytes.
        /// </summary>
        /// <param name="value">The eight-byte floating-point value to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        public override void Write(double value) {
            Align(8);

            base.Write(value);
            TotalWrote += sizeof(double);
        }

        /// <summary>
        /// Writes a decimal value to the current stream and advances the stream position by sixteen bytes.
        /// </summary>
        /// <param name="value">The decimal value to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        public override void Write(decimal value) {
            Align(8);

            base.Write(value);
            TotalWrote += sizeof(decimal);
        }

        /// <summary>
        /// Writes a length-prefixed string to this stream in the current encoding of the <see cref="T:System.IO.BinaryWriter"/>, and advances the current position of the stream in accordance with the encoding used and the specific characters being written to the stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        ///   
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="value"/> is null. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        public override void Write(string value) {
            long position = BaseStream.Position;

            base.Write(value);
            TotalWrote += (int)(BaseStream.Position - position);
        }

        /// <summary>
        /// Resets the total wrote value.
        /// </summary>
        public void Reset() {
            TotalWrote = 0;
        }

        /// <summary>
        /// Aligns the data written.
        /// </summary>
        public void Align() {
            if (!DisableAlignment) {
                Align(4);
            }
        }

        /// <summary>
        /// Aligns the specified size.
        /// </summary>
        /// <param name="size">The size.</param>
        private void Align(int size) {
            int remainder = TotalWrote % size;

            if (remainder != 0) {
                int skip = size - remainder;

                for (int i = 0; i < skip; i++) {
                    Write((byte)0);
                }
            }
        }
    }
}