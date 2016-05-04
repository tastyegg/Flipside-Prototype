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
    double ttime;
    bool stopped;
    Color gold = new Color((240.0f / 255.0f), 230.0f / 255.0f, 140.0f / 255.0f);
    Color silver = new Color(192.0f / 255.0f, 192.0f / 255.0f, 192.0f / 255.0f);
    Color bronze = new Color(205.0f / 255.0f, 127.0f / 255.0f, 50.0f / 255.0f);

    public void restart()
    {
        ttime = 0.0;
        BStar.GetComponent<Image>().fillAmount = 0.0f;
        TStar1.GetComponent<Image>().fillAmount = 1.0f;
        TStar2.GetComponent<Image>().fillAmount = 1.0f;
        TStar3.GetComponent<Image>().fillAmount = 1.0f;
        rank = 4;
        stopped = false;
    }

	// Use this for initialization
	void Start () {
        ttime = 0.0;
        BStar.GetComponent<Image>().fillAmount = 0.0f;
        TStar1.GetComponent<Image>().fillAmount = 1.0f;
        TStar2.GetComponent<Image>().fillAmount = 1.0f;
        TStar3.GetComponent<Image>().fillAmount = 1.0f;
        rank = 4;
        stopped = false;
	}

    public void bonusStar()
    {
        BStar.GetComponent<Image>().fillAmount = 1.0f;
        SpriteRenderer sprite = TStar1.GetComponent<SpriteRenderer>();
        TStar1.rectTransform.localPosition = TStar1.rectTransform.localPosition + (new Vector3(TStar1.rectTransform.localScale.x / 2, 0, 0));
        TStar2.rectTransform.localPosition = TStar2.rectTransform.localPosition + (new Vector3(TStar2.rectTransform.localScale.x / 2, 0, 0));
        TStar3.rectTransform.localPosition = TStar3.rectTransform.localPosition + (new Vector3(TStar3.rectTransform.localScale.x / 2, 0, 0));
    }

	// Update is called once per frame
	void Update () {
        if (stopped == false)
        {
            ttime += Time.deltaTime / Time.timeScale;
            double itime = (int)(ttime * 1000);
            double dtime = (double)itime / 1000;
            if (dtime <= gTime)
            {
                TStar1.GetComponent<Image>().fillAmount = 1 - (float)(dtime / gTime);
            }
            else if (dtime <= sTime)
            {
                TStar2.GetComponent<Image>().fillAmount = 1 - (float)((dtime - gTime) / (sTime - gTime));
            }
            else if (dtime <= bTime)
            {
                TStar3.GetComponent<Image>().fillAmount = 1 - (float)((dtime - sTime) / (bTime - sTime));
            }
        }
	}

    public double stop()
    {
        stopped = true;
        double x = ttime;
        double frac;
        

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
