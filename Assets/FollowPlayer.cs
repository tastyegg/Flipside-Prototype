using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {
	float zPos;
	PlayerController pc;

	// Use this for initialization
	void Start () {
		zPos = transform.position.z;
		pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (FlipMechanic.flipsideD == 1)
		{
			transform.position = new Vector3(zPos * Mathf.Sin(Mathf.PI * (1 - Mathf.Min(1.0f, FlipMechanic.aniTime))),
				0, zPos * Mathf.Cos(Mathf.PI * (1 - Mathf.Min(1.0f, FlipMechanic.aniTime))));
			transform.localEulerAngles = new Vector3(0, (Mathf.PI * (1 - Mathf.Min(1.0f, FlipMechanic.aniTime))) / (2 * Mathf.PI) * 360, 0);
		}
		else if (FlipMechanic.flipsideD == 2)
		{
			transform.position = new Vector3(0, -zPos * Mathf.Sin(Mathf.PI * (1 - Mathf.Min(1.0f, FlipMechanic.aniTime))),
				zPos * Mathf.Cos(Mathf.PI * (1 - Mathf.Min(1.0f, FlipMechanic.aniTime))));
			transform.localEulerAngles = new Vector3((Mathf.PI * (1 - Mathf.Min(1.0f, FlipMechanic.aniTime))) / (2 * Mathf.PI) * 360, 0, 0);
		}
		else if (FlipMechanic.flipsideD == 3)
		{
		}
	}
}
