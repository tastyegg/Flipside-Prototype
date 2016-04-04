﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour {
	Rigidbody2D playerRB;
    private Animator animator;

    bool inSequence;	//Time for frozen animation
	bool grounded;
    bool facingRight;
	public static bool dangerCheck;
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
        animator = this.GetComponent<Animator>();
        recordedPosition = transform.position;
        facingRight = true;
	}
	
	void Reset()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	// Update is called once per frame
	void Update ()
	{
		if (FlipMechanic.aniTime <= 1.0f)
			FlipMechanic.aniTime += 7.0f * Time.deltaTime / Time.timeScale;
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			Time.timeScale = 0.0000001f;
			Time.fixedDeltaTime = 0.02f * Time.timeScale;
		}
		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			inSequence = true;
			recordedPosition = transform.position;
			recordedVelocity = playerRB.velocity;
			playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
			playerRB.isKinematic = true;
			FlipMechanic.aniTime = 0.0f;

			Time.timeScale = 1.0f;
			Time.fixedDeltaTime = 0.02f * Time.timeScale;
		}
		if (inSequence && FlipMechanic.aniTime >= 1.0f)
		{
			inSequence = false;
			transform.position = recordedPosition;
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
       
        grounded = Physics2D.OverlapArea(groundCheckLeft.position, groundCheckRight.position, whatIsGround);
        if (!animator.GetBool("jumping") != grounded)
            animator.SetBool("jumping", !grounded);

        animator.SetBool("walking", false);
        playerRB.velocity = new Vector2(0, playerRB.velocity.y);
		if (!playerRB.isKinematic)
		{
			if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
			{
                if (facingRight) changeDirection();
                animator.SetBool("walking", true);
				playerRB.velocity = new Vector2(-5.5f, playerRB.velocity.y);
			}
			else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
			{
                if (!facingRight) changeDirection();
                animator.SetBool("walking", true);
                playerRB.velocity = new Vector2(5.5f, playerRB.velocity.y);
			}
			if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && grounded && jumpTimer > 1.0f)
			{
				jumpTimer = 0.0f;
                animator.SetBool("jumping", true);
                playerRB.velocity = new Vector2(playerRB.velocity.x, 5.7f);
			}
		}
    }
	
	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.CompareTag("Cannonball") && !collision.collider.GetComponent<Rigidbody2D>().isKinematic && collision.collider.GetComponent<Cannonball>().killsPlayer)
		{
			GetComponent<SpriteRenderer>().enabled = false; //This automatically executes OnBecameInvisible()
			Destroy(collision.collider.gameObject);
        }
        if (collision.collider.bounds.Contains(recordedPosition))
        {
            dangerCheck = true;
            transform.position = recordedPosition;
            inSequence = true;
            playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
            playerRB.isKinematic = true;
            FlipMechanic.aniTime = 0.0f;
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
            Color tmp = GetComponent<SpriteRenderer>().color;
            tmp.a = 0;
            GetComponent<SpriteRenderer>().color = tmp;
            collider.GetComponent<SpriteRenderer>().color = tmp;
            GetComponent<ParticleSystem>().Emit(500);
			Invoke("LoadNextLevel", 2.1f);
        }
        /*else if (collider.gameObject.CompareTag("Portal"))
        {
            //run teleport
        }*/
        
	}

	void OnBecameInvisible()
	{
		//GetComponent<ParticleSystem>().Emit(200);
		Invoke("Reset", 1.4f);
	}

    void changeDirection()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
