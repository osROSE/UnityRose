#region License

/**
 * Copyright (C) 2011 Jack Wakefield
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
using System.IO;
using NUnit.Framework;
using Revise.Files.LTB;

namespace Revise.Files.Tests {
    /// <summary>
    /// Provides testing for the <see cref="LanguageFile"/> class.
    /// </summary>
    [TestFixture]
    public class LanguageFileTests {
        private const string TEST_FILE = "Tests/Revise/Files/ULNGTB_CON.LTB";

        /// <summary>
        /// Tests the load method.
        /// </summary>
        [Test]
        public void TestLoadMethod() {
            const int ROW_COUNT = 200;
            const int COLUMN_COUNT = 6;

            Stream stream = File.OpenRead(TEST_FILE);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            LanguageFile languageFile = new LanguageFile();
            languageFile.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(fileSize, streamPosition, "Not all of the file was read");
            Assert.AreEqual(ROW_COUNT, languageFile.RowCount, "Incorrect row count");
            Assert.AreEqual(COLUMN_COUNT, languageFile.ColumnCount, "Incorrect column count");
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        [Test]
        public void TestSaveMethod() {
            LanguageFile languageFile = new LanguageFile();
            languageFile.Load(TEST_FILE);
            
            MemoryStream savedStream = new MemoryStream();
            languageFile.Save(savedStream);

            savedStream.Seek(0, SeekOrigin.Begin);

            LanguageFile savedLanguageFile = new LanguageFile();
            savedLanguageFile.Load(savedStream);

            savedStream.Close();

            Assert.AreEqual(languageFile.RowCount, savedLanguageFile.RowCount, "Row counts do not match");
            Assert.AreEqual(languageFile.ColumnCount, savedLanguageFile.ColumnCount, "Column counts do not match");

            for (int i = 0; i < languageFile.RowCount; i++) {
                for (int j = 0; j < languageFile.ColumnCount; j++) {
                    Assert.AreEqual(languageFile[i][j], savedLanguageFile[i][j], "Cell values do not match");
                }
            }
        }

        /// <summary>
        /// Tests the row and column methods.
        /// </summary>
        [Test]
        public void TestRowAndColumnMethods() {
            const string CELL_VALUE = "Test Value";

            LanguageFile languageFile = new LanguageFile();
            languageFile.AddRow();
            languageFile.AddColumn();
            languageFile[0][0] = CELL_VALUE;

            Assert.AreEqual(languageFile.ColumnCount, 1, "Column count is incorrect");
            Assert.AreEqual(languageFile.RowCount, 1, "Row count is incorrect");
            Assert.AreEqual(languageFile[0][0], CELL_VALUE, "Row value is incorrect");

            languageFile.RemoveColumn(0);

            Assert.Throws(typeof(ArgumentOutOfRangeException), () => {
                languageFile[0][0] = CELL_VALUE;
            }, "Column not removed");

            languageFile.RemoveRow(0);

            Assert.Throws(typeof(ArgumentOutOfRangeException), () => {
                languageFile[0][0] = CELL_VALUE;
            }, "Row not removed");
        }
    }
}