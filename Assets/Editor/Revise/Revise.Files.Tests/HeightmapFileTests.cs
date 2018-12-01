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
using Revise.Files.HIM;

namespace Revise.Files.Tests {
    /// <summary>
    /// Provides testing for the <see cref="HeightmapFile"/> class.
    /// </summary>
    [TestFixture]
    public class HeightmapFileTests {
        private const string TEST_FILE = "Tests/Revise/Files/31_30.HIM";

        /// <summary>
        /// Tests the load method.
        /// </summary>
        [Test]
        public void TestLoadMethod() {
            const int HEIGHT = 65;
            const int WIDTH = 65;

            Stream stream = File.OpenRead(TEST_FILE);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            HeightmapFile heightmapFile = new HeightmapFile();
            heightmapFile.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(fileSize, streamPosition, "Not all of the file was read");
            Assert.AreEqual(WIDTH, heightmapFile.Width, "Incorrect width");
            Assert.AreEqual(HEIGHT, heightmapFile.Height, "Incorrect height");
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        [Test]
        public void TestSaveMethod() {
            HeightmapFile heightmapFile = new HeightmapFile();
            heightmapFile.Load(TEST_FILE);

            MemoryStream savedStream = new MemoryStream();
            heightmapFile.Save(savedStream);
            heightmapFile.Load(TEST_FILE);

            savedStream.Seek(0, SeekOrigin.Begin);

            HeightmapFile savedHeightmapFile = new HeightmapFile();
            savedHeightmapFile.Load(savedStream);

            savedStream.Close();

            for (int x = 0; x < heightmapFile.Height; x++) {
                for (int y = 0; y < heightmapFile.Width; y++) {
                    Assert.AreEqual(heightmapFile[x, y], savedHeightmapFile[x, y], "Height values do not match");
                }
            }

            for (int x = 0; x < heightmapFile.Patches.GetLength(0); x++) {
                for (int y = 0; y < heightmapFile.Patches.GetLength(1); y++) {
                    Assert.AreEqual(heightmapFile.Patches[x, y].Minimum, savedHeightmapFile.Patches[x, y].Minimum, "Minimum patch values do not match");
                    Assert.AreEqual(heightmapFile.Patches[x, y].Maximum, savedHeightmapFile.Patches[x, y].Maximum, "Maximum patch values do not match");
                }
            }

            for (int i = 0; i < heightmapFile.QuadPatches.Length; i++) {
                Assert.AreEqual(heightmapFile.QuadPatches[i].Minimum, savedHeightmapFile.QuadPatches[i].Minimum, "Minimum quad patch values do not match");
                Assert.AreEqual(heightmapFile.QuadPatches[i].Maximum, savedHeightmapFile.QuadPatches[i].Maximum, "Maximum quad patch values do not match");
            }
        }
    }
}