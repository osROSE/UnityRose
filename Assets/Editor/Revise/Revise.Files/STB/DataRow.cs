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

namespace Revise.Files.STB {
    /// <summary>
    /// Represents an data file row.
    /// </summary>
    public class DataRow {
        private List<string> data;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRow"/> class.
        /// </summary>
        /// <param name="columnCount">The number of columns to add.</param>
        public DataRow(int columnCount) {
            data = new List<string>();

            for (int i = 0; i < columnCount; i++) {
                AddColumn();
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.String"/> of the specified cell.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the specified cell is out of range.</exception>
        public string this[int cell] {
            get {
                if (cell < 0 || cell > data.Count - 1) {
                    throw new ArgumentOutOfRangeException("cell", "Cell is out of range");
                }

                return data[cell];
            }
            set {
                if (cell < 0 || cell > data.Count - 1) {
                    throw new ArgumentOutOfRangeException("cell", "Cell is out of range");
                }

                data[cell] = value;
            }
        }

        /// <summary>
        /// Adds a column with an empty string.
        /// </summary>
        internal void AddColumn() {
            data.Add(string.Empty);
        }

        /// <summary>
        /// Removes the specified column.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the specified column is out of range.</exception>
        internal void RemoveColumn(int column) {
            if (column < 0 || column > data.Count - 1) {
                throw new ArgumentOutOfRangeException("column", "Column is out of range");
            }

            data.RemoveAt(column);
        }
    }
}