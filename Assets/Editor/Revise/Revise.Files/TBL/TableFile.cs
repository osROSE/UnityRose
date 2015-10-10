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

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Revise.Files.TBL {
    /// <summary>
    /// Provides the ability to create, open and save TBL files.
    /// </summary>
    public class TableFile : FileLoader {
        #region Properties

        /// <summary>
        /// Gets the maximum range.
        /// </summary>
        public short MaximumRange {
            get {
                return maximumRange;
            }
            set {
                maximumRange = value;

                Array.Resize<short>(ref startIndices, value);
                Array.Resize<short>(ref indexCounts, value);
            }
        }

        /// <summary>
        /// Gets the start indices.
        /// </summary>
        public short[] StartIndices {
            get {
                return startIndices;
            }
        }

        /// <summary>
        /// Gets the index counts.
        /// </summary>
        public short[] IndexCounts {
            get {
                return indexCounts;
            }
        }

        /// <summary>
        /// Gets the points.
        /// </summary>
        public List<ShortVector2> Points {
            get;
            private set;
        }

        #endregion

        private short maximumRange;
        private short[] startIndices;
        private short[] indexCounts;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableFile"/> class.
        /// </summary>
        public TableFile() {
            Points = new List<ShortVector2>();

            Reset();
        }

        /// <summary>
        /// Loads the file from the specified stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        public override void Load(Stream stream) {
            BinaryReader reader = new BinaryReader(stream, Encoding.GetEncoding("EUC-KR"));

            MaximumRange = reader.ReadInt16();

            for (int i = 0; i < maximumRange; i++) {
                startIndices[i] = reader.ReadInt16();
                indexCounts[i] = reader.ReadInt16();
            }

            int maximumArray = reader.ReadInt16();

            for (int i = 0; i < maximumArray; i++) {
                ShortVector2 point = new ShortVector2();
                point.X = reader.ReadInt16();
                point.Y = reader.ReadInt16();

                Points.Add(point);
            }
        }

        /// <summary>
        /// Saves the file to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to save to.</param>
        public override void Save(Stream stream) {
            BinaryWriter writer = new BinaryWriter(stream, Encoding.GetEncoding("EUC-KR"));

            writer.Write(maximumRange);

            for (int i = 0; i < maximumRange; i++) {
                writer.Write(startIndices[i]);
                writer.Write(indexCounts[i]);
            }

            writer.Write((short)Points.Count);

            Points.ForEach(point => {
                writer.Write(point.X);
                writer.Write(point.Y);
            });
        }

        /// <summary>
        /// Resets properties to their default values.
        /// </summary>
        public override void Reset() {
            base.Reset();

            maximumRange = 0;
            startIndices = new short[0];
            indexCounts = new short[0];

            Points.Clear();
        }
    }
}