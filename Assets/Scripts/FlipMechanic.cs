/*
	Flips the game object around the center x and y position.

*/

using UnityEngine;
using System.Collections;

public class FlipMechanic : MonoBehaviour {
    public static Color previewColor = new Color(0.0f, 0.7f, 1.0f, 0.6f);
	public static float aniTime = 0.0f;
	public static int flipsideD;

	public GameObject preview { get; private set; }

	Vector3 previewStart;
	Vector3 previewStartRotation;
	Vector3 previewGoal;
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
		preview.layer = LayerMask.NameToLayer("Preview");
		preview.transform.parent = transform.parent;
		preview.transform.position = transform.position;
		preview.transform.localScale = transform.localScale;
		SpriteRenderer previewSprite = preview.AddComponent<SpriteRenderer>();
		previewSprite.sprite = GetComponent<SpriteRenderer>().sprite;
		previewSprite.color = Color.clear;
		flipside = 0;
		if (gameObject.GetComponent<RepeatSprite>() != null)
			preview.AddComponent<RepeatSprite>();

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
			transform.eulerAngles = new Vector3(Mathf.Lerp(0, 180, aniTime), Mathf.Lerp(0, 180, aniTime), 0.0f);
		}
	}

	void FlipsidePreview()
	{
		SpriteRenderer previewSprite = preview.GetComponent<SpriteRenderer>();
		previewSprite.color = previewColor;

		if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) ||
			(Input.GetKeyDown(KeyCode.LeftShift) && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))))
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
		if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) ||
			(Input.GetKeyDown(KeyCode.LeftShift) && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))))
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

        previewGoalTemp.transform.position = previewGoal; //added for red
        
		preview.transform.position = new Vector3(Mathf.Lerp(previewStart.x, previewGoal.x, aniTime), Mathf.Lerp(previewStart.y, previewGoal.y, aniTime), preview.transform.position.z);
		preview.transform.eulerAngles = new Vector3(Mathf.Lerp(previewStartRotation.x, previewGoalRotation.x, aniTime), Mathf.Lerp(previewStartRotation.y, previewGoalRotation.y, aniTime), preview.transform.position.z);
    }

    void checkPlayerOverlap()
    {
        CircleCollider2D playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<CircleCollider2D>();
        if ((new Rect(previewGoal - transform.localScale * 0.5f, transform.localScale)).Contains(playerCollider.transform.position))
        {
            previewColor = Color.Lerp(new Color(0.0f, 0.7f, 1.0f, 0.6f), new Color(0.7f, 0.0f, 0.0f, 0.9f), aniTime * 2);
            //print(previewGoalTemp.transform.position.x + " " + previewGoalTemp.transform.position.y + " " + previewGoalTemp.transform.position.z);
            SpriteRenderer previewSprite = preview.GetComponent<SpriteRenderer>();
            previewSprite.color = previewColor;
            //return true;
        }
        previewColor = new Color(0.0f, 0.7f, 1.0f, 0.6f);
        //return false;
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
        previewSprite.color = Color.clear;
		if (inSequence)
		{
			GetComponent<BoxCollider2D>().enabled = false;
			Flipside();
			if (aniTime >= 1.0f)
			{
				PlayerController.dangerCheck = false;
				inSequence = false;
				transform.eulerAngles = Vector3.zero;
                preview.transform.position = transform.position;
			}
		} else
		{
			GetComponent<BoxCollider2D>().enabled = true;

			if (PlayerController.dangerCheck)
			{
				reverseFlipside();
				inSequence = true;
				aniTime = 0.0f;
			}
			else if (Input.GetKey(KeyCode.LeftShift))
			{
				if (Input.GetKeyDown(KeyCode.LeftShift))
				{
					previewFlipside = 0;
					previewStart = transform.position;
					previewStartRotation = Vector3.zero;
					previewGoal = transform.position;
					previewGoalRotation = Vector3.zero;
				}
				FlipsidePreview();
                checkPlayerOverlap();
			}
			else if (Input.GetKeyUp(KeyCode.LeftShift))
			{
                if (previewFlipside != 0)
                {
                    flipside = previewFlipside;
					flipsideD = previewFlipside;
					previewFlipside = 0;
					destination = previewGoal;
                    inSequence = true;
                    aniTime = 0.0f;
                }
            }
			else if (Input.GetKey(KeyCode.LeftShift))
			{
				FlipsidePreview();
                checkPlayerOverlap();
			}
		}
	}
}
