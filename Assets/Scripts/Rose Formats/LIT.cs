// <copyright file="LIT.cs" company="Wadii Bellamine">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Wadii Bellamine, Xadet, Brett19</author>
// <date>2/25/2015 8:37 AM </date>

using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityRose.File;

namespace UnityRose.Formats
{
    /// <summary>
    /// LIT class.
    /// </summary>
    public class LIT
    {
        #region Sub Classes

        /// <summary>
        /// Object class.
        /// </summary>
        public class Object
        {
            #region Sub Classes

            /// <summary>
            /// Part class.
            /// </summary>
            public class Part
            {
                #region Member Declarations

                /// <summary>
                /// Gets or sets the name of the TGA.
                /// </summary>
                /// <value>The name of the TGA.</value>
                public string TGAName { get; set; }

                /// <summary>
                /// Gets or sets the part ID.
                /// </summary>
                /// <value>The part ID.</value>
                public int PartID { get; set; }

                /// <summary>
                /// Gets or sets the name of the DDS.
                /// </summary>
                /// <value>The name of the DDS.</value>
                public string DDSName { get; set; }

                /// <summary>
                /// Gets or sets the lightmap ID.
                /// </summary>
                /// <value>The lightmap ID.</value>
                public int LightmapID { get; set; }

                /// <summary>
                /// Gets or sets the pixels per object.
                /// </summary>
                /// <value>The pixels per object.</value>
                public int PixelsPerObject { get; set; }

                /// <summary>
                /// Gets or sets the width of the objects per.
                /// </summary>
                /// <value>The width of the objects per.</value>
                public int ObjectsPerWidth { get; set; }

                /// <summary>
                /// Gets or sets the map position.
                /// </summary>
                /// <value>The map position.</value>
                public int MapPosition { get; set; }

                #endregion
            }

            #endregion

            #region Member Declarations

            /// <summary>
            /// Gets or sets the object ID.
            /// </summary>
            /// <value>The object ID.</value>
            public int ObjectID { get; set; }

            #endregion

            #region List Declarations

            /// <summary>
            /// Gets or sets the parts.
            /// </summary>
            /// <value>The parts.</value>
            public List<Part> Parts { get; set; }

            #endregion
        }

        /// <summary>
        /// DDS class.
        /// </summary>
        public class DDS
        {
            #region Member Declarations

            /// <summary>
            /// Gets or sets the name of the file.
            /// </summary>
            /// <value>The name of the file.</value>
            public string FileName { get; set; }

            #endregion
        }

        #endregion

        #region Member Declarations

        /// <summary>
        /// Gets or sets the folder.
        /// </summary>
        /// <value>The folder.</value>
        public string Folder { get; set; }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>The file path.</value>
        public string FilePath { get; set; }

        #endregion

        #region List Declarations

        /// <summary>
        /// Gets or sets the objects.
        /// </summary>
        /// <value>The objects.</value>
        public List<Object> Objects { get; set; }

        /// <summary>
        /// Gets or sets the DDS files.
        /// </summary>
        /// <value>The DDS files.</value>
        public List<DDS> DDSFiles { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="LIT"/> class.
        /// </summary>
        public LIT()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LIT"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="device">The device.</param>
        public LIT(string filePath)
        {
            Load(filePath);
        }

        /// <summary>
        /// Loads the specified file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="device">The device.</param>
        public void Load(string filePath)
        {
          /*
            if (!Directory.Exists(filePath))
            {
				Debug.LogError(string.Format(@"Missing File: {0}", filePath));
                Objects = new List<Object>(0);
                DDSFiles = new List<DDS>(0);

                return;
            }
            
            */
            Folder = Path.GetDirectoryName(filePath);

            try
            {
                FileHandler fh = new FileHandler(FilePath = filePath, FileHandler.FileOpenMode.Reading, Encoding.UTF8);

                int objectCount = fh.Read<int>();
                Objects = new List<Object>(objectCount);

                for (int i = 0; i < objectCount; i++)
                {
                    int partCount = fh.Read<int>();

                    Objects.Add(new Object()
                    {
                        ObjectID = fh.Read<int>(),
                        Parts = new List<Object.Part>(partCount)
                    });

                    for (int j = 0; j < partCount; j++)
                    {
                        Objects[i].Parts.Add(new Object.Part()
                        {
                            TGAName = fh.Read<BString>(),
                            PartID = fh.Read<int>(),
                            DDSName = fh.Read<BString>(),
                            LightmapID = fh.Read<int>(),
                            PixelsPerObject = fh.Read<int>(),
                            ObjectsPerWidth = fh.Read<int>(),
                            MapPosition = fh.Read<int>()
                        });
                    }
                }

                int ddsCount = fh.Read<int>();
                DDSFiles = new List<DDS>(ddsCount);

                for (int i = 0; i < ddsCount; i++)
                {
                    string fileName = fh.Read<BString>();

                    DDSFiles.Add(new DDS()
                    {
                        FileName = fileName
                    });
                }

                fh.Close();
            }
            catch
            {
				Debug.LogError (string.Format(@"Error Reading File: {0}\{1}", Folder, Path.GetFileName(filePath)));
            }
        }

        /// <summary>
        /// Saves the file.
        /// </summary>
        public void Save()
        {
            Save(FilePath);
        }

        /// <summary>
        /// Saves the specified file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public void Save(string filePath)
        {
            FileHandler fh = new FileHandler(FilePath = filePath, FileHandler.FileOpenMode.Writing, Encoding.UTF8);

            fh.Write<int>(Objects.Count);

            for (int i = 0; i < Objects.Count; i++)
            {
                fh.Write<int>(Objects[i].Parts.Count);
                fh.Write<int>(Objects[i].ObjectID);

                for (int j = 0; j < Objects[i].Parts.Count; j++)
                {
                    fh.Write<BString>(Objects[i].Parts[j].TGAName);
                    fh.Write<int>(Objects[i].Parts[j].PartID);
                    fh.Write<BString>(Objects[i].Parts[j].DDSName);
                    fh.Write<int>(Objects[i].Parts[j].LightmapID);
                    fh.Write<int>(Objects[i].Parts[j].PixelsPerObject);
                    fh.Write<int>(Objects[i].Parts[j].ObjectsPerWidth);
                    fh.Write<int>(Objects[i].Parts[j].MapPosition);
                }
            }

            fh.Write<int>(DDSFiles.Count);

            for (int i = 0; i < DDSFiles.Count; i++)
                fh.Write<BString>(DDSFiles[i].FileName);

            fh.Close();
        }

        /// <summary>
        /// Searches for the object.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The object index.</returns>
        public int SearchObject(int id)
        {
            for (int i = 0; i < Objects.Count; i++)
            {
                if (Objects[i].ObjectID == id)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Searches the part.
        /// </summary>
        /// <param name="objectID">The object ID.</param>
        /// <param name="id">The id.</param>
        /// <returns>The part index.</returns>
        public int SearchPart(int objectID, int id)
        {
            for (int i = 0; i < Objects[objectID].Parts.Count; i++)
            {
                if (Objects[objectID].Parts[i].PartID == id)
                    return i;
            }

            return -1;
        }
    }
}
