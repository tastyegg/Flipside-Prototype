using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class bestTime : MonoBehaviour {
    public GameObject time;
    public Text tbox;
    Timer stagedata;
    string savekey;
    double dtime;
    Color gold = new Color((240.0f / 255.0f), 230.0f / 255.0f, 140.0f / 255.0f);
    Color silver = new Color(192.0f / 255.0f, 192.0f / 255.0f, 192.0f / 255.0f);
    Color bronze = new Color(205.0f / 255.0f, 127.0f / 255.0f, 50.0f / 255.0f);
	// Use this for initialization
	void Start () {
        stagedata = time.GetComponent<Timer>();
        dtime = 999.999;
        savekey = "btlevel" + stagedata.levelnum;
        if (PlayerPrefs.HasKey(savekey))
        {
            dtime = PlayerPrefs.GetFloat(savekey);
        }
        
        if (dtime < stagedata.gTime)
        {
            tbox.color = gold;
        }
        else if (dtime < stagedata.sTime)
        {
            tbox.color = silver;
        }
        else if (dtime < stagedata.bTime)
        {
            tbox.color = bronze;
        }
        else
        {
            tbox.color = Color.white;
        }
        tbox.text = "" + Mathf.Round((float)dtime * 1000.0f) / 1000.0f; ;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
