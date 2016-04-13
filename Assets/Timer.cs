using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour {
    public double bTime;
    public double sTime;
    public double gTime;
    public int levelnum;
    int rank;
    public Text tbox;
    double ttime;
    bool stopped;
    Color gold = new Color((240.0f / 255.0f), 230.0f / 255.0f, 140.0f / 255.0f);
    Color silver = new Color(192.0f / 255.0f, 192.0f / 255.0f, 192.0f / 255.0f);
    Color bronze = new Color(205.0f / 255.0f, 127.0f / 255.0f, 50.0f / 255.0f);

	// Use this for initialization
	void Start () {
        ttime = 0.0;
        tbox.color = gold;
        rank = 4;
        stopped = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (stopped == false)
        {
            ttime += Time.deltaTime;
            double itime = (int)(ttime * 1000);
            double dtime = (double)itime / 1000;
            tbox.text = "" + dtime;
            if (dtime > gTime && rank == 4)
            {
                tbox.color = silver;
                rank = 23;
            }
            else if (dtime > sTime && rank == 3)
            {
                tbox.color = bronze;
                rank = 2;
            }
            else if (dtime > bTime && rank == 2)
            {
                tbox.color = Color.white;
                rank = 1;
            }
        }
	}

    public double stop()
    {
        stopped = true;
        double x = ttime;
        double frac;
        if (rank == 4)
        {
            frac = 1 - (x / gTime);
        }
        else if (rank == 3)
        {
            frac = 1 - ((x - gTime) / (sTime - gTime));
        }
        else if (rank == 2)
        {
            frac = 1 - ((x - sTime) / (bTime - sTime));
        }
        else
        {
            frac = 0;
        }

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

        return (double) rank + frac;
    }

}
