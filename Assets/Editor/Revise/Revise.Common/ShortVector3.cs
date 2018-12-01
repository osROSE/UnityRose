#region License

// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// -----------------------------------------------------------------------------
// Original code from SlimMath project. http://code.google.com/p/slimmath/
// Greetings to SlimDX Group. Original code published with the following license:
// -----------------------------------------------------------------------------
/*
* Copyright (c) 2007-2011 SlimDX Group
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/
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

using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Revise {
    /// <summary>
    /// Represents a three dimensional mathematical vector.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct ShortVector3 : IEquatable<ShortVector3>, IFormattable {
        /// <summary>
        /// The size of the <see cref="Revise.ShortVector3"/> type, in bytes.
        /// </summary>
        public static readonly int SizeInBytes = Marshal.SizeOf(typeof(ShortVector3));

        /// <summary>
        /// The X component of the vector.
        /// </summary>
        public short X;
        public short x {
            get { return X; }
            set { X = value; }
        }

        /// <summary>
        /// The Y component of the vector.
        /// </summary>
        public short Y;
        public short y {
            get { return Y; }
            set { Y = value; }
        }

        /// <summary>
        /// The Z component of the vector.
        /// </summary>
        public short Z;
        public short z {
            get { return Z; }
            set { Z = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Revise.ShortVector3"/> struct.
        /// </summary>
        /// <param name="value">The value that will be assigned to all components.</param>
        public ShortVector3(short value) {
            X = value;
            Y = value;
            Z = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Revise.ShortVector3"/> struct.
        /// </summary>
        /// <param name="x">Initial value for the X component of the vector.</param>
        /// <param name="y">Initial value for the Y component of the vector.</param>
        /// <param name="z">Initial value for the Z component of the vector.</param>
        public ShortVector3(short x, short y, short z) {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Revise.ShortVector3"/> struct.
        /// </summary>
        /// <param name="values">The values to assign to the X, Y, and Z components of the vector. This must be an array with three elements.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="values"/> contains more or less than three elements.</exception>
        public ShortVector3(short[] values) {
            if (values == null)
                throw new ArgumentNullException("values");
            if (values.Length != 3)
                throw new ArgumentOutOfRangeException("values", "There must be three and only three input values for ShortVector3.");

            X = values[0];
            Y = values[1];
            Z = values[2];
        }

        /// <summary>
        /// Gets or sets the component at the specified index.
        /// </summary>
        /// <value>The value of the X, Y, or Z component, depending on the index.</value>
        /// <param name="index">The index of the component to access. Use 0 for the X component, 1 for the Y component and 2 for the Z component.</param>
        /// <returns>The value of the component at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the <paramref name="index"/> is out of the range [0, 2].</exception>
        public short this[int index] {
            get {
                switch (index) {
                    case 0:
                        return X;
                    case 1:
                        return Y;
                    case 2:
                        return Z;
                }

                throw new ArgumentOutOfRangeException("index", "Indices for ShortVector3 run from 0 to 2, inclusive.");
            }

            set {
                switch (index) {
                    case 0:
                        X = value;
                        break;
                    case 1:
                        Y = value;
                        break;
                    case 2:
                        Z = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("index", "Indices for ShortVector3 run from 0 to 2, inclusive.");
                }
            }
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="left">The first vector to add.</param>
        /// <param name="right">The second vector to add.</param>
        /// <param name="result">When the method completes, contains the sum of the two vectors.</param>
        public static void Add(ref ShortVector3 left, ref ShortVector3 right, out ShortVector3 result) {
            result = new ShortVector3((short)(left.X + right.X), (short)(left.Y + right.Y), (short)(left.Z + right.Z));
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="left">The first vector to add.</param>
        /// <param name="right">The second vector to add.</param>
        /// <returns>The sum of the two vectors.</returns>
        public static ShortVector3 Add(ShortVector3 left, ShortVector3 right) {
            return new ShortVector3((short)(left.X + right.X), (short)(left.Y + right.Y), (short)(left.Z + right.Z));
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="left">The first vector to subtract.</param>
        /// <param name="right">The second vector to subtract.</param>
        /// <param name="result">When the method completes, contains the difference of the two vectors.</param>
        public static void Subtract(ref ShortVector3 left, ref ShortVector3 right, out ShortVector3 result) {
            result = new ShortVector3((short)(left.X - right.X), (short)(left.Y - right.Y), (short)(left.Z - right.Z));
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="left">The first vector to subtract.</param>
        /// <param name="right">The second vector to subtract.</param>
        /// <returns>The difference of the two vectors.</returns>
        public static ShortVector3 Subtract(ShortVector3 left, ShortVector3 right) {
            return new ShortVector3((short)(left.X - right.X), (short)(left.Y - right.Y), (short)(left.Z - right.Z));
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <param name="result">When the method completes, contains the scaled vector.</param>
        public static void Multiply(ref ShortVector3 value, short scale, out ShortVector3 result) {
            result = new ShortVector3((short)(value.X * scale), (short)(value.Y * scale), (short)(value.Z * scale));
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static ShortVector3 Multiply(ShortVector3 value, short scale) {
            return new ShortVector3((short)(value.X * scale), (short)(value.Y * scale), (short)(value.Z * scale));
        }

        /// <summary>
        /// Modulates a vector with another by performing component-wise multiplication.
        /// </summary>
        /// <param name="left">The first vector to modulate.</param>
        /// <param name="right">The second vector to modulate.</param>
        /// <param name="result">When the method completes, contains the modulated vector.</param>
        public static void Modulate(ref ShortVector3 left, ref ShortVector3 right, out ShortVector3 result) {
            result = new ShortVector3((short)(left.X * right.X), (short)(left.Y * right.Y), (short)(left.Z * right.Z));
        }

        /// <summary>
        /// Modulates a vector with another by performing component-wise multiplication.
        /// </summary>
        /// <param name="left">The first vector to modulate.</param>
        /// <param name="right">The second vector to modulate.</param>
        /// <returns>The modulated vector.</returns>
        public static ShortVector3 Modulate(ShortVector3 left, ShortVector3 right) {
            return new ShortVector3((short)(left.X * right.X), (short)(left.Y * right.Y), (short)(left.Z * right.Z));
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <param name="result">When the method completes, contains the scaled vector.</param>
        public static void Divide(ref ShortVector3 value, short scale, out ShortVector3 result) {
            result = new ShortVector3((short)(value.X / scale), (short)(value.Y / scale), (short)(value.Z / scale));
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static ShortVector3 Divide(ShortVector3 value, short scale) {
            return new ShortVector3((short)(value.X / scale), (short)(value.Y / scale), (short)(value.Z / scale));
        }

        /// <summary>
        /// Reverses the direction of a given vector.
        /// </summary>
        /// <param name="value">The vector to negate.</param>
        /// <param name="result">When the method completes, contains a vector facing in the opposite direction.</param>
        public static void Negate(ref ShortVector3 value, out ShortVector3 result) {
            result = new ShortVector3((short)-value.X, (short)-value.Y, (short)-value.Z);
        }

        /// <summary>
        /// Reverses the direction of a given vector.
        /// </summary>
        /// <param name="value">The vector to negate.</param>
        /// <returns>A vector facing in the opposite direction.</returns>
        public static ShortVector3 Negate(ShortVector3 value) {
            return new ShortVector3((short)-value.X, (short)-value.Y, (short)-value.Z);
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="left">The first vector to add.</param>
        /// <param name="right">The second vector to add.</param>
        /// <returns>The sum of the two vectors.</returns>
        public static ShortVector3 operator +(ShortVector3 left, ShortVector3 right) {
            return new ShortVector3((short)(left.X + right.X), (short)(left.Y + right.Y), (short)(left.Z + right.Z));
        }

        /// <summary>
        /// Assert a vector (return it unchanged).
        /// </summary>
        /// <param name="value">The vector to assert (unchange).</param>
        /// <returns>The asserted (unchanged) vector.</returns>
        public static ShortVector3 operator +(ShortVector3 value) {
            return value;
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="left">The first vector to subtract.</param>
        /// <param name="right">The second vector to subtract.</param>
        /// <returns>The difference of the two vectors.</returns>
        public static ShortVector3 operator -(ShortVector3 left, ShortVector3 right) {
            return new ShortVector3((short)(left.X - right.X), (short)(left.Y - right.Y), (short)(left.Z - right.Z));
        }

        /// <summary>
        /// Reverses the direction of a given vector.
        /// </summary>
        /// <param name="value">The vector to negate.</param>
        /// <returns>A vector facing in the opposite direction.</returns>
        public static ShortVector3 operator -(ShortVector3 value) {
            return new ShortVector3((short)-value.X, (short)-value.Y, (short)-value.Z);
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static ShortVector3 operator *(short scale, ShortVector3 value) {
            return new ShortVector3((short)(value.X * scale), (short)(value.Y * scale), (short)(value.Z * scale));
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static ShortVector3 operator *(ShortVector3 value, short scale) {
            return new ShortVector3((short)(value.X * scale), (short)(value.Y * scale), (short)(value.Z * scale));
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        public static ShortVector3 operator /(ShortVector3 value, short scale) {
            return new ShortVector3((short)(value.X / scale), (short)(value.Y / scale), (short)(value.Z / scale));
        }

        /// <summary>
        /// Tests for equality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator ==(ShortVector3 left, ShortVector3 right) {
            return left.Equals(right);
        }

        /// <summary>
        /// Tests for inequality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator !=(ShortVector3 left, ShortVector3 right) {
            return !left.Equals(right);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString() {
            return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1} Z:{2}", X, Y, Z);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public string ToString(string format) {
            if (format == null)
                return ToString();

            return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1} Z:{2}", X.ToString(format, CultureInfo.CurrentCulture),
                Y.ToString(format, CultureInfo.CurrentCulture), Z.ToString(format, CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public string ToString(IFormatProvider formatProvider) {
            return string.Format(formatProvider, "X:{0} Y:{1} Z:{2}", X, Y, Z);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public string ToString(string format, IFormatProvider formatProvider) {
            if (format == null)
                ToString(formatProvider);

            return string.Format(formatProvider, "X:{0} Y:{1} Z:{2}", X.ToString(format, formatProvider),
                Y.ToString(format, formatProvider), Z.ToString(format, formatProvider));
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() {
            return X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="Revise.ShortVector3"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="Revise.ShortVector3"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Revise.ShortVector3"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(ShortVector3 other) {
            return other.X == X && other.Y == Y && other.Z == Z;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="value">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object value) {
            if (value == null)
                return false;

            if (!ReferenceEquals(value.GetType(), typeof(ShortVector3)))
                return false;

            return Equals((ShortVector3)value);
        }
    }
}
