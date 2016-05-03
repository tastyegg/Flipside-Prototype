using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuController : MonoBehaviour {
    public List<GameObject> buttons = new List<GameObject>();
    private int index;
    private int timer;

    void Start()
    {
        timer = 100;
        index = 0;
    }

    void Update()
    {
        if (Input.GetAxis("Vertical") > 0.0 && timer > 99)
        {
            timer = 99;
            selectUp();
        } else if (Input.GetAxis("Vertical") < 0.0 && timer > 99)
        {
            timer = 99;
            selectDown();
        }

        if (timer < 100 && timer > 0)
        {
            timer--;
        } else if (timer < 1)
        {
            timer = 100;
        }

        //buttons[index].transform.Find("Text");
        //get the component
    }

    void selectUp()
    {
        if (index != 0 && buttons.Count > 1)
        {
            index--;
        } else if (buttons.Count > 1 && index == 0) //dont really need 2nd conditional
        {
            index = buttons.Count - 1;
        }
        //else index = 0
    }

    void selectDown()
    {
        if (index != buttons.Count - 1 && buttons.Count > 1)
        {
            index++;
        } else if (buttons.Count > 1 && index == buttons.Count - 1)
        {
            index = 0;
        }
    }
}
