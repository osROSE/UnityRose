using UnityEngine;

namespace SevenHearts.Environment.Junon.Title {
	public class BellRinger : MonoBehaviour {
		[System.Serializable]
		public class Settings {
			public float elasticity = 0.5f;
			public float speed = 0.5f;
			public Vector3 around = new Vector3 (0.0f, 2.0f, 0.0f);
		}

		public Settings settings;

		Transform transform;
		Vector3 position;
		float lastRotate = 0.0f;

		void Start() {
			this.transform = this.gameObject.transform;
			this.position = this.transform.position;
		}

		void Update() {
			Vector3 a = this.position + this.settings.around;
			float wind = this.GetWindLevel() * 10f;
			transform.RotateAround (
				a,
				Vector3.forward,
				-this.lastRotate);
			this.lastRotate = wind;
			transform.RotateAround (
				a,
				Vector3.forward,
				wind);
		}

		float GetWindLevel() {
			return Mathf.Sin ((Time.fixedTime + this.position.z * 4f) * this.settings.speed) * this.settings.elasticity;
		}
	}
}