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

namespace Revise.Files.ZMO.Attributes {
    /// <summary>
    /// Represents an attribute for associating classes with motion channel types.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class MotionChannelTypeAttribute : Attribute {
        #region Properties

        /// <summary>
        /// Gets the class associated with the motion channel type.
        /// </summary>
        public Type Type {
            get;
            private set;
        }

        /// <summary>
        /// Gets the parameters for initialising the motion channel class.
        /// </summary>
        public object[] Parameters {
            get;
            private set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MotionChannelTypeAttribute"/> class.
        /// </summary>
        /// <param name="type">The class associated with the motion channel type.</param>
        public MotionChannelTypeAttribute(Type type, params object[] parameters) {
            Type = type;
            Parameters = parameters;
        }

        /// <summary>
        /// Creates an instance of the motion channel type.
        /// </summary>
        /// <returns>The created instance.</returns>
        public object CreateInstance() {
            return Activator.CreateInstance(Type, Parameters);
        }
    }
}