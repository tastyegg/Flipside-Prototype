using UnityEngine;
using System.Collections;

public class RandomColorPicker : MonoBehaviour {

	// Use this for initialization
	void Awake ()
	{
		ChangeTheDamnColor(0);
		ChangeTheDamnColor(1);
		ChangeTheDamnColor(2);
		ChangeTheDamnColor(3);
	}

	void Update()
	{
		if (Input.GetButtonDown("Change It!"))
		{
			ChangeTheDamnColor(0);
			ChangeTheDamnColor(1);
			ChangeTheDamnColor(2);
			ChangeTheDamnColor(3);
		}
	}

	void ChangeTheDamnColor(int type)
	{
		switch (type) {
			case 0:
				GameObject[] go = GameObject.FindGameObjectsWithTag("Wall");
				Color wallColor = new Color(Random.value, Random.value, Random.value);
				for (int i = 0; i < go.Length; i++)
				{
					go[i].GetComponent<SpriteRenderer>().color = wallColor;
					go[i].GetComponent<FlipMechanic>().basecolor = wallColor;
				}
				break;
			case 1:
				FlipMechanic.previewColor = new Color(Random.value, Random.value, Random.value);
				break;
			case 2:
				FlipMechanic.errcolor = new Color(Random.value, Random.value, Random.value);
				break;
			case 3:
				Color cameraColor = new Color(Random.value, Random.value, Random.value);
				foreach (Camera c in Camera.allCameras)
				{
					c.backgroundColor = cameraColor;
				}
				break;
		}
	}
}
