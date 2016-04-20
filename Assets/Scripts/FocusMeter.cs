using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FocusMeter : MonoBehaviour {
	Image bar;

	// Use this for initialization
	void Start () {
		bar = GetComponentInChildren<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		bar.fillAmount = PlayerController.focusReservior / PlayerController.FOCUS_TIMER;
	}
}
