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
using Revise.Files.TBL;

namespace Revise.Files.Tests {
    /// <summary>
    /// Provides testing for the <see cref="TableFile"/> class.
    /// </summary>
    [TestFixture]
    public class TableFileTests {
        private const string TEST_FILE = "Tests/Revise/Files/O_RANGE.TBL";

        /// <summary>
        /// Tests the load method.
        /// </summary>
        [Test]
        public void TestLoadMethod() {
            const int MAXIMUM_RANGE = 42;

            Stream stream = File.OpenRead(TEST_FILE);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            TableFile tableFile = new TableFile();
            tableFile.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(fileSize, streamPosition, "Not all of the file was read");
            Assert.AreEqual(MAXIMUM_RANGE, tableFile.MaximumRange, "Incorrect maximum range");
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        [Test]
        public void TestSaveMethod() {
            TableFile tableFile = new TableFile();
            tableFile.Load(TEST_FILE);

            MemoryStream savedStream = new MemoryStream();
            tableFile.Save(savedStream);

            savedStream.Seek(0, SeekOrigin.Begin);

            TableFile savedTableFile = new TableFile();
            savedTableFile.Load(savedStream);

            savedStream.Close();

            Assert.AreEqual(tableFile.MaximumRange, savedTableFile.MaximumRange, "Maximum range does not match");

            for (int i = 0; i < tableFile.MaximumRange; i++) {
                Assert.AreEqual(tableFile.StartIndices[i], tableFile.StartIndices[i], "Start index does not match");
                Assert.AreEqual(tableFile.IndexCounts[i], tableFile.IndexCounts[i], "Index count does not match");
            }

            Assert.AreEqual(tableFile.Points.Count, savedTableFile.Points.Count, "Points count does not match");

            for (int i = 0; i < tableFile.Points.Count; i++) {
                Assert.AreEqual(tableFile.Points[i].X, tableFile.Points[i].X, "Points do not match");
                Assert.AreEqual(tableFile.Points[i].Y, tableFile.Points[i].Y, "Points do not match");
            }
        }
    }
}