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
using System.Linq;
using System.Text;

namespace Revise.Files.TSI {
    /// <summary>
    /// Provides the ability to create, open and save TSI files used for sprite information.
    /// </summary>
    public class SpriteFile : FileLoader {
        #region Properties

        public List<SpriteTexture> Textures {
            get;
            private set;
        }

        public List<Sprite> Sprites {
            get;
            private set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteFile"/> class.
        /// </summary>
        public SpriteFile() {
            Textures = new List<SpriteTexture>();
            Sprites = new List<Sprite>();

            Reset();
        }

        /// <summary>
        /// Loads the file from the specified stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        public override void Load(Stream stream) {
            BinaryReader reader = new BinaryReader(stream, Encoding.GetEncoding("EUC-KR"));

            short textureCount = reader.ReadInt16();

            for (int i = 0; i < textureCount; i++) {
                SpriteTexture texture = new SpriteTexture();
                texture.FileName = reader.ReadShortString();
                texture.ColourKey = reader.ReadInt32();

                Textures.Add(texture);
            }

            short totalSpriteCount = reader.ReadInt16();

            for (int i = 0; i < textureCount; i++) {
                short spriteCount = reader.ReadInt16();

                for (int j = 0; j < spriteCount; j++) {
                    Sprite sprite = new Sprite();
                    sprite.Texture = reader.ReadInt16();
                    sprite.X1 = reader.ReadInt32();
                    sprite.Y1 = reader.ReadInt32();
                    sprite.X2 = reader.ReadInt32();
                    sprite.Y2 = reader.ReadInt32();
                    sprite.Colour = reader.ReadInt32();
                    sprite.ID = reader.ReadString(32).TrimEnd('\0');

                    Sprites.Add(sprite);
                }
            }
        }

        /// <summary>
        /// Saves the file to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to save to.</param>
        public override void Save(Stream stream) {
            BinaryWriter writer = new BinaryWriter(stream, Encoding.GetEncoding("EUC-KR"));

            writer.Write((short)Textures.Count);

            Textures.ForEach(texture => {
                writer.WriteShortString(texture.FileName);
                writer.Write(texture.ColourKey);
            });

            writer.Write((short)Sprites.Count);

            for (int i = 0; i < Textures.Count; i++) {
                var sprites = from s in Sprites
                              where s.Texture == i
                              select s;

                writer.Write((short)sprites.Count());

                foreach (Sprite sprite in sprites) {
                    writer.Write(sprite.Texture);
                    writer.Write(sprite.X1);
                    writer.Write(sprite.Y1);
                    writer.Write(sprite.X2);
                    writer.Write(sprite.Y2);
                    writer.Write(sprite.Colour);
                    writer.WriteString(sprite.ID, 32);
                }
            }
        }

        /// <summary>
        /// Resets properties to their default values.
        /// </summary>
        public override void Reset() {
            base.Reset();

            Textures.Clear();
        }
    }
}