using System.Collections.Generic;
using UnityEngine;

namespace SevenHearts.Environment {
	
	public class WindController : MonoBehaviour {
		[System.Serializable]
		public class Settings {
			public float elasticity = 0.5f;
			public float speed = 0.5f;
			public bool bobY = false;
		}

		public Settings settings;
		Mesh mesh;
		Vector3[] vertices;
		List<Vector3> newVerticies = new List<Vector3>();

		void Start() {
			this.mesh = this.gameObject.GetComponent<MeshFilter> ().sharedMesh;
			this.vertices = mesh.vertices;
		}

		void Update() {
			newVerticies.Clear ();
			foreach (Vector3 v in this.vertices) {
				float wind = this.GetWindLevel(v.x, v.z);
				newVerticies.Add(new Vector3(
						v.x + wind,
						v.y - (this.settings.bobY ? wind : 0f),
						v.z + wind
					));
			}

			this.mesh.SetVertices (newVerticies);
		}

		float GetWindLevel(float rx, float ry) {
			Vector3 pos = this.gameObject.transform.position;
			float time = Time.fixedTime * this.settings.speed;
			float micro = this.settings.elasticity * Mathf.Sin(
						Mathf.PerlinNoise (
								pos.x + rx + time,
								pos.z + ry + time));
			time /= 4.0f;
			float macro = micro *
						Mathf.Pow (
								Mathf.PerlinNoise(
									pos.x / 10f + rx + time,
									pos.z / 10f + ry + time), 4f);
			return macro;
		}
	}
}