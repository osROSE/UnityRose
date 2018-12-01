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
using Revise.Files.ZSC;
using UnityEngine;

namespace Revise.Files.Tests {
    /// <summary>
    /// Provides testing for the <see cref="ModelListFile"/> class.
    /// </summary>
    [TestFixture]
    public class ModelListFileTests {
        private const string TEST_FILE = "Tests/Revise/Files/LIST_DECO_JPT.ZSC";

        /// <summary>
        /// Tests the load method.
        /// </summary>
        [Test]
        public void TestLoadMethod() {
            Stream stream = File.OpenRead(TEST_FILE);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            ModelListFile modelListFile = new ModelListFile();
            modelListFile.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(fileSize, streamPosition, "Not all of the file was read");
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        [Test]
        public void TestSaveMethod() {
            ModelListFile modelListFile = new ModelListFile();
            modelListFile.Load(TEST_FILE);

            MemoryStream savedStream = new MemoryStream();
            modelListFile.Save(savedStream);

            savedStream.Seek(0, SeekOrigin.Begin);

            ModelListFile savedModelListFile = new ModelListFile();
            savedModelListFile.Load(savedStream);

            savedStream.Close();

            Assert.AreEqual(modelListFile.ModelFiles.Count, savedModelListFile.ModelFiles.Count, "Model file counts do not match");

            for (int i = 0; i < modelListFile.ModelFiles.Count; i++) {
                Assert.AreEqual(modelListFile.ModelFiles[i], savedModelListFile.ModelFiles[i], "Model file paths do not match");
            }

            Assert.AreEqual(modelListFile.TextureFiles.Count, savedModelListFile.TextureFiles.Count, "Texture file counts do not match");

            for (int i = 0; i < modelListFile.TextureFiles.Count; i++) {
                Assert.AreEqual(modelListFile.TextureFiles[i].FilePath, savedModelListFile.TextureFiles[i].FilePath, "Texture file paths do not match");
                Assert.AreEqual(modelListFile.TextureFiles[i].UseSkinShader, savedModelListFile.TextureFiles[i].UseSkinShader, "Texture use skin shader values do not match");
                Assert.AreEqual(modelListFile.TextureFiles[i].AlphaEnabled, savedModelListFile.TextureFiles[i].AlphaEnabled, "Texture alpha enabled values do not match");
                Assert.AreEqual(modelListFile.TextureFiles[i].TwoSided, savedModelListFile.TextureFiles[i].TwoSided, "Texture two sided values do not match");
                Assert.AreEqual(modelListFile.TextureFiles[i].AlphaTestEnabled, savedModelListFile.TextureFiles[i].AlphaTestEnabled, "Texture alpha test enabled values do not match");
                Assert.AreEqual(modelListFile.TextureFiles[i].AlphaReference, savedModelListFile.TextureFiles[i].AlphaReference, "Texture alpha reference values do not match");
                Assert.AreEqual(modelListFile.TextureFiles[i].DepthTestEnabled, savedModelListFile.TextureFiles[i].DepthTestEnabled, "Texture depth test enabled values do not match");
                Assert.AreEqual(modelListFile.TextureFiles[i].DepthWriteEnabled, savedModelListFile.TextureFiles[i].DepthWriteEnabled, "Texture depth write enabled values do not match");
                Assert.AreEqual(modelListFile.TextureFiles[i].BlendType, savedModelListFile.TextureFiles[i].BlendType, "Texture blend type values do not match");
                Assert.AreEqual(modelListFile.TextureFiles[i].UseSpecularShader, savedModelListFile.TextureFiles[i].UseSpecularShader, "Texture use specular shader values do not match");
                Assert.AreEqual(modelListFile.TextureFiles[i].Alpha, savedModelListFile.TextureFiles[i].Alpha, "Texture alpha values do not match");
                Assert.AreEqual(modelListFile.TextureFiles[i].GlowType, savedModelListFile.TextureFiles[i].GlowType, "Texture glow type values do not match");
                Assert.AreEqual(modelListFile.TextureFiles[i].GlowColour, savedModelListFile.TextureFiles[i].GlowColour, "Texture glow colour do not match");
            }

            Assert.AreEqual(modelListFile.EffectFiles.Count, savedModelListFile.EffectFiles.Count, "Effect file counts do not match");

            for (int j = 0; j < modelListFile.EffectFiles.Count; j++) {
                Assert.AreEqual(modelListFile.EffectFiles[j], savedModelListFile.EffectFiles[j], "Effect file paths do not match");
            }

            Assert.AreEqual(modelListFile.Objects.Count, savedModelListFile.Objects.Count, "Object counts do not match");

            for (int i = 0; i < modelListFile.Objects.Count; i++) {
                Assert.AreEqual(modelListFile.Objects[i].Parts.Count, savedModelListFile.Objects[i].Parts.Count, "Object part counts do not match");

                for (int j = 0; j < modelListFile.Objects[i].Parts.Count; j++) {
                    Assert.AreEqual(modelListFile.Objects[i].Parts[j].Model, savedModelListFile.Objects[i].Parts[j].Model, "Part model values do not match");
                    Assert.AreEqual(modelListFile.Objects[i].Parts[j].Texture, savedModelListFile.Objects[i].Parts[j].Texture, "Part texture values do not match");
                    Assert.AreEqual(modelListFile.Objects[i].Parts[j].Rotation, savedModelListFile.Objects[i].Parts[j].Rotation, "Part rotation values do not match");
                    Assert.AreEqual(modelListFile.Objects[i].Parts[j].Scale, savedModelListFile.Objects[i].Parts[j].Scale, "Part scale values do not match");
                    Assert.AreEqual(modelListFile.Objects[i].Parts[j].AxisRotation, savedModelListFile.Objects[i].Parts[j].AxisRotation, "Part axis rotation values do not match");
                    Assert.AreEqual(modelListFile.Objects[i].Parts[j].Parent, savedModelListFile.Objects[i].Parts[j].Parent, "Part parent values do not match");
                    Assert.AreEqual(modelListFile.Objects[i].Parts[j].Collision, savedModelListFile.Objects[i].Parts[j].Collision, "Part collision values do not match");
                    Assert.AreEqual(modelListFile.Objects[i].Parts[j].AnimationFilePath, savedModelListFile.Objects[i].Parts[j].AnimationFilePath, "Part animation file path values do not match");
                    Assert.AreEqual(modelListFile.Objects[i].Parts[j].VisibleRangeSet, savedModelListFile.Objects[i].Parts[j].VisibleRangeSet, "Part visible range set values do not match");
                    Assert.AreEqual(modelListFile.Objects[i].Parts[j].UseLightmap, savedModelListFile.Objects[i].Parts[j].UseLightmap, "Part use lightmap values do not match");
                    Assert.AreEqual(modelListFile.Objects[i].Parts[j].BoneIndex, savedModelListFile.Objects[i].Parts[j].BoneIndex, "Part bone index values do not match");
                    Assert.AreEqual(modelListFile.Objects[i].Parts[j].DummyIndex, savedModelListFile.Objects[i].Parts[j].DummyIndex, "Part dummy index values do not match");

                    // Can't directly compare since fractional value is not exactly zero
                    // Assert.AreEqual(modelListFile.Objects[i].Parts[j].Position, savedModelListFile.Objects[i].Parts[j].Position, "Part position values do not match");
                    Vector3 readPos = modelListFile.Objects[i].Parts[j].Position;
                    Vector3 savedPos = savedModelListFile.Objects[i].Parts[j].Position;
                    Assert.IsTrue(readPos == savedPos);

                    for (int k = 0; k < modelListFile.Objects[i].Parts[j].MonsterAnimations.Length; k++) {
                        Assert.AreEqual(modelListFile.Objects[i].Parts[j].MonsterAnimations[k], savedModelListFile.Objects[i].Parts[j].MonsterAnimations[k], "Part monster animation file path do not match");
                    }

                    for (int k = 0; k < modelListFile.Objects[i].Parts[j].AvatarAnimations.Length; k++) {
                        Assert.AreEqual(modelListFile.Objects[i].Parts[j].AvatarAnimations[k], savedModelListFile.Objects[i].Parts[j].AvatarAnimations[k], "Part avatar animation file path do not match");
                    }
                }

                Assert.AreEqual(modelListFile.Objects[i].Effects.Count, savedModelListFile.Objects[i].Effects.Count, "Object effect counts do not match");

                for (int j = 0; j < modelListFile.Objects[i].Effects.Count; j++) {
                    Assert.AreEqual(modelListFile.Objects[i].Effects[j].EffectType, savedModelListFile.Objects[i].Effects[j].EffectType, "Effect type values do not match");
                    Assert.AreEqual(modelListFile.Objects[i].Effects[j].Effect, savedModelListFile.Objects[i].Effects[j].Effect, "Effect values do not match");
                    Assert.AreEqual(modelListFile.Objects[i].Effects[j].Position, savedModelListFile.Objects[i].Effects[j].Position, "Effect position values do not match");
                    Assert.AreEqual(modelListFile.Objects[i].Effects[j].Rotation, savedModelListFile.Objects[i].Effects[j].Rotation, "Effect rotation values do not match");
                    Assert.AreEqual(modelListFile.Objects[i].Effects[j].Scale, savedModelListFile.Objects[i].Effects[j].Scale, "Effect scale values do not match");
                    Assert.AreEqual(modelListFile.Objects[i].Effects[j].Parent, savedModelListFile.Objects[i].Effects[j].Parent, "Effect parent values do not match");
                }
            }
        }
    }
}
