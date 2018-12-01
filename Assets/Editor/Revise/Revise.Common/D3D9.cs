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

namespace Revise {
    /// <summary>
    /// Direct3D 9 blends
    /// https://docs.microsoft.com/en-us/windows/desktop/direct3d9/d3dblend
    /// </summary>
    public enum Blend {
        Zero = 1,
        One = 2,
        SourceColor = 3,
        InverseSourceColor = 4,
        SourceAlpha = 5,
        InverseSourceAlpha = 6,
        DestinationAlpha = 7,
        InverseDestinationAlpha = 8,
        DestinationColor = 9,
        InverseDestinationColor = 10,
        SourceAlphaSaturation = 11,
        BothSourceAlpha = 12,
        BothInverseSourceAlpha = 13,
        BlendFactor = 14,
        InverseBlendFactor = 15,
        SourceColor2 = 16,
        InverseSourceColor2 = 17,
    }

    /// <summary>
    /// Direct3D 9 blend operations.
    /// https://docs.microsoft.com/en-us/windows/desktop/direct3d9/d3dblendop
    /// </summary>
    public enum BlendOperation {
        Add = 1,
        Subtract = 2,
        ReverseSubtract = 3,
        Min = 4,
        Max = 5,
    }
}
