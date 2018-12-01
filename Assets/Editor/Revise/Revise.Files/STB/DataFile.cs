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
using Revise.Files.Exceptions;

namespace Revise.Files.STB {
    /// <summary>
    /// Provides the ability to create, open and save STB files for data.
    /// </summary>
    public class DataFile : FileLoader {
        #region Constants

        private const string FILE_IDENTIFIER = "STB";
        private const int FILE_VERSION = 1;

        private const byte DEFAULT_VERSION = 1;
        private const short DEFAULT_COLUMN_WIDTH = 50;
        private const int DEFAULT_ROW_HEIGHT = 17;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of columns.
        /// </summary>
        public int ColumnCount {
            get {
                return columns.Count;
            }
        }

        /// <summary>
        /// Gets the number of rows.
        /// </summary>
        public int RowCount {
            get {
                return rows.Count;
            }
        }

        /// <summary>
        /// Gets or sets the height of the row cell.
        /// </summary>
        public int RowHeight {
            get;
            set;
        }

        #endregion

        private DataColumn rootColumn;
        private List<DataColumn> columns;
        private List<DataRow> rows;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DataFile"/> class.
        /// </summary>
        public DataFile() {
            rootColumn = new DataColumn();
            columns = new List<DataColumn>();
            rows = new List<DataRow>();
            
            Reset();
        }

        /// <summary>
        /// Gets the specified <see cref="DataRow"/>.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the specified row does not exist.</exception>
        public DataRow this[int row] {
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
        /// <param name="stream">The stream to load from.</param>
        /// <exception cref="Revise.Exceptions.FileIdentifierMismatchException">Thrown when the specified file has the incorrect file header expected.</exception>
        /// <exception cref="Revise.Exceptions.InvalidVersionException">Thrown when the version of the file is invalid.</exception>
        public override void Load(Stream stream) {
            BinaryReader reader = new BinaryReader(stream, Encoding.GetEncoding("EUC-KR"));

            string identifier = reader.ReadString(3);

            if (string.Compare(identifier, FILE_IDENTIFIER, false) != 0) {
                throw new FileIdentifierMismatchException(FilePath, FILE_IDENTIFIER, identifier);
            }

            int version = (byte)(reader.ReadByte() - '0');

            if (version != FILE_VERSION) {
                throw new InvalidVersionException(version);
            }

            reader.BaseStream.Seek(4, SeekOrigin.Current);
            int rowCount = reader.ReadInt32();
            int columnCount = reader.ReadInt32();
            RowHeight = reader.ReadInt32();

            SetRootColumnWidth(reader.ReadInt16());

            for (int i = 0; i < columnCount; i++) {
                columns.Add(new DataColumn());
                SetColumnWidth(i, reader.ReadInt16());
            }

            SetRootColumnName(reader.ReadShortString());

            for (int i = 0; i < columnCount; i++) {
                SetColumnName(i, reader.ReadShortString());
            }

            for (int i = 0; i < rowCount - 1; i++) {
                DataRow row = new DataRow(columnCount);
                row[0] = reader.ReadShortString();
                
                rows.Add(row);
            }

            for (int i = 0; i < rowCount - 1; i++) {
                DataRow row = rows[i];

                for (int j = 1; j < columnCount; j++) {
                    row[j] = reader.ReadShortString();
                }
            }
        }

        /// <summary>
        /// Saves the file to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to save to.</param>
        public override void Save(Stream stream) {
            BinaryWriter writer = new BinaryWriter(stream, Encoding.GetEncoding("EUC-KR"));

            writer.WriteString(FILE_IDENTIFIER);
            writer.Write('1');
            writer.Write(0);
            writer.Write(RowCount + 1);
            writer.Write(ColumnCount);
            writer.Write(RowHeight);

            writer.Write(GetRootColumnWidth());

            columns.ForEach(column => {
                writer.Write(column.Width);
            });

            writer.WriteShortString(GetRootColumnName());

            columns.ForEach(column => {
                writer.WriteShortString(column.Name);
            });

            rows.ForEach(row => {
                writer.WriteShortString(row[0]);
            });

            long position = stream.Position;

            rows.ForEach(row => {
                for (int i = 1; i < ColumnCount; i++) {
                    writer.WriteShortString(row[i]);
                }
            });
            
            stream.Seek(FILE_IDENTIFIER.Length + 1, SeekOrigin.Begin);
            writer.Write((int)position);
        }

        /// <summary>
        /// Adds a column with the specified header name and column width.
        /// </summary>
        /// <param name="name">The header name.</param>
        /// <param name="width">The column width.</param>
        public void AddColumn(string name = "", short width = DEFAULT_COLUMN_WIDTH) {
            DataColumn column = new DataColumn();
            column.Name = name;
            column.Width = width;

            columns.Add(column);

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
            if (column < 0 || column > columns.Count - 1) {
                throw new ArgumentOutOfRangeException("column", "Column is out of range");
            }

            columns.RemoveAt(column);

            rows.ForEach(row => {
                row.RemoveColumn(column);
            });
        }

        /// <summary>
        /// Sets the name of the root column.
        /// </summary>
        /// <param name="name">The column name.</param>
        public void SetRootColumnName(string name) {
            rootColumn.Name = name;
        }

        /// <summary>
        /// Gets the name of the root column.
        /// </summary>
        /// <returns>The column name.</returns>
        public string GetRootColumnName() {
            return rootColumn.Name;
        }

        /// <summary>
        /// Sets the name of the specified column.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="name">The column name.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the specified column is out of range.</exception>
        public void SetColumnName(int column, string name) {
            if (column < 0 || column > columns.Count - 1) {
                throw new ArgumentOutOfRangeException("column", "Column is out of range");
            }

            columns[column].Name = name;
        }

        /// <summary>
        /// Gets the name of the specified column.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the specified column is out of range.</exception>
        /// <returns>The column name.</returns>
        public string GetColumnName(int column) {
            if (column < 0 || column > columns.Count - 1) {
                throw new ArgumentOutOfRangeException("column", "Column is out of range");
            }

            return columns[column].Name;
        }

        /// <summary>
        /// Sets the width of the root column.
        /// </summary>
        /// <param name="width">The column width.</param>
        public void SetRootColumnWidth(short width) {
            rootColumn.Width = width;
        }

        /// <summary>
        /// Gets the width of the root column.
        /// </summary>
        /// <returns>The column width.</returns>
        public short GetRootColumnWidth() {
            return rootColumn.Width;
        }

        /// <summary>
        /// Sets the width of the specified column.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="width">The column width.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the specified column is out of range.</exception>
        public void SetColumnWidth(int column, short width) {
            if (column < 0 || column > columns.Count - 1) {
                throw new ArgumentOutOfRangeException("column", "Column is out of range");
            }

            columns[column].Width = width;
        }

        /// <summary>
        /// Gets the width of the specified column.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the specified column is out of range.</exception>
        /// <returns>The column width.</returns>
        public int GetColumnWidth(int column) {
            if (column < 0 || column > columns.Count - 1) {
                throw new ArgumentOutOfRangeException("column", "Column is out of range");
            }

            return columns[column].Width;
        }

        /// <summary>
        /// Adds a new row.
        /// </summary>
        /// <returns>The row created.</returns>
        public DataRow AddRow() {
            DataRow row = new DataRow(ColumnCount);
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
        public void RemoveRow(DataRow row) {
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

            rootColumn.Name = string.Empty;
            rootColumn.Width = DEFAULT_COLUMN_WIDTH;

            RowHeight = DEFAULT_ROW_HEIGHT;

            columns.Clear();
            Clear();
        }
    }
}
