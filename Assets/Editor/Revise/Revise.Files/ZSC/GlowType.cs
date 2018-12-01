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

namespace Revise.Files.ZSC {
    /// <summary>
    /// Defines the glow types.
    /// </summary>
    public enum GlowType {
        /// <summary>
        /// Invalid.
        /// </summary>
        None = 0,

        /// <summary>
        /// Not set, use the visible's default settings.
        /// </summary>
        NotSet = 1,

        /// <summary>
        /// Simple colour glow.
        /// </summary>
        Simple = 2,

        /// <summary>
        /// Lightmap colour glow.
        /// </summary>
        Light = 3,

        /// <summary>
        /// Colour multiplied by texture glow.
        /// </summary>
        Texture = 4,

        /// <summary>
        /// Lightmap colour multiplied by texture glow.
        /// </summary>
        LightTexture = 5,

        /// <summary>
        /// Alpha glow.
        /// </summary>
        Alpha = 6
    }
}