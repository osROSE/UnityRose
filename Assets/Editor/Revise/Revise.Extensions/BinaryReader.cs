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

using System.Collections.Generic;
using System.IO;
using System.Text;
using Revise;
using UnityEngine;

/// <summary>
/// A collection of extensions for the <see cref="BinaryReader"/> class.
/// </summary>
public static class BinaryReaderExtensions {
    /// <summary>
    /// The default encoding to be used when reading strings.
    /// </summary>
    public static Encoding DefaultEncoding;

    /// <summary>
    /// Initializes the <see cref="BinaryReaderExtensions"/> class.
    /// </summary>
    static BinaryReaderExtensions() {
        DefaultEncoding = Encoding.GetEncoding("EUC-KR");
    }

    /// <summary>
    /// Reads a null-terminated string from the current stream.
    /// </summary>
    /// <returns>
    /// The read string.
    /// </returns>
    public static string ReadNullTerminatedString(this BinaryReader reader) {
        return reader.ReadNullTerminatedString(DefaultEncoding);
    }

    /// <summary>
    /// Reads a null-terminated string from the current stream.
    /// </summary>
    /// <param name="encoding">The character encoding.</param>
    /// <returns>
    /// The read string.
    /// </returns>
    public static string ReadNullTerminatedString(this BinaryReader reader, Encoding encoding) {
        List<byte> values = new List<byte>();
        byte value;

        while((value = reader.ReadByte()) != 0) {
            values.Add(value);
        }

        return encoding.GetString(values.ToArray());
    }

    /// <summary>
    /// Reads a string with a pre-fixed length as a 8-bit integer from the underlying stream.
    /// </summary>
    /// <returns>
    /// The read string.
    /// </returns>
    public static string ReadByteString(this BinaryReader reader) {
        return reader.ReadByteString(DefaultEncoding);
    }

    /// <summary>
    /// Reads a string with a pre-fixed length as a 8-bit integer from the underlying stream.
    /// </summary>
    /// <returns>
    /// The read string.
    /// </returns>
    public static string ReadByteString(this BinaryReader reader, Encoding encoding) {
        return reader.ReadString(reader.ReadByte(), DefaultEncoding);
    }

    /// <summary>
    /// Reads a string with a pre-fixed length as a 16-bit integer from the underlying stream.
    /// </summary>
    /// <returns>
    /// The read string.
    /// </returns>
    public static string ReadShortString(this BinaryReader reader) {
        return reader.ReadShortString(DefaultEncoding);
    }

    /// <summary>
    /// Reads a string with a pre-fixed length as a 16-bit integer from the underlying stream.
    /// </summary>
    /// <returns>
    /// The read string.
    /// </returns>
    public static string ReadShortString(this BinaryReader reader, Encoding encoding) {
        return reader.ReadString(reader.ReadInt16(), DefaultEncoding);
    }

    /// <summary>
    /// Reads a string with a pre-fixed length as a 32-bit integer from the underlying stream.
    /// </summary>
    /// <returns>
    /// The read string.
    /// </returns>
    public static string ReadIntString(this BinaryReader reader) {
        return reader.ReadIntString(DefaultEncoding);
    }

    /// <summary>
    /// Reads a string with a pre-fixed length as a 32-bit integer from the underlying stream.
    /// </summary>
    /// <returns>
    /// The read string.
    /// </returns>
    public static string ReadIntString(this BinaryReader reader, Encoding encoding) {
        return reader.ReadString(reader.ReadInt32(), DefaultEncoding);
    }

    /// <summary>
    /// Reads a fixed-length string from the underlying stream.
    /// </summary>
    /// <param name="length">The length of the string.</param>
    /// <returns>
    /// The read string.
    /// </returns>
    public static string ReadString(this BinaryReader reader, int length) {
        return reader.ReadString(length, DefaultEncoding);
    }

    /// <summary>
    /// Reads a fixed-length string from the underlying stream.
    /// </summary>
    /// <param name="length">The length of the string.</param>
    /// <param name="encoding">The character encoding.</param>
    /// <returns>
    /// The read string.
    /// </returns>
    public static string ReadString(this BinaryReader reader, int length, Encoding encoding) {
        return encoding.GetString(reader.ReadBytes(length));
    }

    /// <summary>
    /// Reads a matrix from the underlying stream.
    /// </summary>
    /// <returns>The matrix read.</returns>
    public static Matrix4x4 ReadMatrix(this BinaryReader reader) {
        Matrix4x4 matrix = new Matrix4x4();

        for (int i = 0; i < 16; i++) {
            matrix[i] = reader.ReadSingle();
        }

        return matrix;
    }

    /// <summary>
    /// Reads a vector from the underlying stream.
    /// </summary>
    /// <returns>The vector read.</returns>
    public static Vector2 ReadVector2(this BinaryReader reader) {
        return new Vector2(reader.ReadSingle(), reader.ReadSingle());
    }

    /// <summary>
    /// Reads a vector from the underlying stream.
    /// </summary>
    /// <returns>The vector read.</returns>
    public static Vector3 ReadVector3(this BinaryReader reader) {
        return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
    }

    /// <summary>
    /// Reads a vector from the underlying stream.
    /// </summary>
    /// <returns>The vector read.</returns>
    public static Vector4 ReadVector4(this BinaryReader reader) {
        return new Vector4(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
    }

    /// <summary>
    /// Reads a vector from the underlying stream.
    /// </summary>
    /// <returns>The vector read.</returns>
    public static ShortVector3 ReadShortVector3(this BinaryReader reader) {
        return new ShortVector3(reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16());
    }

    /// <summary>
    /// Reads a vector from the underlying stream.
    /// </summary>
    /// <returns>The vector read.</returns>
    public static ShortVector4 ReadShortVector4(this BinaryReader reader) {
        return new ShortVector4(reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16());
    }

    /// <summary>
    /// Reads a quaternion from the underlying stream.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="w">if set to <c>true</c> the value is read in the order WXYZ, else XYZW.</param>
    /// <returns>The quaternion read.</returns>
    public static Quaternion ReadQuaternion(this BinaryReader reader, bool w = false) {
        if (w) {
            float value = reader.ReadSingle();
            return new Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), value);
        }

        return new Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
    }

    /// <summary>
    /// Reads a colour from the underlying stream.
    /// </summary>
    /// <returns>The vector read.</returns>
    public static Color ReadColour3(this BinaryReader reader) {
        return new Color(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), 1);
    }

    /// <summary>
    /// Reads a colour from the underlying stream.
    /// </summary>
    /// <returns>The vector read.</returns>
    public static Color ReadColour4(this BinaryReader reader) {
        return new Color(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
    }
}
