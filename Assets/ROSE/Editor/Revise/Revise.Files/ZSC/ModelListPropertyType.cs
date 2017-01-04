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
    /// Defines the model list property types.
    /// </summary>
    internal enum ModelListPropertyType : byte {
        Position = 1,
        Rotation = 2,
        Scale = 3,
        AxisRotation = 4,
        BoneIndex = 5,
        DummyIndex = 6,
        Parent = 7,
        Animation = 8,
        Collision = 29,
        ConstantAnimation = 30,
        VisibleRangeSet = 31,
        UseLightmap = 32
    }
}