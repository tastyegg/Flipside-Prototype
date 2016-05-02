/*
	Flips the game object around the center x and y position.

*/

using UnityEngine;
using System.Collections;

public class FlipMechanic : MonoBehaviour {
    public static Color previewColor = new Color(0.0f, 0.7f, 1.0f, 0.8f);
    public static Color errcolor = new Color(0.7f, 0.0f, 0.0f, 0.9f);
    public static Color basecolor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	public static float aniTime = 1.0f;
	public static float previewAniTime = 0.0f;
	public static int flipsideD;
    public static bool done;
	public static bool aniTimeReset;

	public GameObject preview { get; private set; }

	Vector3 previewStart;
	Vector3 previewStartRotation;
	public Vector3 previewGoal { get; private set; }
	Vector3 previewGoalRotation;
    int previewFlipside;
	public static int StaticPreviewFlipside { get; private set; }

	bool inSequence;    //Time for frozen animation
	int flipside;   //1 for horizontial, 2 for vertical
	int CG_flipside;
	Vector3 destination;
	static int lastFlipAttempt;

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
		CG_flipside = 0;
		inSequence = false;
		preview = new GameObject(transform.name + " (Preview)");
		preview.transform.parent = transform.parent;
		preview.transform.position = transform.position;
		preview.transform.localScale = transform.localScale;
        preview.layer = LayerMask.NameToLayer("WallPreview");
		preview.tag = "WallPreview";
		SpriteRenderer previewSprite = preview.AddComponent<SpriteRenderer>();
		previewSprite.sprite = GetComponent<SpriteRenderer>().sprite;
		previewSprite.material = GetComponent<SpriteRenderer>().material;
		previewSprite.color = Color.clear;
		flipside = 0;
        //added for red preview
        previewGoalTemp = preview;
        previewGoalTemp.AddComponent<PolygonCollider2D>();
        previewGoalTemp.GetComponent<PolygonCollider2D>().isTrigger = true;
        blinktime = blinkmax;
        xrot = 1;
        yrot = 1;
		lastFlipAttempt = 0;
	}

	void Flipside()
	{
		if (aniTime >= 0.5f)
			GetComponent<SpriteRenderer>().material.SetFloat("Flipside", CG_flipside % 4);
		if (flipside == 1)
		{
			transform.position = new Vector3(destination.x, transform.position.y, transform.position.z);
			transform.eulerAngles = new Vector3(180 + 180 * yrot, 180 + 180 * xrot, 0.0f);
			preview.transform.eulerAngles = transform.eulerAngles;
			preview.transform.position = transform.position;
			preview.transform.localScale = transform.localScale;
		}
		else if (flipside == 2)
		{
			transform.position = new Vector3(transform.position.x, destination.y, transform.position.z);
			transform.eulerAngles = new Vector3(180 + 180 * yrot, 180 + 180 * xrot, 0.0f);
			preview.transform.eulerAngles = transform.eulerAngles;
			preview.transform.position = transform.position;
			preview.transform.localScale = transform.localScale;
		}
		else if (flipside == 3)
		{
			transform.position = new Vector3(destination.x, destination.y, transform.position.z);
			transform.eulerAngles = new Vector3(180 + 180 * yrot, 180 + 180 * xrot, 0.0f);
			if (aniTime >= 0.5f)
			{
				preview.transform.position = new Vector3(destination.x, destination.y, transform.position.z);
				preview.transform.eulerAngles = new Vector3(180 + 180 * yrot, 180 + 180 * xrot, 0.0f);
				preview.transform.localScale = transform.localScale;
			} else
			{
				preview.transform.position = new Vector3(destination.x, -destination.y, transform.position.z);
				preview.transform.eulerAngles = new Vector3(180 * yrot, 180 * xrot, 0.0f);
				Vector3 previewScale = transform.localScale;
				previewScale.x *= -1;
				preview.transform.localScale = previewScale;
			}
		}
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
			Vector3 errorPosition = transform.localPosition;
			Vector3 errorAngle = transform.localEulerAngles;
			if (lastFlipAttempt >= 2)
			{
				errorPosition.y *= -1;
				errorAngle.x = (errorAngle.x + 180) % 360;
			}
			if (lastFlipAttempt % 2 == 1)
			{
				errorPosition.x *= -1;
				errorAngle.y = (errorAngle.y + 180) % 360;
			}
			errorPosition.z -= 0.001f;
			preview.transform.localPosition = errorPosition;
			preview.transform.localEulerAngles = errorAngle;
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
						GetComponent<SpriteRenderer>().material.SetFloat("Flipside", CG_flipside % 4);
						flipside = previewFlipside;
						if (!aniTimeReset)
						{
							if (flipsideD == previewFlipside)
							{
								FollowPlayer.reverse = true;
								aniTime = 1.0f - aniTime;
							}
							else
							{
								FollowPlayer.reverse = false;
								aniTime = 0;
							}
							aniTimeReset = true;
						}
						flipsideD = previewFlipside;
						if (flipside % 2 == 1)
						{
							if (CG_flipside % 2 == 0)
								CG_flipside++;
							else
								CG_flipside--;
						}
						if (flipside >= 2)
							CG_flipside += 2;
						previewFlipside = 0;
						destination = previewGoal;
						inSequence = true;
						xrot = (xrot + (flipside % 2)) %2;
                        yrot = (yrot + (flipside / 2)) % 2;
					}
                    else if(previewFlipside != 0 && PlayerController.dangerCheck)
					{
						lastFlipAttempt = previewFlipside;
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
					lastFlipAttempt = 1;
					blinktime = 0.0f;
                    done = true;
                }
                else if (PlayerController.axisButtonDownFlipX)
				{
					GetComponent<SpriteRenderer>().material.SetFloat("Flipside", CG_flipside % 4);
					if (CG_flipside % 2 == 0)
						CG_flipside++;
					else
						CG_flipside--;
					flipside = 1;
					if (!aniTimeReset)
					{
						if (flipsideD == 1)
						{
							FollowPlayer.reverse = !FollowPlayer.reverse;
							aniTime = 1.0f - aniTime;
						}
						else
						{
							FollowPlayer.reverse = false;
							aniTime = 0;
						}
						aniTimeReset = true;
					}
					flipsideD = flipside;
                    destination = new Vector3(-transform.position.x, previewGoal.y, previewGoal.z);
                    inSequence = true;
					done = false;
                    xrot = (xrot + 1) % 2;
                }
                if (PlayerController.ydanger && PlayerController.axisButtonDownFlipY)
				{
					lastFlipAttempt = 2;
					blinktime = 0.0f;
                    done = true;
                }
                else if (PlayerController.axisButtonDownFlipY)
				{
					GetComponent<SpriteRenderer>().material.SetFloat("Flipside", CG_flipside % 4);
					CG_flipside += 2;
					flipside = 2;
					if (!aniTimeReset)
					{
						if (flipsideD == 2)
						{
							FollowPlayer.reverse = !FollowPlayer.reverse;
							aniTime = 1.0f - aniTime;
						}
						else
						{
							FollowPlayer.reverse = false;
							aniTime = 0;
						}
						aniTimeReset = true;
					}
					flipsideD = flipside;
                    destination = new Vector3(previewGoal.x, -transform.position.y, previewGoal.z);
                    inSequence = true;
					done = false;
                    yrot = (yrot + 1) % 2;
                }
            }

		}
		if (inSequence)
		{
			previewSprite.color = basecolor;
			Color ghostColor = basecolor;
			ghostColor.a = FlipMechanic.aniTime * 0.2f;
			GetComponent<SpriteRenderer>().color = ghostColor;
			Flipside();
			if (aniTime >= 1.0f)
			{
				PlayerController.dangerCheck = false;
				inSequence = false;
				done = true;
			}
		}

	}
    
}
