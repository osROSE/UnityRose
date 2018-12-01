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
using Revise.Files.TIL;

namespace Revise.Files.Tests {
    /// <summary>
    /// Provides testing for the <see cref="TileFile"/> class.
    /// </summary>
    [TestFixture]
    public class TileFileTests {
        private const string TEST_FILE = "Tests/Revise/Files/31_31.TIL";

        /// <summary>
        /// Tests the load method.
        /// </summary>
        [Test]
        public void TestLoadMethod() {
            const int HEIGHT = 16;
            const int WIDTH = 16;

            Stream stream = File.OpenRead(TEST_FILE);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            TileFile tileFile = new TileFile();
            tileFile.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(fileSize, streamPosition, "Not all of the file was read");
            Assert.AreEqual(WIDTH, tileFile.Width, "Incorrect width");
            Assert.AreEqual(HEIGHT, tileFile.Height, "Incorrect height");
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        [Test]
        public void TestSaveMethod() {
            TileFile tileFile = new TileFile();
            tileFile.Load(TEST_FILE);

            MemoryStream savedStream = new MemoryStream();
            tileFile.Save(savedStream);

            savedStream.Seek(0, SeekOrigin.Begin);

            TileFile savedTileFile = new TileFile();
            savedTileFile.Load(savedStream);

            savedStream.Close();

            Assert.AreEqual(tileFile.Width, savedTileFile.Width, "Width values do not match");
            Assert.AreEqual(tileFile.Height, savedTileFile.Height, "Height values do not match");

            for (int x = 0; x < tileFile.Height; x++) {
                for (int y = 0; y < tileFile.Width; y++) {
                    Assert.AreEqual(tileFile[x, y].Brush, savedTileFile[x, y].Brush, "Brush values do not match");
                    Assert.AreEqual(tileFile[x, y].TileIndex, savedTileFile[x, y].TileIndex, "Tile index values do not match");
                    Assert.AreEqual(tileFile[x, y].TileSet, savedTileFile[x, y].TileSet, "Tile set values do not match");
                    Assert.AreEqual(tileFile[x, y].Tile, savedTileFile[x, y].Tile, "Tile values do not match");
                }
            }
        }
    }
}