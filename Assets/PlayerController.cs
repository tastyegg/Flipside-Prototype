using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	Rigidbody2D playerRB;
	bool grounded;

	// Use this for initialization
	void Start () {
		playerRB = GetComponent<Rigidbody2D>();
		grounded = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
		{
			playerRB.AddForce(new Vector2(-6.5f - playerRB.velocity.x, 0.0f));
			//playerRB.velocity = new Vector2(-2.0f, playerRB.velocity.y);
		} else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			playerRB.AddForce(new Vector2( 6.5f - playerRB.velocity.x, 0.0f));
			//playerRB.velocity = new Vector2( 2.0f, playerRB.velocity.y);
		}
		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
		{
			if (grounded)
			{
				grounded = false;
				playerRB.velocity = new Vector2(playerRB.velocity.x, 6.8f);
			}
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		Collider2D collider = collision.collider;
		if (collider.CompareTag("Wall"))
		{
			if (collision.contacts.Length > 0)
			{
				ContactPoint2D contact = collision.contacts[0];
				if (Vector2.Dot(contact.normal, Vector2.up) > 0.5f) //Check ground collision
				{
					grounded = true;
				}
			}
		}
	}
}
