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
 * MERCHANTABILITY or FITNESS FOR AD:\Code\Revise\Revise.Files Tests\EFTTests.cs PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

#endregion

using System.IO;
using NUnit.Framework;
using Revise.Files.EFT;

namespace Revise.Files.Tests {
    /// <summary>
    /// Provides testing for the <see cref="EffectFile"/> class.
    /// </summary>
    [TestFixture]
    public class EffectFileTests {
        private const string TEST_FILE = "Tests/Revise/Files/_RUNASTON_01.EFT";

        /// <summary>
        /// Tests the load method.
        /// </summary>
        [Test]
        public void TestLoadMethod() {
            Stream stream = File.OpenRead(TEST_FILE);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            EffectFile effectFile = new EffectFile();
            effectFile.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(fileSize, streamPosition, "Not all of the file was read");
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        [Test]
        public void TestSaveMethod() {
            EffectFile effectFile = new EffectFile();
            effectFile.Load(TEST_FILE);

            MemoryStream savedStream = new MemoryStream();
            effectFile.Save(savedStream);

            savedStream.Seek(0, SeekOrigin.Begin);

            EffectFile savedEffectFile = new EffectFile();
            savedEffectFile.Load(savedStream);

            savedStream.Close();

            Assert.AreEqual(effectFile.Name, savedEffectFile.Name, "Name values do not match");
            Assert.AreEqual(effectFile.SoundEnabled, savedEffectFile.SoundEnabled, "Sound enable values do not match");
            Assert.AreEqual(effectFile.SoundFilePath, savedEffectFile.SoundFilePath, "Sound file path values do not match");
            Assert.AreEqual(effectFile.LoopCount, savedEffectFile.LoopCount, "Loop count values do not match");

            Assert.AreEqual(effectFile.Particles.Count, savedEffectFile.Particles.Count, "Particle count values do not match");

            for (int i = 0; i < effectFile.Particles.Count; i++) {
                Assert.AreEqual(effectFile.Particles[i].Name, savedEffectFile.Particles[i].Name, "Particle name values do not match");
                Assert.AreEqual(effectFile.Particles[i].UniqueIdentifier, savedEffectFile.Particles[i].UniqueIdentifier, "Particle unique identifier values do not match");
                Assert.AreEqual(effectFile.Particles[i].ParticleIndex, savedEffectFile.Particles[i].ParticleIndex, "Particle particle index values do not match");
                Assert.AreEqual(effectFile.Particles[i].FilePath, savedEffectFile.Particles[i].FilePath, "Particle file path values do not match");
                Assert.AreEqual(effectFile.Particles[i].AnimationEnabled, savedEffectFile.Particles[i].AnimationEnabled, "Particle animation enabled values do not match");
                Assert.AreEqual(effectFile.Particles[i].AnimationName, savedEffectFile.Particles[i].AnimationName, "Particle animation name values do not match");
                Assert.AreEqual(effectFile.Particles[i].AnimationLoopCount, savedEffectFile.Particles[i].AnimationLoopCount, "Particle animation loop count values do not match");
                Assert.AreEqual(effectFile.Particles[i].AnimationIndex, savedEffectFile.Particles[i].AnimationIndex, "Particle animation index values do not match");
                Assert.AreEqual(effectFile.Particles[i].Position, savedEffectFile.Particles[i].Position, "Particle position values do not match");
                Assert.AreEqual(effectFile.Particles[i].Rotation, savedEffectFile.Particles[i].Rotation, "Particle rotation values do not match");
                Assert.AreEqual(effectFile.Particles[i].Delay, savedEffectFile.Particles[i].Delay, "Particle delay values do not match");
                Assert.AreEqual(effectFile.Particles[i].LinkedToRoot, savedEffectFile.Particles[i].LinkedToRoot, "Particle link to root values do not match");
            }

            Assert.AreEqual(effectFile.Animations.Count, savedEffectFile.Animations.Count, "Animation count values do not match");

            for (int i = 0; i < effectFile.Animations.Count; i++) {
                Assert.AreEqual(effectFile.Animations[i].EffectName, savedEffectFile.Animations[i].EffectName, "Animation effect name values do not match");
                Assert.AreEqual(effectFile.Animations[i].MeshName, savedEffectFile.Animations[i].MeshName, "Animation mesh name values do not match");
                Assert.AreEqual(effectFile.Animations[i].MeshIndex, savedEffectFile.Animations[i].MeshIndex, "Animation mesh index values do not match");
                Assert.AreEqual(effectFile.Animations[i].MeshFilePath, savedEffectFile.Animations[i].MeshFilePath, "Animation mesh file path values do not match");
                Assert.AreEqual(effectFile.Animations[i].AnimationFilePath, savedEffectFile.Animations[i].AnimationFilePath, "Animation animation file path values do not match");
                Assert.AreEqual(effectFile.Animations[i].TextureFilePath, savedEffectFile.Animations[i].TextureFilePath, "Animation texture file path values do not match");
                Assert.AreEqual(effectFile.Animations[i].AlphaEnabled, savedEffectFile.Animations[i].AlphaEnabled, "Animation alpha enabled values do not match");
                Assert.AreEqual(effectFile.Animations[i].TwoSidedEnabled, savedEffectFile.Animations[i].TwoSidedEnabled, "Animation two sided enabled values do not match");
                Assert.AreEqual(effectFile.Animations[i].AlphaTestEnabled, savedEffectFile.Animations[i].AlphaTestEnabled, "Animation alpha test enabled values do not match");
                Assert.AreEqual(effectFile.Animations[i].DepthTestEnabled, savedEffectFile.Animations[i].DepthTestEnabled, "Animation depth test enabled values do not match");
                Assert.AreEqual(effectFile.Animations[i].DepthWriteEnabled, savedEffectFile.Animations[i].DepthWriteEnabled, "Animation depth write enabled values do not match");
                Assert.AreEqual(effectFile.Animations[i].SourceBlend, savedEffectFile.Animations[i].SourceBlend, "Animation source blend values do not match");
                Assert.AreEqual(effectFile.Animations[i].DestinationBlend, savedEffectFile.Animations[i].DestinationBlend, "Animation destination blend values do not match");
                Assert.AreEqual(effectFile.Animations[i].BlendOperation, savedEffectFile.Animations[i].BlendOperation, "Animation blend operation values do not match");
                Assert.AreEqual(effectFile.Animations[i].AnimationEnabled, savedEffectFile.Animations[i].AnimationEnabled, "Animation animation enabled values do not match");
                Assert.AreEqual(effectFile.Animations[i].AnimationName, savedEffectFile.Animations[i].AnimationName, "Animation animation name values do not match");
                Assert.AreEqual(effectFile.Animations[i].AnimationLoopCount, savedEffectFile.Animations[i].AnimationLoopCount, "Animation animation loop count values do not match");
                Assert.AreEqual(effectFile.Animations[i].AnimationIndex, savedEffectFile.Animations[i].AnimationIndex, "Animation animation index values do not match");
                Assert.AreEqual(effectFile.Animations[i].Position, savedEffectFile.Animations[i].Position, "Animation position values do not match");
                Assert.AreEqual(effectFile.Animations[i].Rotation, savedEffectFile.Animations[i].Rotation, "Animation rotation values do not match");
                Assert.AreEqual(effectFile.Animations[i].Delay, savedEffectFile.Animations[i].Delay, "Animation delay values do not match");
                Assert.AreEqual(effectFile.Animations[i].LoopCount, savedEffectFile.Animations[i].LoopCount, "Animation loop count values do not match");
                Assert.AreEqual(effectFile.Animations[i].LinkedToRoot, savedEffectFile.Animations[i].LinkedToRoot, "Animation link to root values do not match");
            }
        }   
    }
}