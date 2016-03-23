using UnityEngine;
using System.Collections;

public class FlipMechanic : MonoBehaviour {
    public static Color previewColor = new Color(0.0f, 0.7f, 1.0f, 0.6f);
	public static float aniTime;
	GameObject preview;
	bool inSequence;    //Time for frozen animation
	int flipside;	//1 for horizontial, 2 for vertical
	float destination;

	void Start () {
		aniTime = 0;
		inSequence = false;
		preview = new GameObject(transform.name + " (Preview)");
		preview.transform.parent = transform.parent;
		preview.transform.position = transform.position;
		preview.transform.localScale = transform.localScale;
		SpriteRenderer previewSprite = preview.AddComponent<SpriteRenderer>();
		previewSprite.sprite = GetComponent<SpriteRenderer>().sprite;
		previewSprite.color = Color.clear;
		flipside = 0;
	}

	void Update ()
	{
		if (inSequence)
		{
			if (flipside == 1)
			{
				transform.position = new Vector3(Mathf.Lerp(-destination, destination, aniTime), transform.position.y, transform.position.z);
				transform.eulerAngles = new Vector3(0.0f, Mathf.Lerp(0, 180, aniTime), 0.0f);
			}
			else {
				transform.position = new Vector3(transform.position.x, Mathf.Lerp(-destination, destination, aniTime), transform.position.z);
				transform.eulerAngles = new Vector3(Mathf.Lerp(0, 180, aniTime), 0.0f, 0.0f);
			}
			if (aniTime >= 1.0f)
			{
				inSequence = false;
				flipside = 0;
			}
		} else
		{
			SpriteRenderer previewSprite = preview.GetComponent<SpriteRenderer>();
			previewSprite.color = Color.clear;

			if (Input.GetKeyDown(KeyCode.X))
			{
				flipside = 1;
			}
			else if (Input.GetKeyDown(KeyCode.Z))
			{
				flipside = 2;
			}
			else if (Input.GetKeyUp(KeyCode.LeftShift))
			{
				flipside = 0;
			}
			if (Input.GetKey(KeyCode.LeftShift))
			{
				previewSprite.color = previewColor;
			
				if (flipside == 1) {
					preview.transform.position = new Vector3(Mathf.Lerp(transform.position.x, - transform.position.x, aniTime), transform.position.y, transform.position.z);
					preview.transform.eulerAngles = new Vector3(0.0f, Mathf.Lerp(0, 180, aniTime), 0.0f);
				}
				else if (flipside == 2) {
					preview.transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, -transform.position.y, aniTime), transform.position.z);
					preview.transform.eulerAngles = new Vector3(Mathf.Lerp(0, 180, aniTime), 0.0f, 0.0f);
				}
				else
				{
					preview.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
					preview.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
				}
			}
			else if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X))
			{
				if (flipside == 1)
					destination = -transform.position.x;
				else if (flipside == 2)
					destination = -transform.position.y;
				inSequence = true;
				aniTime = 0.0f;
			}
		}
	}
}
