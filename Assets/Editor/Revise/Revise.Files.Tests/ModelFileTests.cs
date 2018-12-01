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
using Revise.Files.ZMS;

namespace Revise.Files.Tests {
    /// <summary>
    /// Provides testing for the <see cref="ModelFile"/> class.
    /// </summary>
    [TestFixture]
    public class ModelFileTests {
        private const string TEST_FILE1 = "Tests/Revise/Files/CART01_ABILITY01.ZMS";
        private const string TEST_FILE2 = "Tests/Revise/Files/HEADBAD01.ZMS";
        private const string TEST_FILE3 = "Tests/Revise/Files/STONE014.ZMS";

        /// <summary>
        /// Tests the load method.
        /// </summary>
        private void TestLoadMethod(string filePath) {
            Stream stream = File.OpenRead(filePath);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            ModelFile modelFile = new ModelFile();
            modelFile.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(fileSize, streamPosition, "Not all of the file was read");
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        private void TestSaveMethod(string filePath) {
            ModelFile modelFile = new ModelFile();
            modelFile.Load(filePath);

            MemoryStream savedStream = new MemoryStream();
            modelFile.Save(savedStream);

            savedStream.Seek(0, SeekOrigin.Begin);

            ModelFile savedModelFile = new ModelFile();
            savedModelFile.Load(savedStream);

            savedStream.Close();

            Assert.AreEqual(modelFile.Pool, savedModelFile.Pool, "Pool values do not match");
            Assert.AreEqual(modelFile.BoneTable.Count, savedModelFile.BoneTable.Count, "Bone table counts do not match");

            for (int i = 0; i < modelFile.BoneTable.Count; i++) {
                Assert.AreEqual(modelFile.BoneTable[i], savedModelFile.BoneTable[i], "Bone table values do not match");
            }

            Assert.AreEqual(modelFile.Vertices.Count, savedModelFile.Vertices.Count, "Vertex counts do not match");

            for (int i = 0; i < modelFile.Vertices.Count; i++) {
                Assert.AreEqual(modelFile.Vertices[i].Position, savedModelFile.Vertices[i].Position, "Vertex position values do not match");
                Assert.AreEqual(modelFile.Vertices[i].Normal, savedModelFile.Vertices[i].Normal, "Vertex normal values do not match");
                Assert.AreEqual(modelFile.Vertices[i].Colour, savedModelFile.Vertices[i].Colour, "Vertex colour values do not match");
                Assert.AreEqual(modelFile.Vertices[i].BoneWeights, savedModelFile.Vertices[i].BoneWeights, "Vertex bone weight values do not match");
                Assert.AreEqual(modelFile.Vertices[i].BoneIndices, savedModelFile.Vertices[i].BoneIndices, "Vertex bone index values do not match");
                Assert.AreEqual(modelFile.Vertices[i].TextureCoordinates[0], savedModelFile.Vertices[i].TextureCoordinates[0], "Vertex texture coordinate values do not match");
                Assert.AreEqual(modelFile.Vertices[i].TextureCoordinates[1], savedModelFile.Vertices[i].TextureCoordinates[1], "Vertex texture coordinate values do not match");
                Assert.AreEqual(modelFile.Vertices[i].TextureCoordinates[2], savedModelFile.Vertices[i].TextureCoordinates[2], "Vertex texture coordinate values do not match");
                Assert.AreEqual(modelFile.Vertices[i].TextureCoordinates[3], savedModelFile.Vertices[i].TextureCoordinates[3], "Vertex texture coordinate values do not match");
                Assert.AreEqual(modelFile.Vertices[i].Tangent, savedModelFile.Vertices[i].Tangent, "Vertex tangent values do not match");
            }

            Assert.AreEqual(modelFile.Indices.Count, savedModelFile.Indices.Count, "Index counts do not match");

            for (int i = 0; i < modelFile.Indices.Count; i++) {
                Assert.AreEqual(modelFile.Indices[i], savedModelFile.Indices[i], "Index values do not match");
            }

            Assert.AreEqual(modelFile.Materials.Count, savedModelFile.Materials.Count, "Material counts do not match");

            for (int i = 0; i < modelFile.Materials.Count; i++) {
                Assert.AreEqual(modelFile.Materials[i], savedModelFile.Materials[i], "Material values do not match");
            }

            Assert.AreEqual(modelFile.Strips.Count, savedModelFile.Strips.Count, "Strip counts do not match");

            for (int i = 0; i < modelFile.Strips.Count; i++) {
                Assert.AreEqual(modelFile.Strips[i], savedModelFile.Strips[i], "Strip values do not match");
            }
        }

        [Test]
        public void TestLoadMethod1() {
            TestLoadMethod(TEST_FILE1);
        }

        [Test]
        public void TestLoadMethod2() {
            TestLoadMethod(TEST_FILE2);
        }

        [Test]
        public void TestLoadMethod3() {
            TestLoadMethod(TEST_FILE3);
        }

        [Test]
        public void TestSaveMethod1() {
            TestSaveMethod(TEST_FILE1);
        }

        [Test]
        public void TestSaveMethod2() {
            TestSaveMethod(TEST_FILE2);
        }

        [Test]
        public void TestSaveMethod3() {
            TestSaveMethod(TEST_FILE3);
        }
    }
}
