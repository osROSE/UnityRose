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

namespace Revise.Files.EFT {
    /// <summary>
    /// Provides the ability to create, open and save EFT files for effect data.
    /// </summary>
    public class EffectFile : FileLoader {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the effect.
        /// </summary>
        public string Name {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the sound is enabled.
        /// </summary>
        public bool SoundEnabled {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the path of the sound file.
        /// </summary>
        public string SoundFilePath {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the loop count value.
        /// </summary>
        public int LoopCount {
            get;
            set;
        }

        /// <summary>
        /// Gets the particles.
        /// </summary>
        public List<EffectParticle> Particles {
            get;
            private set;
        }

        /// <summary>
        /// Gets the animations.
        /// </summary>
        public List<EffectAnimation> Animations {
            get;
            private set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectFile"/> class.
        /// </summary>
        public EffectFile() {
            Particles = new List<EffectParticle>();
            Animations = new List<EffectAnimation>();

            Reset();
        }

        /// <summary>
        /// Loads the file from the specified stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        public override void Load(Stream stream) {
            BinaryReader reader = new BinaryReader(stream, Encoding.GetEncoding("EUC-KR"));

            Name = reader.ReadIntString();
            SoundEnabled = reader.ReadInt32() != 0;
            SoundFilePath = reader.ReadIntString();
            LoopCount = reader.ReadInt32();

            int particleCount = reader.ReadInt32();

            for (int i = 0; i < particleCount; i++) {
                EffectParticle particle = new EffectParticle();
                particle.Name = reader.ReadIntString();
                particle.UniqueIdentifier = reader.ReadIntString();
                particle.ParticleIndex = reader.ReadInt32();
                particle.FilePath = reader.ReadIntString();
                particle.AnimationEnabled = reader.ReadInt32() != 0;
                particle.AnimationName = reader.ReadIntString();
                particle.AnimationLoopCount = reader.ReadInt32();
                particle.AnimationIndex = reader.ReadInt32();
                particle.Position = reader.ReadVector3();
                particle.Rotation = reader.ReadQuaternion();
                particle.Delay = reader.ReadInt32();
                particle.LinkedToRoot = reader.ReadInt32() != 0;

                Particles.Add(particle);
            }

            int animationCount = reader.ReadInt32();

            for (int i = 0; i < animationCount; i++) {
                EffectAnimation animation = new EffectAnimation();
                animation.EffectName = reader.ReadIntString();
                animation.MeshName = reader.ReadIntString();
                animation.MeshIndex = reader.ReadInt32();
                animation.MeshFilePath = reader.ReadIntString();
                animation.AnimationFilePath = reader.ReadIntString();
                animation.TextureFilePath = reader.ReadIntString();
                animation.AlphaEnabled = reader.ReadInt32() != 0;
                animation.TwoSidedEnabled = reader.ReadInt32() != 0;
                animation.AlphaTestEnabled = reader.ReadInt32() != 0;
                animation.DepthTestEnabled = reader.ReadInt32() != 0;
                animation.DepthWriteEnabled = reader.ReadInt32() != 0;
                animation.SourceBlend = reader.ReadInt32();
                animation.DestinationBlend = reader.ReadInt32();
                animation.BlendOperation = reader.ReadInt32();
                animation.AnimationEnabled = reader.ReadInt32() != 0;
                animation.AnimationName = reader.ReadIntString();
                animation.AnimationLoopCount = reader.ReadInt32();
                animation.AnimationIndex = reader.ReadInt32();
                animation.Position = reader.ReadVector3();
                animation.Rotation = reader.ReadQuaternion();
                animation.Delay = reader.ReadInt32();
                animation.LoopCount = reader.ReadInt32();
                animation.LinkedToRoot = reader.ReadInt32() != 0;

                Animations.Add(animation);
            }
        }

        /// <summary>
        /// Saves the file to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to save to.</param>
        public override void Save(Stream stream) {
            BinaryWriter writer = new BinaryWriter(stream, Encoding.GetEncoding("EUC-KR"));

            writer.WriteIntString(Name);
            writer.Write(SoundEnabled ? 1 : 0);
            writer.WriteIntString(SoundFilePath);
            writer.Write(LoopCount);

            writer.Write(Particles.Count);

            Particles.ForEach(particle => {
                writer.WriteIntString(particle.Name);
                writer.WriteIntString(particle.UniqueIdentifier);
                writer.Write(particle.ParticleIndex);
                writer.WriteIntString(particle.FilePath);
                writer.Write(particle.AnimationEnabled ? 1 : 0);
                writer.WriteIntString(particle.AnimationName);
                writer.Write(particle.AnimationLoopCount);
                writer.Write(particle.AnimationIndex);
                writer.Write(particle.Position);
                writer.Write(particle.Rotation);
                writer.Write(particle.Delay);
                writer.Write(particle.LinkedToRoot ? 1 : 0);
            });

            writer.Write(Animations.Count);

            Animations.ForEach(animation => {
                writer.WriteIntString(animation.EffectName);
                writer.WriteIntString(animation.MeshName);
                writer.Write(animation.MeshIndex);
                writer.WriteIntString(animation.MeshFilePath);
                writer.WriteIntString(animation.AnimationFilePath);
                writer.WriteIntString(animation.TextureFilePath);
                writer.Write(animation.AlphaEnabled ? 1 : 0);
                writer.Write(animation.TwoSidedEnabled ? 1 : 0);
                writer.Write(animation.AlphaTestEnabled ? 1 : 0);
                writer.Write(animation.DepthTestEnabled ? 1 : 0);
                writer.Write(animation.DepthWriteEnabled ? 1 : 0);
                writer.Write(animation.SourceBlend);
                writer.Write(animation.DestinationBlend);
                writer.Write(animation.BlendOperation);
                writer.Write(animation.AnimationEnabled ? 1 : 0);
                writer.WriteIntString(animation.AnimationName);
                writer.Write(animation.AnimationLoopCount);
                writer.Write(animation.AnimationIndex);
                writer.Write(animation.Position);
                writer.Write(animation.Rotation);
                writer.Write(animation.Delay);
                writer.Write(animation.LoopCount);
                writer.Write(animation.LinkedToRoot ? 1 : 0);
            });
        }

        /// <summary>
        /// Removes all particles and animations.
        /// </summary>
        public void Clear() {
            Particles.Clear();
            Animations.Clear();
        }

        /// <summary>
        /// Resets properties to their default values.
        /// </summary>
        public override void Reset() {
            base.Reset();

            Name = string.Empty;
            SoundEnabled = false;
            SoundFilePath = string.Empty;
            LoopCount = 0;

            Clear();
        }
    }
}