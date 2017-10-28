using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FollowPlayer : MonoBehaviour
{
	FlipMechanic flip;
	int initialFlip;
	int initialFlipPreview;

	public static bool reverse;
	float zPos;

	// Use this for initialization
	void Start ()
	{
		flip = GameObject.FindObjectOfType<FlipMechanic>();

		zPos = transform.position.z;
	}

	void OnEnable()
	{
		//Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	void OnDisable()
	{
		//Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		flip = GameObject.FindObjectOfType<FlipMechanic>();
	}

	void LateUpdate()
	{
		if (Input.GetButton("Focus"))
		{
			if (flip.directionFlipPreview == 0)
			{
				transform.position = new Vector3(0, 0, zPos);
				transform.localEulerAngles = new Vector3(0, 0, 0);
			}
			else
			{
				transform.position = new Vector3(0, 0, -zPos);
				transform.localEulerAngles = new Vector3(0, 180, 0);
			}

			if (flip.directionFlipPreview == 1)
			{
				transform.RotateAround(Vector3.zero, Vector3.up, Mathf.Lerp(0.0f, 180.0f, flip.aniTimePreview));
			}
			else if (flip.directionFlipPreview == 2)
			{
				transform.localEulerAngles = new Vector3(0, 180, 180);
				transform.RotateAround(Vector3.zero, Vector3.right, Mathf.Lerp(0.0f, 180.0f, flip.aniTimePreview));
			}
		} else
		{
			if (flip.directionFlip == 0)
			{
				transform.position = new Vector3(0, 0, zPos);
				transform.localEulerAngles = new Vector3(0, 0, 0);
			}
			else
			{
				transform.position = new Vector3(0, 0, -zPos);
				transform.localEulerAngles = new Vector3(0, 180, 0);
			}

			if (flip.directionFlip == 1)
			{
				transform.RotateAround(Vector3.zero, Vector3.up, Mathf.Lerp(0.0f, 180.0f, flip.aniTime));
			}
			else if (flip.directionFlip == 2)
			{
				transform.localEulerAngles = new Vector3(0, 180, 180);
				transform.RotateAround(Vector3.zero, Vector3.right, Mathf.Lerp(0.0f, 180.0f, flip.aniTime));
			}
			else if (flip.directionFlip == 3)
			{
				if (flip.aniTime < 0.5f)
				{
					transform.position = new Vector3(0, 0, zPos);
					transform.localEulerAngles = new Vector3(0, 0, 180);
					transform.RotateAround(Vector3.zero, Vector3.up, Mathf.Lerp(0.0f, 180.0f, flip.aniTime * 2));
				}
				else
				{
					transform.localEulerAngles = new Vector3(0, 180, 180);
					transform.RotateAround(Vector3.zero, Vector3.right, Mathf.Lerp(0.0f, 180.0f, flip.aniTime * 2 - 1));
				}
			}
		}
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
