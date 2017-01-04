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

using System.Collections.Generic;
using System.IO;

namespace Revise.Files.IFO.Blocks {
    /// <summary>
    /// Represents a map monster spawn.
    /// </summary>
    public class MapMonsterSpawn : MapBlock {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the spawn point.
        /// </summary>
        public string SpawnName {
            get;
            set;
        }

        /// <summary>
        /// Gets the normal spawn points.
        /// </summary>
        public List<MonsterSpawnPoint> NormalSpawnPoints {
            get;
            private set;
        }

        /// <summary>
        /// Gets the tactical spawn points.
        /// </summary>
        public List<MonsterSpawnPoint> TacticalSpawnPoints {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the interval to check whether to spawn more monsters in seconds.
        /// </summary>
        public int Interval {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the monster limit.
        /// </summary>
        public int Limit {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the spawn point range.
        /// </summary>
        public int Range {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the variable used in calculating whether to spawn monsters from the tactical spawn list.
        /// </summary>
        public int TacticalVariable {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MapMonsterSpawn"/> class.
        /// </summary>
        public MapMonsterSpawn() {
            NormalSpawnPoints = new List<MonsterSpawnPoint>();
            TacticalSpawnPoints = new List<MonsterSpawnPoint>();

            Name = string.Empty;
        }

        /// <summary>
        /// Reads the block data from the underlying stream.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public override void Read(BinaryReader reader) {
            base.Read(reader);

            SpawnName = reader.ReadByteString();
            int normalCount = reader.ReadInt32();

            for (int i = 0; i < normalCount; i++) {
                MonsterSpawnPoint spawn = new MonsterSpawnPoint();
                spawn.Name = reader.ReadByteString();
                spawn.Monster = reader.ReadInt32();
                spawn.Count = reader.ReadInt32();

                NormalSpawnPoints.Add(spawn);
            }

            int tacticalCount = reader.ReadInt32();

            for (int i = 0; i < tacticalCount; i++) {
                MonsterSpawnPoint spawn = new MonsterSpawnPoint();
                spawn.Name = reader.ReadByteString();
                spawn.Monster = reader.ReadInt32();
                spawn.Count = reader.ReadInt32();

                TacticalSpawnPoints.Add(spawn);
            }

            Interval = reader.ReadInt32();
            Limit = reader.ReadInt32();
            Range = reader.ReadInt32();
            TacticalVariable = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the block data to the underlying stream.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public override void Write(BinaryWriter writer) {
            base.Write(writer);

            writer.WriteByteString(SpawnName);
            writer.Write(NormalSpawnPoints.Count);

            NormalSpawnPoints.ForEach(spawn => {
                writer.WriteByteString(spawn.Name);
                writer.Write(spawn.Monster);
                writer.Write(spawn.Count);
            });

            writer.Write(TacticalSpawnPoints.Count);

            TacticalSpawnPoints.ForEach(spawn => {
                writer.WriteByteString(spawn.Name);
                writer.Write(spawn.Monster);
                writer.Write(spawn.Count);
            });

            writer.Write(Interval);
            writer.Write(Limit);
            writer.Write(Range);
            writer.Write(TacticalVariable);
        }
    }
}