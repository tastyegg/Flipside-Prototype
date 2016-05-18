using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour
{
	FlipMechanic flip;
	public static bool reverse;
	float zPos;

	// Use this for initialization
	void Start ()
	{
		flip = GameObject.FindObjectOfType<FlipMechanic>();
		zPos = transform.position.z;
	}
	
	void LateUpdate()
	{

	}

	// Update is called once per frame
	void Update () {
		/*
		if (flip.flipside == 1)
		{
			if (reverse)
			{
				transform.position = new Vector3(zPos * Mathf.Sin(Mathf.PI * (1 - Mathf.Max(0.0f, 1 - flip.aniTime))),
					0, zPos * Mathf.Cos(Mathf.PI * (1 - Mathf.Max(0.0f, 1 - flip.aniTime))));
				transform.localEulerAngles = new Vector3(180, (Mathf.PI * (1 - Mathf.Max(0.0f, 1 - flip.aniTime))) / (2 * Mathf.PI) * 360 + 180, 0);
				transform.RotateAround(Vector3.zero, transform.right, 180);
			}
			else
			{
				transform.position = new Vector3(zPos * Mathf.Sin(Mathf.PI * (1 - Mathf.Min(1.0f, flip.aniTime))),
					0, zPos * Mathf.Cos(Mathf.PI * (1 - Mathf.Min(1.0f, flip.aniTime))));
				transform.localEulerAngles = new Vector3(0, (Mathf.PI * (1 - Mathf.Min(1.0f, flip.aniTime))) / (2 * Mathf.PI) * 360, 0);
			}
		}
		else if (flip.flipside == 2)
		{
			if (reverse)
			{
				transform.position = new Vector3(0, -zPos * Mathf.Sin(Mathf.PI * (1 - Mathf.Max(0.0f, 1 - flip.aniTime))),
					zPos * Mathf.Cos(Mathf.PI * (1 - Mathf.Max(0.0f, 1 - flip.aniTime))));
				transform.localEulerAngles = new Vector3((Mathf.PI * (1 - Mathf.Max(0.0f, 1 - flip.aniTime))) / (2 * Mathf.PI) * 360 + 180, 0, 0);
				transform.Rotate(transform.right, 180);
				transform.RotateAround(Vector3.zero, transform.right, 180);
			}
			else
			{
				transform.position = new Vector3(0, -zPos * Mathf.Sin(Mathf.PI * (1 - Mathf.Min(1.0f, flip.aniTime))),
					zPos * Mathf.Cos(Mathf.PI * (1 - Mathf.Min(1.0f, flip.aniTime))));
				transform.localEulerAngles = new Vector3((Mathf.PI * (1 - Mathf.Min(1.0f, flip.aniTime))) / (2 * Mathf.PI) * 360, 0, 0);
			}
		}
		else if (flip.flipside == 3)
		{
			if (reverse)
			{
				if (flip.flipside > 0.5f)
				{
					transform.position = new Vector3(0, -zPos * Mathf.Sin(Mathf.PI * (1 - Mathf.Max(0.0f, 1 - (flip.aniTime * 2 - 1)))),
						zPos * Mathf.Cos(Mathf.PI * (1 - Mathf.Max(0.0f, 1 - (flip.aniTime * 2 - 1)))));
					transform.localEulerAngles = new Vector3((Mathf.PI * (1 - Mathf.Max(0.0f, 1 - (flip.aniTime * 2 - 1)))) / (2 * Mathf.PI) * 360 + 180, 0, 0);
					transform.Rotate(transform.right, 180);
					transform.RotateAround(Vector3.zero, transform.right, 180);
				} else
				{
					transform.position = new Vector3(zPos * Mathf.Sin(Mathf.PI * (1 - Mathf.Max(0.0f, 1 - (flip.aniTime * 2)))),
						0, zPos * Mathf.Cos(Mathf.PI * (1 - Mathf.Max(0.0f, 1 - (flip.aniTime * 2)))));
					transform.localEulerAngles = new Vector3(180, (Mathf.PI * (1 - Mathf.Max(0.0f, 1 - (flip.aniTime * 2)))) / (2 * Mathf.PI) * 360 + 180, 0);
					transform.RotateAround(Vector3.zero, transform.right, 180);
				}
			}
			else
			{
				if (flip.aniTime > 0.5f)
				{
					transform.position = new Vector3(0, -zPos * Mathf.Sin(Mathf.PI * (1 - Mathf.Min(1.0f, (flip.aniTime * 2 - 1)))),
					zPos * Mathf.Cos(Mathf.PI * (1 - Mathf.Min(1.0f, (flip.aniTime * 2 - 1)))));
					transform.localEulerAngles = new Vector3((Mathf.PI * (1 - Mathf.Min(1.0f, (flip.aniTime * 2 - 1)))) / (2 * Mathf.PI) * 360, 0, 0);
				} else
				{
					transform.position = new Vector3(zPos * Mathf.Sin(Mathf.PI * (1 - Mathf.Min(1.0f, (flip.aniTime * 2)))),
						0, zPos * Mathf.Cos(Mathf.PI * (1 - Mathf.Min(1.0f, (flip.aniTime * 2)))));
					transform.localEulerAngles = new Vector3(0, (Mathf.PI * (1 - Mathf.Min(1.0f, (flip.aniTime * 2)))) / (2 * Mathf.PI) * 360, 0);
				}
			}
		}*/
	}
}
