// <copyright file="RosePatch.cs" company="Wadii Bellamine">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Wadii Bellamine</author>
// <date>2/25/2015 8:37 AM </date>

// ------------------------------------------------------------------------------
//  <authors>
//      Wadii Bellamine
//      3/14/2014
//  </authors>
// ------------------------------------------------------------------------------

#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityRose.Formats;

namespace UnityRose.Game 
{
	public class RosePatch
	{
		private const bool realTimeBaking = false;
		private const bool blendNormals = false;
		public DirectoryInfo m_assetDir { get; set; }
        public DirectoryInfo m_unityAssetDir { get; set; }
		public DirectoryInfo m_3dDataDir { get; set; }
		public string m_name { get; set; }
		public bool m_isValid { get; set; }
        public Vector2 center { get; set; }
		
		public HIM m_HIM{ get; set; }
		public TIL m_TIL{ get; set; }
		public ZON m_ZON{ get; set; }
		public IFO m_IFO{ get; set; }
		public ZSC m_ZSC_Cnst{ get; set; }
		public ZSC m_ZSC_Deco{get; set; }
        public LIT m_LIT_Cnst { get; set; }
        public LIT m_LIT_Deco { get; set; }
		// TODO: Add MOV, and any others here
		
        public int m_Col { get; set; }
        public int m_Row { get; set; }
		
        public Mesh m_mesh { get; set; }
        public List<Tile> m_tiles { get; set; }
		
        public Dictionary<String, List<int>> edgeVertexLookup { get; set; }
		
		private string groundLight;
		
		// Default contsructor
		public RosePatch()
		{
			this.m_Col = 0;
			this.m_Row = 0;
			this.m_isValid = false;
		}
		
		// Functional constructor 1
        public RosePatch(DirectoryInfo assetDir)
		{
			this.m_assetDir = assetDir;
			this.m_3dDataDir = new DirectoryInfo(this.m_assetDir.Parent.Parent.Parent.Parent.FullName);
            this.m_name = assetDir.Name.Replace(".*", "");
			this.m_Col = 0;
			this.m_Row = 0;
			this.m_isValid = false;
			this.center = new Vector2(0.0f, 0.0f);
			
			if (assetDir.Exists)
			{
				// figure out row and column
                char[] sep = { '_', '.' };
				string[] tokens = assetDir.Name.Split(sep);
				int col = int.Parse(tokens[0]);
				int row = int.Parse(tokens[1]);
				
				// figure out if the given name exists and this patch is valid
				if (row > 0 && row < 100 && col > 0 && col < 100)
					m_isValid = true;
				
                if (m_isValid)
				{
					m_Row = row;
					m_Col = col;
				}
			}
			else
				m_isValid = false;
			
		}
		
		// Functional constrcutor 2
        public RosePatch(DirectoryInfo assetDir, ZON zon)
            : this(assetDir)
		{
			this.m_ZON = zon;
		}
		
		public bool Load()
		{
            if (!m_isValid)
			{
				Debug.LogError("Cannot load patch at path " + this.m_assetDir);
				return false;
			}
			
			// TODO: add error handling for failure to load the following files
			this.m_HIM = new HIM(this.m_assetDir.Parent.FullName + "/" + this.m_name + ".HIM");
			this.m_TIL = new TIL(this.m_assetDir.Parent.FullName + "/" + this.m_name + ".TIL");
            if (this.m_ZON == null)  // load ZON if it was never passed to the patch constructor
				this.m_ZON = new ZON(this.m_assetDir.Parent.FullName + "/" + this.m_assetDir.Parent.Name + ".ZON");
			this.m_IFO = new IFO(this.m_assetDir.Parent.FullName + "/" + this.m_name + ".IFO");
			
			//	parent x   :       	4	  3		2	  1	   0
			// original dir: .../3ddata/maps/junon/jpt01/30_30
			// desired dir: .../3ddata/junon/LIST_CNST_JPT.ZSC
            char[] trimChars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '_' };
            
            // TODO: use stl's to avoid this mess and be able to load luna/eldeon/oro maps
			string zscPath = m_assetDir.Parent.Name.ToLower().Trim(trimChars);
			if(zscPath.Contains("jd") || zscPath.Contains("jdt") || zscPath.Contains("jg") )
				zscPath = Utils.FixPath(m_3dDataDir + "/" + m_assetDir.Parent.Parent.Name.ToUpper() + "/LIST_CNST_" + zscPath + ".ZSC");// + m_assetDir.Parent.Name.Trim(trimChars).ToUpper() + ".ZSC");
			else
				zscPath = Utils.FixPath(m_3dDataDir + "/" + m_assetDir.Parent.Parent.Name.ToUpper() + "/LIST_" + "CNST_JPT.ZSC");// + m_assetDir.Parent.Name.Trim(trimChars).ToUpper() + ".ZSC");
			
			string litPath = Utils.FixPath(this.m_assetDir.Parent.FullName + "\\" + this.m_name + "\\LIGHTMAP\\BUILDINGLIGHTMAPDATA.LIT");
			groundLight = Utils.FixPath(this.m_assetDir.Parent.FullName + "\\" + this.m_name + "\\" + this.m_name + "_PLANELIGHTINGMAP.DDS");
			m_ZSC_Cnst = new ZSC(zscPath);
			m_ZSC_Deco = new ZSC(zscPath.ToLower().Replace("cnst","deco"));
            m_LIT_Cnst = new LIT(litPath);
            m_LIT_Deco = new LIT(litPath.Replace("building","object"));
			// TODO: add any new file loads here
			
			edgeVertexLookup = new Dictionary<string, List<int>>();
			
			return true;    
		}
		
		
		public void UpdateAtlas(ref Dictionary<string, Rect> atlasRectHash, ref Dictionary<string, Texture2D> atlasTexHash, ref List<Texture2D> textures)
		{
			for (int t_x = 0; t_x < 16; t_x++)
			{
				for (int t_y = 0; t_y < 16; t_y++)
				{
					int tileID = m_TIL.Tiles[t_x, t_y].TileID;
					string texPath1 = m_ZON.Textures[m_ZON.Tiles[tileID].ID1].TexPath;
					string texPath2 = m_ZON.Textures[m_ZON.Tiles[tileID].ID2].TexPath;
					
					Texture2D tex1 = m_ZON.Textures[m_ZON.Tiles[tileID].ID1].Tex;
					Texture2D tex2 = m_ZON.Textures[m_ZON.Tiles[tileID].ID2].Tex;
					// Adding an existing texture to atlas will cause an exception, so catch it but do nothing
					// as this is expected to happen
					try {
						atlasRectHash.Add(texPath1, new Rect());
						atlasTexHash.Add(texPath1, tex1);
						textures.Add ( tex1 );
					} catch( Exception e){}
					
					try {
						atlasRectHash.Add(texPath2, new Rect());
						atlasTexHash.Add(texPath2, tex2);
						textures.Add ( tex2 );
					} catch( Exception e){}
				}
			}
		}
		
		public bool Import(Transform terrainParent, Transform objectsParent, Texture2D atlas, Texture2D atlas_normal, Dictionary<string, Rect> atlasRectHash)
		{
			
            if (!m_isValid)
			{
                Debug.LogError("Cannot Import patch_" + this.m_name);
				return false;
			}
			
			// Begin the real work
			
			
			// Each 4 heights are connected together as a quad
			// Each * below is a datapoint from HIM file representing height z
			//       (0,0)    (1,0)    (2,0)
			//            *----*----*
			//            |    |    |
			//      (0,1) *----*----* (2,1)
			//            |    |    |
			//      (0,2) *----*----* (2,2)
			//
			
			
			// Mesh components
			// path = 16x16 tiles
			// tile = 4x4 quads
			// quad = 2 triangles
			// triangle = 3 vertices
			
            int nVertices = 64 * 64 * 4;
			Vector3[] vertices = new Vector3[nVertices];
			Vector2[] uvsBottom = new Vector2[nVertices];
            Vector2[] uvsTop = new Vector2[nVertices];
			Color[] uvsLight= new Color[nVertices];
			int[] triangles = new int[(m_HIM.Length-1)*(m_HIM.Width-1)*6];
			
			int i_v = 0;      // vertex index
			int i_t = 0;     // triangle index
			
			// TODO: move these hardcoded values to a more appropriate place
            float m_xStride = 2.5f;
            float m_yStride = 2.5f;
            float heightScaler = 300.0f / (m_xStride * 1.2f);
            float x_offset = this.m_Row * m_xStride * 64.0f;
            float y_offset = this.m_Col * m_yStride * 64.0f;
            center = new Vector2(x_offset + m_xStride * 32.0f, y_offset + m_yStride * 32.0f);
			
			m_mesh = new Mesh();
        
			
			
			//  Uv mapping for tiles
			//	x%5 =   0     1	     2	   3     4	 
			//   	 (0,1) (.25,1)(.5,1)(.75,1)(1,1)
			//		 	*-----*-----*-----*-----*
			//			|   / |   / |   / |   / |
			//		    | /   | /   | /   | /   |
			//  (0,.75)	*-----*-----*-----*-----*
			//			|   / |   / |   / |   / |
			//			| /   | /   | /   | /   |
			//	(0,.5)	*-----*-----*-----*-----*
			//			|   / |   / |   / |   / |
			//			| /   | /   | /   | /   |
			//	(0,.25)	*-----*-----*-----*-----*
			//			|   / |   / |   / |   / |
			//			| /   | /   | /   | /   |
			//  (0,0)	*-----*-----*-----*-----*
			//			   (.25,0)(.5,0)(.75,0)(1,0)
			
			
            Vector2[,] uvMatrix = new Vector2[5, 5];
            Vector2[,] uvMatrixLR = new Vector2[5, 5];
            Vector2[,] uvMatrixTB = new Vector2[5, 5];
            Vector2[,] uvMatrixLRTB = new Vector2[5, 5];
            Vector2[,] uvMatrixRotCW = new Vector2[5, 5];  // rotated 90 deg clockwise
            Vector2[,] uvMatrixRotCCW = new Vector2[5, 5];	// rotated 90 counter clockwise
			
            for (int uv_x = 0; uv_x < 5; uv_x++)
			{
                for (int uv_y = 0; uv_y < 5; uv_y++)
				{
                    uvMatrix[uv_y, uv_x] = new Vector2(0.25f * (float)uv_x, 1.0f - 0.25f * (float)uv_y);
                    uvMatrixLR[uv_y, uv_x] = new Vector2(1.0f - 0.25f * (float)uv_x, 1.0f - 0.25f * (float)uv_y);
                    uvMatrixTB[uv_y, uv_x] = new Vector2(0.25f * (float)uv_x, 0.25f * (float)uv_y);
                    uvMatrixLRTB[uv_y, uv_x] = new Vector2(1.0f - 0.25f * (float)uv_x, 0.25f * (float)uv_y);
                    uvMatrixRotCCW[uv_x, uv_y] = new Vector2(0.25f * (float)uv_x, 1.0f - 0.25f * (float)uv_y);
                    uvMatrixRotCW[uv_x, uv_y] = new Vector2(0.25f * (float)uv_y, 1.0f - 0.25f * (float)uv_x);
				}
			}
				
			m_tiles = new List<Tile>();

			// Populate tiles with texture references
            for (int t_x = 0; t_x < 16; t_x++)
			{
                for (int t_y = 0; t_y < 16; t_y++)
				{
					Tile tile = new Tile( );
					int tileID = m_TIL.Tiles[t_y, t_x].TileID;
					string texPath1 = m_ZON.Textures[m_ZON.Tiles[tileID].ID1].TexPath;
					string texPath2 = m_ZON.Textures[m_ZON.Tiles[tileID].ID2].TexPath;
					tile.bottomTex = texPath1;
					tile.topTex = texPath2;
					m_tiles.Add(tile);
					
				}
			}		
			
			//string lightTexPath = "Assets/3DDATA/MAPS/JUNON/JPT01/" + m_Col + "_" + m_Row + "/" + m_Col + "_" + m_Row + "_PLANELIGHTINGMAP.dds";
			Texture2D lightTex = Utils.loadTex( ref groundLight );// Resources.LoadAssetAtPath<Texture2D>(lightTexPath);  //Utils.loadTex(lightTexPath, "Assets/GameData/Textures/Lightmaps/");                          
			//Utils.convertTex( lightTexPath, "Assets/GameData/Textures/Lightmaps/", ref lightTex);
			
			
			// copy rects to tiles
			foreach(Tile tile in m_tiles)
			{
				tile.bottomRect = atlasRectHash[tile.bottomTex];
				tile.topRect = atlasRectHash[tile.topTex];
			}
			
			// Generate a material
			
			Material material = null;
			if(realTimeBaking)
			{
				material = (Material)AssetDatabase.LoadMainAssetAtPath("Assets/Materials/JPT01.mat"); // new Material(Shader.Find( "Custom/StandardTerrain"));
				//material.SetTexture("_MainTex", atlas);
				//material.SetTexture("_DetailAlbedoMap", atlas);
			}
			else
			{
				material = new Material(Shader.Find( "Custom/TerrainShader2"));
				material.SetTexture("_BottomTex", atlas);
				material.SetTexture("_TopTex", atlas);
				material.SetTexture("_LightTex", lightTex);
			}
			
            float l = m_HIM.Length - 1;
            float w = m_HIM.Width - 1;
			
			int triangleID = 0;
			// Generate vertices and triangles
            for (int x = 0; x < m_HIM.Length - 1; x++)
			{
                for (int y = 0; y < m_HIM.Width - 1; y++)
				{
					//    Each quad will be split into two triangles:
					//
					//          a         b
					//            *-----*
					//            |   / |
					//            |  /  |
					//            | /   |
					//          d *-----* c         
					//
					//  The triangles used are: adb and bdc
					
					
					int a = i_v++;
					int b = i_v++;
					int c = i_v++;
					int d = i_v++;
					


                    // Calculate lightmap UV's (placed in color because mesh only has uv and uv2)
                    uvsLight[a] = new Color((float)y / w, 1.0f - (float)x / l, 0.0f);
					uvsLight[b] = new Color((float)y / w, 1.0f - (float)(x + 1) / l, 0.0f);
					uvsLight[c] = new Color((float)(y + 1) / w, 1.0f - (float)(x + 1) / l, 0.0f);
					uvsLight[d] = new Color((float)(y + 1) / w, 1.0f - (float)(x) / l, 0.0f);
                    
					// Calculate vertices
                    vertices[a] = new Vector3(x * m_xStride + x_offset, m_HIM.Heights[x, y] / heightScaler, y * m_yStride + y_offset);
                    vertices[b] = new Vector3((x + 1) * m_xStride + x_offset, m_HIM.Heights[x + 1, y] / heightScaler, y * m_yStride + y_offset);
                    vertices[c] = new Vector3((x + 1) * m_xStride + x_offset, m_HIM.Heights[x + 1, y + 1] / heightScaler, (y + 1) * m_yStride + y_offset);
                    vertices[d] = new Vector3(x * m_xStride + x_offset, m_HIM.Heights[x, y + 1] / heightScaler, (y + 1) * m_yStride + y_offset);
					
					
                    if (y == 0)
					{
						Utils.addVertexToLookup(edgeVertexLookup, vertices[a].ToString(), a);
						Utils.addVertexToLookup(edgeVertexLookup, vertices[a].ToString(), b);
					}
                    if (y == m_HIM.Width - 1)
					{
						Utils.addVertexToLookup(edgeVertexLookup, vertices[a].ToString(), d);
						Utils.addVertexToLookup(edgeVertexLookup, vertices[a].ToString(), c);
					}
                    if (x == 0)
					{
						Utils.addVertexToLookup(edgeVertexLookup, vertices[a].ToString(), a);
						Utils.addVertexToLookup(edgeVertexLookup, vertices[a].ToString(), d);
					}
                    if (x == m_HIM.Length - 1)
					{
						Utils.addVertexToLookup(edgeVertexLookup, vertices[a].ToString(), b);
						Utils.addVertexToLookup(edgeVertexLookup, vertices[a].ToString(), c);
					}
					
					
                    int tileX = x / 4;
                    int tileY = y / 4;
                    int tileID = tileY * 16 + tileX;
					
					
					// Apply UV's
                    ZON.RotationType rotation = m_ZON.Tiles[m_TIL.Tiles[tileX, tileY].TileID].Rotation;
					Vector2[,] rotMatrix;
                    if (rotation == ZON.RotationType.Rotate90Clockwise || rotation == ZON.RotationType.Rotate90CounterClockwise)
                        Debug.Log("Rotation: " + (int)rotation);
                    switch (rotation)
					{
						case ZON.RotationType.Normal:
							rotMatrix = uvMatrix;
							break;
						case ZON.RotationType.LeftRight:
							rotMatrix = uvMatrixLR;
							break;
						case ZON.RotationType.LeftRightTopBottom:
							rotMatrix = uvMatrixLRTB;
							break;
						case ZON.RotationType.Rotate90Clockwise:
							rotMatrix = uvMatrixRotCW;
							break;
						case ZON.RotationType.Rotate90CounterClockwise:
							rotMatrix = uvMatrixRotCCW;
							break;
						case ZON.RotationType.TopBottom:
							rotMatrix = uvMatrixTB;
							break;
						default:
							rotMatrix = uvMatrix;
							break;
					}
                    
                    // Get top and bottom UV's using texture atlas and rotation adjustments
					uvsTop[a] = m_tiles[tileID].GetUVTop(rotMatrix[x % 4, y % 4]);
					uvsTop[b] = m_tiles[tileID].GetUVTop(rotMatrix[(x % 4 + 1) % 5, y % 4]);
					uvsTop[c] = m_tiles[tileID].GetUVTop(rotMatrix[(x % 4 + 1) % 5, (y % 4 + 1) % 5]);
					uvsTop[d] = m_tiles[tileID].GetUVTop(rotMatrix[x % 4, (y % 4 + 1) % 5]);
					
					uvsBottom[a] = m_tiles[tileID].GetUVBottom(rotMatrix[x % 4, y % 4]);
					uvsBottom[b] = m_tiles[tileID].GetUVBottom(rotMatrix[(x % 4 + 1) % 5, y % 4]);
					uvsBottom[c] = m_tiles[tileID].GetUVBottom(rotMatrix[(x % 4 + 1) % 5, (y % 4 + 1) % 5]);
					uvsBottom[d] = m_tiles[tileID].GetUVBottom(rotMatrix[x % 4, (y % 4 + 1) % 5]);
					
					triangles[triangleID ++] = a;
					triangles[triangleID ++] = d;
					triangles[triangleID ++] = b;
					
					triangles[triangleID ++] = b;
					triangles[triangleID ++] = d;
					triangles[triangleID ++] = c;
					
				}  // for y
			}    // for x
			
			
			m_mesh.vertices = vertices;
			m_mesh.triangles = triangles;
			m_mesh.uv = uvsBottom;
            m_mesh.uv2 = uvsTop;
            m_mesh.colors = uvsLight;
			
			
			m_mesh.RecalculateNormals();
			
			if(blendNormals)
			{
				// CalculateSharedNormals: fix all normals as follows:
				// Several triangles share same vertex, but it is duplicated
				// We want to:
				//	1. search the vertex array for shared vertices
				//	2. store each shared vertex id in a data structure comprising rows of shared vertices
				//  3. go through each row of shared vertices and calculate the average normal
				//	4. store the avg normal and all corresponding vertex id's in different data structure
				//	5. traverse the new data structure and assign the new normal to all the vertices it belongs to
				
				Vector3[] normals = new Vector3[m_mesh.vertexCount];
				Dictionary<String, List<int>> vertexLookup = new Dictionary<String, List<int>>();
				// 1. and 2.
                for (int i = 0; i < m_mesh.vertexCount; i++)
					Utils.addVertexToLookup(vertexLookup, m_mesh.vertices[i].ToString(), i);
				
				// traverse the shared vertex list and calculate new normals
				
                foreach (KeyValuePair<String, List<int>> entry in vertexLookup)
				{
					Vector3 avg = Vector3.zero;
                    foreach (int id in entry.Value)
					{
						avg += m_mesh.normals[id];
					}
					
					avg.Normalize();
					
                    foreach (int id in entry.Value)
						normals[id] = avg;	
					
				}
				
				
				m_mesh.normals = normals;
			}
			
			Utils.calculateMeshTangents(m_mesh);
			m_mesh.RecalculateBounds();
			;
			
			//AssetDatabase.CreateAsset( m_mesh, "Assets/patch_" + this.m_name + ".mesh");
			
			GameObject patchObject = new GameObject();
			patchObject.name = "patch_" + this.m_name;
			
			patchObject.AddComponent<MeshFilter>().mesh = m_mesh;
			patchObject.AddComponent<MeshRenderer>();
			patchObject.AddComponent<MeshCollider>();
            
            MeshRenderer patchRenderer = patchObject.GetComponent<MeshRenderer>();
			patchRenderer.material = material;
            //patchRenderer.castShadows = false;
			patchObject.transform.parent = terrainParent;
			patchObject.layer = LayerMask.NameToLayer("Floor");
			
			
			
			//================== TERRAIN OBJECTS==========================
			
			GameObject deco = new GameObject();
            deco.name = "deco_" + this.m_name;
			deco.transform.parent = objectsParent;
			deco.layer = LayerMask.NameToLayer("MapObjects"); 

			
			//================= DECORATION ======================
            for (int obj = 0; obj < m_IFO.Decoration.Count; obj++ )
			{		
                IFO.BaseIFO ifo = m_IFO.Decoration[obj];
				GameObject terrainObject = new GameObject();
				terrainObject.layer = LayerMask.NameToLayer("MapObjects");
				terrainObject.name = "Deco_" + ifo.MapPosition.x + "_" + ifo.MapPosition.y;
				terrainObject.transform.parent = deco.transform;
                terrainObject.transform.localPosition = (ifo.Position / 100.0f);
				bool isAnimated = false;
				AnimationClip clip = new AnimationClip();
				clip.legacy = true;
				
					
				for (int part = 0; part < m_ZSC_Deco.Objects[ifo.ObjectID].Models.Count; part++ )
				{
                    ZSC.Object.Model model = m_ZSC_Deco.Objects[ifo.ObjectID].Models[part];
					// load ZMS
                    string zmsPath = m_3dDataDir.Parent.FullName + "/" + m_ZSC_Deco.Models[model.ModelID].Replace("\\", "/");
                    string texPath = "Assets/" + m_ZSC_Deco.Textures[model.TextureID].Path;
                   	string lightPath = null;
					ZMS zms = null;
					lightPath = Utils.FixPath(this.m_assetDir.Parent.FullName + "\\" + this.m_name + "\\LIGHTMAP\\" + m_LIT_Deco.Objects[obj].Parts[part].DDSName);
                    LIT.Object.Part lmData = m_LIT_Deco.Objects[obj].Parts[part];
					
                    // Calculate light map UV offset and scale
                    float objScale = 1.0f / (float) lmData.ObjectsPerWidth;
                    float rowNum = (float) Math.Floor( (double)( (double)lmData.MapPosition / (double)lmData.ObjectsPerWidth) );
                    float colNum = (float) lmData.MapPosition % lmData.ObjectsPerWidth;
					
                    Vector2 lmOffset = new Vector2(colNum * objScale, rowNum * objScale);
                    Vector2 lmScale = new Vector2(objScale, objScale);

                    zms = new ZMS(zmsPath, lmScale, lmOffset);
					
					// Create material	
					Texture2D mainTex = Utils.loadTex( ref texPath); 

					Material mat = null;
					if(realTimeBaking)
					{
						Texture2D normalMap = Utils.generateNormalMap( texPath );
						mat = new Material(Shader.Find("Standard"));
						mat.SetFloat("_Mode", 1.0f);
						mat.SetTexture("_MainTex", mainTex);
						mat.SetTexture("_OcclusionMap", mainTex);
						mat.SetTexture("_BumpMap", normalMap);
						mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
						mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
						mat.SetInt("_ZWrite", 1);
						mat.EnableKeyword("_ALPHATEST_ON");
						mat.DisableKeyword("_ALPHABLEND_ON");
						mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
						mat.SetFloat("_OcclusionStrength", 0.5f);
						mat.SetFloat("_BumpScale", 0.8f);
						mat.SetFloat("_Glossiness", 0.1f);
						//mat.renderQueue = 2450;
					}
					else
					{
						mat = new Material(Shader.Find("Custom/ObjectShader"));
						mat.SetTexture("_MainTex", mainTex);
						Texture2D lightTexture = Utils.loadTex ( ref lightPath);
						mat.SetTexture("_LightTex", lightTexture);
					}
					


					GameObject modelObject = new GameObject();
					modelObject.layer = LayerMask.NameToLayer("MapObjects");
					modelObject.transform.parent = terrainObject.transform;
					
					modelObject.transform.localScale = model.Scale;
                    modelObject.transform.localPosition = (model.Position / 100.0f);
					modelObject.transform.rotation = model.Rotation;
					
					modelObject.AddComponent<MeshFilter>().mesh = zms.getMesh();
					modelObject.AddComponent<MeshRenderer>();
                    modelObject.name = new DirectoryInfo(zmsPath).Name;
                    MeshRenderer renderer = modelObject.GetComponent<MeshRenderer>();
                    renderer.material = mat;
                    //renderer.castShadows = false;
                    
                    if(model.CollisionLevel != ZSC.CollisionLevelType.None)
						modelObject.AddComponent<MeshCollider>();

					string zmoPath = model.Motion;
                    if (zmoPath!= null && zmoPath.ToLower().Contains("zmo"))
					{
						isAnimated = true;
                        ZMO zmo = new ZMO("assets/" + model.Motion, false, true);
                        clip = zmo.buildAnimationClip(modelObject.name, clip);
					}
					else
						modelObject.isStatic = true;
				}
				
				terrainObject.transform.rotation = ifo.Rotation;
				terrainObject.transform.localScale = ifo.Scale;
				
               	if (isAnimated)
               	{
					Animation animation = terrainObject.GetComponent<Animation>();
					if (animation == null)
						animation = terrainObject.AddComponent<Animation>();
					clip.wrapMode = WrapMode.Loop;
					animation.AddClip(clip, terrainObject.name);
					animation.clip = clip;
				}
				else
				{
					terrainObject.isStatic = true;
       			}
               	
			}
			
			GameObject cnst = new GameObject();
            cnst.name = "cnst_" + this.m_name;
			cnst.transform.parent = objectsParent;
			cnst.layer = LayerMask.NameToLayer("MapObjects");
			//================= CONSTRUCTION ======================
            for (int obj = 0; obj < m_IFO.Construction.Count; obj++)
			{		
                IFO.BaseIFO ifo = m_IFO.Construction[obj];
				GameObject terrainObject = new GameObject();
				terrainObject.layer = LayerMask.NameToLayer("MapObjects");
				terrainObject.name = "Const_" + ifo.MapPosition.x + "_" + ifo.MapPosition.y;
				terrainObject.transform.parent = deco.transform;
                terrainObject.transform.localPosition = (ifo.Position / 100.0f);
				bool isAnimated = false;
				AnimationClip clip = new AnimationClip();
				clip.legacy = true;
                for (int part = 0; part < m_ZSC_Cnst.Objects[ifo.ObjectID].Models.Count; part++)
				{
                    ZSC.Object.Model model = m_ZSC_Cnst.Objects[ifo.ObjectID].Models[part];
					string zmsPath = m_3dDataDir.Parent.FullName + "/" + m_ZSC_Cnst.Models[model.ModelID].Replace("\\","/");
					string texPath = "Assets/" + m_ZSC_Cnst.Textures[model.TextureID].Path;
					
					string lightPath = null;
					ZMS zms = null;
					
					// load ZMS
                    lightPath = Utils.FixPath(this.m_assetDir.Parent.FullName + "\\" + this.m_name + "\\LIGHTMAP\\" + m_LIT_Cnst.Objects[obj].Parts[part].DDSName);
                    LIT.Object.Part lmData = m_LIT_Cnst.Objects[obj].Parts[part];

                    // Calculate light map UV offset and scale
                    float objScale = 1.0f / (float)lmData.ObjectsPerWidth;
                    float rowNum = (float)Math.Floor((double)((double)lmData.MapPosition / (double)lmData.ObjectsPerWidth));
                    float colNum = (float)lmData.MapPosition % lmData.ObjectsPerWidth;

                    Vector2 lmOffset = new Vector2(colNum * objScale, rowNum * objScale);
                    Vector2 lmScale = new Vector2(objScale, objScale);
					
                    zms = new ZMS(zmsPath, lmScale, lmOffset);
  
					// Create material
					Texture2D mainTex =  Utils.loadTex(ref texPath); 
					
					Material mat = null;
					if(realTimeBaking)
					{
						Texture2D normalMap = Utils.generateNormalMap( texPath );
						mat = new Material(Shader.Find("Standard"));
						mat.SetFloat("_Mode", 1.0f);
						mat.SetTexture("_MainTex", mainTex);
						mat.SetTexture("_OcclusionMap", mainTex);
						mat.SetTexture("_BumpMap", normalMap);
						mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
						mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
						mat.SetInt("_ZWrite", 1);
						mat.EnableKeyword("_ALPHATEST_ON");
						mat.DisableKeyword("_ALPHABLEND_ON");
						mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
						mat.SetFloat("_OcclusionStrength", 0.5f);
						mat.SetFloat("_BumpScale", 0.8f);
						mat.SetFloat("_Glossiness", 0.1f);
						//mat.renderQueue = 2450;
					}
					else
					{
						mat = new Material(Shader.Find("Custom/ObjectShader"));
						mat.SetTexture("_MainTex", mainTex);
						Texture2D lightTexture =  Utils.loadTex(ref lightPath); 
						mat.SetTexture("_LightTex", lightTexture);
						
					}
					
					
					GameObject modelObject = new GameObject();
					modelObject.layer = LayerMask.NameToLayer("MapObjects");
					modelObject.transform.parent = terrainObject.transform;
					
					modelObject.transform.localScale = model.Scale;
                    modelObject.transform.localPosition = (model.Position / 100.0f);
					modelObject.transform.rotation = model.Rotation;
					
					modelObject.AddComponent<MeshFilter>().mesh = zms.getMesh();
					modelObject.AddComponent<MeshRenderer>();
                    modelObject.name = new DirectoryInfo(zmsPath).Name;
                    MeshRenderer renderer = modelObject.GetComponent<MeshRenderer>();
                    renderer.material = mat;
                    //renderer.castShadows = false; 
					modelObject.AddComponent<MeshCollider>();

					string zmoPath = model.Motion;
					if (zmoPath != null && zmoPath.ToLower().Contains("zmo"))
					{
						isAnimated = true;
                        ZMO zmo = new ZMO("assets/" + model.Motion, false, true);
                        clip = zmo.buildAnimationClip(modelObject.name, clip);
					}
					else
						modelObject.isStatic = true;
				}
				
				terrainObject.transform.rotation = ifo.Rotation;
				terrainObject.transform.localScale = ifo.Scale;

				
                if (isAnimated)
               	{
					Animation animation = terrainObject.GetComponent<Animation>();
					
                    if (animation == null)
						animation = terrainObject.AddComponent<Animation>();
					
					clip.wrapMode = WrapMode.Loop;
					animation.AddClip(clip, terrainObject.name);
					animation.clip = clip;
                }
                else
                	terrainObject.isStatic = false;
              
			}
			
			
			
			
			/*
			// TODO: add any extra components here
			AssetDatabase.CreateAsset( m_mesh, this.m_unityAssetDir.FullName);
			AssetDatabase.SaveAssets();
			*/
			return true;
		}  // Import()	
		
	}
	
	
	public class Tile
	{
		public Rect bottomRect { get; set; }
		public Rect topRect { get; set; }
		public string bottomTex { get; set; }
		public string topTex { get; set; }
		
		public Tile()
		{
		}
		
		public void setRects(Rect bot, Rect top)
		{
			bottomRect = bot;
			topRect = top;
		}
		

		public Vector2 GetUVTop(Vector2 uv)
		{
			// adjust uv's slightly to hide seams between tiles
			if(uv.x < 0.01f) uv.x+=0.01f;
			else if(uv.x > 0.99f) uv.x *= 0.99f;
			if(uv.y < 0.01f) uv.y+=0.01f;
			else if(uv.y > 0.99f) uv.y *= 0.99f;
			
			return new Vector2((uv.x * topRect.width) + topRect.x, (uv.y* topRect.height) + topRect.y);
		}
		
		public Vector2 GetUVBottom(Vector2 uv)
		{
			if(uv.x < 0.01f) uv.x+=0.01f;
			else if(uv.x > 0.99f) uv.x *= 0.99f;
			if(uv.y < 0.01f) uv.y+=0.01f;
			else if(uv.y > 0.99f) uv.y *= 0.99f;
			
			return new Vector2((uv.x * bottomRect.width) + bottomRect.x, (uv.y * bottomRect.height) + bottomRect.y);
		}
	}
	
	
}

#endif
