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

namespace Revise.Files.TIL {
    /// <summary>
    /// Provides the ability to create, open and save TIL files.
    /// </summary>
    public class TileFile : FileLoader {
        #region Constants

        private const int DEFAULT_WIDTH = 16;
        private const int DEFAULT_HEIGHT = 16;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the width dimension of the tile map.
        /// </summary>
        public int Width {
            get {
                return tiles.GetLength(1);
            }
        }

        /// <summary>
        /// Gets the height dimension of the tile map.
        /// </summary>
        public int Height {
            get {
                return tiles.GetLength(0);
            }
        }

        #endregion

        private TilePatch[,] tiles;

        /// <summary>
        /// Initializes a new instance of the <see cref="TileFile"/> class.
        /// </summary>
        public TileFile() {
            Reset();
        }

        /// <summary>
        /// Gets the tile of the specified coordinates.
        /// </summary>
        public TilePatch this[int x, int y] {
            get {
                return tiles[x, y];
            }
        }

        /// <summary>
        /// Loads the file from the specified stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        public override void Load(Stream stream) {
            BinaryReader reader = new BinaryReader(stream, Encoding.GetEncoding("EUC-KR"));

            int width = reader.ReadInt32();
            int height = reader.ReadInt32();

            tiles = new TilePatch[height, width];

            for (int h = height - 1; h >= 0; h--) {
                for (int w = 0; w < width; w++) {
                    TilePatch tile = new TilePatch();
                    tile.Brush = reader.ReadByte();
                    tile.TileIndex = reader.ReadByte();
                    tile.TileSet = reader.ReadByte();
                    tile.Tile = reader.ReadInt32();

                    tiles[h, w] = tile;
                }
            }
        }

        /// <summary>
        /// Saves the file to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to save to.</param>
        public override void Save(Stream stream) {
            BinaryWriter writer = new BinaryWriter(stream, Encoding.GetEncoding("EUC-KR"));

            writer.Write(Width);
            writer.Write(Height);

            for (int h = Height - 1; h >= 0; h--) {
                for (int w = 0; w < Width; w++) {
                    TilePatch tile = tiles[h, w];
                    writer.Write(tile.Brush);
                    writer.Write(tile.TileIndex);
                    writer.Write(tile.TileSet);
                    writer.Write(tile.Tile);
                }
            }
        }

        /// <summary>
        /// Resets properties to their default values.
        /// </summary>
        public override void Reset() {
            base.Reset();

            tiles = new TilePatch[DEFAULT_HEIGHT, DEFAULT_WIDTH];
        }
    }
}