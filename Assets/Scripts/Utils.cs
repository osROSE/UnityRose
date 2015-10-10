// <copyright file="Utils.cs" company="Wadii Bellamine">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Wadii Bellamine</author>
// <date>2/25/2015 8:37 AM </date>

using System;
using System.Collections.Generic;
using System.IO;
using System.Resources;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityRose;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Utils
{
	public static bool isEmail(string email)
	{
		return email.Contains("@") && email.Contains(".");
	}

	public static Quaternion r2uRotation(Quaternion q)
	{
	    Vector3 axis;
	    float angle;
	    q.ToAngleAxis(out angle, out axis);
	    return Quaternion.AngleAxis(angle, new Vector3(axis.x, axis.z, -axis.y));
	}
	
	public static Vector3 r2uPosition(Vector3 v)
	{
	    return new Vector3(v.x, v.z, -v.y);
	}
	
	public static Vector3 r2uVector(Vector3 v)
	{
	    return new Vector3(v.x, v.z, -v.y);
	}

	public static Vector3 r2uScale(Vector3 v)
	{
		return new Vector3(v.x, v.z, v.y);
	}

	public static Transform findChild(GameObject go, string name)
	{
		Transform[] children = go.GetComponentsInChildren<Transform> ();
		foreach (Transform child in children)
			if (child.name == name)
				return child;
		return null;
	}

	public static List<Transform> findChildren(GameObject go, string name)
	{
		Transform[] children = go.GetComponentsInChildren<Transform> ();
		List<Transform> result = new List<Transform> ();
		foreach (Transform child in children)
			if (child.name == name)
				result.Add (child);
		return result;
	}

	public static void Destroy( GameObject go )
	{
		GameObject.DestroyImmediate (go);
    }

    public static void Destroy( Transform tr )
	{
#if UNITY_EDITOR
		GameObject.DestroyImmediate(tr);
#else
		GameObject.DestroyImmediate (tr);
#endif
    }


    public static bool isOneHanded( WeaponType weapon)
    {
        return weapon == WeaponType.OHAXE ||
                weapon == WeaponType.OHMACE ||
                weapon == WeaponType.OHSWORD ||
                weapon == WeaponType.OHTOOL ||
                weapon == WeaponType.WAND ||
                weapon == WeaponType.XBOX ||
                weapon == WeaponType.EMPTY;

    }

	public static void calculateMeshTangents(Mesh mesh, bool uv2 = false)
	{
		//speed up math by copying the mesh arrays
		int[] triangles = mesh.triangles;
		Vector3[] vertices = mesh.vertices;
		Vector2[] uv = uv2 ? mesh.uv2 : mesh.uv;
		Vector3[] normals = mesh.normals;
		
		//variable definitions
		int triangleCount = triangles.Length;
		int vertexCount = vertices.Length;
		
		Vector3[] tan1 = new Vector3[vertexCount];
		Vector3[] tan2 = new Vector3[vertexCount];
		
		Vector4[] tangents = new Vector4[vertexCount];
		
		for (long a = 0; a < triangleCount; a += 3)
		{
			long i1 = triangles[a + 0];
			long i2 = triangles[a + 1];
			long i3 = triangles[a + 2];
			
			Vector3 v1 = vertices[i1];
			Vector3 v2 = vertices[i2];
			Vector3 v3 = vertices[i3];
			
			Vector2 w1 = uv[i1];
			Vector2 w2 = uv[i2];
			Vector2 w3 = uv[i3];
			
			float x1 = v2.x - v1.x;
			float x2 = v3.x - v1.x;
			float y1 = v2.y - v1.y;
			float y2 = v3.y - v1.y;
			float z1 = v2.z - v1.z;
			float z2 = v3.z - v1.z;
			
			float s1 = w2.x - w1.x;
			float s2 = w3.x - w1.x;
			float t1 = w2.y - w1.y;
			float t2 = w3.y - w1.y;
			
			float r = 1.0f / (s1 * t2 - s2 * t1);
			
			Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
			Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);
			
			tan1[i1] += sdir;
			tan1[i2] += sdir;
			tan1[i3] += sdir;
			
			tan2[i1] += tdir;
			tan2[i2] += tdir;
			tan2[i3] += tdir;
		}
		
		
		for (long a = 0; a < vertexCount; ++a)
		{
			Vector3 n = normals[a];
			Vector3 t = tan1[a];
			
			//Vector3 tmp = (t - n * Vector3.Dot(n, t)).normalized;
			//tangents[a] = new Vector4(tmp.x, tmp.y, tmp.z);
			Vector3.OrthoNormalize(ref n, ref t);
			tangents[a].x = t.x;
			tangents[a].y = t.y;
			tangents[a].z = t.z;
			
			tangents[a].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[a]) < 0.0f) ? -1.0f : 1.0f;
		}
		
		mesh.tangents = tangents;
	}
	
	public static void addVertexToLookup(Dictionary<String,List<int>> lookup, String vertex, int index)
	{
		if(!lookup.ContainsKey(vertex))
		{
			List<int> ids = new List<int>();
			ids.Add (index);
			lookup.Add (vertex.ToString(), ids);
		}
		else
		{
			lookup[vertex].Add(index);
		}
	}
	
	public static Bounds GetMaxBounds(GameObject g)
	{
		var b = new Bounds(g.transform.position, Vector3.zero);
		foreach (Renderer r in g.GetComponentsInChildren<Renderer>())
		{
			b.Encapsulate(r.bounds);
		}
		return b;
	}

    // Returns true if platform is mac
    public static bool IsMac()
    {
        return Application.platform == RuntimePlatform.OSXEditor;
    }

    // fixes the path based on platform and returns fixed string
    public static string FixPath(string file)
    {
        return  file.Contains("\\") ? file.ToLower().Replace("\\", "/") : file.ToLower();
    }

    public static string NormalizePath(string path)
    {
        string newPath = path;

        newPath = newPath.Replace("\\", "/");

        while (true)
        {
            var repPath = newPath.Replace("//", "/");
            if (repPath == newPath)
            {
                break;
            }
            newPath = repPath;
        }

        return newPath;
    }
	
    public static string CombinePath(string path1, string path2)
    {
        return NormalizePath(path1 + "/" + path2);
    }

    public static string CombinePath(string path1, string path2, string path3)
    {
        return CombinePath(CombinePath(path1, path2), path3);
    }
	
    public static string CombinePath(string path1, string path2, string path3, string path4)
    {
        return CombinePath(CombinePath(CombinePath(path1, path2), path3), path4);
    }

	// UI functions
	public static void handleTab(EventSystem system)
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
			
			if (next!= null) {
				
				InputField inputfield = next.GetComponent<InputField>();
				if (inputfield !=null) inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret
				
				system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
			}
		}
	}
	
	public static void handleEnter(EventSystem system)
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
			
			if( next != null) 
			{
				Button button = next.GetComponent<Button>();
				if (button != null)
					button.OnPointerClick( new PointerEventData( system ));
				
				system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
			}
		}
	}

#if !UNITY_EDITOR
	public static Texture2D loadTex ( ref string rosePath )
	{
		// check if dds exists, and load it if it does
		string ddsPath = FixPath(rosePath);
		//ddsPath = GetUnityPath(ddsPath);
		string pngPath = ddsPath.Replace(".dds","");
		rosePath = pngPath;
		return Resources.Load<Texture2D>(pngPath);
	}
#endif

#if UNITY_EDITOR
	

	public static Texture2D loadTex ( ref string rosePath )
	{
		// check if dds exists, and load it if it does
		string ddsPath = FixPath(rosePath);
		ddsPath = GetUnityPath(ddsPath);
		string pngPath = ddsPath.Replace(".dds",".png");
		
		if( File.Exists (ddsPath))
		{
			rosePath = ddsPath;
			return AssetDatabase.LoadAssetAtPath<Texture2D>(ddsPath);
		}
		else
		{
			rosePath = pngPath;
			return AssetDatabase.LoadAssetAtPath<Texture2D>(pngPath);
		}
		return null;
	}

	
	// Converts a rose path to a unity path and creates the directory structure of non-existent
	public static DirectoryInfo r2uDir(string rosePath, string extension = ".asset")
	{
		var roseDir = new DirectoryInfo(FixPath(rosePath).ToLower());
        string unityPath = roseDir.FullName;

        if (roseDir.Extension != "")
            unityPath = unityPath.Replace(roseDir.Extension, extension);
        else
            unityPath += extension;

        if (unityPath.Contains("3ddata"))
            unityPath = unityPath.Replace("3ddata", "GameData");

		var unityDir = new DirectoryInfo(unityPath);
		
		// Creat the parent folder path if it doesn't already exist
		if( !unityDir.Parent.Exists )
			Directory.CreateDirectory(unityDir.Parent.FullName);

        unityDir.Refresh();
		return unityDir;
	}
	
	// Trim path preceding /asset
	public static string GetUnityPath( DirectoryInfo path )
	{
		string result = path.Name;
		DirectoryInfo currentPath = path.Parent;
		while( currentPath.Name.ToLower() != "assets" )
		{
			DirectoryInfo nextPath = new DirectoryInfo(currentPath.FullName);
			result = nextPath.Name + "/" + result;
			currentPath = nextPath.Parent;
		}
			
		return ("assets/" + result);
	}

    // Trim path preceding /asset
    public static string GetUnityPath(string path)
    {
        DirectoryInfo dir = new DirectoryInfo(path);
        return GetUnityPath(dir);
    }
	
	public static Texture2D generateNormalMap(string mainPath)
	{
		DirectoryInfo dir = new DirectoryInfo( mainPath );
		string normalMapPath = dir.FullName.Replace(dir.Extension, "_nm" + dir.Extension);
		if(File.Exists(normalMapPath))
			return AssetDatabase.LoadAssetAtPath<Texture2D>(GetUnityPath(normalMapPath));
		
		
		mainPath = GetUnityPath( mainPath );
		normalMapPath = GetUnityPath( normalMapPath );
		
		AssetDatabase.CopyAsset( mainPath, normalMapPath);
		AssetDatabase.Refresh();
		
		TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(normalMapPath);
		importer.wrapMode = TextureWrapMode.Clamp;
		importer.textureType = TextureImporterType.Bump;
		importer.filterMode = FilterMode.Trilinear;
		importer.normalmapFilter = TextureImporterNormalFilter.Standard;
		importer.convertToNormalmap = true;
		importer.normalmap = true;
		
		importer.SaveAndReimport();
		
		//AssetDatabase.ImportAsset(normalMapPath, ImportAssetOptions.ForceUpdate);
		
		AssetDatabase.SaveAssets();
		
		return AssetDatabase.LoadAssetAtPath<Texture2D>(normalMapPath);
	}
	
	// Saves an asset into the GameData folder using AssetDatabase, then loads it back from that path and returns it
	// The rose file path structure is maintained but 3ddata is replaced by GameData
	public static UnityEngine.Object SaveReloadAsset(UnityEngine.Object asset, string rosePath, string extension = ".asset")
	{
		DirectoryInfo unityDir = r2uDir ( rosePath, extension );
		// Only create the asset if it doesn't already exist

        if( !File.Exists(  unityDir.FullName ) )
		{
			AssetDatabase.CreateAsset( asset, GetUnityPath(unityDir) );
			AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

		return AssetDatabase.LoadMainAssetAtPath( GetUnityPath(unityDir) );
	}

    // Attempts to load an asset using the unity path. If not present, returns null
    public static UnityEngine.Object LoadAsset(string rosePath, string extension = ".asset")
    {
        DirectoryInfo unityDir = r2uDir(rosePath, extension);

        if (!File.Exists(unityDir.FullName))
            return null;   

        return AssetDatabase.LoadMainAssetAtPath(GetUnityPath(unityDir));
    }

    public static string GetUnityTexPath(string rosePath, string extension)
    {
        DirectoryInfo unityDir = new DirectoryInfo( rosePath );//r2uDir(rosePath, extension);
        string texPath = "Assets/GameData/Textures/" + unityDir.Name;
        // Convert the texture name to the given extensio and create intermediate folders if  not present
        DirectoryInfo texDir = r2uDir(texPath, extension);
        return texDir.FullName;
    }
	
	public static void convertTex(string rosePath, string destPath, ref Texture2D texture)
	{
		Texture2D myTex = new Texture2D( texture.width, texture.height);
		myTex.SetPixels32( texture.GetPixels32(0), 0 );
		DirectoryInfo roseDir = new DirectoryInfo( rosePath );
		string dest = destPath + "/" + roseDir.Name.Replace(roseDir.Extension, ".png");
		
		if (!File.Exists( dest ))
		{
			FileStream fs = new FileStream( dest, FileMode.Create);
			BinaryWriter bw = new BinaryWriter(fs);
			bw.Write(myTex.EncodeToPNG());
			bw.Close();
			fs.Close();
			
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
		
		texture = AssetDatabase.LoadAssetAtPath<Texture2D>(dest);
		//return myTex;
	}
	
	public static Texture2D loadTex( string rosePath, string destPath)
	{
		DirectoryInfo roseDir = new DirectoryInfo( rosePath );
		string dest = destPath + roseDir.Name.Replace(roseDir.Extension, ".png");
		return AssetDatabase.LoadAssetAtPath<Texture2D>(dest);
	}
	
	
	
	// Copy the texture from 3ddata to GameData (if it doesn't already exist) then load it as a Texture2D and return it
	public static Texture2D CopyReloadTexAsset( string rosePath )
	{
		/*
        DirectoryInfo unityDir = r2uDir ( rosePath, ".dds" );
        if ( !File.Exists(unityDir.FullName))
        {
            AssetDatabase.CopyAsset(FixPath(rosePath), GetUnityPath(unityDir));
            AssetDatabase.Refresh();
        }
		
		return (Texture2D) AssetDatabase.LoadMainAssetAtPath( GetUnityPath(unityDir) );
         * */

        string texPathDDS = GetUnityTexPath(rosePath, ".dds");
        string texPathPNG = GetUnityTexPath(rosePath, ".png");

        
        if (!File.Exists(texPathPNG) && !File.Exists(texPathDDS))
        {
            // If neither dds nor png are present, move texture to textures folder as dds and load dds
            AssetDatabase.CopyAsset(FixPath(rosePath), texPathDDS);
            AssetDatabase.Refresh();
            return (Texture2D)AssetDatabase.LoadMainAssetAtPath( GetUnityPath(texPathDDS) );
        }
        else if( File.Exists(texPathPNG) )
        {
            // if both dds and png files are present in the textures folder, delete the dds
            if( File.Exists(texPathDDS) )
            {
                AssetDatabase.DeleteAsset(GetUnityPath(texPathDDS));
                AssetDatabase.Refresh();
            }

            return (Texture2D)AssetDatabase.LoadMainAssetAtPath(GetUnityPath(texPathPNG));
        }
        else if( File.Exists(texPathDDS) )
        {
            // if only dds file exists, return it
            return (Texture2D)AssetDatabase.LoadMainAssetAtPath(GetUnityPath(texPathDDS));
        }

        // in case of some failure, return null
        return null;
	}

    public static void EncapsulateTransformedBounds(ref Bounds b, Matrix4x4 t, Bounds i, bool first)
    {
        if (first)
        {
            b.center = t.MultiplyVector(i.min);
            b.size = new Vector3(1, 1, 1);
        } else
        {
            b.Encapsulate(t.MultiplyVector(i.min));
        }
        b.Encapsulate(t.MultiplyVector(i.max));
    }

    public class BoundingSphereBuilder
    {
        private List<Vector3> aPoints = new List<Vector3>();

        public void Add(Matrix4x4 t, Bounds i)
        {
            aPoints.Add(t.MultiplyVector(i.min));
            aPoints.Add(t.MultiplyVector(i.max));
        }

        public BoundingSphere Build()
        {
            Vector3 xmin, xmax, ymin, ymax, zmin, zmax;
            xmin = ymin = zmin = Vector3.one * float.PositiveInfinity;
            xmax = ymax = zmax = Vector3.one * float.NegativeInfinity;
            foreach (var p in aPoints)
            {
                if (p.x < xmin.x) xmin = p;
                if (p.x > xmax.x) xmax = p;
                if (p.y < ymin.y) ymin = p;
                if (p.y > ymax.y) ymax = p;
                if (p.z < zmin.z) zmin = p;
                if (p.z > zmax.z) zmax = p;
            }
            var xSpan = (xmax - xmin).sqrMagnitude;
            var ySpan = (ymax - ymin).sqrMagnitude;
            var zSpan = (zmax - zmin).sqrMagnitude;
            var dia1 = xmin;
            var dia2 = xmax;
            var maxSpan = xSpan;
            if (ySpan > maxSpan)
            {
                maxSpan = ySpan;
                dia1 = ymin; dia2 = ymax;
            }
            if (zSpan > maxSpan)
            {
                dia1 = zmin; dia2 = zmax;
            }
            var center = (dia1 + dia2) * 0.5f;
            var sqRad = (dia2 - center).sqrMagnitude;
            var radius = Mathf.Sqrt(sqRad);
            foreach (var p in aPoints)
            {
                float d = (p - center).sqrMagnitude;
                if (d > sqRad)
                {
                    var r = Mathf.Sqrt(d);
                    radius = (radius + r) * 0.5f;
                    sqRad = radius * radius;
                    var offset = r - radius;
                    center = (radius * center + offset * p) / r;
                }
            }
            return new BoundingSphere(center, radius);
        }

    }

    public static bool CurvesMatch(AnimationCurve x, AnimationCurve y)
    {
        if (x.preWrapMode != y.preWrapMode)
            return false;
        if (x.postWrapMode != y.postWrapMode)
            return false;
        if (x.keys.Length != y.keys.Length)
            return false;
        for (var i = 0; i < x.keys.Length; ++i)
        {
            if (x.keys[i].time != y.keys[i].time)
                return false;
            if (x.keys[i].value != y.keys[i].value)
                return false;
            if (x.keys[i].tangentMode != y.keys[i].tangentMode)
                return false;
            if (x.keys[i].inTangent != y.keys[i].inTangent)
                return false;
            if (x.keys[i].outTangent != y.keys[i].outTangent)
                return false;
        }
        return true;
    }

    //****************************************************************************************************
    //  static function DrawLine(rect : Rect) : void
    //  static function DrawLine(rect : Rect, color : Color) : void
    //  static function DrawLine(rect : Rect, width : float) : void
    //  static function DrawLine(rect : Rect, color : Color, width : float) : void
    //  static function DrawLine(Vector2 pointA, Vector2 pointB) : void
    //  static function DrawLine(Vector2 pointA, Vector2 pointB, color : Color) : void
    //  static function DrawLine(Vector2 pointA, Vector2 pointB, width : float) : void
    //  static function DrawLine(Vector2 pointA, Vector2 pointB, color : Color, width : float) : void
    //  
    //  Draws a GUI line on the screen.
    //  
    //  DrawLine makes up for the severe lack of 2D line rendering in the Unity runtime GUI system.
    //  This function works by drawing a 1x1 texture filled with a color, which is then scaled
    //   and rotated by altering the GUI matrix.  The matrix is restored afterwards.
    //****************************************************************************************************

    public static Texture2D lineTex;
	
	public static void DrawLine(Rect rect) { DrawLine(rect, GUI.contentColor, 1.0f); }
	public static void DrawLine(Rect rect, Color color) { DrawLine(rect, color, 1.0f); }
	public static void DrawLine(Rect rect, float width) { DrawLine(rect, GUI.contentColor, width); }
	public static void DrawLine(Rect rect, Color color, float width) { DrawLine(new Vector2(rect.x, rect.y), new Vector2(rect.x + rect.width, rect.y + rect.height), color, width); }
	public static void DrawLine(Vector2 pointA, Vector2 pointB) { DrawLine(pointA, pointB, GUI.contentColor, 1.0f); }
	public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color) { DrawLine(pointA, pointB, color, 1.0f); }
	public static void DrawLine(Vector2 pointA, Vector2 pointB, float width) { DrawLine(pointA, pointB, GUI.contentColor, width); }
	public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color, float width)
	{
		// Save the current GUI matrix, since we're going to make changes to it.
		Matrix4x4 matrix = GUI.matrix;
		
		// Generate a single pixel texture if it doesn't exist
		if (!lineTex) { lineTex = new Texture2D(1, 1); }
		
		// Store current GUI color, so we can switch it back later,
		// and set the GUI color to the color parameter
		Color savedColor = GUI.color;
		GUI.color = color;
		
		// Determine the angle of the line.
		float angle = Vector3.Angle(pointB - pointA, Vector2.right);
		
		// Vector3.Angle always returns a positive number.
		// If pointB is above pointA, then angle needs to be negative.
		if (pointA.y > pointB.y) { angle = -angle; }
		
		// Use ScaleAroundPivot to adjust the size of the line.
		// We could do this when we draw the texture, but by scaling it here we can use
		//  non-integer values for the width and length (such as sub 1 pixel widths).
		// Note that the pivot point is at +.5 from pointA.y, this is so that the width of the line
		//  is centered on the origin at pointA.
		GUIUtility.ScaleAroundPivot(new Vector2((pointB - pointA).magnitude, width), new Vector2(pointA.x, pointA.y + 0.5f));
		
		// Set the rotation for the line.
		//  The angle was calculated with pointA as the origin.
		GUIUtility.RotateAroundPivot(angle, pointA);
		
		// Finally, draw the actual line.
		// We're really only drawing a 1x1 texture from pointA.
		// The matrix operations done with ScaleAroundPivot and RotateAroundPivot will make this
		//  render with the proper width, length, and angle.
		GUI.DrawTexture(new Rect(pointA.x, pointA.y, 1, 1), lineTex);
		
		// We're done.  Restore the GUI matrix and GUI color to whatever they were before.
		GUI.matrix = matrix;
		GUI.color = savedColor;
	}

#endif
}