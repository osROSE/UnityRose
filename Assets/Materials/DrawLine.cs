using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DrawLine : MonoBehaviour {

	private LineRenderer lineRenderer;
	
	public Transform p0;
	public Transform p1;
	// Use this for initialization
	void Start () {
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.SetPosition(0, p0.position);
		lineRenderer.SetPosition(1, p1.position);
		lineRenderer.SetWidth(.02f, 0.02f);
	}
	
	// Update is called once per frame
	void Update () {
		lineRenderer.SetPosition(0, p0.position);
		lineRenderer.SetPosition (1, p1.position);
	}
	
	void OnGUI()
	{
		lineRenderer.SetPosition(0, p0.position);
		lineRenderer.SetPosition (1, p1.position);
	}
	
}
