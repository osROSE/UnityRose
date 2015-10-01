// <copyright file="ZMS.cs" company="Wadii Bellamine">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Wadii Bellamine, Xadet, Brett19</author>
// <date>2/25/2015 8:37 AM </date>

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityRose.File;

namespace UnityRose.Formats
{

	
	
    public class WeightIdPair : IComparable
	{
		public float weight;
		public int id;
		
        int IComparable.CompareTo(object obj)
        {
			WeightIdPair a = this;
            WeightIdPair b = (WeightIdPair)obj;
			
            if (a.weight > b.weight)
				return 1;
            else if (a.weight < b.weight)
				return -1;
			else
				return 0;
		}
		
		public WeightIdPair(int id, float weight)
		{
			this.id = id;
			this.weight = weight;
		}
		
		
			
	}
	
	/// <summary>
	/// ZMS class.
	/// </summary>
	public class ZMS
	{
		public class Support{
			public bool vertices;
			public bool normals;
			public bool bones;
			public bool color;
			public bool tangents;
			public bool uv0;
			public bool uv1;
			public bool uv2;
			public bool uv3;
			
			public Support()
			{
				vertices = normals = bones = color = tangents = uv0 = uv1 = uv2 = uv3 = false;
			}
		}
		
		#region Member Declarations
		
		
		/// <summary>
		/// Gets or sets the file path.
		/// </summary>
		/// <value>The file path.</value>
		public string FilePath { get; set; }
		
		public Vector3[] vertices { get; set; }
		
		public int[] triangles { get; set; }
		
		public short[] bones { get; set; }
		
		public BoneWeight[] boneWeights { get; set; }
		
		public Vector3[] normals { get; set; }
		
		public Vector2[] uv0 { get; set; }
		public Vector2[] uv1 { get; set; }
		public Vector2[] uv2 { get; set; }
		public Vector2[] uv3 { get; set; }
		
        public Vector2 lmOffset { get; set; }
        public Vector2 lmScale { get; set; }
		
		public short[] materials {get; set;}
		public short[] strips {get; set;}
		
		public Support support { get; set; }
		
        public short MaterialCount { get; set; }
        public short StripCount { get; set; }
        public short PoolType { get; set; }
		
		
		/// <summary>
		/// Gets or sets the vertex count.
		/// </summary>
		/// <value>The vertex count.</value>
		public short VertexCount { get; set; }
		
		/// <summary>
		/// Gets or sets the index count.
		/// </summary>
		/// <value>The index count.</value>
		public short IndexCount { get; set; }
		
		private bool RecalcNormals;
		private bool isTree;

		
		#endregion
		
		/// <summary>
		/// Initializes a new instance of the <see cref="ZMS"/> class.
		/// </summary>
		public ZMS()
		{
            this.lmScale = new Vector2(1.0f, 1.0f);
            this.lmOffset = new Vector2(0.0f, 0.0f);
        }
		
		/// <summary>
		/// Initializes a new instance of the <see cref="ZMS"/> class.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		public ZMS(string filePath)
		{
            this.lmScale = new Vector2(1.0f, 1.0f);
            this.lmOffset = new Vector2(0.0f, 0.0f);
            isTree = filePath.ToLower().Contains("tree") && !filePath.ToLower().Contains("street");
			Load(filePath);
		}
		
        public ZMS(string filePath, Vector2 lmScale, Vector2 lmOffset)
        {
            this.lmScale = lmScale;
            this.lmOffset = lmOffset;
			isTree = filePath.ToLower().Contains("tree") && !filePath.ToLower().Contains("street");
            Load(filePath);  
        }
		
		public Mesh getMesh()
		{
			Mesh mesh = new Mesh();
			mesh.vertices = vertices;
			mesh.triangles = triangles;
			mesh.normals = normals;
			mesh.uv = uv0;
			//mesh.uv1 = uv1;
			mesh.uv2 = uv1;
			mesh.boneWeights = boneWeights;
            if (RecalcNormals)
			{
				mesh.RecalculateNormals();
				/*
				Vector3[] normals2 = new Vector3[mesh.vertexCount];
				Dictionary<String, List<int>> vertexLookup = new Dictionary<String, List<int>>();
				// 1. and 2.
                for (int i = 0; i < mesh.vertexCount; i++)
					Utils.Utils.addVertexToLookup(vertexLookup, mesh.vertices[i].ToString(), i);
				
				// traverse the shared vertex list and calculate new normals
				
                foreach (KeyValuePair<String, List<int>> entry in vertexLookup)
				{
					Vector3 avg = Vector3.zero;
                    foreach (int id in entry.Value)
					{
						avg = mesh.normals[id];
                        foreach (int id2 in entry.Value)
						{
                            if (Math.Abs(Vector3.Angle(mesh.normals[id], mesh.normals[id2])) <= 80.0f)
								avg += mesh.normals[id2];
						}
						avg.Normalize();
						normals2[id] = avg;
					}	
				}

				mesh.normals = normals2;
                 * */
			}
			Utils.calculateMeshTangents(mesh);
			mesh.RecalculateBounds();
			mesh.Optimize();
			return mesh;
		}

        private TextAsset asset;
        private FileHandler fh;

        /// <summary>
        /// Loads the specified file.  If resource is found, it is loaded as a Text asset.  Otherwise, the function
        /// assumes this is an editor load and reads from disk
        /// </summary>
        /// <param name="filePath">The file path of the Text Asset resource (without extension) or file (with extension) to load</param>
        public void Load(string filePath)
        {
            asset = Resources.Load(filePath) as TextAsset;
            if (asset != null)
                fh = new FileHandler(asset, null);
            else
                fh = new FileHandler(filePath, FileHandler.FileOpenMode.Reading, null);

            Load();

        }

        /// <summary>
        /// Loads the specified file.
        /// </summary>
        private void Load()
		{
			support = new Support();
			fh.Seek(0, SeekOrigin.Begin); // bounding box
			string formatCode = fh.Read<ZString>();
			int version = int.Parse(formatCode.Substring(6));
			
			int format = fh.Read<int>();
			
			fh.Seek(24, SeekOrigin.Current); // bounding box
			
			short boneCount = fh.Read<short>();
			
			bones = new short[boneCount];
			
            for (int i = 0; i < boneCount; i++)
				bones[i] = fh.Read<short>();
			
			VertexCount = fh.Read<short>();
			
			// Vertices
			if ((format & 2) > 0)
			{
				support.vertices = true;
				vertices = new Vector3[VertexCount];
				for (int i = 0; i < VertexCount; i++)
				{
                    if (version <= 6)
						fh.Read<short>();
                    vertices[i] = Utils.r2uPosition(new Vector3()
                    {
                        x = fh.Read<float>(),
                        y = fh.Read<float>(),
                        z = fh.Read<float>()
                    });
					
					//Vertices[i].Alpha = new Color(0, 0, 0, 255);
				}
			}
			
			// normals
			if ((format & 4) > 0)
			{
				support.normals = true;
				normals = new Vector3[VertexCount];
				RecalcNormals = false;
				for (int i = 0; i < VertexCount; i++)
				{
                    if (version <= 6)
						fh.Read<short>();
                    normals[i] = Utils.r2uVector(new Vector3()
                    {
                        x = fh.Read<float>(),
                        y = fh.Read<float>(),
                        z = fh.Read<float>()
                    });
				}
				//fh.Read<Vector3>();
			}
			else
				RecalcNormals = true;
				
			if( isTree )
				RecalcNormals = true;
			
			// color (not used - skip)
			if ((format & 8) > 0)
				fh.Seek(4 * VertexCount, SeekOrigin.Current);
			
								
			// bone weights
			if ((format & 16) > 0 && (format & 32) > 0)
			{
				support.bones = true;
				boneWeights = new BoneWeight[VertexCount];
                for (int i = 0; i < VertexCount; i++)
				{
                    if (version <= 6)
						fh.Read<short>();
					
					BoneWeight weight = new BoneWeight();
					weight.weight0 = fh.Read<float>();
					weight.weight1 = fh.Read<float>();
					weight.weight2 = fh.Read<float>();
					weight.weight3 = fh.Read<float>();
		
					
					weight.boneIndex0 = bones[fh.Read<short>()];
					weight.boneIndex1 = bones[fh.Read<short>()];
					weight.boneIndex2 = bones[fh.Read<short>()];
					weight.boneIndex3 = bones[fh.Read<short>()];
					
					
					List<WeightIdPair> weightIdList = new List<WeightIdPair>(4);
                    weightIdList.Add(new WeightIdPair(weight.boneIndex0, weight.weight0));
                    weightIdList.Add(new WeightIdPair(weight.boneIndex1, weight.weight1));
                    weightIdList.Add(new WeightIdPair(weight.boneIndex2, weight.weight2));
                    weightIdList.Add(new WeightIdPair(weight.boneIndex3, weight.weight3));
					
					weightIdList.Sort();
					
					weight.boneIndex0 = weightIdList[3].id;
					weight.boneIndex1 = weightIdList[2].id;
					weight.boneIndex2 = weightIdList[1].id;
					weight.boneIndex3 = weightIdList[0].id;
					
					weight.weight0 = weightIdList[3].weight;
					weight.weight1 = weightIdList[2].weight;
					weight.weight2 = weightIdList[1].weight;
					weight.weight3 = weightIdList[0].weight;
					
					this.boneWeights[i] = weight;
				}
			}
			
			// tangents
			if ((format & 64) > 0)
				fh.Seek(12 * VertexCount, SeekOrigin.Current);
				
			if ((format & 128) > 0) {
				support.uv0 = true;
				uv0 = new Vector2[VertexCount];
				for (int i = 0; i < VertexCount; i++)
                {
					uv0[i] = new Vector2()
				{
					x = fh.Read<float>(),
					y = 1.0f - fh.Read<float>()
				};
			}
			}
			
            if ((format & 256) > 0)
            {
				support.uv1 = true;
				uv1 = new Vector2[VertexCount];
				for (int i = 0; i < VertexCount; i++)
                {
					uv1[i] = new Vector2()
				{
					x = fh.Read<float>(),
                        y = fh.Read<float>()
				};

                    uv1[i] =  Vector2.Scale(uv1[i], lmScale) + lmOffset;
                    uv1[i].y = 1.0f - uv1[i].y;
                }
			}
			
            if ((format & 512) > 0)
            {
				support.uv2 = true;
				uv2 = new Vector2[VertexCount];
				for (int i = 0; i < VertexCount; i++)
					uv2[i] = new Vector2()
				{
					x = fh.Read<float>(),
					y = 1.0f - fh.Read<float>()
				};
			}
			
            if ((format & 1024) > 0)
            {
				support.uv3 = true;
				uv3 = new Vector2[VertexCount];
				for (int i = 0; i < VertexCount; i++)
					uv3[i] = new Vector2()
				{
					x = fh.Read<float>(),
					y = 1.0f - fh.Read<float>()
				};
			}

			IndexCount = fh.Read<short>();
			int vs = isTree ? 6 : 3;
			triangles = new int[IndexCount * vs];
			for (int i = 0; i < IndexCount; i++)
			{
                triangles[i * vs + 0] = (int)fh.Read<short>();
                triangles[i * vs + 1] = (int)fh.Read<short>();
                triangles[i * vs + 2] = (int)fh.Read<short>();	
                
                if( isTree )
                {
					triangles[i * vs + 3] = triangles[i * vs + 2];
					triangles[i * vs + 4] = triangles[i * vs + 1];
					triangles[i * vs + 5] = triangles[i * vs + 0];	
                }
			}
			
			MaterialCount = fh.Read<short>();
			materials = new short[MaterialCount];
            for (int i = 0; i < MaterialCount; i++)
            {
				materials[i] = fh.Read<short>();
			}
			
			StripCount = fh.Read<short>();
			strips = new short[StripCount];
            for (int i = 0; i < StripCount; i++)
            {
				strips[i] = fh.Read<short>();
			}
			
            if (version >= 8)
            {
				PoolType = fh.Read<short>();
			}
			
			fh.Close();

            // Release the unity resource since its contents are now in memory
            if( asset != null )
                Resources.UnloadAsset(asset);
        }

    }
}