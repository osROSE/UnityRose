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
using Revise.Files.CHR;

namespace Revise.Files.Tests {
    /// <summary>
    /// Provides testing for the <see cref="CharacterFile"/> class.
    /// </summary>
    [TestFixture]
    public class CharacterFileTests {
        private const string TEST_FILE = "Tests/Revise/Files/LIST_NPC.CHR";

        /// <summary>
        /// Tests the load method.
        /// </summary>
        [Test]
        public void TestLoadMethod() {
            Stream stream = File.OpenRead(TEST_FILE);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            CharacterFile characterFile = new CharacterFile();
            characterFile.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(fileSize, streamPosition, "Not all of the file was read");
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        [Test]
        public void TestSaveMethod() {
            CharacterFile characterFile = new CharacterFile();
            characterFile.Load(TEST_FILE);

            MemoryStream savedStream = new MemoryStream();
            characterFile.Save(savedStream);

            savedStream.Seek(0, SeekOrigin.Begin);

            CharacterFile savedCharacterFile = new CharacterFile();
            savedCharacterFile.Load(savedStream);

            savedStream.Close();

            Assert.AreEqual(characterFile.SkeletonFiles.Count, savedCharacterFile.SkeletonFiles.Count, "Skeleton file counts do not match");

            for (int i = 0; i < characterFile.SkeletonFiles.Count; i++) {
                Assert.AreEqual(characterFile.SkeletonFiles[i], savedCharacterFile.SkeletonFiles[i], "Skeleton file names do not match");
            }

            Assert.AreEqual(characterFile.MotionFiles.Count, savedCharacterFile.MotionFiles.Count, "Motion file counts do not match");

            for (int i = 0; i < characterFile.MotionFiles.Count; i++) {
                Assert.AreEqual(characterFile.MotionFiles[i], savedCharacterFile.MotionFiles[i], "Motion file names do not match");
            }

            Assert.AreEqual(characterFile.EffectFiles.Count, savedCharacterFile.EffectFiles.Count, "Effect file counts do not match");

            for (int i = 0; i < characterFile.EffectFiles.Count; i++) {
                Assert.AreEqual(characterFile.EffectFiles[i], savedCharacterFile.EffectFiles[i], "Effect file names do not match");
            }

            Assert.AreEqual(characterFile.Characters.Count, savedCharacterFile.Characters.Count, "Character counts do not match");

            for (int i = 0; i < characterFile.Characters.Count; i++) {
                Assert.AreEqual(characterFile.Characters[i].IsEnabled, savedCharacterFile.Characters[i].IsEnabled, "Character enabled values do not match");

                if (characterFile.Characters[i].IsEnabled) {
                    Assert.AreEqual(characterFile.Characters[i].ID, savedCharacterFile.Characters[i].ID, "Character ID values do not match");
                    Assert.AreEqual(characterFile.Characters[i].Name, savedCharacterFile.Characters[i].Name, "Character name values do not match");

                    Assert.AreEqual(characterFile.Characters[i].Objects.Count, savedCharacterFile.Characters[i].Objects.Count, "Character object counts do not match");

                    for (int j = 0; j < characterFile.Characters[i].Objects.Count; j++) {
                        Assert.AreEqual(characterFile.Characters[i].Objects[j].Object, savedCharacterFile.Characters[i].Objects[j].Object, "Character object values do not match");
                    }

                    Assert.AreEqual(characterFile.Characters[i].Animations.Count, savedCharacterFile.Characters[i].Animations.Count, "Character animation counts do not match");

                    for (int j = 0; j < characterFile.Characters[i].Animations.Count; j++) {
                        Assert.AreEqual(characterFile.Characters[i].Animations[j].Type, savedCharacterFile.Characters[i].Animations[j].Type, "Character animation type values do not match");
                        Assert.AreEqual(characterFile.Characters[i].Animations[j].Animation, savedCharacterFile.Characters[i].Animations[j].Animation, "Character animation values do not match");
                    }

                    Assert.AreEqual(characterFile.Characters[i].Effects.Count, savedCharacterFile.Characters[i].Effects.Count, "Character effect counts do not match");

                    for (int j = 0; j < characterFile.Characters[i].Effects.Count; j++) {
                        Assert.AreEqual(characterFile.Characters[i].Effects[j].Bone, savedCharacterFile.Characters[i].Effects[j].Bone, "Character effect bone values do not match");
                        Assert.AreEqual(characterFile.Characters[i].Effects[j].Effect, savedCharacterFile.Characters[i].Effects[j].Effect, "Character effect values do not match");
                    }
                }
            }
        }
    }
}