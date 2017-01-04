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

namespace Revise.Files.EFT {
    /// <summary>
    /// Repreesnts a particle of an effect file.
    /// </summary>
    public class EffectParticle {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the particle.
        /// </summary>
        public string Name {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public string UniqueIdentifier {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the STB row index of the particle.
        /// </summary>
        public int ParticleIndex {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the path of the particle file.
        /// </summary>
        public string FilePath {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the animation is enabled.
        /// </summary>
        public bool AnimationEnabled {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the animation.
        /// </summary>
        public string AnimationName {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the animation loop count value.
        /// </summary>
        public int AnimationLoopCount {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the STB row index of the animation.
        /// </summary>
        public int AnimationIndex {
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
        /// Gets or sets the delay value in milliseconds.
        /// </summary>
        public int Delay {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the object is aligned to the world matrix or to the root object.
        /// </summary>
        public bool LinkedToRoot {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectParticle"/> class.
        /// </summary>
        public EffectParticle() {
            Name = string.Empty;
            UniqueIdentifier = string.Empty;
            FilePath = string.Empty;
            AnimationName = string.Empty;
        }
    }
}