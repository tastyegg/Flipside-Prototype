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
    float levelSelectAxis;

    void Awake() {
		c = GetComponent<Canvas>();
		t = c.GetComponentInChildren<Button>().GetComponentInChildren<Text>();
		Button[] buttons = c.GetComponentsInChildren<Button>();
		shiftButton = buttons[3];
		leftButton = buttons[4];
		rightButton = buttons[5];

		Image[] images = buttons[0].GetComponentsInChildren<Image>();
		levelThumbnail = images[1];
		loadingThumbnail = images[2];

        levelSelectAxis = 0;
		sceneIndex = 1;
		UpdateSceneSelection();
	}

	void Update()
    {
        float levelSelectAxisCurrent = Input.GetAxis("SelectLevel");

        leftButton.gameObject.GetComponent<Image>().color = Color.white;
		rightButton.gameObject.GetComponent<Image>().color = Color.white;
		if (PlayerController.inFocus)
		{
			shiftButton.gameObject.GetComponent<Image>().color = Color.cyan;
		} else if (PlayerController.exitingFocus)
		{
			shiftButton.gameObject.GetComponent<Image>().color = Color.white;
		}
		if (PlayerController.axisButtonFlipY)
		{
			leftButton.gameObject.GetComponent<Image>().color = Color.cyan;
		}
		if (PlayerController.axisButtonFlipX)
		{
			rightButton.gameObject.GetComponent<Image>().color = Color.cyan;
		}
		if (levelSelectAxis != levelSelectAxisCurrent && levelSelectAxisCurrent < 0)
		{
			PreviousLevel();
		} else if (levelSelectAxis != levelSelectAxisCurrent && levelSelectAxisCurrent > 0)
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
		
		if (Input.GetButtonDown("FlipX") || Input.GetButtonDown("FlipY"))
		{
			flip++;
		}
		if (flip % 2 == 1)
		{
			GameObject g = GameObject.Find("Panel");
			Transform t = g.transform;
			t.localScale = new Vector3(-t.localScale.x, t.localScale.y, t.localScale.z);
			flip--;
		}
		if (flip == 2)
		{
			GameObject g = GameObject.Find("Panel");
			Transform t = g.transform;
			t.localScale = new Vector3(t.localScale.x, -t.localScale.y, t.localScale.z);
			flip = 0;
		}
		levelSelectAxis = levelSelectAxisCurrent;
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
