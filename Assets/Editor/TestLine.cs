using UnityEngine;
using System.Collections;

public class TestLine : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawLine(new Vector3(0f,0f,0f), new Vector3(100f,100f,100f), Color.green, 2f, false);	
	}
}
