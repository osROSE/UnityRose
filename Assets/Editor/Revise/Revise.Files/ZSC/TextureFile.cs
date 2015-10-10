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
    /// Represents a model list texture file.
    /// </summary>
    public class TextureFile {
        #region Properties

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        public string FilePath {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether use the skin shader.
        /// </summary>
        public bool UseSkinShader {
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
        public bool TwoSided {
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
        /// Gets or sets the alpha reference values.
        /// </summary>
        public short AlphaReference {
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
        /// Gets or sets the blend type.
        /// </summary>
        public BlendType BlendType {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use the specular reflection shader.
        /// </summary>
        public bool UseSpecularShader {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the alpha value.
        /// </summary>
        public float Alpha {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the glow type.
        /// </summary>
        public GlowType GlowType {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the glow colour.
        /// </summary>
        public Color GlowColour {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureFile"/> class.
        /// </summary>
        public TextureFile() {
            FilePath = string.Empty;
        }
    }
}