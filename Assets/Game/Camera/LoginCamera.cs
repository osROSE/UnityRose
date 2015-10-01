using UnityEngine;

namespace SevenHearts.Cam {
	public class LoginCamera : MonoBehaviour {
		Vector3 originalRotation;

		void Start () {
			this.originalRotation = this.gameObject.transform.eulerAngles;
		}

		void Update () {
			Transform transform = this.gameObject.transform;
			transform.rotation = Quaternion.Euler (
				this.originalRotation.x,
				this.originalRotation.y + Mathf.Sin (Time.fixedTime / 2) * 1.8f,
				this.originalRotation.z);
		}
	}
}