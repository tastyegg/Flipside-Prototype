using UnityEngine;
using System.Collections;

public class testGrouded : MonoBehaviour {
    public GameObject player;

	// Use this for initialization
	void Start () {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.color = Color.black;
	}
	
	// Update is called once per frame
	void Update () {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (player.GetComponent<PlayerController>().grounded)
        {
            sprite.color = Color.black;
        }
        else
        {
            sprite.color = Color.blue;
        }
	}
}
