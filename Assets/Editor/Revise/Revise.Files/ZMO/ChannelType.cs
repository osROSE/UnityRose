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

using Revise.Files.ZMO.Attributes;
using Revise.Files.ZMO.Channels;

namespace Revise.Files.ZMO {
    /// <summary>
    /// Defines the motion channel types.
    /// </summary>
    public enum ChannelType {
        None = 1 << 0,

        [MotionChannelTypeAttribute(typeof(PositionChannel))]
        Position = 1 << 1,

        [MotionChannelTypeAttribute(typeof(RotationChannel))]
        Rotation = 1 << 2,

        [MotionChannelTypeAttribute(typeof(NormalChannel))]
        Normal = 1 << 3,

        [MotionChannelTypeAttribute(typeof(AlphaChannel))]
        Alpha = 1 << 4,

        [MotionChannelTypeAttribute(typeof(TextureCoordinateChannel), ChannelType.TextureCoordinate1)]
        TextureCoordinate1 = 1 << 5,

        [MotionChannelTypeAttribute(typeof(TextureCoordinateChannel), ChannelType.TextureCoordinate2)]
        TextureCoordinate2 = 1 << 6,

        [MotionChannelTypeAttribute(typeof(TextureCoordinateChannel), ChannelType.TextureCoordinate3)]
        TextureCoordinate3 = 1 << 7,

        [MotionChannelTypeAttribute(typeof(TextureCoordinateChannel), ChannelType.TextureCoordinate4)]
        TextureCoordinate4 = 1 << 8,

        [MotionChannelTypeAttribute(typeof(TextureAnimationChannel))]
        TextureAnimation = 1 << 9,

        [MotionChannelTypeAttribute(typeof(ScaleChannel))]
        Scale = 1 << 10,
    }
}