using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour {
	public double bTime;
	public double sTime;
	public double gTime;
	public int levelnum;
	int rank;
	public Image TStar1;
	public Image TStar2;
	public Image TStar3;
	public Image BStar;
	float ttime;
	float tinterval = 15;
	bool stopped;

	public void restart()
	{
		ttime = 0.0f;
		rank = 4;
		stopped = false;
	}

	// Use this for initialization
	void Start () {
		ttime = 0.0f;
		rank = 4;
		stopped = false;
	}

	public void bonusStar()
	{
		ttime -= tinterval;
	}

	// Update is called once per frame
	void Update () {
		if (stopped == false)
		{
			ttime += Time.deltaTime / Time.timeScale;

			TStar1.GetComponent<Image>().fillAmount = 1 - (ttime / tinterval);
			TStar2.GetComponent<Image>().fillAmount = 1 - ((ttime - tinterval) / tinterval);
			TStar3.GetComponent<Image>().fillAmount = 1 - ((ttime - tinterval * 2) / tinterval);
			BStar.GetComponent<Image>().fillAmount = 1 - ((ttime + tinterval) / tinterval);
		}
	}

	public double stop()
	{
		stopped = true;
		double x = ttime;

		if (PlayerPrefs.HasKey("btlevel" + levelnum) && x < PlayerPrefs.GetFloat("btlevel" + levelnum))
		{
			PlayerPrefs.SetFloat("btlevel" + levelnum, (float)x);
			PlayerPrefs.Save();
		}
		else
		{
			PlayerPrefs.SetFloat("btlevel" + levelnum, (float)x);
			PlayerPrefs.Save();
		}

		return (double) rank;
	}

}
