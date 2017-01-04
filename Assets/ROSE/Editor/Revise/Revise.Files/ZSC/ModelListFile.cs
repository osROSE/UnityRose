#region License

/**
 * Copyright (C) 2012 Jack Wakefield
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

using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Revise.Files.ZSC {
	/// <summary>
	/// Provides the ability to create, open and save ZSC files for model data.
	/// </summary>
	public class ModelListFile : FileLoader {
		#region Properties

		/// <summary>
		/// Gets the model files.
		/// </summary>
		public List<string> ModelFiles {
			get;
			private set;
		}

		/// <summary>
		/// Gets the texture files.
		/// </summary>
		public List<TextureFile> TextureFiles {
			get;
			private set;
		}

		/// <summary>
		/// Gets the effect files.
		/// </summary>
		public List<string> EffectFiles {
			get;
			private set;
		}

		/// <summary>
		/// Gets the objects.
		/// </summary>
		public List<ModelListObject> Objects {
			get;
			private set;
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="ModelListFile"/> class.
		/// </summary>
		public ModelListFile() {
			ModelFiles = new List<string>();
			TextureFiles = new List<TextureFile>();
			EffectFiles = new List<string>();
			Objects = new List<ModelListObject>();

			Reset();
		}

		/// <summary>
		/// Loads the file from the specified stream.
		/// </summary>
		/// <param name="stream">The stream to read from.</param>
		public override void Load(Stream stream) {
			BinaryReader reader = new BinaryReader(stream, Encoding.GetEncoding("EUC-KR"));

			short modelFileCount = reader.ReadInt16();

			for (int i = 0; i < modelFileCount; i++) {
				string modelFile = reader.ReadNullTerminatedString();

				ModelFiles.Add(modelFile);
			}

			short textureFileCount = reader.ReadInt16();

			for (int i = 0; i < textureFileCount; i++) {
				TextureFile texture = new TextureFile();
				texture.FilePath = reader.ReadNullTerminatedString();
				texture.UseSkinShader = reader.ReadInt16() != 0;
				texture.AlphaEnabled = reader.ReadInt16() != 0;
				texture.TwoSided = reader.ReadInt16() != 0;
				texture.AlphaTestEnabled = reader.ReadInt16() != 0;
				texture.AlphaReference = reader.ReadInt16();
				texture.DepthTestEnabled = reader.ReadInt16() != 0;
				texture.DepthWriteEnabled = reader.ReadInt16() != 0;
				texture.BlendType = (BlendType)reader.ReadInt16();
				texture.UseSpecularShader = reader.ReadInt16() != 0;
				texture.Alpha = reader.ReadSingle();
				texture.GlowType = (GlowType)reader.ReadInt16();
				texture.GlowColour = reader.ReadColour3();

				TextureFiles.Add(texture);
			}

			short effectFileCount = reader.ReadInt16();

			for (int i = 0; i < effectFileCount; i++) {
				string effectFile = reader.ReadNullTerminatedString();

				EffectFiles.Add(effectFile);
			}

			short objectCount = reader.ReadInt16();

			for (int i = 0; i < objectCount; i++) {
				ModelListObject @object = new ModelListObject();

				int cylinderRadius = reader.ReadInt32();
				@object.BoundingCylinder = new BoundingCylinder(new Vector2(reader.ReadInt32(), reader.ReadInt32()), cylinderRadius);

				int partCount = reader.ReadInt16();

				if (partCount > 0) {
					for (int j = 0; j < partCount; j++) {
						ModelListPart part = new ModelListPart();
						part.Model = reader.ReadInt16();
						part.Texture = reader.ReadInt16();

						byte propertyType = 0;

						while ((propertyType = reader.ReadByte()) != 0) {
							byte size = reader.ReadByte();

							switch ((ModelListPropertyType)propertyType) {
								case ModelListPropertyType.Position:
									part.Position = reader.ReadVector3();
									break;
								case ModelListPropertyType.Rotation:
									part.Rotation = reader.ReadQuaternion(true);
									break;
								case ModelListPropertyType.Scale:
									part.Scale = reader.ReadVector3();
									break;
								case ModelListPropertyType.AxisRotation:
									part.AxisRotation = reader.ReadQuaternion(true);
									break;
								case ModelListPropertyType.Parent:
									part.Parent = reader.ReadInt16();
									break;
								case ModelListPropertyType.Collision:
									part.Collision = (CollisionType)reader.ReadInt16();
									break;
								case ModelListPropertyType.ConstantAnimation:
									part.AnimationFilePath = reader.ReadString(size);
									break;
								case ModelListPropertyType.VisibleRangeSet:
									part.VisibleRangeSet = reader.ReadInt16();
									break;
								case ModelListPropertyType.UseLightmap:
									part.UseLightmap = reader.ReadInt16() != 0;
									break;
								case ModelListPropertyType.BoneIndex:
									part.BoneIndex = reader.ReadInt16();
									break;
								case ModelListPropertyType.DummyIndex:
									part.DummyIndex = reader.ReadInt16();
									break;
								default:
									if (propertyType >= (int)ModelListPropertyType.Animation && propertyType < (int)ModelListPropertyType.Animation + ModelListPart.ANIMATION_COUNT) {
										propertyType -= (int)ModelListPropertyType.Animation;

										if (propertyType < ModelListPart.MONSTER_ANIMATION_COUNT) {
											part.MonsterAnimations[propertyType] = reader.ReadString(size);
										} else {
											propertyType -= ModelListPart.MONSTER_ANIMATION_COUNT;
											part.AvatarAnimations[propertyType] = reader.ReadString(size);
										}
									} else {
										stream.Seek(size, SeekOrigin.Current);
									}
									break;
							}
						}

						@object.Parts.Add(part);
					}

					int effectCount = reader.ReadInt16();

					for (int j = 0; j < effectCount; j++) {
						ModelListEffect effect = new ModelListEffect();
						effect.EffectType = (EffectType)reader.ReadInt16();
						effect.Effect = reader.ReadInt16();

						byte propertyType = 0;

						while ((propertyType = reader.ReadByte()) != 0) {
							byte size = reader.ReadByte();

							switch ((ModelListPropertyType)propertyType) {
								case ModelListPropertyType.Position:
									effect.Position = reader.ReadVector3();
									break;
								case ModelListPropertyType.Rotation:
									effect.Rotation = reader.ReadQuaternion(true);
									break;
								case ModelListPropertyType.Scale:
									effect.Scale = reader.ReadVector3();
									break;
								case ModelListPropertyType.Parent:
									effect.Parent = reader.ReadInt16();
									break;
								default:
									stream.Seek(size, SeekOrigin.Current);
									break;
							}
						}

						@object.Effects.Add(effect);
					}

					@object.BoundingBox = new Bounds(reader.ReadVector3(), reader.ReadVector3());
				}

				Objects.Add(@object);
			}
		}

		/// <summary>
		/// Saves the file to the specified stream.
		/// </summary>
		/// <param name="stream">The stream to save to.</param>
		public override void Save(Stream stream) {
			BinaryWriter writer = new BinaryWriter(stream, Encoding.GetEncoding("EUC-KR"));

			writer.Write((short)ModelFiles.Count);

			ModelFiles.ForEach(modelFile => {
				writer.WriteString(modelFile);
				writer.Write((byte)0);
			});

			writer.Write((short)TextureFiles.Count);

			TextureFiles.ForEach(texture => {
				writer.WriteString(texture.FilePath);
				writer.Write((byte)0);
				writer.Write((short)(texture.UseSkinShader ? 1 : 0));
				writer.Write((short)(texture.AlphaEnabled ? 1 : 0));
				writer.Write((short)(texture.TwoSided ? 1 : 0));
				writer.Write((short)(texture.AlphaTestEnabled ? 1 : 0));
				writer.Write(texture.AlphaReference);
				writer.Write((short)(texture.DepthTestEnabled ? 1 : 0));
				writer.Write((short)(texture.DepthWriteEnabled ? 1 : 0));
				writer.Write((short)texture.BlendType);
				writer.Write((short)(texture.UseSpecularShader ? 1 : 0));
				writer.Write(texture.Alpha);
				writer.Write((short)texture.GlowType);
				writer.Write(texture.GlowColour.r);
				writer.Write(texture.GlowColour.g);
				writer.Write(texture.GlowColour.b);
				writer.Write(texture.GlowColour.a);
			});

			writer.Write((short)EffectFiles.Count);

			EffectFiles.ForEach(effectFile => {
				writer.WriteString(effectFile);
				writer.Write((byte)0);
			});

			writer.Write((short)Objects.Count);

			Objects.ForEach(@object => {
				writer.Write((int)@object.BoundingCylinder.Radius);
				writer.Write((int)@object.BoundingCylinder.Center.x);
				writer.Write((int)@object.BoundingCylinder.Center.y);

				writer.Write((short)@object.Parts.Count);

				if (@object.Parts.Count > 0) {
					@object.Parts.ForEach(part => {
						writer.Write(part.Model);
						writer.Write(part.Texture);

						if (part.Position != Vector3.zero) {
							writer.Write((byte)ModelListPropertyType.Position);
							writer.Write((byte)(sizeof(float) * 3));
							writer.Write(part.Position);
						}

						if (part.Rotation != Quaternion.identity)
						{
							writer.Write((byte)ModelListPropertyType.Rotation);
							writer.Write((byte)(sizeof(float) * 4));
							writer.Write(part.Rotation, true);
						}

						if (part.Scale != Vector3.zero)
						{
							writer.Write((byte)ModelListPropertyType.Scale);
							writer.Write((byte)(sizeof(float) * 3));
							writer.Write(part.Scale);
						}

						if (part.AxisRotation != Quaternion.identity)
						{
							writer.Write((byte)ModelListPropertyType.AxisRotation);
							writer.Write((byte)(sizeof(float) * 4));
							writer.Write(part.AxisRotation, true);
						}

						if (part.Parent != 0) {
							writer.Write((byte)ModelListPropertyType.Parent);
							writer.Write((byte)sizeof(short));
							writer.Write(part.Parent);
						}

						if (part.Collision != 0) {
							writer.Write((byte)ModelListPropertyType.Collision);
							writer.Write((byte)sizeof(short));
							writer.Write((short)part.Collision);
						}

						if (string.Compare(part.AnimationFilePath, string.Empty) != 0) {
							writer.Write((byte)ModelListPropertyType.ConstantAnimation);
							writer.WriteByteString(part.AnimationFilePath);
						}

						if (part.VisibleRangeSet != 0) {
							writer.Write((byte)ModelListPropertyType.VisibleRangeSet);
							writer.Write((byte)sizeof(short));
							writer.Write(part.VisibleRangeSet);
						}

						if (!part.UseLightmap) {
							writer.Write((byte)ModelListPropertyType.UseLightmap);
							writer.Write((byte)sizeof(short));
							writer.Write((short)0);
						}

						if (part.BoneIndex != 0) {
							writer.Write((byte)ModelListPropertyType.BoneIndex);
							writer.Write((byte)sizeof(short));
							writer.Write(part.BoneIndex);
						}

						if (part.DummyIndex != 0) {
							writer.Write((byte)ModelListPropertyType.DummyIndex);
							writer.Write((byte)sizeof(short));
							writer.Write(part.DummyIndex);
						}

						for (int i = 0; i < part.MonsterAnimations.Length; i++) {
							string animationFile = part.MonsterAnimations[i];

							if (string.Compare(animationFile, string.Empty) != 0) {
								writer.Write((byte)((byte)ModelListPropertyType.Animation + i));
								writer.WriteByteString(animationFile);
							}
						}

						for (int i = 0; i < part.AvatarAnimations.Length; i++) {
							string animationFile = part.AvatarAnimations[i];

							if (string.Compare(animationFile, string.Empty) != 0) {
								writer.Write((byte)((byte)ModelListPropertyType.Animation + ModelListPart.MONSTER_ANIMATION_COUNT + i));
								writer.WriteByteString(animationFile);
							}
						}

						writer.Write((byte)0);
					});

					writer.Write((short)@object.Effects.Count);

					@object.Effects.ForEach(effect => {
						writer.Write((short)effect.EffectType);
						writer.Write(effect.Effect);

						if (effect.Position != Vector3.zero) {
							writer.Write((byte)ModelListPropertyType.Position);
							writer.Write((byte)(sizeof(float) * 3));
							writer.Write(effect.Position);
						}

						if (effect.Rotation != Quaternion.identity) {
							writer.Write((byte)ModelListPropertyType.Rotation);
							writer.Write((byte)(sizeof(float) * 4));
							writer.Write(effect.Rotation, true);
						}

						if (effect.Scale != Vector3.zero)
						{
							writer.Write((byte)ModelListPropertyType.Scale);
							writer.Write((byte)(sizeof(float) * 3));
							writer.Write(effect.Scale);
						}

						if (effect.Parent != 0) {
							writer.Write((byte)ModelListPropertyType.Parent);
							writer.Write((byte)sizeof(short));
							writer.Write(effect.Parent);
						}

						writer.Write((byte)0);
					});

					writer.Write(@object.BoundingBox.min);
					writer.Write(@object.BoundingBox.max);
				}
			});
		}

		/// <summary>
		/// Clears all file systems.
		/// </summary>
		public void Clear() {
			ModelFiles.Clear();
			TextureFiles.Clear();
			EffectFiles.Clear();
			Objects.Clear();
		}

		/// <summary>
		/// Resets properties to their default values.
		/// </summary>
		public override void Reset() {
			base.Reset();
			Clear();
		}
	}
}