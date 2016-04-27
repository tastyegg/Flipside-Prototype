/*
	Flips the game object around the center x and y position.

*/

using UnityEngine;
using System.Collections;

public class FlipMechanic : MonoBehaviour {
    public static Color previewColor = new Color(0.0f, 0.7f, 1.0f, 0.6f);
    public static Color errcolor = new Color(0.7f, 0.0f, 0.0f, 0.9f);
    public static Color basecolor = new Color(0.8f, 0.8f, 1.0f, 1.0f);
	public static float aniTime = 0.0f;
	public static float previewAniTime = 0.0f;
	public static int flipsideD;
    public static bool done;

	public GameObject preview { get; private set; }

	Vector3 previewStart;
	Vector3 previewStartRotation;
	public Vector3 previewGoal { get; private set; }
	Vector3 previewGoalRotation;
    int previewFlipside;
	public static int StaticPreviewFlipside { get; private set; }

	bool inSequence;    //Time for frozen animation
	int flipside;   //1 for horizontial, 2 for vertical
	Vector3 destination;

    public bool getSeq() { return inSequence; }

    //for red checking
    public GameObject previewGoalTemp;
    
    //Redblinking
    public static float blinktime;
    public static float blinkmax = 2.0f;

    int xrot;
    int yrot;

	void Start ()
	{
		inSequence = false;
		preview = new GameObject(transform.name + " (Preview)");
		preview.transform.parent = transform.parent;
		preview.transform.position = transform.position;
		preview.transform.localScale = transform.localScale;
        preview.layer = LayerMask.NameToLayer("WallPreview");
		preview.tag = "WallPreview";
		SpriteRenderer previewSprite = preview.AddComponent<SpriteRenderer>();
		previewSprite.sprite = GetComponent<SpriteRenderer>().sprite;
		previewSprite.color = Color.clear;
		flipside = 0;
        //added for red preview
        previewGoalTemp = preview;
        previewGoalTemp.AddComponent<PolygonCollider2D>();
        previewGoalTemp.GetComponent<PolygonCollider2D>().isTrigger = true;
        blinktime = blinkmax + 0.1f;
        xrot = 1;
        yrot = 1;
	}

	void Flipside()
	{
		if (flipside == 1)
		{
			preview.transform.position = new Vector3(Mathf.Lerp(-destination.x, destination.x, aniTime), transform.position.y, transform.position.z);
			transform.position = new Vector3(destination.x, transform.position.y, transform.position.z);
			preview.transform.eulerAngles = new Vector3(180 + 180 * yrot, Mathf.Lerp(180 * xrot, 180 + 180 * xrot, aniTime), 0.0f);
		}
		else if (flipside == 2)
		{
			preview.transform.position = new Vector3(transform.position.x, Mathf.Lerp(-destination.y, destination.y, aniTime), transform.position.z);
			transform.position = new Vector3(transform.position.x, destination.y, transform.position.z);
			preview.transform.eulerAngles = new Vector3(Mathf.Lerp(180 * yrot, 180 + 180 * yrot, aniTime), 180 + 180 * xrot, 0.0f);
		}
		else if (flipside == 3)
		{
			preview.transform.position = new Vector3(Mathf.Lerp(-destination.x, destination.x, aniTime * 2), Mathf.Lerp(-destination.y, destination.y, aniTime * 2 - 1.0f), transform.position.z);
			transform.position = new Vector3(destination.x, destination.y, transform.position.z);
			preview.transform.eulerAngles = new Vector3(Mathf.Lerp(180 * yrot, 180 + 180 * yrot, aniTime * 2 - 1.0f), Mathf.Lerp(180 * xrot, 180 + 180 * xrot, aniTime * 2), 0.0f);
		}
		transform.eulerAngles = new Vector3(180 + 180 * yrot, 180 + 180 * xrot, 0.0f);
	}

	void FlipsidePreview()
	{
        //previewSprite.color = previewColor;

        //broken line of code
        //if (Input.GetButtonDown("FlipX") || GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().axisX && !axisX)
        if (PlayerController.axisButtonDownFlipX)
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
            previewAniTime = 0;
            previewStartRotation = new Vector3(previewGoalRotation.x, previewGoalRotation.y, previewGoalRotation.z);
            previewGoalRotation = new Vector3(previewGoalRotation.x, (previewGoalRotation.y + 180) % 360, previewGoalRotation.z);
        }
        //broken line of code
        //if (Input.GetButtonDown("FlipY") || GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().axisY && !axisY)
        if (PlayerController.axisButtonDownFlipY)
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
			previewAniTime = 0;
            previewStartRotation = new Vector3(previewGoalRotation.x, previewGoalRotation.y, previewGoalRotation.z);
            previewGoalRotation = new Vector3((previewGoalRotation.x + 180) % 360, previewGoalRotation.y, previewGoalRotation.z);
        }
        if (Input.GetButtonDown("Cancel"))
        {
            previewStart = previewGoal;
            previewFlipside = 0;
            previewGoal = new Vector3(transform.position.x, transform.position.y, previewGoal.z);
			previewAniTime = 0;
            previewStartRotation = new Vector3(previewGoalRotation.x, previewGoalRotation.y, previewGoalRotation.z);
            previewGoalRotation = new Vector3(transform.rotation.x * -180, transform.rotation.y * -180, transform.rotation.z * -180);
        }
		StaticPreviewFlipside = previewFlipside;

        previewGoalTemp.transform.position = previewGoal; //added for red
        
		preview.transform.position = new Vector3(Mathf.Lerp(previewStart.x, previewGoal.x, previewAniTime), Mathf.Lerp(previewStart.y, previewGoal.y, previewAniTime), preview.transform.position.z);
		preview.transform.eulerAngles = new Vector3(Mathf.Lerp(previewStartRotation.x, previewGoalRotation.x, previewAniTime), Mathf.Lerp(previewStartRotation.y, previewGoalRotation.y, previewAniTime), previewGoalRotation.z);
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
        GetComponent<SpriteRenderer>().color = basecolor;
        if (blinktime < blinkmax){
			if (PlayerController.ydanger)
			{
				preview.transform.localPosition = new Vector3(transform.localPosition.x, -transform.localPosition.y, transform.localPosition.z);
				Vector3 angle = transform.localEulerAngles;
				angle.x = (angle.x + 180) % 360;
				preview.transform.localEulerAngles = angle;
			}
			else if (PlayerController.xdanger)
			{
				preview.transform.localPosition = new Vector3(-transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
				Vector3 angle = transform.localEulerAngles;
				angle.y = (angle.y + 180) % 360;
				preview.transform.localEulerAngles = angle;
			}
			previewSprite.GetComponent<SpriteRenderer>().color = errcolor;
        }
        else
		{
			if (PlayerController.inFocus)
			{
				if (PlayerController.enteringFocus)
				{
					previewFlipside = 0;
					previewStart = transform.position;
					previewStartRotation = transform.eulerAngles;
					previewGoal = transform.position;
					previewGoalRotation = transform.eulerAngles;
					done = false;
                }
				FlipsidePreview();
            }
			else {
				previewSprite.color = Color.clear;

                if (PlayerController.exitingFocus)
				{
					if (previewFlipside != 0 && !PlayerController.dangerCheck)
					{
						flipside = previewFlipside;
						flipsideD = previewFlipside;
						previewFlipside = 0;
						destination = previewGoal;
						inSequence = true;
						aniTime = 0.0f;
                        xrot = (xrot + (flipside % 2)) %2;
                        yrot = (yrot + (flipside / 2)) % 2;
					}
                    else if(previewFlipside != 0 && PlayerController.dangerCheck)
                    {
                        blinktime = 0.0f;
                        done = true;
                    }
                    else
                    {
                        done = true;
                    }
				}
                if (PlayerController.xdanger && PlayerController.axisButtonDownFlipX)
                {
                    blinktime = 0.0f;
                    done = true;
                }
                else if (PlayerController.axisButtonDownFlipX)
                {
                    flipside = 1;
                    flipsideD = flipside;
                    destination = new Vector3(-transform.position.x, previewGoal.y, previewGoal.z);
                    inSequence = true;
                    aniTime = 0.0f;
                    done = false;
                    xrot = (xrot + 1) % 2;
                }
                if (PlayerController.ydanger && PlayerController.axisButtonDownFlipY)
                {
                    blinktime = 0.0f;
                    done = true;
                }
                else if (PlayerController.axisButtonDownFlipY)
                {
                    flipside = 2;
                    flipsideD = flipside;
                    destination = new Vector3(previewGoal.x, -transform.position.y, previewGoal.z);
                    inSequence = true;
                    aniTime = 0.0f;
                    done = false;
                    yrot = (yrot + 1) % 2;
                }
            }

		}
		if (inSequence)
		{
			previewSprite.color = basecolor;
			GetComponent<SpriteRenderer>().color = Color.clear;
			Flipside();
			if (aniTime >= 1.0f)
			{
				PlayerController.dangerCheck = false;
				inSequence = false;
				preview.transform.position = transform.position;
				done = true;
				transform.rotation = Quaternion.Euler(new Vector3(180 + 180 * yrot, 180 + 180 * xrot, 0));
			}
		}

	}
    
}
