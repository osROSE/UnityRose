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

using Revise.Files.IFO.Attributes;
using Revise.Files.IFO.Blocks;

namespace Revise.Files.IFO {
    /// <summary>
    /// Defines map block types.
    /// </summary>
    public enum MapBlockType {
        MapInformation = 0,

        [MapBlockTypeAttribute(typeof(MapObject))]
        Object = 1,
        
        [MapBlockTypeAttribute(typeof(MapNPC))]
        NPC = 2,

        [MapBlockTypeAttribute(typeof(MapBuilding))]
        Building = 3,

        [MapBlockTypeAttribute(typeof(MapSound))]
        Sound = 4,

        [MapBlockTypeAttribute(typeof(MapEffect))]
        Effect = 5,

        [MapBlockTypeAttribute(typeof(MapAnimation))]
        Animation = 6,

        [MapBlockTypeAttribute(typeof(MapWaterPatches))]
        WaterPatch = 7,

        [MapBlockTypeAttribute(typeof(MapMonsterSpawn))]
        MonsterSpawn = 8,

        [MapBlockTypeAttribute(typeof(MapWaterPlane))]
        WaterPlane = 9,

        [MapBlockTypeAttribute(typeof(MapWarpPoint))]
        WarpPoint = 10,

        [MapBlockTypeAttribute(typeof(MapCollisionObject))]
        CollisionObject = 11,

        [MapBlockTypeAttribute(typeof(MapEventObject))]
        EventObject = 12
    }
}