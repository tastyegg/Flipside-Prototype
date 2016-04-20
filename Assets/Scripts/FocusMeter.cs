using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FocusMeter : MonoBehaviour {
	Image bar;
	Color barColor;

	// Use this for initialization
	void Start () {
		bar = GetComponentInChildren<Image>();
		barColor = bar.color;
	}
	
	// Update is called once per frame
	void Update () {
		bar.fillAmount = PlayerController.focusReservior / PlayerController.FOCUS_TIMER;
		bar.color = Color.Lerp(Color.white, barColor, Mathf.Pow(bar.fillAmount, 0.5f));
	}
}
