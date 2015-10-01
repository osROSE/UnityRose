#if UNITY_EDITOR
using UnityEngine;
using UnityRose.Formats;
using UnityEditor;
public class UtilWindow : EditorWindow {
	
	Vector3 p1 = new Vector3(0.0f, 0.0f, 0.0f);
	Vector3 p2 = new Vector3(1000.0f, 1000.0f, 1000.0f);
	
	// Add menu named "My Window" to the Window menu
	[MenuItem ("Window/UnityUtils")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		UtilWindow window = (UtilWindow)EditorWindow.GetWindow (typeof (UtilWindow));
		
	}
	
	
	void OnGUI () {
		p1 = EditorGUILayout.Vector3Field("Point 1:", p1);
		p2 = EditorGUILayout.Vector3Field("Point 2:", p2);

		if(GUILayout.Button("Draw"))
		{
			Vector3 p3 = new Vector3(0.0f, 0.0f, 0.0f);
			Vector3 p4 = new Vector3(1000.0f, 1000.0f, -1000.0f);
			Debug.DrawLine(p1,p2);
			Debug.DrawLine(p3,p4);
			Debug.DrawLine(p2,p4);
		}

		
	} // OnGui()

	
}

[CustomEditor (typeof(BoneNode))]
public class BoneViewer : Editor {
	
	void OnSceneGUI()
	{
		//BoneNode rootBone = ((ZMD)target).RootBone;
		Vector3 p1 = new Vector3(0.0f, 0.0f, 0.0f);
		Vector3 p2 = new Vector3(1000.0f, 1000.0f, 1000.0f);
		Handles.DrawLine(p1, p2);
	}
}

#endif
	