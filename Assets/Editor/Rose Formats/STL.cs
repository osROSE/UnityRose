// <copyright file="STL.cs" company="Wadii Bellamine">
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
    /// STL class.
    /// </summary>
    public class STL
    {
        #region Sub Classes

        /// <summary>
        /// Entry class.
        /// </summary>
        public class Entry
        {
            #region Member Declarations

            /// <summary>
            /// Gets or sets the string ID.
            /// </summary>
            /// <value>The string ID.</value>
            public string StringID { get; set; }

            /// <summary>
            /// Gets or sets the ID.
            /// </summary>
            /// <value>The ID.</value>
            public int ID { get; set; }

            #endregion
        }

        /// <summary>
        /// Row class.
        /// </summary>
        public class Row
        {
            #region Member Declarations

            /// <summary>
            /// Gets or sets the text.
            /// </summary>
            /// <value>The text.</value>
            public string Text { get; set; }

            /// <summary>
            /// Gets or sets the comment.
            /// </summary>
            /// <value>The comment.</value>
            public string Comment { get; set; }

            /// <summary>
            /// Gets or sets the quest1.
            /// </summary>
            /// <value>The quest1.</value>
            public string Quest1 { get; set; }

            /// <summary>
            /// Gets or sets the quest2.
            /// </summary>
            /// <value>The quest2.</value>
            public string Quest2 { get; set; }

            #endregion
        }

        #endregion

        #region Member Declarations

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the entries.
        /// </summary>
        /// <value>The entries.</value>
        public List<Entry> Entries { get; set; }

        /// <summary>
        /// Gets or sets the rows.
        /// </summary>
        /// <value>The rows.</value>
        public List<List<Row>> Rows { get; set; }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>The file path.</value>
        public string FilePath { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="STL"/> class.
        /// </summary>
        public STL()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="STL"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public STL(string filePath)
        {
            Load(filePath);
        }

        /// <summary>
        /// Loads the specified file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public void Load(string filePath)
        {
            FileHandler fh = new FileHandler(FilePath = filePath, FileHandler.FileOpenMode.Reading, Encoding.UTF8);

            Type = fh.Read<BString>();

            int entryCount = fh.Read<int>();

            Entries = new List<Entry>(entryCount);

            for (int i = 0; i < entryCount; i++)
            {
                Entries.Add(new Entry()
                {
                    StringID = fh.Read<BString>(),
                    ID = fh.Read<int>()
                });
            }

            int languageCount = fh.Read<int>();
            int[] languageOffsets = new int[languageCount];

            for (int i = 0; i < languageCount; i++)
                languageOffsets[i] = fh.Read<int>();

            int[,] entryOffsets = new int[languageCount, entryCount];

            for (int i = 0; i < languageCount; i++)
            {
                fh.Seek(languageOffsets[i], SeekOrigin.Begin);

                for (int j = 0; j < entryCount; j++)
                    entryOffsets[i, j] = fh.Read<int>();
            }

            Rows = new List<List<Row>>(languageCount);

            for (int i = 0; i < languageCount; i++)
            {
                Rows.Add(new List<Row>(entryCount));

                for (int j = 0; j < entryCount; j++)
                {
                    Rows[i].Add(new Row());

                    fh.Seek(entryOffsets[i, j], System.IO.SeekOrigin.Begin);

                    Rows[i][j].Text = fh.Read<BString>();

                    if (Type == "QEST01" || Type == "ITST01")
                    {
                        Rows[i][j].Comment = fh.Read<BString>();

                        if (Type == "QEST01")
                        {
                            Rows[i][j].Quest1 = fh.Read<BString>();
                            Rows[i][j].Quest2 = fh.Read<BString>();
                        }
                    }
                }
            }

            fh.Close();
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
            FileHandler fh = new FileHandler(FilePath = filePath, FileHandler.FileOpenMode.Writing, Encoding.UTF8);

            fh.Write<BString>(Type);

            fh.Write<int>(Entries.Count);

            Entries.ForEach(delegate(Entry entry)
            {
                fh.Write<BString>(entry.StringID);
                fh.Write<int>(entry.ID);
            });

            fh.Write<int>(Rows.Count);

            int langaugeOffset = fh.Tell();

            fh.Write<byte[]>(new byte[Rows.Count * sizeof(int)]);

            int[] languageOffsets = new int[Rows.Count];

            for (int i = 0; i < Rows.Count; i++)
            {
                languageOffsets[i] = fh.Tell();

                fh.Write<byte[]>(new byte[Entries.Count * sizeof(int)]);
            }

            for (int i = 0; i < Rows.Count; i++)
            {
                int[] entryOffsets = new int[Entries.Count];

                for (int j = 0; j < Entries.Count; j++)
                {
                    entryOffsets[j] = fh.Tell();

                    fh.Write<BString>(Rows[i][j].Text);

                    if (Type == "QEST01" || Type == "ITST01")
                    {
                        fh.Write<BString>(Rows[i][j].Comment);

                        if (Type == "QEST01")
                        {
                            fh.Write<BString>(Rows[i][j].Quest1);
                            fh.Write<BString>(Rows[i][j].Quest2);
                        }
                    }
                }

                int rowOffset = fh.Tell();

                fh.Seek(languageOffsets[i], SeekOrigin.Begin);

                for (int j = 0; j < Entries.Count; j++)
                    fh.Write<int>(entryOffsets[j]);

                fh.Seek(rowOffset, SeekOrigin.Begin);
            }
            
            fh.Seek(langaugeOffset, SeekOrigin.Begin);

            for (int i = 0; i < Rows.Count; i++)
                fh.Write<int>(languageOffsets[i]);

            fh.Close();
        }
        
        /// <summary>
        /// Searches for the specified string ID.
        /// </summary>
        /// <param name="stringID">The string ID.</param>
        /// <returns>The result or if nothing is found, the string ID.</returns>
        public string Search(string stringID)
        {
            for (int i = 0; i < Entries.Count; i++)
            {
                if (string.Compare(Entries[i].StringID, stringID, true) == 0)
                    return Rows[1][i].Text;
            }

            return stringID;
        }
    }
}