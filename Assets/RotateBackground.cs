using UnityEngine;
using System.Collections;

public class RotateBackground : MonoBehaviour {
	float spinVelocity = Mathf.PI / 2;

	// Update is called once per frame
	void Update () {
		spinVelocity += 0.0003f;

		Vector3 r = transform.localEulerAngles;
		r.z += Mathf.Sin(spinVelocity) * 0.04f * Time.timeScale;
		transform.localEulerAngles = r;
	}
}
