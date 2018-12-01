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
    /// Repreesnts an animation of an effect file.
    /// </summary>
    public class EffectAnimation {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the effect.
        /// </summary>
        public string EffectName {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the mesh.
        /// </summary>
        public string MeshName {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the STB row index of the mesh.
        /// </summary>
        public int MeshIndex {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the mesh file path.
        /// </summary>
        public string MeshFilePath {
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
        /// Gets or sets the texture file path.
        /// </summary>
        public string TextureFilePath {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether alpha blending is enabled.
        /// </summary>
        public bool AlphaEnabled {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to draw both sides of the texture.
        /// </summary>
        public bool TwoSidedEnabled {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether alpha testing is enabled.
        /// </summary>
        public bool AlphaTestEnabled {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether depth testing is enabled.
        /// </summary>
        public bool DepthTestEnabled {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether depth writing is enabled.
        /// </summary>
        public bool DepthWriteEnabled {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the source blend value.
        /// </summary>
        public int SourceBlend {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the destination blend value.
        /// </summary>
        public int DestinationBlend {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the blend operation value.
        /// </summary>
        public int BlendOperation {
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
        /// Gets or sets the loop count.
        /// </summary>
        public int LoopCount {
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
        /// Initializes a new instance of the <see cref="EffectAnimation"/> class.
        /// </summary>
        public EffectAnimation() {
            EffectName = string.Empty;
            MeshName = string.Empty;
            MeshFilePath = string.Empty;
            AnimationFilePath = string.Empty;
            TextureFilePath = string.Empty;
            AnimationFilePath = string.Empty;
        }
    }
}