using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Focus : MonoBehaviour {
    public float FOCUS_TIMER = 10.0f;
    public float rateOfDecay = 1.0f;

    public Text textBox;

    bool dropFocus;
    float focusTimer;

	// Use this for initialization
	void Start () {
        focusTimer = FOCUS_TIMER;
        textBox.text = "text";
        textBox.color = Color.green;
	}
	
	// Update is called once per frame
	void Update () {
        textBox.text = "" + focusTimer.ToString("0.00");

	    if (dropFocus) {
            focusTimer -= rateOfDecay;
        }

        //remove
        //if (focusTimer < 0)
        //{
        //    resetFocus();
        //}
	}

    public void startFocus()
    {
        dropFocus = true;
    }

    public void resetFocus()
    {
        //end focus mode
        dropFocus = false;
        focusTimer = FOCUS_TIMER;
    }

    public float getFocus() { return focusTimer; }
}
