using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour {
	Rigidbody2D playerRB;
	
	bool inSequence;    //Time for frozen animation
	bool grounded;
    public Transform groundCheck;
	public LayerMask whatIsGround;
	float groundRadius = 0.1f;
    float jumpTimer = 0.0f;
	Vector3 recordedPosition;
	Vector2 recordedVelocity;

	// Use this for initialization
	void Start ()
	{
		inSequence = false;
		playerRB = GetComponent<Rigidbody2D>();
		grounded = true;
		recordedPosition = transform.position;
	}
	
	void Reset()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	// Update is called once per frame
	void Update ()
	{
		FlipMechanic.aniTime += 0.11f;
		if (!inSequence && !playerRB.isKinematic && (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.LeftShift)))
		{
			FlipMechanic.aniTime = 0.0f;
			if (!Input.GetKey(KeyCode.LeftShift))
			{
				inSequence = true;
				recordedPosition = transform.position;
				recordedVelocity = playerRB.velocity;
				playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
				playerRB.isKinematic = true;
			} else if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				recordedVelocity = playerRB.velocity;
				playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
			}
		} else if (!inSequence && !playerRB.isKinematic && Input.GetKeyUp(KeyCode.LeftShift))
		{
			playerRB.velocity = recordedVelocity;
			playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
		}
		if (inSequence && FlipMechanic.aniTime >= 1.0f)
		{
			inSequence = false;
			playerRB.velocity = recordedVelocity;
			playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
			playerRB.isKinematic = false;
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
		if (!playerRB.isKinematic)
		{
			if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
			{
				playerRB.velocity = new Vector2(-4.0f, playerRB.velocity.y);
			}
			else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
			{
				playerRB.velocity = new Vector2(4.0f, playerRB.velocity.y);
			}
			if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && grounded && jumpTimer > 1.0f)
			{
				jumpTimer = 0.0f;
				playerRB.velocity = new Vector2(playerRB.velocity.x, 6.6f);
			}
		}
    }

	void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.collider.bounds.Contains(recordedPosition))
		{
			GetComponent<ParticleSystem>().Emit(200);
			GetComponent<SpriteRenderer>().enabled = false;
			//Invoke("Reset", 1.0f);	//This automatically executes with OnBecameInvisible()
		}
	}

	void LoadNextLevel()
	{
		if (SceneManager.sceneCountInBuildSettings > SceneManager.GetActiveScene().buildIndex + 1)
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		else
			SceneManager.LoadScene(0);
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.CompareTag("Finish"))
		{
			GetComponent<ParticleSystem>().Emit(200);
			Invoke("LoadNextLevel", 1.2f);
		}
	}

	void OnBecameInvisible()
	{
		Invoke("Reset", 1.0f);
	}
}
