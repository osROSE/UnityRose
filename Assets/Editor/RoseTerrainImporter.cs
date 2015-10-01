
#if UNITY_EDITOR

// Importer for rose maps and player
using UnityEditor;
using UnityEngine;
using UnityRose.Formats;
using System;
using System.IO;
using System.Collections.Generic;
using UnityRose.Game;
using UnityRose;

public class RoseTerrainWindow : EditorWindow {
	string m_inputDir = "";
	string m_szcPath = "";
	int bodyPart;
	int objID;
	RosePlayer player;
	Transform transform;

	// Add menu named "My Window" to the Window menu
	[MenuItem ("GameObject/Create Other/Rose Object")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		RoseTerrainWindow window = (RoseTerrainWindow)EditorWindow.GetWindow (typeof (RoseTerrainWindow));
	}
	
	private void ImportGalaxy()
	{
		/* TODO: Add code to loop through all Planets and load them */
		
	}
	
	private void ImportPlanet()
	{
		/* TODO: Add code to loop through all maps and load them */
	}
	
	private struct PatchNormalIndex
	{
		public int patchID;
		public int normalID;
		public PatchNormalIndex(int patchID, int normalID)
		{
			this.patchID = patchID;
			this.normalID = normalID;
		}
	}
	
	private void ImportMap(string mapName)
	{
		Debug.Log ("Importing map from " + m_inputDir + "...");
		bool success = true;
		
		DirectoryInfo dirs = new DirectoryInfo(m_inputDir);
		
		GameObject map = new GameObject();
		map.name = mapName;
		
		GameObject terrain = new GameObject();
		terrain.name = "Ground";
		terrain.transform.parent = map.transform;
		terrain.layer = LayerMask.NameToLayer("Floor");
		
		GameObject terrainObjects = new GameObject();
		terrainObjects.name = "Objects";
		terrainObjects.transform.parent = map.transform;
		terrainObjects.layer = LayerMask.NameToLayer("MapObjects");
		
		List<RosePatch> patches = new List<RosePatch>();
		Dictionary<string, Rect> atlasRectHash = new Dictionary<string, Rect>();
		Dictionary<string, Texture2D> atlasTexHash = new Dictionary< string, Texture2D >();
		List<Texture2D> textures = new List<Texture2D>();
		
		
		// Instantiate all patches
		foreach(DirectoryInfo dir in dirs.GetDirectories())
		{
			if(!dir.Name.Contains("."))
			{
				RosePatch patch = new RosePatch( dir ); 
				patch.Load();
				patch.UpdateAtlas( ref atlasRectHash, ref atlasTexHash, ref textures );
				patches.Add ( patch );
			}
		}
		// Create a texture atlas from the textures of all patches and populate the rectangles in the hash
		
		// Figure out the required size of the atlas from the number of textures in the atlas
		int height, width;  // these must be powers of 2 to be compatible with iPhone
		if( atlasRectHash.Count <= 16 )  width = height = 4*256; 
		else if( atlasRectHash.Count <= 32 )  { width = 8*256; height = 4*256; }
		else if( atlasRectHash.Count <= 64 )  { width = 8*256; height = 8*256; }
		else if( atlasRectHash.Count <= 128 ) { width = 16*256; height = 8*256; }
		else if( atlasRectHash.Count <= 256 ) { width = 16*256; height = 16*256; }
		else throw new Exception("Number of tiles in terrain is larger than supported by terrain atlas");
		
		
		Texture2D atlas = new Texture2D(width, height);
		
		// Pack the textures into one texture atlas
		Rect[] rects = atlas.PackTextures( textures.ToArray(), 0, Math.Max(width, height) );
		atlas.anisoLevel = 11;
        
		Texture2D myAtlas = new Texture2D(width, height);
		myAtlas.SetPixels32( atlas.GetPixels32(0), 0);
		
		string atlasPath = "Assets/Terrain/Textures/" + mapName + "_atlas.png";
		
        if( !File.Exists( atlasPath ))
		{
			FileStream fs = new FileStream( atlasPath, FileMode.Create);
			BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(myAtlas.EncodeToPNG());
			bw.Close();
			fs.Close();
			
			AssetDatabase.Refresh();
		}
        
        myAtlas = Utils.loadTex(ref atlasPath);
		
		
		// copy rects back to hash (should update rect refs in Tile objects
		int rectID = 0;
		foreach( string key in atlasTexHash.Keys)
			atlasRectHash[key] = rects[rectID++];
		
		// Generate the patches
		foreach(RosePatch patch in patches)
            patch.Import(terrain.transform, terrainObjects.transform, myAtlas, myAtlas, atlasRectHash);
		
		
		//blend vertex normals at the seams between patches
		Dictionary<string, List<PatchNormalIndex>> patchNormalLookup = new Dictionary<string, List<PatchNormalIndex>>();
		int patchID = 0;
		// combine all normal lookups into one big lookup containing patch ID and normal ID
		foreach(RosePatch patch in patches)
		{
			
			// go through the lookup of this patch and append all normal id's to big lookup
			foreach(string vertex in patch.edgeVertexLookup.Keys)
			{
				List<PatchNormalIndex> ids = new List<PatchNormalIndex>();
				foreach(int id in patch.edgeVertexLookup[vertex])
					ids.Add(new PatchNormalIndex(patchID, id));
				
				if(!patchNormalLookup.ContainsKey(vertex))
					patchNormalLookup.Add(vertex, ids);
				else
					patchNormalLookup[vertex].AddRange(ids);	
				
			}
			
			patchID++;	
		}
		
		// go through each enttry in the big lookup and calculate avg normal, then assign to corresponding patches
		foreach(string vertex in patchNormalLookup.Keys)
		{
			Vector3 avg = Vector3.zero;
			// First pass: calculate average normal
			foreach(PatchNormalIndex entry in patchNormalLookup[vertex])
				avg += patches[entry.patchID].m_mesh.normals[entry.normalID];
			
			avg.Normalize();
			
			// Second pass: assign new normal to corresponding patches
			foreach(PatchNormalIndex entry in patchNormalLookup[vertex])
				patches[entry.patchID].m_mesh.normals[entry.normalID] = avg;
			
		}
		
		
		terrainObjects.transform.localScale = new Vector3(1.0f, 1.0f, -1.0f);
		terrainObjects.transform.Rotate (0.0f, -90.0f, 0.0f);
		terrainObjects.transform.position = new Vector3(5200.0f, 0.0f, 5200.0f);
		
		if(success)
			Debug.Log ("Map Import Complete");
		else
			Debug.Log ("!Map Import Failed");
	}
	

	/*
	private RosePatch ImportPatch(string inputDir, Transform terrainParent, Transform objectsParent)
	{
		// Patch consists of the following elements:
		//	- .HIM file specifying the heighmap of the terrain: 65x65 image of floats 
		//	- .TIL file specifying the texture tileset: 16x16 tiles, containing ID's that index into .ZON.Tiles, which returns index into .ZON.Textures
		//  - TODO: add other fileTypes here after researching
		
		
		bool success = true;
		
		Debug.Log ("Importing patch from " + inputDir + "...");
		
		RosePatch patch = new RosePatch(new DirectoryInfo(inputDir));
		
		success &= patch.Load();
		success &= patch.Import(terrainParent, objectsParent);
		
		
		if(success)
			Debug.Log ("Patch Import complete");
		else
			Debug.Log ("!Patch Import failed");
		
		return patch;
	}
	*/
	
	void OnGUI () {
		// Need to do the following:
		// - Specify a specific map to load (path + directory browser?)
		// - Specify an output directory (path + directory browser?)
		// - Button to begin conversion
		// - add any extra settings here ...
		
		// ----------------Example GUI elements-----------------------
		// GUILayout.Label ("Settings", EditorStyles.boldLabel);
		// myBool = EditorGUILayout.Toggle ("Toggle", myBool);
		// myFloat = EditorGUILayout.Slider ("Slider", myFloat, -3, 3);
		// -----------------------------------------------------------
		
		//=================== MAP =======================
		
		EditorGUILayout.BeginToggleGroup("Map", true);
		m_inputDir = EditorGUILayout.TextField ("Input dir: ", m_inputDir);
		
		/*
		if(GUILayout.Button("Import"))
		{
			STB stb = new STB(m_inputDir);
			// STL:
			//		  row		  col   row
			//Entries[0-62]  Rows[0-5][0-61] 
			//LZON001  korean "Canyon City of Zant" korean korean korean
			//LZON002  korean "City of Junon Polis" korean korean korean
			//LZON003  korean "Junon Cartel" ...
			//...
			
			
			int x=1;
		}
		*/
			
		 
		if(GUILayout.Button("Import"))
		{
			// Several options based on the given path
			// 1. 3DDATA/MAPS/						-> convert rose universe
			// 2. 3DDATA/MAPS/JUNON					-> convert rose planet
			// 3. 3DDATA/MAPS/JUNON/JPT01			-> convert rose map
			// 4. 3DDATA/MAPS/JUNON/JTP01/30_30		-> convert rose patch
			// 5. Some/invalid/path
			bool notFound = false;
			DirectoryInfo inDirInfo = new DirectoryInfo(m_inputDir);
			switch (inDirInfo.Name.ToLower())
			{
			case "maps": 						// 1.
				ImportGalaxy();
				break;
			case "junon": 						// 2.
			case "eldeon":				
			case "lunar":		
				// TODO: add any new planets here...				
				ImportPlanet();
				break;
			default:
				notFound = true;
				break;	
			} // switch
			
			if(notFound)
			{
				switch (inDirInfo.Parent.Name.ToLower())
				{
				case "junon":				// 3.
				case "eldeon":
				case "lunar":
					// TODO: add any new planets here...
					ImportMap(inDirInfo.Name.ToLower());
					break;
				default:
					GameObject terrain = new GameObject();
					terrain.name = "Terrain";
					
					GameObject terrainObjects = new GameObject();
					terrainObjects.name = "Terrain Objects";
					//ImportPatch(m_inputDir, terrain.transform, terrainObjects.transform);			// 4. (LoadPatch will handle 5. properly)	
					break;		
				}	// switch
			} // if
		} // if
		
		EditorGUILayout.EndToggleGroup ();
		// ======================== OBJECT ==========================
		EditorGUILayout.BeginToggleGroup("Animated Object", true);
		m_szcPath = EditorGUILayout.TextField ("ZSC: ", m_szcPath);
		objID = EditorGUILayout.IntField ("ID: ", objID);
		bodyPart = EditorGUILayout.IntField ("Body Part: ", bodyPart);
		transform = EditorGUILayout.ObjectField ("Transform: ", transform, typeof(Transform), true) as Transform;
		if (GUILayout.Button ("Create")) {
			if(transform != null)
				player = new RosePlayer (transform.position); // Note: Player reference is lost after hitting play.  Must create new after that.
			else
				player = new RosePlayer();
		}

        if (GUILayout.Button("Create Char Select"))
        {
            CharModel model = new CharModel();
            model.rig = RigType.CHARSELECT;
            model.state = States.HOVERING;

            if(transform != null)
                model.pos = transform.position;

            player = new RosePlayer(model); // Note: Player reference is lost after hitting play.  Must create new after that.
 
        }

        if (GUILayout.Button("Equip"))
		{
            if( player != null )
                player.equip( (BodyPartType)bodyPart, objID );

            //RosePlayer player = new RosePlayer(GenderType.MALE, WeaponType.THSWORD);
            //ResourceManager.Instance.GenerateAnimationAsset(GenderType.MALE, WeaponType.EMPTY);
            //ResourceManager.Instance.GenerateAnimationAssets();
            //GenerateCharSelectAnimations();

        }

        if (GUILayout.Button("GenerateAnimations"))
        {
            //RosePlayer player = new RosePlayer(GenderType.MALE, WeaponType.THSWORD);
            //ResourceManager.Instance.GenerateAnimationAsset(GenderType.MALE, WeaponType.EMPTY);
            //ResourceManager.Instance.GenerateAnimationAssets();
            GenerateCharSelectAnimations();
        }

        EditorGUILayout.EndToggleGroup ();
	} // OnGui()

    
    void GenerateCharSelectAnimations()
    {
        foreach (GenderType gender in Enum.GetValues(typeof(GenderType)))
        {
            bool m = (gender == GenderType.MALE);
            Dictionary<String, String> clips = new Dictionary<String, String>();
            clips.Add("standup", "3ddata/motion/avatar/empty_stand_" + (m ? "m" : "f") + "1.zmo");
            clips.Add("standing", "3ddata/motion/avatar/empty_stop1_" + (m ? "m" : "f") + "1.zmo");
            clips.Add("sit", "3ddata/motion/avatar/empty_sit_" + (m ? "m" : "f") + "1.zmo");
            clips.Add("sitting", "3ddata/motion/avatar/empty_siting_" + (m ? "m" : "f") + "1.zmo");

            clips.Add("hovering", "3ddata/motion/avatar/event_creat_m1.zmo");
            clips.Add("select", "3ddata/motion/avatar/event_select_m1.zmo");

            ResourceManager.Instance.GenerateAnimationAsset(gender, RigType.CHARSELECT, clips);
        }
    }
}

#endif