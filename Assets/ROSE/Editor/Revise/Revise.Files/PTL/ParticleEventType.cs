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

using Revise.Files.PTL.Attributes;
using Revise.Files.PTL.Events;

namespace Revise.Files.PTL {
    /// <summary>
    /// Defines particle event types.
    /// </summary>
    public enum ParticleEventType {
        [ParticleEventTypeAttribute(typeof(ParticleEvent))]
        None = 0,

        [ParticleEventTypeAttribute(typeof(ScaleEvent))]
        Scale = 1,

        [ParticleEventTypeAttribute(typeof(TimerEvent))]
        Timer = 2,

        [ParticleEventTypeAttribute(typeof(RedColourEvent))]
        RedColour = 3,

        [ParticleEventTypeAttribute(typeof(GreenColourEvent))]
        GreenColour = 4,

        [ParticleEventTypeAttribute(typeof(BlueColourEvent))]
        BlueColour = 5,

        [ParticleEventTypeAttribute(typeof(AlphaEvent))]
        Alpha = 6,

        [ParticleEventTypeAttribute(typeof(ColourEvent))]
        Colour = 7,

        [ParticleEventTypeAttribute(typeof(VelocityXEvent))]
        VelocityX = 8,

        [ParticleEventTypeAttribute(typeof(VelocityYEvent))]
        VelocityY = 9,

        [ParticleEventTypeAttribute(typeof(VelocityZEvent))]
        VelocityZ = 10,

        [ParticleEventTypeAttribute(typeof(VelocityEvent))]
        Velocity = 11,

        [ParticleEventTypeAttribute(typeof(TextureEvent))]
        Texture = 12,

        [ParticleEventTypeAttribute(typeof(RotationEvent))]
        Rotation = 13
    }
}