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

namespace Revise.Files.LTB {
    /// <summary>
    /// Provides the ability to create, open and save LTB files for language ata.
    /// </summary>
    public class LanguageFile : FileLoader {
        #region Properties

        /// <summary>
        /// Gets the column count.
        /// </summary>
        public int ColumnCount {
            get;
            private set;
        }

        /// <summary>
        /// Gets the row count.
        /// </summary>
        public int RowCount {
            get {
                return rows.Count;
            }
        }

        #endregion

        private List<LanguageRow> rows;

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageFile"/> class.
        /// </summary>
        public LanguageFile() {
            rows = new List<LanguageRow>();

            Reset();
        }

        /// <summary>
        /// Gets the specified <see cref="LanguageRow"/>.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the specified row does not exist.</exception>
        public LanguageRow this[int row] {
            get {
                if (row < 0 || row > rows.Count - 1) {
                    throw new ArgumentOutOfRangeException("row", "Row is out of range");
                }

                return rows[row];
            }
        }

        /// <summary>
        /// Loads the file from the specified stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        public override void Load(Stream stream) {
            BinaryReader reader = new BinaryReader(stream, Encoding.Unicode);

            ColumnCount = reader.ReadInt32();
            int rowCount = reader.ReadInt32();

            int[,] offsets = new int[rowCount, ColumnCount];
            short[,] lengths = new short[rowCount, ColumnCount];

            for (int i = 0; i < rowCount; i++) {
                for (int j = 0; j < ColumnCount; j++) {
                    offsets[i, j] = reader.ReadInt32();
                    lengths[i, j] = reader.ReadInt16();
                }
            }

            for (int i = 0; i < rowCount; i++) {
                LanguageRow row = new LanguageRow(ColumnCount);

                for (int j = 0; j < ColumnCount; j++) {
                    stream.Seek(offsets[i, j], SeekOrigin.Begin);

                    row[j] = reader.ReadString(lengths[i, j] * 2, Encoding.Unicode);
                }

                rows.Add(row);
            }
        }

        /// <summary>
        /// Saves the file to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to save to.</param>
        public override void Save(Stream stream) {
            BinaryWriter writer = new BinaryWriter(stream, Encoding.Unicode);

            writer.Write(ColumnCount);
            writer.Write(rows.Count);
            
            long[,] offsets = new long[rows.Count, ColumnCount];
            
            for (int i = 0; i < rows.Count; i++) {
                LanguageRow row = rows[i];

                for (int j = 0; j < ColumnCount; j++) {
                    offsets[i, j] = stream.Position;

                    writer.Write(0);
                    writer.Write((short)row[j].Length);
                }
            }
            
            for (int i = 0; i < rows.Count; i++) {
                LanguageRow row = rows[i];

                for (int j = 0; j < ColumnCount; j++) {
                    long offset = stream.Position;
                    writer.WriteString(row[j], Encoding.Unicode);

                    long previousPosition = stream.Position;
                    stream.Seek(offsets[i, j], SeekOrigin.Begin);

                    writer.Write((int)offset);

                    stream.Seek(previousPosition, SeekOrigin.Begin);
                }
            }
        }

        /// <summary>
        /// Adds a column.
        /// </summary>
        public void AddColumn() {
            ColumnCount++;

            rows.ForEach(row => {
                row.AddColumn();
            });
        }

        /// <summary>
        /// Removes the specified column.
        /// </summary>
        /// <param name="column">The column to remove.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the specified column is out of range.</exception>
        public void RemoveColumn(int column) {
            if (column < 0 || column > ColumnCount - 1) {
                throw new ArgumentOutOfRangeException("column", "Column is out of range");
            }

            ColumnCount--;

            rows.ForEach(row => {
                row.RemoveColumn(column);
            });
        }

        /// <summary>
        /// Adds a new row.
        /// </summary>
        /// <returns>The row created.</returns>
        public LanguageRow AddRow() {
            LanguageRow row = new LanguageRow(ColumnCount);
            rows.Add(row);

            return row;
        }

        /// <summary>
        /// Removes the specified row.
        /// </summary>
        /// <param name="row">The row to remove.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the specified row is out of range.</exception>
        public void RemoveRow(int row) {
            if (row < 0 || row > rows.Count - 1) {
                throw new ArgumentOutOfRangeException("row", "Row is out of range");
            }

            rows.RemoveAt(row);
        }

        /// <summary>
        /// Removes the specified row.
        /// </summary>
        /// <param name="row">The row to remove.</param>
        /// <exception cref="System.ArgumentException">Thrown when the file does not contain the specified row.</exception>
        public void RemoveRow(LanguageRow row) {
            if (!rows.Contains(row)) {
                throw new ArgumentException("row", "File does not contain the specified row");
            }

            int rowIndex = rows.IndexOf(row);
            RemoveRow(rowIndex);
        }

        /// <summary>
        /// Removes all rows.
        /// </summary>
        public void Clear() {
            rows.Clear();
        }

        /// <summary>
        /// Resets properties to their default values.
        /// </summary>
        public override void Reset() {
            base.Reset();

            ColumnCount = 0;
            rows.Clear();
        }
    }
}