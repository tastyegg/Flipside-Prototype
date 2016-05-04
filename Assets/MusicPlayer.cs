﻿using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {
    int musicind;
    public AudioSource player;
	// Use this for initialization
	void Start () {
        musicind = 0;
	}
	
	// Update is called once per frame
	void Update () {
        GameObject g = GameObject.Find("MusicBox");
        if (g)
        {
            MusicBox mb = g.GetComponent<MusicBox>();
            if (mb.musicind != musicind)
            {
                player.Stop();
                player.clip = mb.bgm.clip;
                musicind = mb.musicind;
                player.Play();
            }
        }
	}
}