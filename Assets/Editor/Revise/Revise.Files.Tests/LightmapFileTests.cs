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
using Revise.Files.LIT;

namespace Revise.Files.Tests {
    /// <summary>
    /// Provides testing for the <see cref="LightmapFile"/> class.
    /// </summary>
    [TestFixture]
    public class LightmapFileTests {
        private const string TEST_FILE = "Tests/Revise/Files/OBJECTLIGHTMAPDATA.LIT";

        /// <summary>
        /// Tests the load method.
        /// </summary>
        [Test]
        public void TestLoadMethod() {
            const int OBJECT_COUNT = 266;

            Stream stream = File.OpenRead(TEST_FILE);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            LightmapFile lightmapFile = new LightmapFile();
            lightmapFile.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(fileSize, streamPosition, "Not all of the file was read");
            Assert.AreEqual(OBJECT_COUNT, lightmapFile.Objects.Count, "Incorrect object count");
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        [Test]
        public void TestSaveMethod() {
            LightmapFile lightmapFile = new LightmapFile();
            lightmapFile.Load(TEST_FILE);

            MemoryStream savedStream = new MemoryStream();
            lightmapFile.Save(savedStream);

            savedStream.Seek(0, SeekOrigin.Begin);

            LightmapFile savedLightmapFile = new LightmapFile();
            savedLightmapFile.Load(savedStream);

            savedStream.Close();

            Assert.AreEqual(lightmapFile.Objects.Count, savedLightmapFile.Objects.Count, "Object counts do not match");

            for (int i = 0; i < lightmapFile.Objects.Count; i++) {
                Assert.AreEqual(lightmapFile.Objects[i].ID, savedLightmapFile.Objects[i].ID, "Object IDs do not match");
                Assert.AreEqual(lightmapFile.Objects[i].Parts.Count, savedLightmapFile.Objects[i].Parts.Count, "Part counts do not match");

                for (int j = 0; j < lightmapFile.Objects[i].Parts.Count; j++) {
                    Assert.AreEqual(lightmapFile.Objects[i].Parts[j].Name, savedLightmapFile.Objects[i].Parts[j].Name, "Part names do not match");
                    Assert.AreEqual(lightmapFile.Objects[i].Parts[j].ID, savedLightmapFile.Objects[i].Parts[j].ID, "Part IDs do not match");
                    Assert.AreEqual(lightmapFile.Objects[i].Parts[j].FileName, savedLightmapFile.Objects[i].Parts[j].FileName, "Part file names do not match");
                    Assert.AreEqual(lightmapFile.Objects[i].Parts[j].PixelsPerObject, savedLightmapFile.Objects[i].Parts[j].PixelsPerObject, "Part pixel per object values do not match");
                    Assert.AreEqual(lightmapFile.Objects[i].Parts[j].ObjectsPerWidth, savedLightmapFile.Objects[i].Parts[j].ObjectsPerWidth, "Part objects per width values do not match");
                    Assert.AreEqual(lightmapFile.Objects[i].Parts[j].ObjectPosition, savedLightmapFile.Objects[i].Parts[j].ObjectPosition, "Part position values do not match");
                }
            }

            Assert.AreEqual(lightmapFile.Files.Count, savedLightmapFile.Files.Count, "File counts do not match");

            for (int i = 0; i < lightmapFile.Files.Count; i++) {
                Assert.AreEqual(lightmapFile.Files[i], savedLightmapFile.Files[i], "File names do not match");
            }
        }
    }
}