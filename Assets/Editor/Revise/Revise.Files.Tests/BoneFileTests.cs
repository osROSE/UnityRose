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
using Revise.Files.ZMD;

namespace Revise.Files.Tests {
    /// <summary>
    /// Provides testing for the <see cref="BoneFile"/> class.
    /// </summary>
    [TestFixture]
    public class BoneFileTests {
        private const string TEST_FILE = "Tests/Revise/Files/CART01.ZMD";

        /// <summary>
        /// Tests the load method.
        /// </summary>
        [Test]
        public void TestLoadMethod() {
            Stream stream = File.OpenRead(TEST_FILE);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            BoneFile boneFile = new BoneFile();
            boneFile.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(fileSize, streamPosition, "Not all of the file was read");
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        [Test]
        public void TestSaveMethod() {
            BoneFile boneFile = new BoneFile();
            boneFile.Load(TEST_FILE);

            MemoryStream savedStream = new MemoryStream();
            boneFile.Save(savedStream);

            savedStream.Seek(0, SeekOrigin.Begin);

            BoneFile savedBoneFile = new BoneFile();
            savedBoneFile.Load(savedStream);

            savedStream.Close();

            Assert.AreEqual(boneFile.Bones.Count, savedBoneFile.Bones.Count, "Bone counts do not match");
            Assert.AreEqual(boneFile.DummyBones.Count, savedBoneFile.DummyBones.Count, "Dummy bones counts do not match");

            for (int i = 0; i < boneFile.Bones.Count; i++) {
                Assert.AreEqual(boneFile.Bones[i].Name, savedBoneFile.Bones[i].Name, "Bone names do not match");
                Assert.AreEqual(boneFile.Bones[i].Parent, savedBoneFile.Bones[i].Parent, "Bone parents do not match");
                Assert.AreEqual(boneFile.Bones[i].Translation, savedBoneFile.Bones[i].Translation, "Bone positions do not match");
                Assert.AreEqual(boneFile.Bones[i].Rotation, savedBoneFile.Bones[i].Rotation, "Bone rotations do not match");
            }

            for (int i = 0; i < boneFile.DummyBones.Count; i++) {
                Assert.AreEqual(boneFile.DummyBones[i].Name, savedBoneFile.DummyBones[i].Name, "Dummy bone names do not match");
                Assert.AreEqual(boneFile.DummyBones[i].Parent, savedBoneFile.DummyBones[i].Parent, "Dummy bone parents do not match");
                Assert.AreEqual(boneFile.DummyBones[i].Translation, savedBoneFile.DummyBones[i].Translation, "Dummy bone positions do not match");
                Assert.AreEqual(boneFile.DummyBones[i].Rotation, savedBoneFile.DummyBones[i].Rotation, "Dummy bone rotations do not match");
            }
        }
    }
}