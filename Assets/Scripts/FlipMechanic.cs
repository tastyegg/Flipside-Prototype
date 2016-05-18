using UnityEngine;
using System.Collections.Generic;

public class FlipMechanic : MonoBehaviour {
	public Color baseColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	public Color previewColor = new Color(0.3f, 0.3f, 0.8f, 0.4f);
	public Color errorColor = new Color(0.8f, 0.2f, 0.2f, 0.8f);
	public float aniTime { get; private set; }
	public float aniTimePreview { get; private set; }
	public float animationSpeed = 3.0f;
	public float preivewAnimationSpeed = 3.0f;
	float errorTime;
	public int flipside { get; private set; }
	public int directionFlip { get; private set; }
	public int flipsidePreview { get; private set; }
	public int directionFlipPreview { get; private set; }
	int flipsideBefore;

	int[,] directionTable = new int[4, 4] {
		{0, 1, 2, 3},
		{1, 0 ,3, 2},
		{2, 3, 0, 1},
		{3, 2, 1, 0} };

	PlayerController player;
	List<Flippable> blocks;

	// Use this for initialization
	void Start () {
		aniTime = 1.0f;
		aniTimePreview = 1.0f;
		errorTime = 1.0f;
		flipside = 0;
		blocks = new List<Flippable>();

		blocks.AddRange(FindObjectsOfType<Flippable>());
		player = FindObjectOfType<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (aniTime < 1.0f)
		{
			aniTime += animationSpeed * Time.deltaTime / Time.timeScale;
			if (aniTime > 1.0f)
			{
				aniTime = 1.0f;
				ChangeBaseColor(baseColor);
				ChangePreviewColor(Color.clear);
			}
			else
			{
				Color ghostColor = baseColor;
				ghostColor.a = Mathf.Lerp(0.2f, 0.3f, aniTime);
				ChangeBaseColor(ghostColor);
				ChangePreviewColor(baseColor);
			}
		}
		if (aniTimePreview < 1.0f)
		{
			aniTimePreview += preivewAnimationSpeed * Time.deltaTime / Time.timeScale;
			if (aniTimePreview > 1.0f)
				aniTimePreview = 1.0f;
		}
		if (errorTime < 1.0f)
		{
			errorTime += animationSpeed * Time.deltaTime / Time.timeScale;
			if (errorTime > 1.0f)
			{
				ChangePreviewColor(Color.clear);
				errorTime = 1.0f;
			}
			else if (!Input.GetButton("Focus"))
			{
				ChangePreviewColor(errorColor);
			}
		}

		flipsideBefore = flipside;
		ActivatePreview();
		if (!Input.GetButton("Focus"))
		{
			ActivateFlip();
		}
	}

	void ActivatePreview()
	{
		if (Input.GetButtonDown("Focus"))
		{
			errorTime = 1.0f;
			flipsidePreview = flipside;
			directionFlipPreview = 0;
		}
		if (Input.GetButton("Focus"))
		{
			if (Input.GetButtonDown("FlipX"))
			{
				aniTimePreview = 0.0f;
				flipsidePreview = (flipsidePreview + 1) % 2 + (flipsidePreview / 2) * 2;
				directionFlipPreview = 1;
			}
			if (Input.GetButtonDown("FlipY"))
			{
				aniTimePreview = 0.0f;
				flipsidePreview = (flipsidePreview + 2) % 4;
				directionFlipPreview = 2;
			}
			FlipPreviewBlocks();
			if (CheckPlayerCollisionOnPreview())
			{
				ChangePreviewColor(errorColor);
			} else
			{
				ChangePreviewColor(previewColor);
			}
		}
		else if (Input.GetButtonUp("Focus"))
		{
			ChangePreviewColor(Color.clear);
			flipside = flipsidePreview;
			FlipBlocks();
		}
	}

	bool CheckPlayerCollisionOnPreview()
	{
		foreach (Flippable f in blocks)
		{
			Collider2D col = f.previewObject.GetComponent<Collider2D>();
			if (col && col.OverlapPoint(player.transform.localPosition))
			{
				return true;
			}
		}
		return false;
	}

	void FlipPreviewBlocks()
	{
		foreach (Flippable f in blocks)
		{
			if (f)
			{
				Vector3 newPosition = f.originalPosition;
				Vector3 newRotation = f.originalRotation;
				Vector3 newScale = f.originalScale;
				if (flipsidePreview % 2 == 1)
				{
					newPosition.x = -newPosition.x;
					newRotation.z = 360 - newRotation.z;
					newScale.x = -newScale.x;
				}
				if ((flipsidePreview / 2) % 2 == 1)
				{
					newPosition.y = -newPosition.y;
					newRotation.z = 360 - newRotation.z;
					newScale.y = -newScale.y;
				}
				f.previewObject.transform.position = newPosition;
				f.previewObject.transform.localEulerAngles = newRotation;
				f.previewObject.transform.localScale = newScale;
			}
		}
	}

	void ActivateFlip()
	{
		if (Input.GetButtonDown("FlipX"))
		{
			flipside = (flipside + 1) % 2 + (flipside / 2) * 2;
			FlipBlocks();
		}
		if (Input.GetButtonDown("FlipY"))
		{
			flipside = (flipside + 2) % 4;
			FlipBlocks();
		}
	}

	void FlipBlocks()
	{
		if (flipside != flipsideBefore)
		{
			flipsidePreview = flipside;
			FlipPreviewBlocks();
			if (!CheckPlayerCollisionOnPreview())
			{
				directionFlip = directionTable[flipsideBefore, flipside];
				foreach (Flippable f in blocks)
				{
					if (f)
					{
						Vector3 newPosition = f.originalPosition;
						Vector3 newRotation = f.originalRotation;
						Vector3 newScale = f.originalScale;
						if (flipside % 2 == 1)
						{
							newPosition.x = -newPosition.x;
							newRotation.z = 360 - newRotation.z;
							newScale.x = -newScale.x;
						}
						if ((flipside / 2) % 2 == 1)
						{
							newPosition.y = -newPosition.y;
							newRotation.z = 360 - newRotation.z;
							newScale.y = -newScale.y;
						}
						f.transform.position = newPosition;
						f.transform.localEulerAngles = newRotation;
						f.transform.localScale = newScale;
					}
				}
				ChangePreviewColor(Color.clear);
				aniTime = 0.0f;
				errorTime = 1.0f;
				aniTimePreview = 1.0f;
			}
			else
			{
				flipside = flipsideBefore;
				errorTime = 0.0f;
				aniTimePreview = 1.0f;
			}
		}
	}

	void ChangeBaseColor(Color c)
	{
		foreach (Flippable f in blocks)
		{
			SpriteRenderer sr = f.GetComponent<SpriteRenderer>();
			sr.color = c;
		}
	}

	void ChangePreviewColor(Color c)
	{
		foreach (Flippable f in blocks)
		{
			SpriteRenderer sr = f.previewObject.GetComponent<SpriteRenderer>();
			sr.color = c;
		}
	}

	public void Reset()
	{
		flipside = 0;
		FlipBlocks();
	}
}
