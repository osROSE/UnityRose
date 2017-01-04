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

namespace Revise.Files.MOV {
    /// <summary>
    /// Provides the ability to create, open and save MOV files for movement restriction.
    /// </summary>
    public class MovementFile : FileLoader {
        #region Constants

        private const int DEFAULT_WIDTH = 32;
        private const int DEFAULT_HEIGHT = 32;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the width dimension of the tile map.
        /// </summary>
        public int Width {
            get {
                return moveability.GetLength(1);
            }
        }

        /// <summary>
        /// Gets the height dimension of the tile map.
        /// </summary>
        public int Height {
            get {
                return moveability.GetLength(0);
            }
        }

        #endregion

        private bool[,] moveability;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovementFile"/> class.
        /// </summary>
        public MovementFile() {
            Reset();
        }

        /// <summary>
        /// Gets the moveability of the specified coordinates.
        /// </summary>
        public bool this[int x, int y] {
            get {
                return moveability[x, y];
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

            moveability = new bool[height, width];

            for (int h = 0; h < height; h++) {
                for (int w = 0; w < width; w++) {
                    moveability[h, w] = reader.ReadBoolean();
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

            for (int h = 0; h < Height; h++) {
                for (int w = 0; w < Width; w++) {
                    writer.Write(moveability[h, w]);
                }
            }
        }

        /// <summary>
        /// Resets properties to their default values.
        /// </summary>
        public override void Reset() {
            base.Reset();

            moveability = new bool[DEFAULT_HEIGHT, DEFAULT_WIDTH];
        }
    }
}