// <copyright file="FileWriter.cs" company="Wadii Bellamine">
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
using System.Security.Permissions;

namespace UnityRose.File
{
    public class FileWriter : BinaryWriter
    {
        public string filename { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref"FileWriter"/> class.
        /// </summary>
        /// <param name="filename">The filename of the file to be writen</param>
        public FileWriter(string filename) : base(System.IO.File.Open(filename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
        {
            this.filename = filename;
        }

        /// <summary>
        /// Writes a character array.
        /// </summary>
        /// <param name="value">Character Arrray.</param>
        public void WriteString(char[] value)
        {
            Write(Encoding.ASCII.GetBytes(value));
        }

        public void WriteString(string value, int size)
        {
            char[] buffer = new char[size];

            Array.Copy(value.ToCharArray(), buffer, value.Length);

            WriteString(buffer);
        }

        public void WriteUnicode(string value, int size)
        {
            byte[] unicode = Encoding.Unicode.GetBytes(value);

            char[] buffer = new char[size];

            if(unicode.Length > size)
                Array.Copy(unicode, buffer, size);
            else
                Array.Copy(unicode, buffer, unicode.Length);

            WriteString(buffer);
        }

        public void WriteKorean(string value, int size)
        {
            byte[] data = Encoding.UTF8.GetBytes(value);

            byte[] buffer = new byte[size];

            for (int i = 0; i < size; ++i)
                buffer[i] = data[i];

            Write(buffer);
        }

        /// <summary>
        /// Writes a structure C/C++ Style.
        /// </summary>
        /// <param name="value">Structure</param>
        public void WriteStructure(object value)
        {
            byte[] buffer = new byte[Marshal.SizeOf(value)];

            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);

            Marshal.StructureToPtr(value, handle.AddrOfPinnedObject(), false);

            handle.Free();

            Write(buffer);
        }
    }
}
