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
using Revise.Files.MOV;

namespace Revise.Files.Tests {
    /// <summary>
    /// Provides testing for the <see cref="MovementFile"/> class.
    /// </summary>
    [TestFixture]
    public class MovementFileTests {
        private const string TEST_FILE = "Tests/Revise/Files/31_31.MOV";

        /// <summary>
        /// Tests the load method.
        /// </summary>
        [Test]
        public void TestLoadMethod() {
            const int HEIGHT = 32;
            const int WIDTH = 32;

            Stream stream = File.OpenRead(TEST_FILE);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            MovementFile movementFile = new MovementFile();
            movementFile.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(fileSize, streamPosition, "Not all of the file was read");
            Assert.AreEqual(WIDTH, movementFile.Width, "Incorrect width");
            Assert.AreEqual(HEIGHT, movementFile.Height, "Incorrect height");
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        [Test]
        public void TestSaveMethod() {
            MovementFile movementFile = new MovementFile();
            movementFile.Load(TEST_FILE);

            MemoryStream savedStream = new MemoryStream();
            movementFile.Save(savedStream);

            savedStream.Seek(0, SeekOrigin.Begin);

            MovementFile savedMovementFile = new MovementFile();
            savedMovementFile.Load(savedStream);

            savedStream.Close();

            Assert.AreEqual(movementFile.Width, savedMovementFile.Width, "Width values do not match");
            Assert.AreEqual(movementFile.Height, savedMovementFile.Height, "Height values do not match");

            for (int x = 0; x < movementFile.Height; x++) {
                for (int y = 0; y < movementFile.Width; y++) {
                    Assert.AreEqual(movementFile[x, y], savedMovementFile[x, y], "Values do not match");
                }
            }
        }
    }
}