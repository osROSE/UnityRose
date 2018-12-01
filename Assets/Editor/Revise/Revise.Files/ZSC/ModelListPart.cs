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

using UnityEngine;

namespace Revise.Files.ZSC {
    /// <summary>
    /// Represents a model list part.
    /// </summary>
    public class ModelListPart {
        #region Constants
        
        public const int MONSTER_ANIMATION_COUNT = 5;
        public const int AVATAR_ANIMATION_COUNT = 16;
        public const int ANIMATION_COUNT = MONSTER_ANIMATION_COUNT + AVATAR_ANIMATION_COUNT;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        public short Model {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the texture.
        /// </summary>
        public short Texture {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        public Vector3 Position {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the rotation.
        /// </summary>
        public Quaternion Rotation {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        public Vector3 Scale {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the axis rotation.
        /// </summary>
        public Quaternion AxisRotation {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        public short Parent {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the collision type and flags.
        /// </summary>
        public CollisionType Collision {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the animation file path.
        /// </summary>
        public string AnimationFilePath {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the visible range set to use from the RangeSet.STB <see cref="DataFile"/>.
        /// </summary>
        public short VisibleRangeSet {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use lightmaps.
        /// </summary>
        public bool UseLightmap {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the bone index.
        /// </summary>
        public short BoneIndex {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the dummy bone index.
        /// </summary>
        public short DummyIndex {
            get;
            set;
        }

        /// <summary>
        /// Gets the monster animation file paths.
        /// </summary>
        public string[] MonsterAnimations {
            get;
            private set;
        }

        /// <summary>
        /// Gets the avatar animation file paths.
        /// </summary>
        public string[] AvatarAnimations {
            get;
            private set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelListPart"/> class.
        /// </summary>
        public ModelListPart() {
            AnimationFilePath = string.Empty;
            MonsterAnimations = new string[MONSTER_ANIMATION_COUNT];
            AvatarAnimations = new string[AVATAR_ANIMATION_COUNT];

            for (int i = 0; i < MONSTER_ANIMATION_COUNT; i++) {
                MonsterAnimations[i] = string.Empty;
            }

            for (int i = 0; i < AVATAR_ANIMATION_COUNT; i++) {
                AvatarAnimations[i] = string.Empty;
            }
        }
    }
}