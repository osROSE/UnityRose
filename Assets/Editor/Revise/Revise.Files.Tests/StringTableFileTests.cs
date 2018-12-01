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

using System.IO;
using NUnit.Framework;
using Revise.Files.STL;

namespace Revise.Files.Tests {
    /// <summary>
    /// Provides testing for the <see cref="StringTableFile"/> class.
    /// </summary>
    [TestFixture]
    public class StringTableFileTests {
        private const string ITEM_TEST_FILE = "Tests/Revise/Files/LIST_FACEITEM_S.STL";
        private const string QUEST_TEST_FILE = "Tests/Revise/Files/LIST_QUEST_S.STL";
        private const string NORMAL_TEST_FILE = "Tests/Revise/Files/STR_ITEMTYPE.STL";

        /// <summary>
        /// Tests the load method using the specified file path and row count.
        /// </summary>
        private void TestLoadMethod(string filePath, int rowCount) {
            Stream stream = File.OpenRead(filePath);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            StringTableFile stringTableFile = new StringTableFile();
            stringTableFile.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(fileSize, streamPosition, "Not all of the file was read");
            Assert.AreEqual(rowCount, stringTableFile.RowCount, "Incorrect row count");
        }

        /// <summary>
        /// Tests the save method using the specified file path.
        /// </summary>
        private void TestSaveMethod(string filePath) {
            StringTableFile stringTableFile = new StringTableFile();
            stringTableFile.Load(filePath);

            MemoryStream savedStream = new MemoryStream();
            stringTableFile.Save(savedStream);
            savedStream.Seek(0, SeekOrigin.Begin);

            StringTableFile savedStringTableFile = new StringTableFile();
            savedStringTableFile.Load(savedStream);
            savedStream.Close();

            Assert.AreEqual(stringTableFile.TableType, stringTableFile.TableType, "Table types do not match");
            Assert.AreEqual(stringTableFile.RowCount, stringTableFile.RowCount, "Row counts do not match");

            for (int i = 0; i < stringTableFile.RowCount; i++) {
                for (int j = 0; j < stringTableFile.LanguageCount; j++) {
                    StringTableLanguage language = (StringTableLanguage)j;

                    Assert.AreEqual(stringTableFile[i].GetText(language), savedStringTableFile[i].GetText(language), "Text values do not match");

                    if (stringTableFile.TableType == StringTableType.Item || stringTableFile.TableType == StringTableType.Quest) {
                        Assert.AreEqual(stringTableFile[i].GetDescription(language), savedStringTableFile[i].GetDescription(language), "Description values do not match");

                        if (stringTableFile.TableType == StringTableType.Quest) {
                            Assert.AreEqual(stringTableFile[i].GetStartMessage(language), savedStringTableFile[i].GetStartMessage(language), "Start message values do not match");
                            Assert.AreEqual(stringTableFile[i].GetEndMessage(language), savedStringTableFile[i].GetEndMessage(language), "End message values do not match");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Tests the load method using an item file type.
        /// </summary>
        [Test]
        public void TestItemLoadMethod() {
            const int ROW_COUNT = 43;
            TestLoadMethod(ITEM_TEST_FILE, ROW_COUNT);
        }

        /// <summary>
        /// Tests the load method using a quest file type.
        /// </summary>
        [Test]
        public void TestQuestLoadMethod() {
            const int ROW_COUNT = 235;
            TestLoadMethod(QUEST_TEST_FILE, ROW_COUNT);
        }

        /// <summary>
        /// Tests the load method using a normal file type.
        /// </summary>
        [Test]
        public void TestNormalLoadMethod() {
            const int ROW_COUNT = 75;
            TestLoadMethod(NORMAL_TEST_FILE, ROW_COUNT);
        }

        /// <summary>
        /// Tests the save method using an item file type.
        /// </summary>
        [Test]
        public void TestItemSaveMethod() {
            TestSaveMethod(ITEM_TEST_FILE);
        }

        /// <summary>
        /// Tests the save method using a quest file type.
        /// </summary>
        [Test]
        public void TestQuestSaveMethod() {
            TestSaveMethod(QUEST_TEST_FILE);
        }

        /// <summary>
        /// Tests the save method using a normal file type.
        /// </summary>
        [Test]
        public void TestNormalSaveMethod() {
            TestSaveMethod(NORMAL_TEST_FILE);
        }

        /// <summary>
        /// Tests the row methods.
        /// </summary>
        [Test]
        public void TestRowMethods() {
            const string ROW_KEY = "Test Key";
            const int ROW_ID = 1;
            const string ROW_VALUE_1 = "Test Value 1";
            const string ROW_VALUE_2 = "Test Value 2";

            StringTableFile savedStringTableFile = new StringTableFile();
            savedStringTableFile.AddRow(ROW_KEY, ROW_ID);

            Assert.AreEqual(savedStringTableFile.RowCount, 1, "Row count is incorrect");

            savedStringTableFile[0].SetText(ROW_VALUE_1);
            string rowValue = savedStringTableFile[0].GetText();

            Assert.AreEqual(rowValue, ROW_VALUE_1, "Row value is incorrect");

            savedStringTableFile[ROW_KEY].SetText(ROW_VALUE_2);
            rowValue = savedStringTableFile[ROW_KEY].GetText();

            Assert.AreEqual(rowValue, ROW_VALUE_2, "Row value is incorrect");

            savedStringTableFile.RemoveRow(ROW_KEY);

            Assert.AreEqual(savedStringTableFile.RowCount, 0, "Row count is incorrect");
        }
    }
}