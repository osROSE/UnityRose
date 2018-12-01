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

namespace Revise.Files.CHR {
    /// <summary>
    /// Provides the ability to create, open and save CHR files for character data.
    /// </summary>
    public class CharacterFile : FileLoader {
        #region Properties

        /// <summary>
        /// Gets the skeleton files.
        /// </summary>
        public List<string> SkeletonFiles {
            get;
            private set;
        }

        /// <summary>
        /// Gets the motion files.
        /// </summary>
        public List<string> MotionFiles {
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
        /// Gets the characters.
        /// </summary>
        public List<Character> Characters {
            get;
            private set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterFile"/> class.
        /// </summary>
        public CharacterFile() {
            SkeletonFiles = new List<string>();
            MotionFiles = new List<string>();
            EffectFiles = new List<string>();
            Characters = new List<Character>();

            Reset();
        }

        /// <summary>
        /// Loads the file from the specified stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        public override void Load(Stream stream) {
            BinaryReader reader = new BinaryReader(stream, Encoding.GetEncoding("EUC-KR"));

            short skeletonFileCount = reader.ReadInt16();

            for (int i = 0; i < skeletonFileCount; i++) {
                string skeletonFile = reader.ReadNullTerminatedString();

                SkeletonFiles.Add(skeletonFile);
            }

            short motionFileCount = reader.ReadInt16();

            for (int i = 0; i < motionFileCount; i++) {
                string motionFile = reader.ReadNullTerminatedString();

                MotionFiles.Add(motionFile);
            }

            short effectFileCount = reader.ReadInt16();

            for (int i = 0; i < effectFileCount; i++) {
                string effectFile = reader.ReadNullTerminatedString();

                EffectFiles.Add(effectFile);
            }

            short characterCount = reader.ReadInt16();

            for (int i = 0; i < characterCount; i++) {
                Character character = new Character();
                character.IsEnabled = reader.ReadBoolean();

                if (character.IsEnabled) {
                    character.ID = reader.ReadInt16();
                    character.Name = reader.ReadNullTerminatedString();

                    short objectCount = reader.ReadInt16();

                    for (int j = 0; j < objectCount; j++) {
                        CharacterObject @object = new CharacterObject();
                        @object.Object = reader.ReadInt16();

                        character.Objects.Add(@object);
                    }

                    short animationCount = reader.ReadInt16();

                    for (int j = 0; j < animationCount; j++) {
                        CharacterAnimation animation = new CharacterAnimation();
                        animation.Type = (AnimationType)reader.ReadInt16();
                        animation.Animation = reader.ReadInt16();

                        character.Animations.Add(animation);
                    }

                    short effectCount = reader.ReadInt16();

                    for (int j = 0; j < effectCount; j++) {
                        CharacterEffect effect = new CharacterEffect();
                        effect.Bone = reader.ReadInt16();
                        effect.Effect = reader.ReadInt16();

                        character.Effects.Add(effect);
                    }
                }

                Characters.Add(character);
            }
        }

        /// <summary>
        /// Saves the file to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to save to.</param>
        public override void Save(Stream stream) {
            BinaryWriter writer = new BinaryWriter(stream, Encoding.GetEncoding("EUC-KR"));

            writer.Write((short)SkeletonFiles.Count);

            SkeletonFiles.ForEach(file => {
                writer.WriteString(file);
                writer.Write((byte)0);
            });

            writer.Write((short)MotionFiles.Count);

            MotionFiles.ForEach(file => {
                writer.WriteString(file);
                writer.Write((byte)0);
            });

            writer.Write((short)EffectFiles.Count);

            EffectFiles.ForEach(file => {
                writer.WriteString(file);
                writer.Write((byte)0);
            });

            writer.Write((short)Characters.Count);

            Characters.ForEach(character => {
                writer.Write(character.IsEnabled);

                if (character.IsEnabled) {
                    writer.Write(character.ID);
                    writer.WriteString(character.Name);
                    writer.Write((byte)0);

                    writer.Write((short)character.Objects.Count);

                    character.Objects.ForEach(@object => {
                        writer.Write(@object.Object);
                    });

                    writer.Write((short)character.Animations.Count);

                    character.Animations.ForEach(animation => {
                        writer.Write((short)animation.Type);
                        writer.Write(animation.Animation);
                    });

                    writer.Write((short)character.Effects.Count);

                    character.Effects.ForEach(effect => {
                        writer.Write(effect.Bone);
                        writer.Write(effect.Effect);
                    });
                }
            });
        }

        /// <summary>
        /// Clears all file systems.
        /// </summary>
        public void Clear() {
            SkeletonFiles.Clear();
            MotionFiles.Clear();
            EffectFiles.Clear();
            Characters.Clear();
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