/*
	Flips the game object around the center x and y position.

*/

using UnityEngine;
using System.Collections;

public class FlipMechanic : MonoBehaviour {
    public static Color previewColor = new Color(0.0f, 0.7f, 1.0f, 0.6f);
    public static Color errcolor = new Color(0.7f, 0.0f, 0.0f, 0.9f);
    public static Color basecolor;
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

    //axis check
    bool axisX;
    bool axisY;

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
        axisX = false;
        axisY = false;
        blinktime = blinkmax + 0.1f;
        basecolor = new Color(1f, 1f, 1f, 1f);
        xrot = 1;
        yrot = 1;
	}

	void Flipside()
	{
		if (flipside == 1)
		{
			transform.position = new Vector3(Mathf.Lerp(-destination.x, destination.x, aniTime), transform.position.y, transform.position.z);
            transform.eulerAngles = new Vector3(180 + 180 * yrot, Mathf.Lerp(180 * xrot, 180 + 180 * xrot, aniTime), 0.0f);
		}
		else if (flipside == 2)
		{
			transform.position = new Vector3(transform.position.x, Mathf.Lerp(-destination.y, destination.y, aniTime), transform.position.z);
            transform.eulerAngles = new Vector3(Mathf.Lerp(180 * yrot, 180 + 180 * yrot, aniTime), 180 + 180 * xrot, 0.0f);
		}
		else if (flipside == 3)
		{
			transform.position = new Vector3(Mathf.Lerp(-destination.x, destination.x, aniTime), Mathf.Lerp(-destination.y, destination.y, aniTime), transform.position.z);
            transform.eulerAngles = new Vector3(0.0f, 0.0f, Mathf.Lerp(0, 180, aniTime));
		}
        axisX = false;
        axisY = false;
	}

	void FlipsidePreview()
	{
		SpriteRenderer previewSprite = preview.GetComponent<SpriteRenderer>();
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
            aniTime = 0;
            previewStartRotation = new Vector3(previewGoalRotation.x, previewGoalRotation.y, previewGoalRotation.z);
            previewGoalRotation = new Vector3(previewGoalRotation.x, (previewGoalRotation.y + 180) % 360, previewGoalRotation.z);
            axisX = true;
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
            aniTime = 0;
            previewStartRotation = new Vector3(previewGoalRotation.x, previewGoalRotation.y, previewGoalRotation.z);
            previewGoalRotation = new Vector3((previewGoalRotation.x + 180) % 360, previewGoalRotation.y, previewGoalRotation.z);
            axisY = true;
        }
        if (Input.GetButtonDown("Cancel"))
        {
            previewStart = previewGoal;
            previewFlipside = 0;
            previewGoal = new Vector3(transform.position.x, transform.position.y, previewGoal.z);
            aniTime = 0;
            previewStartRotation = new Vector3(previewGoalRotation.x, previewGoalRotation.y, previewGoalRotation.z);
            previewGoalRotation = new Vector3(transform.rotation.x * -180, transform.rotation.y * -180, transform.rotation.z * -180);
        }


        previewGoalTemp.transform.position = previewGoal; //added for red
        
		preview.transform.position = new Vector3(Mathf.Lerp(previewStart.x, previewGoal.x, aniTime), Mathf.Lerp(previewStart.y, previewGoal.y, aniTime), preview.transform.position.z);
		preview.transform.eulerAngles = new Vector3(Mathf.Lerp(previewStartRotation.x, previewGoalRotation.x, aniTime), Mathf.Lerp(previewStartRotation.y, previewGoalRotation.y, aniTime), previewGoalRotation.z);
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
        axisX = false;
        axisY = false;
        gameObject.GetComponent<SpriteRenderer>().color = basecolor;
		if (inSequence)
		{
			previewSprite.color = Color.clear;
			GetComponent<PolygonCollider2D>().enabled = false;
			Flipside();
			if (aniTime >= 1.0f)
			{
				PlayerController.dangerCheck = false;
				inSequence = false;
				//transform.eulerAngles = Vector3.zero;
                preview.transform.position = transform.position;
                done = true;
                transform.rotation = Quaternion.Euler(new Vector3(180 + 180 * yrot, 180 + 180 * xrot, 0));
			}
        }
        else if (blinktime < blinkmax){
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

    //        if (blinktime > 0.33 * blinkmax && blinktime < 0.66 * blinkmax)
    //        {
				//previewSprite.GetComponent<SpriteRenderer>().color = Color.clear;
    //        }
        }
        else //if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().getFocus() > 0.0f)
		{
			GetComponent<PolygonCollider2D>().enabled = true;

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
        
	}
    
}
