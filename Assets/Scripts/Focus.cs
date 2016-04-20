using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Focus : MonoBehaviour {
    public static float FOCUS_TIMER = 10.0f;
    public float rateOfDecay = 1.0f;
    public float increase = 3.0f;

    public Text textBox;

    bool dropFocus;
	public static float focusTimer { get; private set; }

	// Use this for initialization
	void Start () {
        focusTimer = FOCUS_TIMER;
        textBox.text = "text";
        textBox.color = Color.green;
        dropFocus = false;
	}
	
	// Update is called once per frame
	void Update () {
        textBox.text = "" + focusTimer.ToString("0.00");

	    if (dropFocus) {
            focusTimer -= rateOfDecay;
        } else if (focusTimer < FOCUS_TIMER && !dropFocus) {
            focusTimer = Mathf.Clamp(focusTimer + (rateOfDecay / increase), 0.0f, FOCUS_TIMER);
        }

	}

    public void startFocus()
    {
        dropFocus = true;
    }

    public void resetFocus()
    {
        //end focus mode
        dropFocus = false;
        //focusTimer = FOCUS_TIMER;
    }

    public float getFocus() { return focusTimer; }
}
