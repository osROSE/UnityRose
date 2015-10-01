// <copyright file="FileHandler.cs" company="Wadii Bellamine">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Wadii Bellamine</author>
// <date>2/25/2015 8:37 AM </date>

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UnityRose.File
{
    #region New Value Types

    /// <summary>
    /// NString class.
    /// </summary>
    public class NString
    {
        #region Member Declarations

        /// <summary>
        /// Gets or sets the string data.
        /// </summary>
        /// <value>The string data.</value>
        private string stringData { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="NString"/> class.
        /// </summary>
        public NString()
        {
            stringData = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NString"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public NString(string value)
        {
            stringData = value;
        }

        /// <summary>
        /// Gets the length.
        /// </summary>
        /// <value>The length.</value>
        public int Length
        {
            get { return stringData.Length; }
        }

        /// <summary>
        /// Gets the <see cref="System.Char"/> at the specified index.
        /// </summary>
        /// <value></value>
        public char this[int index]
        {
            get { return stringData[index]; }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="Map_Editor.Engine.NString"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator NString(string value)
        {
            return new NString(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Map_Editor.Engine.NString"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator string(NString value)
        {
            return value.stringData;
        }
    }

    /// <summary>
    /// BaseString class.
    /// </summary>
    public class BaseString
    {
        #region Member Declarations

        /// <summary>
        /// Gets or sets the string data.
        /// </summary>
        /// <value>The string data.</value>
        private string stringData { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseString"/> class.
        /// </summary>
        public BaseString()
        {
            stringData = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseString"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public BaseString(string value)
        {
            stringData = value;
        }

        /// <summary>
        /// Gets the length.
        /// </summary>
        /// <value>The length.</value>
        public int Length
        {
            get { return stringData.Length; }
        }

        /// <summary>
        /// Toes the char array.
        /// </summary>
        /// <returns></returns>
        public char[] ToCharArray()
        {
            return stringData.ToCharArray();
        }

        /// <summary>
        /// Gets the <see cref="System.Char"/> at the specified index.
        /// </summary>
        /// <value></value>
        public char this[int index]
        {
            get { return stringData[index]; }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="Map_Editor.Engine.BaseString"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator BaseString(string value)
        {
            return new BaseString(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Map_Editor.Engine.BaseString"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator string(BaseString value)
        {
            return value.stringData;
        }
    }

    /// <summary>
    /// Class for the read/write value "BString"
    /// </summary>
    public class BString
    {
        #region Member Declarations

        /// <summary>
        /// Gets or sets the string data.
        /// </summary>
        /// <value>The string data.</value>
        private string stringData { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="BString"/> class.
        /// </summary>
        public BString()
        {
            stringData = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BString"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public BString(string value)
        {
            stringData = value;
        }

        /// <summary>
        /// Gets the length.
        /// </summary>
        /// <value>The length.</value>
        public int Length
        {
            get { return stringData.Length; }
        }

        /// <summary>
        /// Toes the char array.
        /// </summary>
        /// <returns></returns>
        public char[] ToCharArray()
        {
            return stringData.ToCharArray();
        }

        /// <summary>
        /// Gets the <see cref="System.Char"/> at the specified index.
        /// </summary>
        /// <value></value>
        public char this[int index]
        {
            get { return stringData[index]; }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="Map_Editor.Engine.BString"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator BString(string value)
        {
            return new BString(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Map_Editor.Engine.BString"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator string(BString value)
        {
            return value.stringData;
        }
    }

    /// <summary>
    /// ZString class.
    /// </summary>
    public class ZString
    {
        #region Member Declarations

        /// <summary>
        /// Gets or sets the string data.
        /// </summary>
        /// <value>The string data.</value>
        private string stringData { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ZString"/> class.
        /// </summary>
        public ZString()
        {
            stringData = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZString"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ZString(string value)
        {
            stringData = value;
        }

        /// <summary>
        /// Gets the length.
        /// </summary>
        /// <value>The length.</value>
        public int Length
        {
            get { return stringData.Length; }
        }

        /// <summary>
        /// Toes the char array.
        /// </summary>
        /// <returns></returns>
        public char[] ToCharArray()
        {
            return stringData.ToCharArray();
        }

        /// <summary>
        /// Gets the <see cref="System.Char"/> at the specified index.
        /// </summary>
        /// <value></value>
        public char this[int index]
        {
            get { return stringData[index]; }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="Map_Editor.Engine.zString"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ZString(string value)
        {
            return new ZString(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Map_Editor.Engine.zString"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator string(ZString value)
        {
            return value.stringData;
        }
    }

    #endregion

    /// <summary>
    /// FileHandler class.
    /// </summary>
    public class FileHandler
    {
        /// <summary>
        /// File mode.
        /// </summary>
        public enum FileOpenMode
        {
            /// <summary>
            /// Reading.
            /// </summary>
            Reading,

            /// <summary>
            /// Writing
            /// </summary>
            Writing,
        }

        #region Member Declarations

        /// <summary>
        /// Gets or sets the file stream.
        /// </summary>
        /// <value>The file stream.</value>
        private FileStream fileStream { get; set; }

        /// <summary>
        /// Gets or sets the memory stream.
        /// </summary>
        /// <value>The memory stream.</value>
        private MemoryStream memoryStream { get; set; }

        /// <summary>
        /// Gets or sets the binary reader.
        /// </summary>
        /// <value>The binary reader.</value>
        private BinaryReader binaryReader { get; set; }

        /// <summary>
        /// Gets or sets the binary writer.
        /// </summary>
        /// <value>The binary writer.</value>
        private BinaryWriter binaryWriter { get; set; }

        /// <summary>
        /// Gets or sets the file open mode.
        /// </summary>
        /// <value>The file open mode.</value>
        private FileOpenMode fileOpenMode { get; set; }

        /// <summary>
        /// Gets or sets the encoding.
        /// </summary>
        /// <value>The encoding.</value>
        private Encoding encoding { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="FileHandler"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="encodeType">Type of encoding.</param>
        public FileHandler(string filePath, FileOpenMode mode, Encoding encodeType)
        {
            encoding = encodeType;
            fileOpenMode = mode;

			Regex pathRegex = new Regex ("[\\\\/]");
			filePath = pathRegex.Replace(filePath, Path.DirectorySeparatorChar.ToString());

            if (fileOpenMode == FileOpenMode.Reading)
            {
                fileStream = System.IO.File.OpenRead(filePath);
                binaryReader = new BinaryReader(fileStream, encodeType ?? Encoding.Default);
            }
            else if (fileOpenMode == FileOpenMode.Writing)
            {
                fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
                binaryWriter = new BinaryWriter(fileStream, encodeType ?? Encoding.Default);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileHandler"/> class.
        /// </summary>
        /// <param name="asset">A text asset loaded from Resources</param>
        /// <param name="encodeType">Type of encoding.</param>
        public FileHandler(TextAsset asset, Encoding encodeType)
        {
            encoding = encodeType;
            memoryStream = new MemoryStream(asset.bytes);
            binaryReader = new BinaryReader(memoryStream, encodeType ?? Encoding.Default);
        }

        /// <summary>
        /// Seeks the specified offset.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="origin">The origin.</param>
        public void Seek(int offset, SeekOrigin origin)
        {
            if (memoryStream != null)
                memoryStream.Seek(offset, origin);
            else
                fileStream.Seek(offset, origin);
        }

        /// <summary>
        /// Get the stream position.
        /// </summary>
        /// <returns>Offset</returns>
        public int Tell()
        {
            if (memoryStream != null)
                return (int)memoryStream.Position;
            else
                return (int)fileStream.Position;
        }

        #region Reading

        /// <summary>
        /// Reads the specified length.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="length">The length.</param>
        /// <returns>The value.</returns>
        public T Read<T>(int length)
        {
            if (typeof(T) == typeof(byte[]))
                return (T)((object)binaryReader.ReadBytes(length));

            if (typeof(T) == typeof(string))
                return (T)((object)encoding.GetString(binaryReader.ReadBytes(length)));

            if (typeof(T) == typeof(BaseString))
                return (T)((object)new BaseString(new string(binaryReader.ReadChars(length))));

            if (typeof(T) == typeof(NString))
            {
                List<byte> dynamicBuffer = new List<byte>(length);
                byte currentByte = 0;

                for (int i = 0; i < length; i++)
                {
                    if ((currentByte = binaryReader.ReadByte()) != 0 && currentByte != 0xCD)
                        dynamicBuffer.Add(currentByte);
                }

                return (T)((object)new NString(encoding.GetString(dynamicBuffer.ToArray())));
            }

            throw new Exception("Invalid data type");
        }

        /// <summary>
        /// Reads a value.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns>The value.</returns>
        public T Read<T>()
        {
            if (typeof(T) == typeof(byte))
                return (T)((object)binaryReader.ReadByte());

            if (typeof(T) == typeof(sbyte))
                return (T)((object)binaryReader.ReadSByte());

            if (typeof(T) == typeof(char))
                return (T)((object)binaryReader.ReadChar());

            if (typeof(T) == typeof(short))
                return (T)((object)binaryReader.ReadInt16());

            if (typeof(T) == typeof(ushort))
                return (T)((object)binaryReader.ReadUInt16());

            if (typeof(T) == typeof(int))
                return (T)((object)binaryReader.ReadInt32());

            if (typeof(T) == typeof(uint))
                return (T)((object)binaryReader.ReadUInt32());

            if (typeof(T) == typeof(long))
                return (T)((object)binaryReader.ReadInt64());

            if (typeof(T) == typeof(ulong))
                return (T)((object)binaryReader.ReadUInt64());

            if (typeof(T) == typeof(float))
                return (T)((object)binaryReader.ReadSingle());

            if (typeof(T) == typeof(double))
                return (T)((object)binaryReader.ReadDouble());

            if (typeof(T) == typeof(decimal))
                return (T)((object)binaryReader.ReadDecimal());

            if (typeof(T) == typeof(Vector3))
            {
                return (T)((object)new Vector3()
                {
                    x = binaryReader.ReadSingle(),
                    y = binaryReader.ReadSingle(),
                    z = binaryReader.ReadSingle()
                });
            }

            if (typeof(T) == typeof(Quaternion))
            {
                return (T)((object)new Quaternion()
                {
                    x = binaryReader.ReadSingle(),
                    y = binaryReader.ReadSingle(),
                    z = binaryReader.ReadSingle(),
                    w = binaryReader.ReadSingle()
                });
            }

            if (typeof(T) == typeof(BString))
                return (T)((object)((BString)binaryReader.ReadString()));

            if (typeof(T) == typeof(ZString))
            {
                ZString returnString = string.Empty;

                while (true)
                {
                    char character = binaryReader.ReadChar();

                    if (character == 0)
                        break;

                    returnString += character;
                }

                return (T)((object)(returnString));
            }

            GCHandle handle = GCHandle.Alloc(binaryReader.ReadBytes(Marshal.SizeOf(typeof(T))), GCHandleType.Pinned);
            T value;

            try
            {
                value = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                handle.Free();
            }

            return value;
        }

        #endregion

        #region Writing

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="length">The length.</param>
        public void Write<T>(T value, int length)
        {
            if (typeof(T) == typeof(NString))
            {
                NString nString = (NString)((object)value);

                byte[] stringData = encoding.GetBytes(nString);
                byte[] data = new byte[length];

                for (int i = 0; i < stringData.Length; i++)
                    data[i] = stringData[i];

                binaryWriter.Write(data);
            }
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="value">The value.</param>
        public void Write<T>(T value)
        {
            if (typeof(T) == typeof(string))
                binaryWriter.Write(encoding.GetBytes((string)((object)value)));
            else if (typeof(T) == typeof(BString))
                binaryWriter.Write((string)((BString)((object)value)));
            else if (typeof(T) == typeof(BaseString))
                binaryWriter.Write(((BaseString)((object)value)).ToCharArray());
            else if (typeof(T) == typeof(byte))
                binaryWriter.Write((byte)((object)value));
            else if (typeof(T) == typeof(byte[]))
                binaryWriter.Write((byte[])((object)value));
            else if (typeof(T) == typeof(sbyte))
                binaryWriter.Write((sbyte)((object)value));
            else if (typeof(T) == typeof(char))
                binaryWriter.Write((char)((object)value));
            else if (typeof(T) == typeof(short))
                binaryWriter.Write((short)((object)value));
            else if (typeof(T) == typeof(ushort))
                binaryWriter.Write((ushort)((object)value));
            else if (typeof(T) == typeof(int))
                binaryWriter.Write((int)((object)value));
            else if (typeof(T) == typeof(uint))
                binaryWriter.Write((uint)((object)value));
            else if (typeof(T) == typeof(long))
                binaryWriter.Write((long)((object)value));
            else if (typeof(T) == typeof(ulong))
                binaryWriter.Write((ulong)((object)value));
            else if (typeof(T) == typeof(float))
                binaryWriter.Write((float)((object)value));
            else if (typeof(T) == typeof(double))
                binaryWriter.Write((double)((object)value));
            else if (typeof(T) == typeof(decimal))
                binaryWriter.Write((decimal)((object)value));
            else if (typeof(T) == typeof(Vector2))
            {
                binaryWriter.Write(((Vector2)((object)value)).x);
                binaryWriter.Write(((Vector2)((object)value)).y);
            }
            else if (typeof(T) == typeof(Vector3))
            {
                binaryWriter.Write(((Vector3)((object)value)).x);
                binaryWriter.Write(((Vector3)((object)value)).y);
                binaryWriter.Write(((Vector3)((object)value)).z);
            }
            else if (typeof(T) == typeof(Quaternion))
            {
                binaryWriter.Write(((Quaternion)((object)value)).x);
                binaryWriter.Write(((Quaternion)((object)value)).y);
                binaryWriter.Write(((Quaternion)((object)value)).z);
                binaryWriter.Write(((Quaternion)((object)value)).w);
            }
            else
            {
                int unmanagedSize = Marshal.SizeOf(typeof(T));
                byte[] buffer = new byte[unmanagedSize];
                GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);

                try
                {
                    Marshal.StructureToPtr(value, handle.AddrOfPinnedObject(), false);
                    Marshal.Copy(handle.AddrOfPinnedObject(), buffer, 0, unmanagedSize);
                }
                finally
                {
                    handle.Free();
                }

                binaryWriter.Write(buffer);
            }
        }

        #endregion

        /// <summary>
        /// Closes the file stream.
        /// </summary>
        public void Close()
        {
			if( fileStream != null )
            	fileStream.Close();
			if (memoryStream != null)
				memoryStream.Close();
        }
    }
}