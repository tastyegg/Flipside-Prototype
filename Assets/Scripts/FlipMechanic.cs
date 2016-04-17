﻿using UnityEngine;
using System.Collections;

public class FlipMechanic : MonoBehaviour {
    public static Color previewColor = new Color(0.0f, 0.7f, 1.0f, 0.8f);
	public static float aniTime = 0.0f;
	public static int flipsideD;
    public static bool done;

	public GameObject preview { get; private set; }

	Vector3 previewStart;
	Vector3 previewStartRotation;
	public Vector3 previewGoal { get; private set; }
	Vector3 previewGoalRotation;
    int previewFlipside;

	bool inSequence;    //Time for frozen animation
	int flipside;   //1 for horizontial, 2 for vertical
	Vector3 destination;

    public bool getSeq() { return inSequence; }

    //for red checking
    public GameObject previewGoalTemp;

	void Start ()
	{
		inSequence = false;
		preview = new GameObject(transform.name + " (Preview)");
		preview.transform.parent = transform.parent;
		preview.transform.position = transform.position;
		preview.transform.localScale = transform.localScale;
        preview.layer = LayerMask.NameToLayer("WallPreview");
		SpriteRenderer previewSprite = preview.AddComponent<SpriteRenderer>();
		previewSprite.sprite = GetComponent<SpriteRenderer>().sprite;
		previewSprite.color = Color.clear;
		flipside = 0;
        //added for red preview
        previewGoalTemp = preview;
        previewGoalTemp.AddComponent<BoxCollider2D>();
        previewGoalTemp.GetComponent<BoxCollider2D>().isTrigger = true;
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
            transform.eulerAngles = new Vector3(0.0f, 0.0f, Mathf.Lerp(0, 180, aniTime));
		}
	}

	void FlipsidePreview()
	{
		SpriteRenderer sprite = GetComponent<SpriteRenderer>();
		Color spriteColor = sprite.color;
		spriteColor.a = 1.0f - PlayerController.focusTimer / PlayerController.FOCUS_TIMER;
		sprite.color = spriteColor;
		
		SpriteRenderer previewSprite = preview.GetComponent<SpriteRenderer>();
		Color previewSpriteColor = previewSprite.color;
		previewSpriteColor.a = PlayerController.focusTimer / PlayerController.FOCUS_TIMER;
		previewSprite.color = previewSpriteColor;

		if (Input.GetButtonDown("FlipX"))
        {
            previewStart = previewGoal;
            if (previewFlipside % 2 == 1)
            {
                previewFlipside -= 1;
                previewGoal = new Vector3(transform.position.x, previewGoal.y, previewGoal.z);
            }
            else if (previewFlipside % 2 == 0)
            {
                previewFlipside += 1;
                previewGoal = new Vector3(-transform.position.x, previewGoal.y, previewGoal.z);
            }
            aniTime = 0;
            previewStartRotation = new Vector3(previewGoalRotation.x, 0, previewGoalRotation.z);
            previewGoalRotation = new Vector3(previewGoalRotation.x, 180, previewGoalRotation.z);
        }
        if (Input.GetButtonDown("FlipY"))
        {
            previewStart = previewGoal;
            if (previewFlipside > 1)
            {
                previewFlipside -= 2;
                previewGoal = new Vector3(previewGoal.x, transform.position.y, previewGoal.z);
            }
            else if (previewFlipside < 2)
            {
                previewFlipside += 2;
                previewGoal = new Vector3(previewGoal.x, -transform.position.y, previewGoal.z);
            }
            aniTime = 0;
            previewStartRotation = new Vector3(0, previewGoalRotation.y, previewGoalRotation.z);
            previewGoalRotation = new Vector3(180, previewGoalRotation.y, previewGoalRotation.z);
        }
        if (Input.GetButtonDown("Cancel"))
        {
            previewStart = previewGoal;
            previewFlipside = 0;
            previewGoal = new Vector3(transform.position.x, transform.position.y, previewGoal.z);
            aniTime = 0;
            previewStartRotation = new Vector3(0, previewGoalRotation.y, previewGoalRotation.z);
            previewGoalRotation = new Vector3(0, previewGoalRotation.y, previewGoalRotation.z);
        }

        previewGoalTemp.transform.position = previewGoal; //added for red
        
		preview.transform.position = new Vector3(Mathf.Lerp(previewStart.x, previewGoal.x, aniTime), Mathf.Lerp(previewStart.y, previewGoal.y, aniTime), preview.transform.position.z);
		preview.transform.eulerAngles = new Vector3(Mathf.Lerp(previewStartRotation.x, previewGoalRotation.x, aniTime), Mathf.Lerp(previewStartRotation.y, previewGoalRotation.y, aniTime), preview.transform.position.z);
    }

	void reverseFlipside()
	{
		if (flipside == 1)
		{
			destination = new Vector3(-destination.x, destination.y, destination.z);
		}
		else if (flipside == 2)
		{
			destination = new Vector3(destination.x, -destination.y, destination.z);
		}
		else if (flipside == 3)
		{
			destination = new Vector3(-destination.x, -destination.y, destination.z);
		}
	}


	void Update ()
	{
		SpriteRenderer previewSprite = preview.GetComponent<SpriteRenderer>();
		if (inSequence)
		{
			SpriteRenderer sprite = GetComponent<SpriteRenderer>();
			Color spriteColor = sprite.color;
			spriteColor.a = 1.0f;
			sprite.color = spriteColor;

			previewSprite.color = Color.clear;
			GetComponent<BoxCollider2D>().enabled = false;
			Flipside();
			if (aniTime >= 1.0f)
			{
				PlayerController.dangerCheck = false;
				inSequence = false;
				transform.eulerAngles = Vector3.zero;
                preview.transform.position = transform.position;
                done = true;
			}
        }
        else
		{
			GetComponent<BoxCollider2D>().enabled = true;

			if (Input.GetButton("Focus") && PlayerController.focusTimer > 0)
			{
				if (Input.GetButtonDown("Focus"))
				{
					previewFlipside = 0;
					previewStart = transform.position;
					previewStartRotation = Vector3.zero;
					previewGoal = transform.position;
					previewGoalRotation = Vector3.zero;
                    done = false;
                }
				FlipsidePreview();
            }
			else
			{
				SpriteRenderer sprite = GetComponent<SpriteRenderer>();
				Color spriteColor = sprite.color;
				spriteColor.a = 1.0f;
				sprite.color = spriteColor;

				previewSprite.color = Color.clear;

                if (Input.GetButtonUp("Focus") && PlayerController.focusTimer > 0)
				{
					if (previewFlipside != 0 && !PlayerController.dangerCheck)
					{
						flipside = previewFlipside;
						flipsideD = previewFlipside;
						previewFlipside = 0;
						destination = previewGoal;
						inSequence = true;
						aniTime = 0.0f;
					}
                    else
                    {
                        done = true;
                    }
				}
                if (Input.GetButtonDown("FlipX"))
                {
                    flipside = 1;
                    flipsideD = flipside;
                    destination = new Vector3(-transform.position.x, previewGoal.y, previewGoal.z);
                    inSequence = true;
                    aniTime = 0.0f;
                    done = false;
                }
                if (Input.GetButtonDown("FlipY"))
                {
                    flipside = 2;
                    flipsideD = flipside;
                    destination = new Vector3(previewGoal.x, -transform.position.y, previewGoal.z);
                    inSequence = true;
                    aniTime = 0.0f;
                    done = false;
                }
            }
        }
	}
}
