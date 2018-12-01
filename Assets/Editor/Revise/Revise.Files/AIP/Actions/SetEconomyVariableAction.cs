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

using System.IO;
using Revise.Files.AIP.Interfaces;

namespace Revise.Files.AIP.Actions {
    /// <summary>
    /// Represents an action to set the specified economy variable.
    /// </summary>
    public class SetEconomyVariableAction : IArtificialIntelligenceAction {
        #region Properties

        /// <summary>
        /// Gets the action type.
        /// </summary>
        public ArtificialIntelligenceAction Type {
            get {
                return ArtificialIntelligenceAction.SetEconomyVariable;
            }
        }

        /// <summary>
        /// Gets or sets the variable number.
        /// </summary>
        public short Variable {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public int Value {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the operator to use when setting <see cref="Variable"/> to <see cref="Value"/>.
        /// </summary>
        public VariableOperator Operator {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Reads the condition data from the underlying stream.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public void Read(BinaryReader reader) {
            Variable = reader.ReadInt16();
            Value = reader.ReadInt32();
            Operator = (VariableOperator)reader.ReadByte();
        }

        /// <summary>
        /// Writes the condition data to the underlying stream.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void Write(BinaryWriter writer) {
            writer.Write(Variable);
            writer.Write(Value);
            writer.Write((byte)Operator);
        }
    }
}