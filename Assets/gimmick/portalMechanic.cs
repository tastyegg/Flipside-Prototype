using UnityEngine;
using System.Collections;

public class portalMechanic : MonoBehaviour {
    public GameObject otherEnd; // other end of the protal
    public GameObject player; //links to the player
    int timer;
    bool activated;
    //bool flip;

    
	// Use this for initialization
	void Start () {
        timer = 0;
        activated = true;
        //flip = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if (timer > 0) {
            timer--;
            if (timer == 0)
            {
                activated = true;
            }
        }

        if (otherEnd.GetComponent<FlipMechanic>().getSeq())
        {
            activated = false;
            timer = 30;
            //flip = true;
        }
        /*else if (flip)
        {
            flip = false;
            activated = true;
        }*/
	}


    void teleport()
    {
        portalMechanic script = otherEnd.GetComponent<portalMechanic>();
        script.activated = false;
        script.timer = 60;
        player.transform.position = otherEnd.transform.position;
        //animation?
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (activated && collider.gameObject.CompareTag("Player")) {
            teleport();
        }
        //if wwe have bullets, enemies, or other objects in the future add them here
    }
}
