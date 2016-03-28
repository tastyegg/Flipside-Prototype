using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour {
	Rigidbody2D playerRB;
	
	bool inSequence;    //Time for frozen animation
	bool grounded;
    public Transform groundCheckLeft;
    public Transform groundCheckRight;
    public LayerMask whatIsGround;
    float jumpTimer = 0.0f;
	Vector3 recordedPosition;
	Vector2 recordedVelocity;

	// Use this for initialization
	void Start ()
	{
		playerRB = GetComponent<Rigidbody2D>();
		recordedPosition = transform.position;
	}
	
	void Reset()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	// Update is called once per frame
	void Update ()
	{
		if (FlipMechanic.aniTime <= 1.0f)
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
				Time.timeScale = 0.05f;
				Time.fixedDeltaTime = 0.02f * Time.timeScale;
				//recordedVelocity = playerRB.velocity;
				//playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
			}
		} else if (!inSequence && !playerRB.isKinematic && Input.GetKeyUp(KeyCode.LeftShift))
		{
			Time.timeScale = 1.0f;
			Time.fixedDeltaTime = 0.02f * Time.timeScale;
			//playerRB.velocity = recordedVelocity;
			//playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
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
        //grounded = checkGround();
       
        grounded = Physics2D.OverlapArea(groundCheckLeft.position, groundCheckRight.position, whatIsGround);
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

	bool checkGround()
	{
		SpriteRenderer sprite = GetComponent<SpriteRenderer>();
		RaycastHit2D leftRay = Physics2D.Raycast(transform.position - new Vector3(sprite.bounds.size.x, sprite.bounds.size.y) * 0.5f, Vector2.down);
		RaycastHit2D rightRay = Physics2D.Raycast(transform.position - new Vector3(-sprite.bounds.size.x, sprite.bounds.size.y) * 0.5f, Vector2.down);

		return leftRay.collider != null || rightRay.collider != null;
		//return Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
	}

	void OnCollisionStay2D(Collision2D collision)
	{
		//grounded = checkGround();
		if (collision.collider.bounds.Contains(recordedPosition))
		{
			//Kill the player
			GetComponent<SpriteRenderer>().enabled = false;

			//Invoke("Reset", 1.0f);	//This automatically executes with OnBecameInvisible()
			//GetComponent<ParticleSystem>().Emit(200);
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		//grounded = false;
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
			Invoke("LoadNextLevel", 2.1f);
        }
        /*else if (collider.gameObject.CompareTag("Portal"))
        {
            //run teleport
        }*/
        
	}

	void OnBecameInvisible()
	{
		GetComponent<ParticleSystem>().Emit(200);
		Invoke("Reset", 1.4f);
	}
}
