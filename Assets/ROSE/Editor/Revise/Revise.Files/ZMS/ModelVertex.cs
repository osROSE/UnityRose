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

namespace Revise.Files.ZMS {
    /// <summary>
    /// Represents vertex properties from a model file.
    /// </summary>
    public class ModelVertex {
        #region Properties

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        public Vector3 Position {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the normal value.
        /// </summary>
        public Vector3 Normal {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the colour.
        /// </summary>
        public Color Colour {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the bone weights.
        /// </summary>
        public Vector4 BoneWeights {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the bone indices.
        /// </summary>
        public ShortVector4 BoneIndices {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the tangent.
        /// </summary>
        public Vector3 Tangent {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the texture coordinates.
        /// </summary>
        public Vector2[] TextureCoordinates {
            get;
            private set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelVertex"/> class.
        /// </summary>
        public ModelVertex() {
            TextureCoordinates = new Vector2[4];
        }
    }
}