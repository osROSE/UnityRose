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

namespace Revise.Files.IFO.Blocks {
    /// <summary>
    /// Represents a map sound.
    /// </summary>
    public class MapSound : MapBlock {
        #region Properties

        /// <summary>
        /// Gets or sets the file path of the sound.
        /// </summary>
        public string FilePath {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the range of the sound.
        /// </summary>
        public int Range {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the interval to play the sound in seconds.
        /// </summary>
        public int Interval {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MapSound"/> class.
        /// </summary>
        public MapSound() {
            FilePath = string.Empty;
        }

        /// <summary>
        /// Reads the block data from the underlying stream.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public override void Read(BinaryReader reader) {
            base.Read(reader);

            FilePath = reader.ReadByteString();
            Range = reader.ReadInt32();
            Interval = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the block data to the underlying stream.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public override void Write(BinaryWriter writer) {
            base.Write(writer);

            writer.WriteByteString(FilePath);
            writer.Write(Range);
            writer.Write(Interval);
        }
    }
}