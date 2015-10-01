// <copyright file="STB.cs" company="Wadii Bellamine">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Wadii Bellamine, Xadet, Brett19</author>
// <date>2/25/2015 8:37 AM </date>

using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityRose.File;

namespace UnityRose.Formats
{
    /// <summary>
    /// STB class.
    /// </summary>
    public class STB
    {
        #region Member Declarations

        /// <summary>
        /// Gets or sets the size of the row.
        /// </summary>
        /// <value>The size of the row.</value>
        public int RowSize { get; set; }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>The file path.</value>
        public string FilePath { get; set; }

        #endregion

        #region List Declarations

        /// <summary>
        /// Gets or sets the column sizes.
        /// </summary>
        /// <value>The column sizes.</value>
        public List<short> ColumnSizes { get; set; }

        /// <summary>
        /// Gets or sets the column names.
        /// </summary>
        /// <value>The column names.</value>
        public List<string> ColumnNames { get; set; }

        /// <summary>
        /// Gets or sets the cells.
        /// </summary>
        /// <value>The cells.</value>
        public List<List<string>> Cells { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="STB"/> class.
        /// </summary>
        public STB()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="STB"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public STB(string filePath)
        {
            Load(filePath);
        }

        private TextAsset asset;
        private FileHandler fh;

        /// <summary>
        /// Loads the specified file.  If resource is found, it is loaded as a Text asset.  Otherwise, the function
        /// assumes this is an editor load and reads from disk
        /// </summary>
        /// <param name="filePath">The file path of the Text Asset resource (without extension) or file (with extension) to load</param>
        public void Load(string filePath)
        {
            asset = Resources.Load(filePath) as TextAsset;
            if (asset != null)
                fh = new FileHandler(asset, Encoding.UTF8);
            else
                fh = new FileHandler(filePath, FileHandler.FileOpenMode.Reading, Encoding.UTF8);

            Load();
        }

        /// <summary>
        /// Loads the specified file.
        /// </summary>
        private void Load()
        {
            
            fh.Read<BaseString>(4);

            int offset = fh.Read<int>();

            int rowCount = fh.Read<int>();
            int columnCount = fh.Read<int>();
            RowSize = fh.Read<int>();

            ColumnSizes = new List<short>(columnCount + 1);

            for (int i = 0; i < columnCount + 1; i++)
                ColumnSizes.Add(fh.Read<short>());

            ColumnNames = new List<string>(columnCount + 1);

            for (int i = 0; i < columnCount + 1; i++)
                ColumnNames.Add(fh.Read<string>(fh.Read<short>()));

            Cells = new List<List<string>>(rowCount);

            for (int i = 0; i < rowCount - 1; i++)
            {
                Cells.Add(new List<string>());

                Cells[i].Add(fh.Read<string>(fh.Read<short>()));
            }

            for (int i = 0; i < rowCount - 1; i++)
            {
                for (int j = 0; j < columnCount - 1; j++)
                    Cells[i].Add(fh.Read<string>(fh.Read<short>()));
            }

            fh.Close();

            if (asset != null)
                Resources.UnloadAsset(asset);
        }

        /// <summary>
        /// Saves the file.
        /// </summary>
        public void Save()
        {
            Save(FilePath);
        }

        /// <summary>
        /// Saves the specified file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public void Save(string filePath)
        {
            Encoding encoding = Encoding.UTF8;

            FileHandler fh = new FileHandler(FilePath = filePath, FileHandler.FileOpenMode.Writing, encoding);

            fh.Write<BaseString>("STB1");
            fh.Write<int>(0);

            int rowCount = Cells.Count;
            int columnCount = (Cells.Count > 0) ? Cells[0].Count : 0;

            fh.Write<int>(rowCount + 1);
            fh.Write<int>(columnCount);
            fh.Write<int>(RowSize);

            for (int i = 0; i < ColumnSizes.Count; i++)
                fh.Write<short>((short)ColumnSizes[i]);

            for (int i = 0; i < ColumnNames.Count; i++)
            {
                fh.Write<short>((short)encoding.GetByteCount(ColumnNames[i]));
                fh.Write<string>(ColumnNames[i]);
            }

            for (int i = 0; i < rowCount; i++)
            {
                fh.Write<short>((short)encoding.GetByteCount(Cells[i][0]));
                fh.Write<string>(Cells[i][0]);
            }

            int dataOffset = fh.Tell();

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 1; j < columnCount; j++)
                {
                    fh.Write<short>((short)encoding.GetByteCount(Cells[i][j]));
                    fh.Write<string>(Cells[i][j]);
                }
            }

            fh.Seek(4, SeekOrigin.Begin);
            fh.Write<int>(dataOffset);

            fh.Close();
        }
    }
}