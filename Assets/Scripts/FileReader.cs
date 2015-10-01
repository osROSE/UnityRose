// <copyright file="FileReader.cs" company="Wadii Bellamine">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Wadii Bellamine</author>
// <date>2/25/2015 8:37 AM </date>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace UnityRose.File
{
    public class FileReader : BinaryReader
    {
        /// <summary>
        /// Gets the filename of the file that's being read.
        /// </summary>
        public string filename { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileReader"/> class
        /// </summary>
        /// <param name="filename">The Filename of the file to be read</param> 
        public FileReader(string filename) : base(System.IO.File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            this.filename = filename;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileReader"/> class
        /// </summary>
        /// <param name="filestream">The generic Stream to be read</param>
        public FileReader(Stream stream) : base(stream)
        {
            this.filename = string.Empty;
        }

        /// <summary>
        /// Generic function to read binary data.
        /// </summary>
        /// <typeparam name="T">Variable Type.</typeparam>
        /// <returns>Single variable of binary data.</returns>
        public T Read<T>()
        {
            T value = default(T);

            Type valueType = typeof(T);

            try
            {
                if (valueType == typeof(short))
                {
                    value = (T)(object)ReadInt16();
                }
                else if (valueType == typeof(int))
                {
                    value = (T)(object)ReadInt32();
                }
                else if (valueType == typeof(long))
                {
                    value = (T)(object)ReadInt64();
                }
                else if (valueType == typeof(ushort))
                {
                    value = (T)(object)ReadUInt16();
                }
                else if (valueType == typeof(uint))
                {
                    value = (T)(object)ReadUInt32();
                }
                else if (valueType == typeof(ulong))
                {
                    value = (T)(object)ReadUInt64();
                }
                else if (valueType == typeof(bool))
                {
                    value = (T)(object)ReadBoolean();
                }
                else if (valueType == typeof(float))
                {
                    value = (T)(object)ReadSingle();
                }
                else if (valueType == typeof(decimal))
                {
                    value = (T)(object)ReadDecimal();
                }
                else if (valueType == typeof(double))
                {
                    value = (T)(object)ReadDouble();
                }
                else if (valueType == typeof(char))
                {
                    value = (T)(object)ReadChar();
                }
                else if (valueType == typeof(byte))
                {
                    value = (T)(object)ReadByte();
                }
                else
                {
                    throw new IOException("Read<T>() : Unkown Type!");
                }
            }
            catch (IOException ex)
            {
                
            }

            return value;
        }

        /// <summary>
        /// Generic function to read a array of binary data.
        /// </summary>
        /// <typeparam name="T">Array Type.</typeparam>
        /// <param name="size">Size of Array.</param>
        /// <returns>Array of binary data.</returns>
        public T Read<T>(int size)
        {
            T value = default(T);

            Type valueType = typeof(T);

            try
            {
                if (valueType == typeof(byte[]))
                {
                    value = (T)(object)ReadBytes(size);
                }
                else if (valueType == typeof(char[]))
                {
                    value = (T)(object)ReadChars(size);
                }
                else
                {
                    throw new IOException("Read<T>(int size) : Unkown Type!");
                }
            }
            catch (IOException ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message, "IOException");
            }

            return value;
        }

        /// <summary>
        /// Reads a string of characters.
        /// </summary>
        /// <param name="size">Size of the string.</param>
        /// <returns>The string</returns>
        public string ReadString(int size)
        {
            return Encoding.ASCII.GetString(ReadBytes(size));
        }

        public string ReadUnicode(int size)
        {
            return Encoding.Unicode.GetString(ReadBytes(size));
        }

        public string ReadUTF7(int size)
        {
            return Encoding.UTF7.GetString(ReadBytes(size));
        }

        public string ReadUTF8(int size)
        {
            return Encoding.UTF8.GetString(ReadBytes(size));
        }

        public string ReadUTF32(int size)
        {
            return Encoding.UTF32.GetString(ReadBytes(size));
        }

        public string ReadKorean(int size)
        {
            return Encoding.UTF8.GetString(ReadBytes(size));
        }

        /// <summary>
        /// Reads a structure C/C++ Style.
        /// </summary>
        /// <typeparam name="T">Structure Type.</typeparam>
        /// <returns></returns>
        public T ReadStruct<T>()
        {
            T value = default(T);

            byte[] buffer = ReadBytes(Marshal.SizeOf(typeof(T)));

            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            
            value = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));

            handle.Free();

            return value;
        }
    }
}
