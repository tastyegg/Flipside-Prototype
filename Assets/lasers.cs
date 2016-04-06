﻿using UnityEngine;
using System.Collections;

public class lasers : MonoBehaviour {
    public int direction;
    public GameObject player;
    PlayerController pc;
    LineRenderer lr;
    RaycastHit2D rch2d;
    Vector2 dvec;
	// Use this for initialization
	void Start () {
        lr = gameObject.GetComponent<LineRenderer>();
        if (direction == 0)
        {
            dvec = Vector2.up;
        }
        else if (direction == 1)
        {
            dvec = Vector2.right;
        }
        else if(direction ==2){
            dvec = Vector2.down;
        }
        else
        {
            dvec = Vector2.left;
        }
        pc = player.GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
        lr.SetPosition(0, gameObject.transform.position);
        float x = gameObject.transform.position.x;
        float y = gameObject.transform.position.y;
        if (direction == 0)
        {
            rch2d = Physics2D.Raycast(transform.position, Vector2.up);
            y += 50;
        }
        else if (direction == 1)
        {
            rch2d = Physics2D.Raycast(transform.position, Vector2.right);
            x += 50;
        }
        else if (direction == 2)
        {
            rch2d = Physics2D.Raycast(transform.position, Vector2.down);
            y -= 50;
        }
        else
        {
            rch2d = Physics2D.Raycast(transform.position, Vector2.left);
            x -= 50;
        }
        lr.SetPosition(1, new Vector3(x, y, 2));

        
        if (rch2d.collider != null)
        {
            if (rch2d.collider.tag == "Player")
            {
                pc.Reset();
            }
            else
            {
                if (direction == 0 || direction == 2)
                {
                    y = rch2d.collider.transform.position.y;
                    Vector3 npos = new Vector3(transform.position.x, y, 1);
                    lr.SetPosition(1, npos);
                }
                else
                {
                    x = rch2d.collider.transform.position.x;
                    Vector3 npos = new Vector3(x, transform.position.y, 1);
                    lr.SetPosition(1, npos);
                }
            }
        }


	}
}