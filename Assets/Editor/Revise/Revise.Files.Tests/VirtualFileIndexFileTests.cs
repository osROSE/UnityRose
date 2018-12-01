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
using Revise.Files.IDX;

namespace Revise.Files.Tests {
    /// <summary>
    /// Provides testing for the <see cref="VirtualFileIndexFile"/> class.
    /// </summary>
    [TestFixture]
    public class VirtualFileIndexFileTests {
        private const string TEST_FILE = "Tests/Revise/Files/data.idx";

        /// <summary>
        /// Tests the load method.
        /// </summary>
        [Test]
        public void TestLoadMethod() {
            const int FILE_SYSTEM_COUNT = 2;

            Stream stream = File.OpenRead(TEST_FILE);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            VirtualFileIndexFile virtualFileIndexFile = new VirtualFileIndexFile();
            virtualFileIndexFile.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(fileSize, streamPosition, "Not all of the file was read");
            Assert.AreEqual(FILE_SYSTEM_COUNT, virtualFileIndexFile.FileSystems.Count, "Incorrect file system count");
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        [Test]
        public void TestSaveMethod() {
            VirtualFileIndexFile virtualFileIndexFile = new VirtualFileIndexFile();
            virtualFileIndexFile.Load(TEST_FILE);

            MemoryStream savedStream = new MemoryStream();
            virtualFileIndexFile.Save(savedStream);

            savedStream.Seek(0, SeekOrigin.Begin);

            VirtualFileIndexFile savedVirtualFileIndexFile = new VirtualFileIndexFile();
            savedVirtualFileIndexFile.Load(savedStream);

            savedStream.Close();

            Assert.AreEqual(virtualFileIndexFile.BaseVersion, savedVirtualFileIndexFile.BaseVersion, "Base version values do not match");
            Assert.AreEqual(virtualFileIndexFile.CurrentVersion, savedVirtualFileIndexFile.CurrentVersion, "Current version values do not match");
            Assert.AreEqual(virtualFileIndexFile.FileSystems.Count, savedVirtualFileIndexFile.FileSystems.Count, "File system counts do not match");

            for (int i = 0; i < virtualFileIndexFile.FileSystems.Count; i++) {
                Assert.AreEqual(virtualFileIndexFile.FileSystems[i].FileName, savedVirtualFileIndexFile.FileSystems[i].FileName, "File names do not match");
                Assert.AreEqual(virtualFileIndexFile.FileSystems[i].Files.Count, savedVirtualFileIndexFile.FileSystems[i].Files.Count, "File counts do not match");

                for (int j = 0; j < virtualFileIndexFile.FileSystems[i].Files.Count; j++) {
                    Assert.AreEqual(virtualFileIndexFile.FileSystems[i].Files[j].FilePath, virtualFileIndexFile.FileSystems[i].Files[j].FilePath, "File paths do not match");
                    Assert.AreEqual(virtualFileIndexFile.FileSystems[i].Files[j].Offset, virtualFileIndexFile.FileSystems[i].Files[j].Offset, "Offset values do not match");
                    Assert.AreEqual(virtualFileIndexFile.FileSystems[i].Files[j].Size, virtualFileIndexFile.FileSystems[i].Files[j].Size, "Size values do not match");
                    Assert.AreEqual(virtualFileIndexFile.FileSystems[i].Files[j].BlockSize, virtualFileIndexFile.FileSystems[i].Files[j].BlockSize, "Block size values do not match");
                    Assert.AreEqual(virtualFileIndexFile.FileSystems[i].Files[j].IsDeleted, virtualFileIndexFile.FileSystems[i].Files[j].IsDeleted, "Deleted values do not match");
                    Assert.AreEqual(virtualFileIndexFile.FileSystems[i].Files[j].IsCompressed, virtualFileIndexFile.FileSystems[i].Files[j].IsCompressed, "Compresed values do not match");
                    Assert.AreEqual(virtualFileIndexFile.FileSystems[i].Files[j].IsEncrypted, virtualFileIndexFile.FileSystems[i].Files[j].IsEncrypted, "Encrypted value sdo not match");
                    Assert.AreEqual(virtualFileIndexFile.FileSystems[i].Files[j].Version, virtualFileIndexFile.FileSystems[i].Files[j].Version, "Version values do not match");
                    Assert.AreEqual(virtualFileIndexFile.FileSystems[i].Files[j].Checksum, virtualFileIndexFile.FileSystems[i].Files[j].Checksum, "Checksum values do not match");
                }
            }
        }
    }
}