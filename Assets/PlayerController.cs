using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	Rigidbody2D playerRB;

	bool grounded;
    public Transform groundCheck;
    float groundRadius = 0.1f;
    public LayerMask whatIsGround;
    float jumpTimer = 0.0f;

	// Use this for initialization
	void Start () {
		playerRB = GetComponent<Rigidbody2D>();
		grounded = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
        if (Input.GetKeyDown(KeyCode.Space))
			FlipMechanic.aniTime = 0.0f;
		else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) ||
			Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
			if (FlipMechanic.direction.x == 0)
				FlipMechanic.aniTime = 0.0f;
		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
		{
			if (FlipMechanic.direction.y == 0)
				FlipMechanic.aniTime = 0.0f;
			FlipMechanic.direction = Vector2.up;
		} else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
		{
			if (FlipMechanic.direction.y == 0)
				FlipMechanic.aniTime = 0.0f;
			FlipMechanic.direction = Vector2.down;
		}
		FlipMechanic.aniTime += 0.11f;
	}

    void FixedUpdate()
    {
        if (jumpTimer <= 1.0f) jumpTimer += 0.1f;
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        playerRB.velocity = new Vector2(0, playerRB.velocity.y);
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            FlipMechanic.direction = Vector2.left;
            playerRB.velocity = new Vector2(-6.0f, playerRB.velocity.y);
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            FlipMechanic.direction = Vector2.right;
            playerRB.velocity = new Vector2(6.0f, playerRB.velocity.y);
        }
        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && grounded && jumpTimer > 1.0f)
        {
            jumpTimer = 0.0f;
            playerRB.velocity = new Vector2(playerRB.velocity.x, 6.8f);
        }
    }
}
