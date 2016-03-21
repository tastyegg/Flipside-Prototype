using UnityEngine;
using System.Collections;

public class FlipMechanic : MonoBehaviour {
	public static float aniTime;
	GameObject preview;
	bool inSequence;    //Time for frozen animation
	bool horizontalFlip;
	float destination;
	Vector3 originalform, originalscale;

	void Start () {
		aniTime = 0;
		inSequence = false;
		originalform = transform.position;
		originalscale = transform.localScale;
		preview = new GameObject(transform.name + " (Preview)");
		preview.transform.position = transform.position;
		preview.transform.localScale = transform.localScale;
		SpriteRenderer previewSprite = preview.AddComponent<SpriteRenderer>();
		previewSprite.sprite = GetComponent<SpriteRenderer>().sprite;
		previewSprite.color = Color.clear;
        horizontalFlip = true;
	}

	void Update ()
	{
		if (inSequence)
		{
			if (horizontalFlip)
			{
				transform.position = new Vector3(Mathf.Lerp(-destination, destination, aniTime), transform.position.y, transform.position.z);
				transform.eulerAngles = new Vector3(0.0f, Mathf.Lerp(0, 180, aniTime), 0.0f);
			}
			else {
				transform.position = new Vector3(transform.position.x, Mathf.Lerp(-destination, destination, aniTime), transform.position.z);
				transform.eulerAngles = new Vector3(Mathf.Lerp(0, 180, aniTime), 0.0f, 0.0f);
			}
			if (aniTime >= 1.0f)
				inSequence = false;
		} else
		{
			SpriteRenderer previewSprite = preview.GetComponent<SpriteRenderer>();
			previewSprite.color = Color.clear;

			if (Input.GetKeyDown(KeyCode.X))
			{
				horizontalFlip = true;
			}
			else if (Input.GetKeyDown(KeyCode.Z))
			{
				horizontalFlip = false;
			}
			if (Input.GetKey(KeyCode.LeftShift))
			{
				previewSprite.color = Color.blue;
			
				if (horizontalFlip) {
					preview.transform.position = new Vector3(Mathf.Lerp(transform.position.x, - transform.position.x, aniTime), transform.position.y, transform.position.z);
					preview.transform.eulerAngles = new Vector3(0.0f, Mathf.Lerp(0, 180, aniTime), 0.0f);
				}
				else
				{
					preview.transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, -transform.position.y, aniTime), transform.position.z);
					preview.transform.eulerAngles = new Vector3(Mathf.Lerp(0, 180, aniTime), 0.0f, 0.0f);
				}
			}
			else if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X))
			{
				if (horizontalFlip)
					destination = -transform.position.x;
				else
					destination = -transform.position.y;
				inSequence = true;
				aniTime = 0.0f;
			}
		}
	}
}
