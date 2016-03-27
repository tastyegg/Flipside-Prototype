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
    int previewFlipside;

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

		if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
		{
            if (previewFlipside == 2 || previewFlipside == 3)
            {
                previewFlipside -= 2;
                previewGoal = new Vector3(previewGoal.x, transform.position.y, previewGoal.z);
            }
            else if (previewFlipside == 0 || previewFlipside == 1)
            {
                previewFlipside += 2;
                previewGoal = new Vector3(previewGoal.x, -transform.position.y, previewGoal.z);
            }
            aniTime = 0;
            previewStart = preview.transform.position;
            previewStartRotation = preview.transform.eulerAngles;
            previewGoalRotation = new Vector3(180, previewGoalRotation.y, previewGoalRotation.z);
        }
		if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
		{
            if (previewFlipside == 1 || previewFlipside == 3)
            {
                previewFlipside -= 1;
                previewGoal = new Vector3(transform.position.x, previewGoal.y, previewGoal.z);
            }
            else if (previewFlipside == 0 || previewFlipside == 2)
            {
                previewFlipside += 1;
                previewGoal = new Vector3(-transform.position.x, previewGoal.y, previewGoal.z);
            }
            aniTime = 0;
			previewStart = preview.transform.position;
			previewStartRotation = preview.transform.eulerAngles;
            previewGoalRotation = new Vector3(previewGoalRotation.x, 180, previewGoalRotation.z);
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
                if (previewFlipside != 0)
                {
                    flipside = previewFlipside;
                    previewFlipside = 0;
                    destination = previewGoal;
                    inSequence = true;
                    aniTime = 0.0f;
                }
            }
			else if (Input.GetKey(KeyCode.LeftShift))
			{
				FlipsidePreview();
			}
		}
	}
}
