using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.IO;
using System.Collections;

public class SceneSelector : MonoBehaviour {
	Canvas c;
	Text t;
	Image levelThumbnail, loadingThumbnail;

	Button shiftButton, leftButton, rightButton;

	int sceneIndex, maxScenes;
	int flip = 0;
	
	void Awake() {
		c = GetComponent<Canvas>();
		t = c.GetComponentInChildren<Button>().GetComponentInChildren<Text>();
		Button[] buttons = c.GetComponentsInChildren<Button>();
		shiftButton = buttons[3];
		leftButton = buttons[4];
		rightButton = buttons[5];
		leftButton.gameObject.SetActive(false);
		rightButton.gameObject.SetActive(false);

		Image[] images = buttons[0].GetComponentsInChildren<Image>();
		levelThumbnail = images[1];
		loadingThumbnail = images[2];

		sceneIndex = 1;
		UpdateSceneSelection();
	}

	void FlipChildren(Transform t)
	{
		for (int i = 0; i < t.childCount; i++)
		{
			FlipPosition(t.GetChild(i));
		}
	}

	void FlipPosition(Transform t)
	{
		Vector3 v = t.position;
		t.position = new Vector3(-v.x, v.y, v.z);
	}

	void Update()
	{
		leftButton.gameObject.GetComponent<Image>().color = Color.white;
		rightButton.gameObject.GetComponent<Image>().color = Color.white;
		if (Input.GetButtonDown("Focus"))
		{
			shiftButton.gameObject.GetComponent<Image>().color = Color.cyan;
			RevealButton(true);
		} else if (Input.GetButtonUp("Focus"))
		{
			shiftButton.gameObject.GetComponent<Image>().color = Color.white;
			RevealButton(false);
		}
		if (Input.GetButtonDown("Horizontal") && Input.GetAxis("Horizontal") < 0)
		{
			leftButton.gameObject.GetComponent<Image>().color = Color.cyan;
		}
		if (Input.GetButtonDown("Horizontal") && Input.GetAxis("Horizontal") > 0)
		{
			rightButton.gameObject.GetComponent<Image>().color = Color.cyan;
		}
		if (!Input.GetButtonDown("Focus") && Input.GetButtonDown("Horizontal") && Input.GetAxis("Horizontal") < 0)
		{
			PreviousLevel();
		} else if (!Input.GetButtonDown("Focus") && Input.GetButtonDown("Horizontal") && Input.GetAxis("Horizontal") > 0)
		{
			NextLevel();
		}
		if (Input.GetButtonDown("Submit"))
		{
			LoadLevel();
		}
		if (levelThumbnail.sprite != null)
		{
			loadingThumbnail.color = Color.Lerp(loadingThumbnail.color, Color.clear, 0.1f);
		}

		if (Input.GetKey(KeyCode.LeftShift))
		{
			if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
			{
				flip++;
			}
		}
		if (Input.GetKeyUp(KeyCode.LeftShift) && flip % 2 == 1)
		{
			flip = 0;
			FlipChildren(transform);
		}
	}

	void UpdateSceneSelection()
	{
		if (sceneIndex < EditorBuildSettings.scenes.Length)
		{
			string scenePath = EditorBuildSettings.scenes[sceneIndex].path;
			scenePath = scenePath.Substring(scenePath.LastIndexOf('/') + 1);
			t.text = scenePath.Split('.')[0];

			string imagePath = EditorBuildSettings.scenes[sceneIndex].path;
			imagePath = imagePath.Split('.')[0] + ".png";
			levelThumbnail.sprite = LoadSprite(imagePath);
			loadingThumbnail.color = Color.black;
		}
	}

	Sprite LoadSprite(string path)
	{
		try {
			byte[] data = File.ReadAllBytes(path);
			Texture2D texture = new Texture2D(64, 64, TextureFormat.ARGB32, false);
			texture.LoadImage(data);
			texture.name = Path.GetFileNameWithoutExtension(path);
			return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
		} catch (FileNotFoundException)
		{
			return new Sprite();
		}
	}

	public void RevealButton(bool value)
	{
		leftButton.gameObject.SetActive(value);
		rightButton.gameObject.SetActive(value);
	}

	public void PreviousLevel()
	{
		sceneIndex--;
		if (sceneIndex < 1)
		{
			sceneIndex = EditorBuildSettings.scenes.Length - 1;
		}
		UpdateSceneSelection();
	}

	public void NextLevel()
	{
		sceneIndex++;
		if (sceneIndex >= EditorBuildSettings.scenes.Length)
		{
			sceneIndex = 1;
		}
		UpdateSceneSelection();
	}

	public void LoadLevel()
	{
		SceneManager.LoadScene(sceneIndex);
	}
}
