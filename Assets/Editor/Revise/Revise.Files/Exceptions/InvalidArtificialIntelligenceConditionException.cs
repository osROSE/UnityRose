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

namespace Revise.Files.Exceptions {
    /// <summary>
    /// The exception that is thrown when an AI condition type is invalid.
    /// </summary>
    public class InvalidArtificialIntelligenceConditionException : Exception {
        /// <summary>
        /// The format of the exception message.
        /// </summary>
        private const string MESSAGE_FORMAT = "Condition '{0}' is invalid";

        #region Properties

        /// <summary>
        /// Gets the condition.
        /// </summary>
        public int Condition {
            get; 
            private set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidArtificialIntelligenceConditionException"/> class.
        /// </summary>
        public InvalidArtificialIntelligenceConditionException(int condition)
            : base(string.Format(MESSAGE_FORMAT, condition)) {
            Condition = condition;
        }
    }
}