// <copyright file="TIL.cs" company="Wadii Bellamine">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Wadii Bellamine, Xadet, Brett19</author>
// <date>2/25/2015 8:37 AM </date>

using System.Text;
using UnityRose.File;

namespace UnityRose.Formats
{
    /// <summary>
    /// TIL class.
    /// </summary>
    public class TIL
    {
        /// <summary>
        /// Tile structure.
        /// </summary>
        public struct Tile
        {
            #region Member Declarations

            /// <summary>
            /// Gets or sets the brush ID.
            /// </summary>
            /// <value>The brush ID.</value>
            public byte BrushID { get; set; }

            /// <summary>
            /// Gets or sets the index of the tile.
            /// </summary>
            /// <value>The index of the tile.</value>
            public byte TileIndex { get; set; }

            /// <summary>
            /// Gets or sets the tile set number.
            /// </summary>
            /// <value>The tile set number.</value>
            public byte TileSetNumber { get; set; }

            /// <summary>
            /// Gets or sets the tile ID.
            /// </summary>
            /// <value>The tile ID.</value>
            public int TileID { get; set; }

            #endregion
        };

        #region Member Declarations

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>The file path.</value>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the tiles.
        /// </summary>
        /// <value>The tiles.</value>
        public Tile[,] Tiles { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="TIL"/> class.
        /// </summary>
        public TIL()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TIL"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public TIL(string filePath)
        {
            Load(filePath);
        }

        /// <summary>
        /// Loads the specified file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public void Load(string filePath)
        {
            FileHandler fh = new FileHandler(FilePath = filePath, FileHandler.FileOpenMode.Reading, null);

            int width = fh.Read<int>();
            int height = fh.Read<int>();

            Tiles = new Tile[height, width];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Tiles[y, x] = new Tile()
                    {
                        BrushID = fh.Read<byte>(),
                        TileIndex = fh.Read<byte>(),
                        TileSetNumber = fh.Read<byte>(),
                        TileID = fh.Read<int>()
                    };
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
        /// Saves the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public void Save(string filePath)
        {
            FileHandler fh = new FileHandler(filePath, FileHandler.FileOpenMode.Writing, null);

            fh.Write<int>(16);
            fh.Write<int>(16);

            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    fh.Write<byte>(Tiles[y, x].BrushID);
                    fh.Write<byte>(Tiles[y, x].TileIndex);
                    fh.Write<byte>(Tiles[y, x].TileSetNumber);
                    fh.Write<int>(Tiles[y, x].TileID);
                }
            }

            fh.Close();
        }
    }
}