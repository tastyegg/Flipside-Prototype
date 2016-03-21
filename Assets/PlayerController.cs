using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour {

	Rigidbody2D playerRB;
	Vector3 respawnLocation;

	bool inSequence;    //Time for frozen animation
	bool grounded;
    public Transform groundCheck;
	public LayerMask whatIsGround;
	float groundRadius = 0.1f;
    float jumpTimer = 0.0f;
	Vector2 recordedVelocity;

	// Use this for initialization
	void Start ()
	{
		inSequence = false;
		playerRB = GetComponent<Rigidbody2D>();
		grounded = true;
		respawnLocation = transform.position;
	}
	
	void Reset()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	// Update is called once per frame
	void Update ()
	{
		FlipMechanic.aniTime += 0.11f;
		if (!inSequence && (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.LeftShift)))
		{
			FlipMechanic.aniTime = 0.0f;
			if (!Input.GetKey(KeyCode.LeftShift))
			{
				inSequence = true;
				recordedVelocity = playerRB.velocity;
				playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
			}
		}
		if (inSequence && FlipMechanic.aniTime >= 1.0f)
		{
			inSequence = false;
			playerRB.velocity = recordedVelocity;
			playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			Reset();
		}
	}

    void FixedUpdate()
    {
        if (jumpTimer <= 1.0f) jumpTimer += 0.1f;
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        playerRB.velocity = new Vector2(0, playerRB.velocity.y);
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            playerRB.velocity = new Vector2(-6.0f, playerRB.velocity.y);
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            playerRB.velocity = new Vector2(6.0f, playerRB.velocity.y);
        }
        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && grounded && jumpTimer > 1.0f)
        {
            jumpTimer = 0.0f;
            playerRB.velocity = new Vector2(playerRB.velocity.x, 6.8f);
        }
    }

	void OnBecameInvisible()
	{
		Reset();
	}
}
