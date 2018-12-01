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
using Revise.Files.ZON;

namespace Revise.Files.Tests {
    /// <summary>
    /// Provides testing for the <see cref="ZoneFile"/> class.
    /// </summary>
    [TestFixture]
    public class ZoneFileTests {
        private const string TEST_FILE = "Tests/Revise/Files/JPT01.ZON";

        /// <summary>
        /// Tests the load method.
        /// </summary>
        [Test]
        public void TestLoadMethod() {
            Stream stream = File.OpenRead(TEST_FILE);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            ZoneFile zoneFile = new ZoneFile();
            zoneFile.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(fileSize, streamPosition, "Not all of the file was read");
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        [Test]
        public void TestSaveMethod() {
            ZoneFile zoneFile = new ZoneFile();
            zoneFile.Load(TEST_FILE);

            MemoryStream savedStream = new MemoryStream();
            zoneFile.Save(savedStream);

            savedStream.Seek(0, SeekOrigin.Begin);

            ZoneFile savedZoneFile = new ZoneFile();
            savedZoneFile.Load(savedStream);

            savedStream.Close();

            Assert.AreEqual(zoneFile.Type, savedZoneFile.Type, "Type values do not match");
            Assert.AreEqual(zoneFile.Width, savedZoneFile.Width, "Width values do not match");
            Assert.AreEqual(zoneFile.Height, savedZoneFile.Height, "Height values do not match");
            Assert.AreEqual(zoneFile.GridCount, savedZoneFile.GridCount, "Grid count values do not match");
            Assert.AreEqual(zoneFile.GridSize, savedZoneFile.GridSize, "Grid size values do not match");
            Assert.AreEqual(zoneFile.StartPosition, savedZoneFile.StartPosition, "Start position values do not match");

            for (int x = 0; x < zoneFile.Width; x++) {
                for (int y = 0; y < zoneFile.Height; y++) {
                    Assert.AreEqual(zoneFile.Positions[x, y].IsUsed, savedZoneFile.Positions[x, y].IsUsed, "Is used values do not match");
                    Assert.AreEqual(zoneFile.Positions[x, y].Position, savedZoneFile.Positions[x, y].Position, "Positions do not match");
                }
            }

            Assert.AreEqual(zoneFile.SpawnPoints.Count, savedZoneFile.SpawnPoints.Count, "Spawn counts do not match");

            for (int i = 0; i < zoneFile.SpawnPoints.Count; i++) {
                Assert.AreEqual(zoneFile.SpawnPoints[i].Position, savedZoneFile.SpawnPoints[i].Position, "Spawn point positions do not match");
                Assert.AreEqual(zoneFile.SpawnPoints[i].Name, savedZoneFile.SpawnPoints[i].Name, "Spawn point names do not match");
            }

            Assert.AreEqual(zoneFile.Textures.Count, savedZoneFile.Textures.Count, "Texture counts do not match");

            for (int i = 0; i < zoneFile.Textures.Count; i++) {
                Assert.AreEqual(zoneFile.Textures[i], savedZoneFile.Textures[i], "Texture file paths do not match");
            }

            Assert.AreEqual(zoneFile.Tiles.Count, savedZoneFile.Tiles.Count, "Tile counts do not match");

            for (int i = 0; i < zoneFile.Tiles.Count; i++) {
                Assert.AreEqual(zoneFile.Tiles[i].Layer1, savedZoneFile.Tiles[i].Layer1, "Tile layer 1 values do not match");
                Assert.AreEqual(zoneFile.Tiles[i].Layer2, savedZoneFile.Tiles[i].Layer2, "Tile layer 2 values do not match");
                Assert.AreEqual(zoneFile.Tiles[i].Offset1, savedZoneFile.Tiles[i].Offset1, "Tile offset 1 values do not match");
                Assert.AreEqual(zoneFile.Tiles[i].Offset2, savedZoneFile.Tiles[i].Offset2, "Tile offset 2 values do not match");
                Assert.AreEqual(zoneFile.Tiles[i].BlendingEnabled, savedZoneFile.Tiles[i].BlendingEnabled, "Tile blending enabled values do not match");
                Assert.AreEqual(zoneFile.Tiles[i].Rotation, savedZoneFile.Tiles[i].Rotation, "Tile rotation values do not match");
                Assert.AreEqual(zoneFile.Tiles[i].TileType, savedZoneFile.Tiles[i].TileType, "Tile type values do not match");
            }

            Assert.AreEqual(zoneFile.Name, savedZoneFile.Name, "Name values do not match");
            Assert.AreEqual(zoneFile.IsUnderground, savedZoneFile.IsUnderground, "Is underground values do not match");
            Assert.AreEqual(zoneFile.BackgroundMusicFilePath, savedZoneFile.BackgroundMusicFilePath, "Background music file paths do not match");
            Assert.AreEqual(zoneFile.SkyFilePath, savedZoneFile.SkyFilePath, "Sky file paths do not match");
            Assert.AreEqual(zoneFile.EconomyCheckRate, savedZoneFile.EconomyCheckRate, "Economy check rate values do not match");
            Assert.AreEqual(zoneFile.PopulationBase, savedZoneFile.PopulationBase, "Population base values do not match");
            Assert.AreEqual(zoneFile.PopulationGrowthRate, savedZoneFile.PopulationGrowthRate, "Population growth rate values do not match");
            Assert.AreEqual(zoneFile.MetalConsumption, savedZoneFile.MetalConsumption, "Metal consumption values do not match");
            Assert.AreEqual(zoneFile.StoneConsumption, savedZoneFile.StoneConsumption, "Stone consumption values do not match");
            Assert.AreEqual(zoneFile.WoodConsumption, savedZoneFile.WoodConsumption, "Wood consumption values do not match");
            Assert.AreEqual(zoneFile.LeatherConsumption, savedZoneFile.LeatherConsumption, "Leather consumption values do not match");
            Assert.AreEqual(zoneFile.ClothConsumption, savedZoneFile.ClothConsumption, "Cloth consumption values do not match");
            Assert.AreEqual(zoneFile.AlchemyConsumption, savedZoneFile.AlchemyConsumption, "Alchemy consumption values do not match");
            Assert.AreEqual(zoneFile.ChemicalConsumption, savedZoneFile.ChemicalConsumption, "Chemical consumption values do not match");
            Assert.AreEqual(zoneFile.IndustrialConsumption, savedZoneFile.IndustrialConsumption, "Industrial consumption values do not match");
            Assert.AreEqual(zoneFile.MedicineConsumption, savedZoneFile.MedicineConsumption, "Medicine consumption values do not match");
            Assert.AreEqual(zoneFile.FoodConsumption, savedZoneFile.FoodConsumption, "Food consumption values do not match");
        }
    }
}