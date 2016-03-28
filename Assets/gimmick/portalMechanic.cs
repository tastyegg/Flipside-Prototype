using UnityEngine;
using System.Collections;

public class portalMechanic : MonoBehaviour {
    public GameObject otherEnd; // other end of the protal
    public GameObject player; //links to the player
    int timer;
    bool activated;
    
	// Use this for initialization
	void Start () {
        timer = 0;
        activated = true;
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
	}

    void teleport()
    {
        portalMechanic script = otherEnd.GetComponent<portalMechanic>();
        script.activated = false;
        script.timer = 100;
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
