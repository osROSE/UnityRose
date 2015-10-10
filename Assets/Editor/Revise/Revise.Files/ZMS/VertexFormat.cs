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

using System;

namespace Revise.Files.ZMS {
    /// <summary>
    /// Specifies the properties of a vertex.
    /// </summary>
    [Flags]
    internal enum VertexFormat {
        None = 1 << 0,
        Position = 1 << 1,
        Normal = 1 << 2,
        Colour = 1 << 3,
        BlendWeight = 1 << 4,
        BlendIndex = 1 << 5,
        Tangent = 1 << 6,
        TextureCoordinate1 = 1 << 7,
        TextureCoordinate2 = 1 << 8,
        TextureCoordinate3 = 1 << 9,
        TextureCoordinate4 = 1 << 10,
    }
}