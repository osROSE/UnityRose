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

namespace Revise.Files.LOD {
    /// <summary>
    /// Provides the ability to create, open and save LOD files for level of detail data.
    /// </summary>
    public class LevelOfDetailFile : FileLoader {
        #region Constants

        private const int DEFAULT_WIDTH = 31;
        private const int DEFAULT_HEIGHT = 31;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the width dimension of the heightmap.
        /// </summary>
        public int Width {
            get {
                return values.GetLength(1);
            }
        }

        /// <summary>
        /// Gets the height dimension of the heightmap.
        /// </summary>
        public int Height {
            get {
                return values.GetLength(0);
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name {
            get;
            set;
        }

        #endregion

        private int[,] values;

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelOfDetailFile"/> class.
        /// </summary>
        public LevelOfDetailFile() {
            Reset();
        }

        /// <summary>
        /// Gets the height value of the specified coordinates.
        /// </summary>
        public int this[int x, int y] {
            get {
                return values[x, y];
            }
        }

        /// <summary>
        /// Loads the file from the specified stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        public override void Load(Stream stream) {
            BinaryReader reader = new BinaryReader(stream, Encoding.GetEncoding("EUC-KR"));

            Name = reader.ReadNullTerminatedString();

            for (int h = 0; h < DEFAULT_HEIGHT; h++) {
                for (int w = 0; w < DEFAULT_WIDTH; w++) {
                    values[h, w] = reader.ReadInt32();
                }
            }
        }

        /// <summary>
        /// Saves the file to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to save to.</param>
        public override void Save(Stream stream) {
            BinaryWriter writer = new BinaryWriter(stream, Encoding.GetEncoding("EUC-KR"));

            writer.WriteString(Name);
            writer.Write((byte)0);

            for (int h = 0; h < Height; h++) {
                for (int w = 0; w < Width; w++) {
                    writer.Write(values[h, w]);
                }
            }
        }

        /// <summary>
        /// Resets properties to their default values.
        /// </summary>
        public override void Reset() {
            base.Reset();

            Name = string.Empty;
            values = new int[DEFAULT_HEIGHT, DEFAULT_WIDTH];
        }
    }
}