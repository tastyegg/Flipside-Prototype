using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
    public List<GameObject> buttons = new List<GameObject>();
    private int index;
    private int timer;

    public string firstLevelName;


    void Start()
    {
        timer = 15;
        index = 0;
    }

    void Update()
    {
        Text text = buttons[index].transform.GetChild(0).GetComponent<Text>();
        
        if (Input.GetButtonDown("Select")) {
            handleSelect(text.text);
        }

        if ((Input.GetAxis("Vertical") > 0.1f && timer > 14) || (Input.GetKeyDown(KeyCode.W) && timer > 14))
        {
            timer = 14;
            text.color = new Color(1.0f, 1.0f, 1.0f);
            selectUp();
        }
        else if ((Input.GetAxis("Vertical") < -0.1f && timer > 14) || (Input.GetKeyDown(KeyCode.S) && timer > 14))
        {
            timer = 14;
            text.color = new Color(1.0f, 1.0f, 1.0f);
            selectDown();
        }

        if (timer < 15 && timer > 0)
        {
            timer--;
        } else if (timer < 1)
        {
            timer = 15;
        }

        text = buttons[index].transform.GetChild(0).GetComponent<Text>();
        text.color = new Color(0.0784f, 0.9607f, 0.4157f);     
    }

    void selectUp()
    {
        if (buttons.Count > 1) {
            if (index != 0)
            {
                index--;
            }
            else // index == 0
            {
                index = buttons.Count - 1;
            }
        }
        else
        {
            index = 0;
        }
    }

    void selectDown()
    {
        if (buttons.Count > 1) {
            if (index != buttons.Count - 1)
            {
                index++;
            }
            else // index == last
            {
                index = 0;
            }
        }
        else
        {
            index = 0;
        }
    }

    void LoadScene(string level)
    {
        SceneManager.LoadScene(level);
    }

    void LoadScene(int level)
    {
        SceneManager.LoadScene(level);
    }

    void handleSelect(string data)
    {
        if (data.Equals("Start Game")) {
            //put level 1 here
            //LoadScene("test2");
            LoadScene(firstLevelName);
        } else if (data.Equals("Level Select")) {
            LoadScene("SelectionScreen");
        }
        else
        {
            //logic for other levels
        }
    }
}
