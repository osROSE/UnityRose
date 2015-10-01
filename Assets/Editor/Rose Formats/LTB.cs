// <copyright file="LTB.cs" company="Wadii Bellamine">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Wadii Bellamine, Xadet, Brett19</author>
// <date>2/25/2015 8:37 AM </date>

using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityRose.File;

namespace UnityRose.Formats
{
    /// <summary>
    /// LTB class.
    /// </summary>
    public class LTB
    {
        #region Member Declarations

        /// <summary>
        /// Gets or sets the cells.
        /// </summary>
        /// <value>The cells.</value>
        public List<List<string>> Cells { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="LTB"/> class.
        /// </summary>
        public LTB()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LTB"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public LTB(string filePath)
        {
            Load(filePath);
        }

        /// <summary>
        /// Loads the specified file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public void Load(string filePath)
        {
            FileHandler fh = new FileHandler(filePath, FileHandler.FileOpenMode.Reading, Encoding.GetEncoding("Unicode"));

            int columnCount = fh.Read<int>();
            int rowCount = fh.Read<int>();

            int[,] offsets = new int[rowCount, columnCount];
            short[,] lengths = new short[rowCount, columnCount];

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    offsets[i, j] = fh.Read<int>();
                    lengths[i, j] = fh.Read<short>();
                }
            }

            Cells = new List<List<string>>(rowCount);

            for (int i = 0; i < rowCount; i++)
            {
                Cells.Add(new List<string>(columnCount));

                for (int j = 0; j < columnCount; j++)
                {
                    fh.Seek(offsets[i, j], SeekOrigin.Begin);

                    Cells[i].Add(fh.Read<string>(lengths[i, j] * 2));
                }
            }

            fh.Close();
        }

        /// <summary>
        /// Saves the specified file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public void Save(string filePath)
        {
            FileHandler fh = new FileHandler(filePath, FileHandler.FileOpenMode.Writing, Encoding.GetEncoding("Unicode"));

            int columnCount = (Cells.Count > 0) ? Cells[0].Count : 0;

            fh.Write<int>(columnCount);
            fh.Write<int>(Cells.Count);

            int[,] offsets = new int[Cells.Count, columnCount];

            for (int i = 0; i < Cells.Count; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    offsets[i, j] = fh.Tell();

                    fh.Write<int>(0);
                    fh.Write<short>((short)Cells[i][j].Length);
                }
            }

            for (int i = 0; i < Cells.Count; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    int position = fh.Tell();

                    fh.Seek(offsets[i, j], System.IO.SeekOrigin.Begin);

                    fh.Write<int>(position);

                    fh.Seek(position, System.IO.SeekOrigin.Begin);

                    fh.Write<string>(Cells[i][j]);
                    fh.Write<byte>(0);
                }
            }

            fh.Close();
        }
    }
}
