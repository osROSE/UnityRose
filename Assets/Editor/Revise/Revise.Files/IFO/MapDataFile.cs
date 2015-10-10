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
using Revise.Files.Exceptions;  
using Revise.Files.IFO.Attributes;
using Revise.Files.IFO.Blocks;
using Revise.Files.IFO.Interfaces;
using UnityEngine;

namespace Revise.Files.IFO {
    /// <summary>
    /// Provides the ability to create, open and save IFO files for map data.
    /// </summary>
    public class MapDataFile : FileLoader {
        private const int BLOCK_COUNT = 13;

        #region Properties

        /// <summary>
        /// Gets or sets the map position.
        /// </summary>
        public IntVector2 MapPosition {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the zone position.
        /// </summary>
        public IntVector2 ZonePosition {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the world matrix.
        /// </summary>
        public Matrix4x4 World {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the block name.
        /// </summary>
        public string Name {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the size of the water.
        /// </summary>
        public float WaterSize {
            get;
            set;
        }

        /// <summary>
        /// Gets the water planes.
        /// </summary>
        public List<MapWaterPlane> WaterPlanes {
            get;
            private set;
        }

        /// <summary>
        /// Gets the objects.
        /// </summary>
        public List<MapObject> Objects {
            get;
            private set;
        }

        /// <summary>
        /// Gets the NP cs.
        /// </summary>
        public List<MapNPC> NPCs {
            get;
            private set;
        }

        /// <summary>
        /// Gets the buildings.
        /// </summary>
        public List<MapBuilding> Buildings {
            get;
            private set;
        }

        /// <summary>
        /// Gets the sounds.
        /// </summary>
        public List<MapSound> Sounds {
            get;
            private set;
        }

        /// <summary>
        /// Gets the effects.
        /// </summary>
        public List<MapEffect> Effects {
            get;
            private set;
        }

        /// <summary>
        /// Gets the animations.
        /// </summary>
        public List<MapAnimation> Animations {
            get;
            private set;
        }

        /// <summary>
        /// Gets the water patches.
        /// </summary>
        public MapWaterPatches WaterPatches {
            get;
            private set;
        }

        /// <summary>
        /// Gets the monster spawns.
        /// </summary>
        public List<MapMonsterSpawn> MonsterSpawns {
            get;
            private set;
        }

        /// <summary>
        /// Gets the warp points.
        /// </summary>
        public List<MapWarpPoint> WarpPoints {
            get;
            private set;
        }

        /// <summary>
        /// Gets the collision objects.
        /// </summary>
        public List<MapCollisionObject> CollisionObjects {
            get;
            private set;
        }

        /// <summary>
        /// Gets the event objects.
        /// </summary>
        public List<MapEventObject> EventObjects {
            get;
            private set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MapDataFile"/> class.
        /// </summary>
        public MapDataFile() {
            WaterPlanes = new List<MapWaterPlane>();
            Objects = new List<MapObject>();
            NPCs = new List<MapNPC>();
            Buildings = new List<MapBuilding>();
            Sounds = new List<MapSound>();
            Effects = new List<MapEffect>();
            Animations = new List<MapAnimation>();
            WaterPatches = new MapWaterPatches();
            MonsterSpawns = new List<MapMonsterSpawn>();
            WarpPoints = new List<MapWarpPoint>();
            CollisionObjects = new List<MapCollisionObject>();
            EventObjects = new List<MapEventObject>();

            Reset();
        }

        /// <summary>
        /// Loads the file from the specified stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        public override void Load(Stream stream) {
            BinaryReader reader = new BinaryReader(stream, Encoding.GetEncoding("EUC-KR"));

            int blockCount = reader.ReadInt32();

            for (int i = 0; i < blockCount; i++) {
                MapBlockType type = (MapBlockType)reader.ReadInt32();

                if (!Enum.IsDefined(typeof(MapBlockType), type)) {
                    throw new InvalidMapBlockTypeException((int)type);
                }

                int offset = reader.ReadInt32();
                long nextBlock = stream.Position;

                stream.Seek(offset, SeekOrigin.Begin);

                if (type == MapBlockType.MapInformation) {
                    MapPosition = new IntVector2(reader.ReadInt32(), reader.ReadInt32());
                    ZonePosition = new IntVector2(reader.ReadInt32(), reader.ReadInt32());
                    World = reader.ReadMatrix();
                    Name = reader.ReadString();
                }else if(type == MapBlockType.WaterPatch){
                    WaterPatches = new MapWaterPatches();
                    WaterPatches.Read(reader);
                } else {
                    if (type == MapBlockType.WaterPlane) {
                        WaterSize = reader.ReadSingle();
                    }

                    int entryCount = reader.ReadInt32();
                    Type classType = type.GetAttributeValue<MapBlockTypeAttribute, Type>(x => x.Type);

                    for (int j = 0; j < entryCount; j++) {
                        IMapBlock block = (IMapBlock)Activator.CreateInstance(classType);
                        block.Read(reader);

                        switch (type) {
                            case MapBlockType.Object:
                                Objects.Add((MapObject)block);
                                break;
                            case MapBlockType.NPC:
                                NPCs.Add((MapNPC)block);
                                break;
                            case MapBlockType.Building:
                                Buildings.Add((MapBuilding)block);
                                break;
                            case MapBlockType.Sound:
                                Sounds.Add((MapSound)block);
                                break;
                            case MapBlockType.Effect:
                                Effects.Add((MapEffect)block);
                                break;
                            case MapBlockType.Animation:
                                Animations.Add((MapAnimation)block);
                                break;
                            case MapBlockType.MonsterSpawn:
                                MonsterSpawns.Add((MapMonsterSpawn)block);
                                break;
                            case MapBlockType.WaterPlane:
                                WaterPlanes.Add((MapWaterPlane)block);
                                break;
                            case MapBlockType.WarpPoint:
                                WarpPoints.Add((MapWarpPoint)block);
                                break;
                            case MapBlockType.CollisionObject:
                                CollisionObjects.Add((MapCollisionObject)block);
                                break;
                            case MapBlockType.EventObject:
                                EventObjects.Add((MapEventObject)block);
                                break;
                        }
                    }
                }


                if (i < blockCount - 1) {
                    stream.Seek(nextBlock, SeekOrigin.Begin);
                }
            }
        }

        /// <summary>
        /// Saves the file to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to save to.</param>
        public override void Save(Stream stream) {
            BinaryWriter writer = new BinaryWriter(stream, Encoding.GetEncoding("EUC-KR"));

            writer.Write(BLOCK_COUNT);

            for (int i = 0; i < BLOCK_COUNT; i++) {
                writer.Write(0);
                writer.Write(0);
            }

            long[] offsets = new long[BLOCK_COUNT];

            offsets[0] = stream.Position;
            writer.Write(MapPosition.X);
            writer.Write(MapPosition.Y);
            writer.Write(ZonePosition.X);
            writer.Write(ZonePosition.Y);
            writer.Write(World);
            writer.Write(Name);

            offsets[1] = stream.Position;
            writer.Write(Objects.Count);

            Objects.ForEach(@object => {
                @object.Write(writer);
            });

            offsets[2] = stream.Position;
            writer.Write(NPCs.Count);

            NPCs.ForEach(npc => {
                npc.Write(writer);
            });

            offsets[3] = stream.Position;
            writer.Write(Buildings.Count);

            Buildings.ForEach(building => {
                building.Write(writer);
            });

            offsets[4] = stream.Position;
            writer.Write(Sounds.Count);

            Sounds.ForEach(sound => {
                sound.Write(writer);
            });

            offsets[5] = stream.Position;
            writer.Write(Effects.Count);

            Effects.ForEach(effect => {
                effect.Write(writer);
            });

            offsets[6] = stream.Position;
            writer.Write(Animations.Count);

            Animations.ForEach(animation => {
                animation.Write(writer);
            });

            offsets[7] = stream.Position;
            WaterPatches.Write(writer);

            offsets[8] = stream.Position;
            writer.Write(MonsterSpawns.Count);

            MonsterSpawns.ForEach(monsterSpawn => {
                monsterSpawn.Write(writer);
            });

            offsets[9] = stream.Position;
            writer.Write(WaterSize);
            writer.Write(WaterPlanes.Count);

            WaterPlanes.ForEach(waterPlane => {
                waterPlane.Write(writer);
            });

            offsets[10] = stream.Position;
            writer.Write(WarpPoints.Count);

            WarpPoints.ForEach(warpPoint => {
                warpPoint.Write(writer);
            });

            offsets[11] = stream.Position;
            writer.Write(CollisionObjects.Count);

            CollisionObjects.ForEach(collisionObject => {
                collisionObject.Write(writer);
            });

            offsets[12] = stream.Position;
            writer.Write(EventObjects.Count);

            EventObjects.ForEach(eventObject => {
                eventObject.Write(writer);
            });

            stream.Seek(4, SeekOrigin.Begin);

            for (int i = 0; i < BLOCK_COUNT; i++) {
                writer.Write(i);
                writer.Write((int)offsets[i]);
            }
        }

        /// <summary>
        /// Removes all light objects.
        /// </summary>
        public void Clear() {
            WaterPlanes.Clear();
            Objects.Clear();
            NPCs.Clear();
            Buildings.Clear();
            Sounds.Clear();
            Effects.Clear();
            Animations.Clear();
            MonsterSpawns.Clear();
            WarpPoints.Clear();
            CollisionObjects.Clear();
            EventObjects.Clear();
        }

        /// <summary>
        /// Resets properties to their default values.
        /// </summary>
        public override void Reset() {
            base.Reset();

            WaterPatches = new MapWaterPatches();
            MapPosition = new IntVector2();
            ZonePosition = new IntVector2();
            World = Matrix4x4.identity;
            Name = string.Empty;
            WaterSize = 2000.0f;

            Clear();
        }
    }
}