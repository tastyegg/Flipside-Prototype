using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour {
    public Text title;
    public Text backButton;
    public Image img;
    //public List<GameObject> buttons = new List<GameObject>();
    public List<string> levels = new List<string>();
    public List<Sprite> imgs = new List<Sprite>(); //how to laod with path? 
    public GameObject button;
    public GameObject back;
    //prob a picture

    private int index;
    private bool navIdx;
    private int timer;
    private int timer2;

    void Start()
    {
        timer = 15;
        index = 0;
        navIdx = true;
    }

    void Update()
    {
        //Text text = buttons[index].transform.GetChild(0).GetComponent<Text>();
        
        if (Input.GetButtonDown("Select"))
        {
            if (navIdx)
            {
                handleSelect(index+1);
            }
            else
            {
                handleSelect(0);
            }
            
        } 

        if ((Input.GetAxis("Horizontal") > 0.1f && timer > 14 || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && navIdx)
        {
            timer = 14;
            selectRight();
        }
        else if ((Input.GetAxis("Horizontal") < -0.1f && timer > 14 || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && navIdx)
        {
            timer = 14;
            selectLeft();
        }

        if ((Input.GetAxis("Vertical") > 0.1f && timer2 > 14) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            timer2 = 14;
            navIdx = !navIdx;
        }
        else if ((Input.GetAxis("Vertical") < -0.1f && timer2 > 14) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            timer2 = 14;
            navIdx = !navIdx;
        }

        //for movement feel
        if (timer < 15 && timer > 0)
        {
            timer--;
        }
        else if (timer < 1)
        {
            timer = 15;
        }

        if (timer2 < 15 && timer2 > 0)
        {
            timer2--;
        }
        else if (timer2 < 1)
        {
            timer2 = 15;
        }

        if (navIdx)
        {
            title.color = new Color(0.0784f, 0.9607f, 0.4157f);
            backButton.color = new Color(1.0f, 1.0f, 1.0f);
        }
        else
        {
            backButton.color = new Color(0.0784f, 0.9607f, 0.4157f);
            title.color = new Color(1.0f, 1.0f, 1.0f);
        }

        //title.text = levels[index];
        title.text = "Level " + (index + 1);
        img.sprite = imgs[index];
        //text = buttons[index].transform.GetChild(0).GetComponent<Text>();
        //text.color = new Color(0.0784f, 0.9607f, 0.4157f);
    }

    void selectLeft()
    {
        if (levels.Count > 1)
        {
            if (index != 0)
            {
                index--;
            }
            else // index == 0
            {
                index = levels.Count - 1;
            }
        }
        else
        {
            index = 0;
        }
    }

    void selectRight()
    {
        if (levels.Count > 1)
        {
            if (index != levels.Count - 1)
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

    void LoadScene(int level)
    {
        SceneManager.LoadScene(level);
    }

    void handleSelect(int data)
    {
        LoadScene(data);
    }

}
