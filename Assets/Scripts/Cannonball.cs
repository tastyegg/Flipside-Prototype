﻿using UnityEngine;
using System.Collections;

public class Cannonball : MonoBehaviour {
	public bool killsPlayer;
	public bool reflectOnWall;
	Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		gameObject.AddComponent<CircleCollider2D>();
		rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter2D(Collision2D collision)
	{
		if (reflectOnWall && collision.collider.CompareTag("Wall")) {
			rb.velocity = new Vector2(-rb.velocity.x, -rb.velocity.y);
		}
		else {
			if (gameObject.GetComponent<FlipMechanic>())
				Destroy(gameObject.GetComponent<FlipMechanic>().preview);
			Destroy(gameObject);
		}
	}

	void OnBecameInvisible()
	{
		if (gameObject.GetComponent<FlipMechanic>())
			Destroy(gameObject.GetComponent<FlipMechanic>().preview);
		Destroy(gameObject);
	}
}
