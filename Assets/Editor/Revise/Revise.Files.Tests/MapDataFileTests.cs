#region License

/**
 * Copyright (C) 2011 Jack Wakefield
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
using NUnit.Framework;
using Revise.Files.IFO;

namespace Revise.Files.Tests {
    /// <summary>
    /// Provides testing for the <see cref="MapDataFile"/> class.
    /// </summary>
    [TestFixture]
    public class MapDataFileTests {
        private const string TEST_FILE = "Tests/Revise/Files/33_32.IFO";

        /// <summary>
        /// Tests the load method.
        /// </summary>
        [Test]
        public void TestLoadMethod() {
            MapDataFile mapDataFile = new MapDataFile();
            mapDataFile.Load(TEST_FILE);
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        [Test]
        public void TestSaveMethod() {
            MapDataFile mapDataFile = new MapDataFile();
            mapDataFile.Load(TEST_FILE);

            MemoryStream savedStream = new MemoryStream();
            mapDataFile.Save(savedStream);

            savedStream.Seek(0, SeekOrigin.Begin);

            MapDataFile savedMapDataFile = new MapDataFile();
            savedMapDataFile.Load(savedStream);

            savedStream.Close();

            Assert.AreEqual(mapDataFile.MapPosition, savedMapDataFile.MapPosition, "Map position values do not match");
            Assert.AreEqual(mapDataFile.ZonePosition, savedMapDataFile.ZonePosition, "Zone position values do not match");
            Assert.AreEqual(mapDataFile.World, savedMapDataFile.World, "World matrices do not match");
            Assert.AreEqual(mapDataFile.Name, savedMapDataFile.Name, "Names do not match");

            Assert.AreEqual(mapDataFile.Objects.Count, savedMapDataFile.Objects.Count, "Object counts do not match");

            for (int i = 0; i < mapDataFile.Objects.Count; i++) {
                Assert.AreEqual(mapDataFile.Objects[i].Name, savedMapDataFile.Objects[i].Name, "Object names do not match");
                Assert.AreEqual(mapDataFile.Objects[i].WarpID, savedMapDataFile.Objects[i].WarpID, "Object warp ID values do not match");
                Assert.AreEqual(mapDataFile.Objects[i].EventID, savedMapDataFile.Objects[i].EventID, "Object event ID values do not match");
                Assert.AreEqual(mapDataFile.Objects[i].ObjectType, savedMapDataFile.Objects[i].ObjectType, "Object object type values do not match");
                Assert.AreEqual(mapDataFile.Objects[i].ObjectID, savedMapDataFile.Objects[i].ObjectID, "Object object ID values do not match");
                Assert.AreEqual(mapDataFile.Objects[i].MapPosition, savedMapDataFile.Objects[i].MapPosition, "Object map positions do not match");
                Assert.AreEqual(mapDataFile.Objects[i].Rotation, savedMapDataFile.Objects[i].Rotation, "Object rotations do not match");
                Assert.AreEqual(mapDataFile.Objects[i].Position, savedMapDataFile.Objects[i].Position, "Object positions do not match");
                Assert.AreEqual(mapDataFile.Objects[i].Scale, savedMapDataFile.Objects[i].Scale, "Object scales do not match");
            }

            Assert.AreEqual(mapDataFile.NPCs.Count, savedMapDataFile.NPCs.Count, "NPC counts do not match");

            for (int i = 0; i < mapDataFile.NPCs.Count; i++) {
                Assert.AreEqual(mapDataFile.NPCs[i].Name, savedMapDataFile.NPCs[i].Name, "NPC names do not match");
                Assert.AreEqual(mapDataFile.NPCs[i].WarpID, savedMapDataFile.NPCs[i].WarpID, "NPC warp ID values do not match");
                Assert.AreEqual(mapDataFile.NPCs[i].EventID, savedMapDataFile.NPCs[i].EventID, "NPC event ID values do not match");
                Assert.AreEqual(mapDataFile.NPCs[i].ObjectType, savedMapDataFile.NPCs[i].ObjectType, "NPC object type values do not match");
                Assert.AreEqual(mapDataFile.NPCs[i].ObjectID, savedMapDataFile.NPCs[i].ObjectID, "NPC object ID values do not match");
                Assert.AreEqual(mapDataFile.NPCs[i].MapPosition, savedMapDataFile.NPCs[i].MapPosition, "NPC map positions do not match");
                Assert.AreEqual(mapDataFile.NPCs[i].Rotation, savedMapDataFile.NPCs[i].Rotation, "NPC rotations do not match");
                Assert.AreEqual(mapDataFile.NPCs[i].Position, savedMapDataFile.NPCs[i].Position, "NPC positions do not match");
                Assert.AreEqual(mapDataFile.NPCs[i].Scale, savedMapDataFile.NPCs[i].Scale, "NPC scales do not match");
                Assert.AreEqual(mapDataFile.NPCs[i].AI, savedMapDataFile.NPCs[i].AI, "NPC AI values do not match");
                Assert.AreEqual(mapDataFile.NPCs[i].ConversationFile, savedMapDataFile.NPCs[i].ConversationFile, "NPC conversation file values do not match");
            }

            Assert.AreEqual(mapDataFile.Buildings.Count, savedMapDataFile.Buildings.Count, "Building counts do not match");

            for (int i = 0; i < mapDataFile.Buildings.Count; i++) {
                Assert.AreEqual(mapDataFile.Buildings[i].Name, savedMapDataFile.Buildings[i].Name, "Building names do not match");
                Assert.AreEqual(mapDataFile.Buildings[i].WarpID, savedMapDataFile.Buildings[i].WarpID, "Building warp ID values do not match");
                Assert.AreEqual(mapDataFile.Buildings[i].EventID, savedMapDataFile.Buildings[i].EventID, "Building event ID values do not match");
                Assert.AreEqual(mapDataFile.Buildings[i].ObjectType, savedMapDataFile.Buildings[i].ObjectType, "Building object type values do not match");
                Assert.AreEqual(mapDataFile.Buildings[i].ObjectID, savedMapDataFile.Buildings[i].ObjectID, "Building object ID values do not match");
                Assert.AreEqual(mapDataFile.Buildings[i].MapPosition, savedMapDataFile.Buildings[i].MapPosition, "Building map positions do not match");
                Assert.AreEqual(mapDataFile.Buildings[i].Rotation, savedMapDataFile.Buildings[i].Rotation, "Building rotations do not match");
                Assert.AreEqual(mapDataFile.Buildings[i].Position, savedMapDataFile.Buildings[i].Position, "Building positions do not match");
                Assert.AreEqual(mapDataFile.Buildings[i].Scale, savedMapDataFile.Buildings[i].Scale, "Building scales do not match");
            }

            Assert.AreEqual(mapDataFile.Sounds.Count, savedMapDataFile.Sounds.Count, "Sound counts do not match");

            for (int i = 0; i < mapDataFile.Sounds.Count; i++) {
                Assert.AreEqual(mapDataFile.Sounds[i].Name, savedMapDataFile.Sounds[i].Name, "Sound names do not match");
                Assert.AreEqual(mapDataFile.Sounds[i].WarpID, savedMapDataFile.Sounds[i].WarpID, "Sound warp ID values do not match");
                Assert.AreEqual(mapDataFile.Sounds[i].EventID, savedMapDataFile.Sounds[i].EventID, "Sound event ID values do not match");
                Assert.AreEqual(mapDataFile.Sounds[i].ObjectType, savedMapDataFile.Sounds[i].ObjectType, "Sound object type values do not match");
                Assert.AreEqual(mapDataFile.Sounds[i].ObjectID, savedMapDataFile.Sounds[i].ObjectID, "Sound object ID values do not match");
                Assert.AreEqual(mapDataFile.Sounds[i].MapPosition, savedMapDataFile.Sounds[i].MapPosition, "Sound map positions do not match");
                Assert.AreEqual(mapDataFile.Sounds[i].Rotation, savedMapDataFile.Sounds[i].Rotation, "Sound rotations do not match");
                Assert.AreEqual(mapDataFile.Sounds[i].Position, savedMapDataFile.Sounds[i].Position, "Sound positions do not match");
                Assert.AreEqual(mapDataFile.Sounds[i].Scale, savedMapDataFile.Sounds[i].Scale, "Sound scales do not match");
                Assert.AreEqual(mapDataFile.Sounds[i].FilePath, savedMapDataFile.Sounds[i].FilePath, "Sound file paths do not match");
                Assert.AreEqual(mapDataFile.Sounds[i].Range, savedMapDataFile.Sounds[i].Range, "Sound range values do not match");
                Assert.AreEqual(mapDataFile.Sounds[i].Interval, savedMapDataFile.Sounds[i].Interval, "Sound interval values do not match");
            }

            Assert.AreEqual(mapDataFile.Effects.Count, savedMapDataFile.Effects.Count, "Effect counts do not match");

            for (int i = 0; i < mapDataFile.Effects.Count; i++) {
                Assert.AreEqual(mapDataFile.Effects[i].Name, savedMapDataFile.Effects[i].Name, "Effect names do not match");
                Assert.AreEqual(mapDataFile.Effects[i].WarpID, savedMapDataFile.Effects[i].WarpID, "Effect warp ID values do not match");
                Assert.AreEqual(mapDataFile.Effects[i].EventID, savedMapDataFile.Effects[i].EventID, "Effect event ID values do not match");
                Assert.AreEqual(mapDataFile.Effects[i].ObjectType, savedMapDataFile.Effects[i].ObjectType, "Effect object type values do not match");
                Assert.AreEqual(mapDataFile.Effects[i].ObjectID, savedMapDataFile.Effects[i].ObjectID, "Effect object ID values do not match");
                Assert.AreEqual(mapDataFile.Effects[i].MapPosition, savedMapDataFile.Effects[i].MapPosition, "Effect map positions do not match");
                Assert.AreEqual(mapDataFile.Effects[i].Rotation, savedMapDataFile.Effects[i].Rotation, "Effect rotations do not match");
                Assert.AreEqual(mapDataFile.Effects[i].Position, savedMapDataFile.Effects[i].Position, "Effect positions do not match");
                Assert.AreEqual(mapDataFile.Effects[i].Scale, savedMapDataFile.Effects[i].Scale, "Effect scales do not match");
                Assert.AreEqual(mapDataFile.Effects[i].FilePath, savedMapDataFile.Effects[i].FilePath, "Effect file paths do not match");
            }

            Assert.AreEqual(mapDataFile.Animations.Count, savedMapDataFile.Animations.Count, "Animation counts do not match");

            for (int i = 0; i < mapDataFile.Animations.Count; i++) {
                Assert.AreEqual(mapDataFile.Animations[i].Name, savedMapDataFile.Animations[i].Name, "Animation names do not match");
                Assert.AreEqual(mapDataFile.Animations[i].WarpID, savedMapDataFile.Animations[i].WarpID, "Animation warp ID values do not match");
                Assert.AreEqual(mapDataFile.Animations[i].EventID, savedMapDataFile.Animations[i].EventID, "Animation event ID values do not match");
                Assert.AreEqual(mapDataFile.Animations[i].ObjectType, savedMapDataFile.Animations[i].ObjectType, "Animation object type values do not match");
                Assert.AreEqual(mapDataFile.Animations[i].ObjectID, savedMapDataFile.Animations[i].ObjectID, "Animation object ID values do not match");
                Assert.AreEqual(mapDataFile.Animations[i].MapPosition, savedMapDataFile.Animations[i].MapPosition, "Animation map positions do not match");
                Assert.AreEqual(mapDataFile.Animations[i].Rotation, savedMapDataFile.Animations[i].Rotation, "Animation rotations do not match");
                Assert.AreEqual(mapDataFile.Animations[i].Position, savedMapDataFile.Animations[i].Position, "Animation positions do not match");
                Assert.AreEqual(mapDataFile.Animations[i].Scale, savedMapDataFile.Animations[i].Scale, "Animation scales do not match");
            }

            Assert.AreEqual(mapDataFile.WaterPatches.Name, savedMapDataFile.WaterPatches.Name, "Water patch names do not match");
            Assert.AreEqual(mapDataFile.WaterPatches.WarpID, savedMapDataFile.WaterPatches.WarpID, "Water patch warp ID values do not match");
            Assert.AreEqual(mapDataFile.WaterPatches.EventID, savedMapDataFile.WaterPatches.EventID, "Water patch event ID values do not match");
            Assert.AreEqual(mapDataFile.WaterPatches.ObjectType, savedMapDataFile.WaterPatches.ObjectType, "Water patch object type values do not match");
            Assert.AreEqual(mapDataFile.WaterPatches.ObjectID, savedMapDataFile.WaterPatches.ObjectID, "Water patch object ID values do not match");
            Assert.AreEqual(mapDataFile.WaterPatches.MapPosition, savedMapDataFile.WaterPatches.MapPosition, "Water patch map positions do not match");
            Assert.AreEqual(mapDataFile.WaterPatches.Rotation, savedMapDataFile.WaterPatches.Rotation, "Water patch rotations do not match");
            Assert.AreEqual(mapDataFile.WaterPatches.Position, savedMapDataFile.WaterPatches.Position, "Water patch positions do not match");
            Assert.AreEqual(mapDataFile.WaterPatches.Scale, savedMapDataFile.WaterPatches.Scale, "Water patch scales do not match");
            Assert.AreEqual(mapDataFile.WaterPatches.Width, savedMapDataFile.WaterPatches.Width, "Water patch width values do not match");
            Assert.AreEqual(mapDataFile.WaterPatches.Height, savedMapDataFile.WaterPatches.Height, "Water patch height values do not match");

            for (int h = 0; h < mapDataFile.WaterPatches.Height; h++) {
                for (int w = 0; w < mapDataFile.WaterPatches.Width; w++) {
                    Assert.AreEqual(mapDataFile.WaterPatches.Patches[h, w].HasWater, mapDataFile.WaterPatches.Patches[h, w].HasWater, "Water patch has water values do not match");
                    Assert.AreEqual(mapDataFile.WaterPatches.Patches[h, w].Height, mapDataFile.WaterPatches.Patches[h, w].Height, "Water patch has water values do not match");
                    Assert.AreEqual(mapDataFile.WaterPatches.Patches[h, w].Type, mapDataFile.WaterPatches.Patches[h, w].Type, "Water patch has water values do not match");
                    Assert.AreEqual(mapDataFile.WaterPatches.Patches[h, w].ID, mapDataFile.WaterPatches.Patches[h, w].ID, "Water patch has water values do not match");
                    Assert.AreEqual(mapDataFile.WaterPatches.Patches[h, w].Reserved, mapDataFile.WaterPatches.Patches[h, w].Reserved, "Water patch has water values do not match");
                }
            }

            Assert.AreEqual(mapDataFile.MonsterSpawns.Count, savedMapDataFile.MonsterSpawns.Count, "Monster spawn counts do not match");

            for (int i = 0; i < mapDataFile.MonsterSpawns.Count; i++) {
                Assert.AreEqual(mapDataFile.MonsterSpawns[i].Name, savedMapDataFile.MonsterSpawns[i].Name, "Monster spawn names do not match");
                Assert.AreEqual(mapDataFile.MonsterSpawns[i].WarpID, savedMapDataFile.MonsterSpawns[i].WarpID, "Monster spawn warp ID values do not match");
                Assert.AreEqual(mapDataFile.MonsterSpawns[i].EventID, savedMapDataFile.MonsterSpawns[i].EventID, "Monster spawn event ID values do not match");
                Assert.AreEqual(mapDataFile.MonsterSpawns[i].ObjectType, savedMapDataFile.MonsterSpawns[i].ObjectType, "Monster spawn object type values do not match");
                Assert.AreEqual(mapDataFile.MonsterSpawns[i].ObjectID, savedMapDataFile.MonsterSpawns[i].ObjectID, "Monster spawn object ID values do not match");
                Assert.AreEqual(mapDataFile.MonsterSpawns[i].MapPosition, savedMapDataFile.MonsterSpawns[i].MapPosition, "Monster spawn map positions do not match");
                Assert.AreEqual(mapDataFile.MonsterSpawns[i].Rotation, savedMapDataFile.MonsterSpawns[i].Rotation, "Monster spawn rotations do not match");
                Assert.AreEqual(mapDataFile.MonsterSpawns[i].Position, savedMapDataFile.MonsterSpawns[i].Position, "Monster spawn positions do not match");
                Assert.AreEqual(mapDataFile.MonsterSpawns[i].Scale, savedMapDataFile.MonsterSpawns[i].Scale, "Monster spawn scales do not match");
                Assert.AreEqual(mapDataFile.MonsterSpawns[i].SpawnName, savedMapDataFile.MonsterSpawns[i].SpawnName, "Monster spawn names do not match");

                Assert.AreEqual(mapDataFile.MonsterSpawns[i].NormalSpawnPoints.Count, savedMapDataFile.MonsterSpawns[i].NormalSpawnPoints.Count, "Normal spawn point counts do not match");

                for (int j = 0; j < mapDataFile.MonsterSpawns[i].NormalSpawnPoints.Count; j++) {
                    Assert.AreEqual(mapDataFile.MonsterSpawns[i].NormalSpawnPoints[j].Name, mapDataFile.MonsterSpawns[i].NormalSpawnPoints[j].Name, "Normal spawn point names do not match");
                    Assert.AreEqual(mapDataFile.MonsterSpawns[i].NormalSpawnPoints[j].Monster, mapDataFile.MonsterSpawns[i].NormalSpawnPoints[j].Monster, "Normal spawn point monster values do not match");
                    Assert.AreEqual(mapDataFile.MonsterSpawns[i].NormalSpawnPoints[j].Count, mapDataFile.MonsterSpawns[i].NormalSpawnPoints[j].Count, "Normal spawn point Count values do not match");
                }

                Assert.AreEqual(mapDataFile.MonsterSpawns[i].TacticalSpawnPoints.Count, savedMapDataFile.MonsterSpawns[i].TacticalSpawnPoints.Count, "Tactical spawn point counts do not match");

                for (int j = 0; j < mapDataFile.MonsterSpawns[i].TacticalSpawnPoints.Count; j++) {
                    Assert.AreEqual(mapDataFile.MonsterSpawns[i].TacticalSpawnPoints[j].Name, mapDataFile.MonsterSpawns[i].TacticalSpawnPoints[j].Name, "Tactical spawn point names do not match");
                    Assert.AreEqual(mapDataFile.MonsterSpawns[i].TacticalSpawnPoints[j].Monster, mapDataFile.MonsterSpawns[i].TacticalSpawnPoints[j].Monster, "Tactical spawn point monster values do not match");
                    Assert.AreEqual(mapDataFile.MonsterSpawns[i].TacticalSpawnPoints[j].Count, mapDataFile.MonsterSpawns[i].TacticalSpawnPoints[j].Count, "Tactical spawn point Count values do not match");
                }
                Assert.AreEqual(mapDataFile.MonsterSpawns[i].Interval, savedMapDataFile.MonsterSpawns[i].Interval, "Monster spawn interval values do not match");
                Assert.AreEqual(mapDataFile.MonsterSpawns[i].Limit, savedMapDataFile.MonsterSpawns[i].Limit, "Monster spawn limit values do not match");
                Assert.AreEqual(mapDataFile.MonsterSpawns[i].Range, savedMapDataFile.MonsterSpawns[i].Range, "Monster spawn range values do not match");
                Assert.AreEqual(mapDataFile.MonsterSpawns[i].TacticalVariable, savedMapDataFile.MonsterSpawns[i].TacticalVariable, "Monster spawn tactical variable values do not match");
            }

            Assert.AreEqual(mapDataFile.WaterSize, savedMapDataFile.WaterSize, "Water size values do not match");
            Assert.AreEqual(mapDataFile.WaterPlanes.Count, savedMapDataFile.WaterPlanes.Count, "Water plane counts do not match");

            for (int i = 0; i < mapDataFile.WaterPlanes.Count; i++) {
                Assert.AreEqual(mapDataFile.WaterPlanes[i].StartPosition, savedMapDataFile.WaterPlanes[i].StartPosition, "Water plane start positions do not match");
                Assert.AreEqual(mapDataFile.WaterPlanes[i].EndPosition, savedMapDataFile.WaterPlanes[i].EndPosition, "Water plane end positions do not match");
            }

            Assert.AreEqual(mapDataFile.WarpPoints.Count, savedMapDataFile.WarpPoints.Count, "Warp point counts do not match");

            for (int i = 0; i < mapDataFile.WarpPoints.Count; i++) {
                Assert.AreEqual(mapDataFile.WarpPoints[i].Name, savedMapDataFile.WarpPoints[i].Name, "Warp point names do not match");
                Assert.AreEqual(mapDataFile.WarpPoints[i].WarpID, savedMapDataFile.WarpPoints[i].WarpID, "Warp point warp ID values do not match");
                Assert.AreEqual(mapDataFile.WarpPoints[i].EventID, savedMapDataFile.WarpPoints[i].EventID, "Warp point event ID values do not match");
                Assert.AreEqual(mapDataFile.WarpPoints[i].ObjectType, savedMapDataFile.WarpPoints[i].ObjectType, "Warp point object type values do not match");
                Assert.AreEqual(mapDataFile.WarpPoints[i].ObjectID, savedMapDataFile.WarpPoints[i].ObjectID, "Warp point object ID values do not match");
                Assert.AreEqual(mapDataFile.WarpPoints[i].MapPosition, savedMapDataFile.WarpPoints[i].MapPosition, "Warp point map positions do not match");
                Assert.AreEqual(mapDataFile.WarpPoints[i].Rotation, savedMapDataFile.WarpPoints[i].Rotation, "Warp point rotations do not match");
                Assert.AreEqual(mapDataFile.WarpPoints[i].Position, savedMapDataFile.WarpPoints[i].Position, "Warp point positions do not match");
                Assert.AreEqual(mapDataFile.WarpPoints[i].Scale, savedMapDataFile.WarpPoints[i].Scale, "Warp point scales do not match");
            }

            Assert.AreEqual(mapDataFile.CollisionObjects.Count, savedMapDataFile.CollisionObjects.Count, "Collision object counts do not match");

            for (int i = 0; i < mapDataFile.CollisionObjects.Count; i++) {
                Assert.AreEqual(mapDataFile.CollisionObjects[i].Name, savedMapDataFile.CollisionObjects[i].Name, "Collision object names do not match");
                Assert.AreEqual(mapDataFile.CollisionObjects[i].WarpID, savedMapDataFile.CollisionObjects[i].WarpID, "Collision object warp ID values do not match");
                Assert.AreEqual(mapDataFile.CollisionObjects[i].EventID, savedMapDataFile.CollisionObjects[i].EventID, "Collision object event ID values do not match");
                Assert.AreEqual(mapDataFile.CollisionObjects[i].ObjectType, savedMapDataFile.CollisionObjects[i].ObjectType, "Collision object object type values do not match");
                Assert.AreEqual(mapDataFile.CollisionObjects[i].ObjectID, savedMapDataFile.CollisionObjects[i].ObjectID, "Collision object object ID values do not match");
                Assert.AreEqual(mapDataFile.CollisionObjects[i].MapPosition, savedMapDataFile.CollisionObjects[i].MapPosition, "Collision object map positions do not match");
                Assert.AreEqual(mapDataFile.CollisionObjects[i].Rotation, savedMapDataFile.CollisionObjects[i].Rotation, "Collision object rotations do not match");
                Assert.AreEqual(mapDataFile.CollisionObjects[i].Position, savedMapDataFile.CollisionObjects[i].Position, "Collision object positions do not match");
                Assert.AreEqual(mapDataFile.CollisionObjects[i].Scale, savedMapDataFile.CollisionObjects[i].Scale, "Collision object scales do not match");
            }

            Assert.AreEqual(mapDataFile.EventObjects.Count, savedMapDataFile.EventObjects.Count, "Event object counts do not match");

            for (int i = 0; i < mapDataFile.EventObjects.Count; i++) {
                Assert.AreEqual(mapDataFile.EventObjects[i].Name, savedMapDataFile.EventObjects[i].Name, "Event object names do not match");
                Assert.AreEqual(mapDataFile.EventObjects[i].WarpID, savedMapDataFile.EventObjects[i].WarpID, "Event object warp ID values do not match");
                Assert.AreEqual(mapDataFile.EventObjects[i].EventID, savedMapDataFile.EventObjects[i].EventID, "Event object event ID values do not match");
                Assert.AreEqual(mapDataFile.EventObjects[i].ObjectType, savedMapDataFile.EventObjects[i].ObjectType, "Event object object type values do not match");
                Assert.AreEqual(mapDataFile.EventObjects[i].ObjectID, savedMapDataFile.EventObjects[i].ObjectID, "Event object object ID values do not match");
                Assert.AreEqual(mapDataFile.EventObjects[i].MapPosition, savedMapDataFile.EventObjects[i].MapPosition, "Event object map positions do not match");
                Assert.AreEqual(mapDataFile.EventObjects[i].Rotation, savedMapDataFile.EventObjects[i].Rotation, "Event object rotations do not match");
                Assert.AreEqual(mapDataFile.EventObjects[i].Position, savedMapDataFile.EventObjects[i].Position, "Event object positions do not match");
                Assert.AreEqual(mapDataFile.EventObjects[i].Scale, savedMapDataFile.EventObjects[i].Scale, "Event object scales do not match");
                Assert.AreEqual(mapDataFile.EventObjects[i].FunctionName, savedMapDataFile.EventObjects[i].FunctionName, "Event object function names do not match");
                Assert.AreEqual(mapDataFile.EventObjects[i].ConversationFile, savedMapDataFile.EventObjects[i].ConversationFile, "Event object conversation file values do not match");
            }
        }
    }
}