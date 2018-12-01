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

namespace Revise.Files.ZON {
    /// <summary>
    /// Specifies the tile rotation values.
    /// </summary>
    public enum TileRotation {
        None = 1,
        FlipHorizontal = 2,
        FlipVertical = 3,
        Flip = 4,
        Clockwise90Degrees = 5,
        CounterClockwise90Degrees = 6
    }
}