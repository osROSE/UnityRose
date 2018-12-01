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
using Revise.Files.ZCA;

namespace Revise.Files.Tests {
    /// <summary>
    /// Provides testing for the <see cref="CameraFile"/> class.
    /// </summary>
    [TestFixture]
    public class CameraFileTests {
        private const string TEST_FILE = "Tests/Revise/Files/CAMERA.ZCA";

        /// <summary>
        /// Tests the load method.
        /// </summary>
        [Test]
        public void TestLoadMethod() {
            Stream stream = File.OpenRead(TEST_FILE);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            CameraFile cameraFile = new CameraFile();
            cameraFile.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(fileSize, streamPosition, "Not all of the file was read");
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        [Test]
        public void TestSaveMethod() {
            CameraFile cameraFile = new CameraFile();
            cameraFile.Load(TEST_FILE);

            MemoryStream savedStream = new MemoryStream();
            cameraFile.Save(savedStream);

            savedStream.Seek(0, SeekOrigin.Begin);

            CameraFile savedCameraFile = new CameraFile();
            savedCameraFile.Load(savedStream);

            savedStream.Close();

            Assert.AreEqual(cameraFile.ProjectionType, savedCameraFile.ProjectionType, "Projection types do not match");
            Assert.AreEqual(cameraFile.ModelView, savedCameraFile.ModelView, "Model view matrices do not match");
            Assert.AreEqual(cameraFile.Projection, savedCameraFile.Projection, "Projection matrices do not match");
            Assert.AreEqual(cameraFile.FieldOfView, savedCameraFile.FieldOfView, "Field of view values do not match");
            Assert.AreEqual(cameraFile.AspectRatio, savedCameraFile.AspectRatio, "Aspect ratio values do not match");
            Assert.AreEqual(cameraFile.NearPlane, savedCameraFile.NearPlane, "Near plane values do not match");
            Assert.AreEqual(cameraFile.FarPlane, savedCameraFile.FarPlane, "Far plane values do not match");
            Assert.AreEqual(cameraFile.Eye, savedCameraFile.Eye, "Eye positions do not match");
            Assert.AreEqual(cameraFile.Center, savedCameraFile.Center, "Center position do not match");
            Assert.AreEqual(cameraFile.Up, savedCameraFile.Up, "Up positions do not match");
        }
    }
}