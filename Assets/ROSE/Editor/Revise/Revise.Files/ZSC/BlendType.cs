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
    /// Defines the texture blend types.
    /// </summary>
    public enum BlendType {
        /// <summary>
        /// The source blend mode is set to source alpha and destination to inverse source alpha, the blend operation is set to add.
        /// </summary>
        Normal = 0,

        /// <summary>
        /// The source and destination blend modes are set to one, the blend operation set to add.
        /// </summary>
        Lighten = 1,

        /// <summary>
        /// Custom blend types set by the client.
        /// </summary>
        Custom = 3,

        /// <summary>
        /// Skips setting the blending types.
        /// </summary>
        None = 255
    }
}