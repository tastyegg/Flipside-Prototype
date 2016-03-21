using UnityEngine;
using System.Collections;

public class FlipMechanic : MonoBehaviour {
	public static Vector2 direction;
	public static float aniTime;
	GameObject preview;

	void Start () {
		direction = Vector2.right;
		aniTime = 0;
		preview = new GameObject(transform.name + " (Preview)");
		preview.transform.position = transform.position;
		preview.transform.localScale = transform.localScale;
		SpriteRenderer previewSprite = preview.AddComponent<SpriteRenderer>();
		previewSprite.sprite = GetComponent<SpriteRenderer>().sprite;
		previewSprite.color = Color.clear;
	}
	
	void Update () {
		if (Input.GetKeyUp(KeyCode.Space))
		{
			SpriteRenderer previewSprite = preview.GetComponent<SpriteRenderer>();
			previewSprite.color = Color.clear;
			if (direction.x != 0)
				transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
			else if (direction.y != 0)
				transform.position = new Vector3(transform.position.x, -transform.position.y, transform.position.z);
		} else if (Input.GetKey(KeyCode.Space))
		{
			SpriteRenderer previewSprite = preview.GetComponent<SpriteRenderer>();
			previewSprite.color = Color.blue;

			if (direction.x != 0)
			{
				preview.transform.position = new Vector3(Mathf.Lerp(transform.position.x, - transform.position.x, aniTime), transform.position.y, transform.position.z);
				preview.transform.eulerAngles = new Vector3(0.0f, Mathf.Lerp(0, 180, aniTime), 0.0f);
			}
			else if (direction.y != 0)
			{
				preview.transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, -transform.position.y, aniTime), transform.position.z);
				preview.transform.eulerAngles = new Vector3(Mathf.Lerp(0, 180, aniTime), 0.0f, 0.0f);
			}
        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            SpriteRenderer previewSprite = preview.GetComponent<SpriteRenderer>();
            previewSprite.color = Color.clear;
            if (direction.y != 0)
                transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
            else if (direction.x != 0)
                transform.position = new Vector3(transform.position.x, -transform.position.y, transform.position.z);
        }
        else if (Input.GetKey(KeyCode.LeftAlt))
        {
            SpriteRenderer previewSprite = preview.GetComponent<SpriteRenderer>();
            previewSprite.color = Color.blue;

            if (direction.y != 0)
            {
                preview.transform.position = new Vector3(Mathf.Lerp(transform.position.x, -transform.position.x, aniTime), transform.position.y, transform.position.z);
                preview.transform.eulerAngles = new Vector3(0.0f, Mathf.Lerp(0, 180, aniTime), 0.0f);
            }
            else if (direction.x != 0)
            {
                preview.transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, -transform.position.y, aniTime), transform.position.z);
                preview.transform.eulerAngles = new Vector3(Mathf.Lerp(0, 180, aniTime), 0.0f, 0.0f);
            }
        }
	}
}
