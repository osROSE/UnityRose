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
using System.Collections.Generic;
using System.IO;
using System.Text;
using Revise.Files.AIP.Attributes;
using Revise.Files.AIP.Interfaces;
using Revise.Files.Exceptions;
using Revise.IO;

namespace Revise.Files.AIP {
    /// <summary>
    /// Provides the ability to create, open and save AIP files for artificial intelligence events.
    /// </summary>
    public class ArtificialIntelligenceFile : FileLoader {
        private const int CONDITION_MASK = 0x04000000;
        private const int ACTION_MASK = 0x0B000000;

        #region Properties

        /// <summary>
        /// Gets or sets the interval in seconds for running idle events.
        /// </summary>
        public int IdleInterval {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the rate for running damage events as a percentage.
        /// </summary>
        public int DamageRate {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the AI name.
        /// </summary>
        public string Name {
            get;
            set;
        }

        public List<ArtificialIntelligencePattern> Patterns {
            get;
            private set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtificialIntelligenceFile"/> class.
        /// </summary>
        public ArtificialIntelligenceFile() {
            Patterns = new List<ArtificialIntelligencePattern>();

            Reset();
        }

        /// <summary>
        /// Loads the file from the specified stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        public override void Load(Stream stream) {
            BinaryReader reader = new BinaryReader(stream, Encoding.GetEncoding("EUC-KR"));
            AlignedBinaryReader alignedReader = new AlignedBinaryReader(stream, Encoding.GetEncoding("EUC-KR"));

            int patternCount = reader.ReadInt32();
            IdleInterval = reader.ReadInt32();
            DamageRate = reader.ReadInt32();
            Name = reader.ReadIntString();

            for (int i = 0; i < patternCount; i++) {
                var pattern = new ArtificialIntelligencePattern();
                pattern.Name = reader.ReadString(32).TrimEnd('\0');

                int eventCount = reader.ReadInt32();

                for (int j = 0; j < eventCount; j++) {
                    var @event = new ArtificialIntelligenceEvent();
                    @event.Name = reader.ReadString(32).TrimEnd('\0');

                    int conditionCount = reader.ReadInt32();

                    for (int k = 0; k < conditionCount; k++) {
                        long position = stream.Position;
                        int size = reader.ReadInt32();
                        var type = (ArtificialIntelligenceCondition)(reader.ReadInt32() ^ CONDITION_MASK);

                        if (!Enum.IsDefined(typeof(ArtificialIntelligenceCondition), type)) {
                            throw new InvalidArtificialIntelligenceConditionException((int)type);
                        }

                        Type classType = type.GetAttributeValue<ArtificialIntelligenceTypeAttribute, Type>(x => x.Type);
                        var condition = (IArtificialIntelligenceCondition)Activator.CreateInstance(classType);

                        alignedReader.Reset();
                        condition.Read(alignedReader);
                        alignedReader.Align();

                        @event.Conditions.Add(condition);

                        if (stream.Position - position != size) {
                            stream.Seek(position + size, SeekOrigin.Begin);
                        }
                    }

                    int actionCount = reader.ReadInt32();

                    for (int k = 0; k < actionCount; k++) {
                        long position = stream.Position;
                        int size = reader.ReadInt32();
                        var type = (ArtificialIntelligenceAction)(reader.ReadInt32() ^ ACTION_MASK);

                        if (!Enum.IsDefined(typeof(ArtificialIntelligenceAction), type)) {
                            throw new InvalidArtificialIntelligenceActionException((int)type);
                        }

                        Type classType = type.GetAttributeValue<ArtificialIntelligenceTypeAttribute, Type>(x => x.Type);
                        var action = (IArtificialIntelligenceAction)Activator.CreateInstance(classType);

                        alignedReader.Reset();
                        action.Read(alignedReader);
                        alignedReader.Align();

                        @event.Actions.Add(action);

                        if (stream.Position - position != size) {
                            stream.Seek(position + size, SeekOrigin.Begin);
                        }
                    }

                    pattern.Events.Add(@event);
                }

                Patterns.Add(pattern);
            }
        }

        /// <summary>
        /// Saves the file to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to save to.</param>
        public override void Save(Stream stream) {
            BinaryWriter writer = new BinaryWriter(stream, Encoding.GetEncoding("EUC-KR"));
            AlignedBinaryWriter alignedWriter = new AlignedBinaryWriter(stream, Encoding.GetEncoding("EUC-KR"));

            writer.Write(Patterns.Count);
            writer.Write(IdleInterval);
            writer.Write(DamageRate);
            writer.WriteIntString(Name);

            Patterns.ForEach(pattern => {
                writer.WriteString(pattern.Name, 32);
                writer.Write(pattern.Events.Count);

                pattern.Events.ForEach(@event => {
                    writer.WriteString(@event.Name, 32);
                    writer.Write(@event.Conditions.Count);

                    @event.Conditions.ForEach(condition => {
                        writer.Write(0);
                        writer.Write((int)condition.Type | CONDITION_MASK);

                        alignedWriter.Reset();
                        condition.Write(alignedWriter);
                        alignedWriter.Align();

                        int size = alignedWriter.TotalWrote + 8;

                        stream.Seek(-size, SeekOrigin.Current);
                        writer.Write(size);

                        stream.Seek(size - 4, SeekOrigin.Current);
                    });

                    writer.Write(@event.Actions.Count);

                    @event.Actions.ForEach(action => {
                        writer.Write(0);
                        writer.Write((int)action.Type | ACTION_MASK);

                        alignedWriter.Reset();
                        action.Write(alignedWriter);
                        alignedWriter.Align();

                        int size = alignedWriter.TotalWrote + 8;

                        stream.Seek(-size, SeekOrigin.Current);
                        writer.Write(size);

                        stream.Seek(size - 4, SeekOrigin.Current);
                    });
                });
            });
        }

        /// <summary>
        /// Clears all file systems.
        /// </summary>
        public void Clear() {
            Patterns.Clear();
        }

        /// <summary>
        /// Resets properties to their default values.
        /// </summary>
        public override void Reset() {
            base.Reset();

            IdleInterval = 4;
            DamageRate = 30;
            Name = string.Empty;

            Clear();
        }
    }
}