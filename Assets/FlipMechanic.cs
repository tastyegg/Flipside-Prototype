using UnityEngine;
using System.Collections;

public class FlipMechanic : MonoBehaviour {
    public static Color previewColor = new Color(0.0f, 0.7f, 1.0f, 0.6f);
	public static float aniTime = 0.0f;

	GameObject preview;
	Vector3 previewStart;
	Vector3 previewStartRotation;
	Vector3 previewGoal;
	Vector3 previewGoalRotation;

	bool inSequence;    //Time for frozen animation
	int flipside;   //1 for horizontial, 2 for vertical
	Vector3 destination;

	void Start ()
	{
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

	void Flipside()
	{
		if (flipside == 1)
		{
			transform.position = new Vector3(Mathf.Lerp(-destination.x, destination.x, aniTime), transform.position.y, transform.position.z);
			transform.eulerAngles = new Vector3(0.0f, Mathf.Lerp(0, 180, aniTime), 0.0f);
		}
		else if (flipside == 2)
		{
			transform.position = new Vector3(transform.position.x, Mathf.Lerp(-destination.y, destination.y, aniTime), transform.position.z);
			transform.eulerAngles = new Vector3(Mathf.Lerp(0, 180, aniTime), 0.0f, 0.0f);
		}
		else if (flipside == 3)
		{
			transform.position = new Vector3(Mathf.Lerp(-destination.x, destination.x, aniTime), Mathf.Lerp(-destination.y, destination.y, aniTime), transform.position.z);
			transform.eulerAngles = new Vector3(Mathf.Lerp(0, 180, aniTime), Mathf.Lerp(0, 180, aniTime), 0.0f);
		}
	}

	void FlipsidePreview()
	{
		SpriteRenderer previewSprite = preview.GetComponent<SpriteRenderer>();
		previewSprite.color = previewColor;

		if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyUp(KeyCode.Z))
		{
			aniTime = 0;
			previewStart = preview.transform.position;
			previewStartRotation = preview.transform.eulerAngles;
			if (Input.GetKeyDown(KeyCode.Z))
			{
				previewGoal = new Vector3(previewGoal.x, -transform.position.y, previewGoal.z);
				previewGoalRotation = new Vector3(180, previewGoalRotation.y, previewGoalRotation.z);
			}
			else
			{
				previewGoal = new Vector3(previewGoal.x, transform.position.y, previewGoal.z);
				previewGoalRotation = new Vector3(0, previewGoalRotation.y, previewGoalRotation.z);
			}
		}
		if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyUp(KeyCode.X))
		{
			aniTime = 0;
			previewStart = preview.transform.position;
			previewStartRotation = preview.transform.eulerAngles;
			if (Input.GetKeyDown(KeyCode.X))
			{
				previewGoal = new Vector3(-transform.position.x, previewGoal.y, previewGoal.z);
				previewGoalRotation = new Vector3(previewGoalRotation.x, 180, previewGoalRotation.z);
			}
			else
			{
				previewGoal = new Vector3(transform.position.x, previewGoal.y, previewGoal.z);
				previewGoalRotation = new Vector3(previewGoalRotation.x, 0, previewGoalRotation.z);
			}
		}

		preview.transform.position = new Vector3(Mathf.Lerp(previewStart.x, previewGoal.x, aniTime), Mathf.Lerp(previewStart.y, previewGoal.y, aniTime), preview.transform.position.z);
		preview.transform.eulerAngles = new Vector3(Mathf.Lerp(previewStartRotation.x, previewGoalRotation.x, aniTime), Mathf.Lerp(previewStartRotation.y, previewGoalRotation.y, aniTime), preview.transform.position.z);
	}

	void Update ()
	{
		if (inSequence)
		{
			Flipside();
			if (aniTime >= 1.0f)
			{
				inSequence = false;
				flipside = 0;
			}
		} else
		{
			SpriteRenderer previewSprite = preview.GetComponent<SpriteRenderer>();
			previewSprite.color = Color.clear;
			
			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				previewStart = transform.position;
				previewStartRotation = Vector3.zero;
				previewGoal = transform.position;
				previewGoalRotation = Vector3.zero;
			}
			if (Input.GetKeyUp(KeyCode.LeftShift))
			{
				if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.X))
				{
					flipside = 0;
					if (Input.GetKey(KeyCode.Z))
						flipside += 2;
					if (Input.GetKey(KeyCode.X))
						flipside ++;
					destination = previewGoal;
					inSequence = true;
					aniTime = 0.0f;
				}
			}
			else if (Input.GetKey(KeyCode.LeftShift))
			{
				FlipsidePreview();
			}
			else if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X))
			{
				if (Input.GetKey(KeyCode.X))
				{
					flipside = 1;
					destination = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
				}
				else if (Input.GetKey(KeyCode.Z))
				{
					flipside = 2;
					destination = new Vector3(transform.position.x, -transform.position.y, transform.position.z);
				}
				inSequence = true;
				aniTime = 0.0f;
			}
		}
	}
}
