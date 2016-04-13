﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    public float walkVelocity = 5.5f;
    public float jumpForce = 7.5f;

	Rigidbody2D playerRB;
    private Animator animator;
    private AudioSource audioPlayer;

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
    
    public AudioClip smokeAudio;
    public AudioClip spawnAudio;
    public AudioClip jumpAudio;

    // Use this for initialization
    void Start ()
	{
		playerRB = GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
        audioPlayer = this.GetComponent<AudioSource>();
        recordedPosition = transform.position;
        facingRight = true;
        StartCoroutine("Spawn");
    }
	
	public void Reset()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	// Update is called once per frame
	void Update ()
	{
		if (FlipMechanic.aniTime <= 1.0f)
			FlipMechanic.aniTime += 6.0f * Time.deltaTime / Time.timeScale;
		if (Input.GetButtonDown("Flip"))
		{
			Time.timeScale = 0.1f;
			Time.fixedDeltaTime = 0.02f * Time.timeScale;
		}
		if (Input.GetButtonUp("Flip"))
		{
			inSequence = true;
			recordedPosition = transform.position;
			recordedVelocity = playerRB.velocity;
			playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
			playerRB.isKinematic = true;
			
			Time.timeScale = 1.0f;
			Time.fixedDeltaTime = 0.02f * Time.timeScale;
		}
		if (inSequence && FlipMechanic.aniTime >= 1.0f && FlipMechanic.done)
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
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			SceneManager.LoadScene(0);
		}

		dangerCheck = false;
		foreach (GameObject g in FindObjectsOfType<GameObject>())
		{
			FlipMechanic f = g.GetComponent<FlipMechanic>();
			if (f && f.preview)
			{
				if ((new Rect(f.previewGoal - g.transform.localScale * 0.5f, g.transform.localScale)).Contains(transform.position))
				{
                    dangerCheck = true;
				}
			}
		}

		foreach (GameObject g in FindObjectsOfType<GameObject>())
		{
			FlipMechanic f = g.GetComponent<FlipMechanic>();
			if (f && f.preview)
			{
                if (dangerCheck)
					f.preview.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.0f, 0.0f, 0.9f);
				else
					f.preview.GetComponent<SpriteRenderer>().color = FlipMechanic.previewColor;
			}
		}
	}

    void FixedUpdate()
    {
        if (jumpTimer <= 2.0f) jumpTimer += 0.1f;
        
        grounded = Physics2D.OverlapArea(groundCheckLeft.position, groundCheckRight.position, whatIsGround);
        if (!animator.GetBool("jumping") != grounded)
            animator.SetBool("jumping", !grounded);

        if (!Input.GetButton("Flip"))
        {
            animator.SetBool("walking", false);
            playerRB.velocity = new Vector2(0, playerRB.velocity.y);
            if (!playerRB.isKinematic)
            {
                if (Input.GetAxis("Horizontal") < 0)
                {
                    if (facingRight) ChangeDirection();
                    animator.SetBool("walking", true);
                    playerRB.velocity = new Vector2(walkVelocity * Input.GetAxis("Horizontal"), playerRB.velocity.y);
                }
                else if (Input.GetAxis("Horizontal") > 0)
                {
                    if (!facingRight) ChangeDirection();
                    animator.SetBool("walking", true);
                    playerRB.velocity = new Vector2(walkVelocity * Input.GetAxis("Horizontal"), playerRB.velocity.y);
                }
                if ((Input.GetAxis("Vertical") > 0.25f || Input.GetButton("Jump")) && grounded && jumpTimer > 2.0f)
                {
                    jumpTimer = 0.0f;
                    audioPlayer.PlayOneShot(jumpAudio);
                    animator.SetBool("jumping", true);
                    playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
                }
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
		if (collider.CompareTag("Finish") && GetComponent<SpriteRenderer>().color.a != 0)
		{
            Color tmp = GetComponent<SpriteRenderer>().color;
            tmp.a = 0;
            GetComponent<SpriteRenderer>().color = tmp;
            collider.GetComponent<SpriteRenderer>().color = tmp;
            audioPlayer.PlayOneShot(smokeAudio);
            GetComponent<ParticleSystem>().Emit(500);
			Invoke("LoadNextLevel", 2.1f);
            double tRank = GameObject.Find("Text").GetComponent<Text>().GetComponent<Timer>().stop();
        }
        /*else if (collider.gameObject.CompareTag("Portal"))
        {
            //run teleport
        }*/
        
	}

	void OnBecameInvisible()
	{
        if (Camera.allCamerasCount > 0)
        {
            Vector3 posPixel = Camera.allCameras[0].WorldToScreenPoint(transform.position);
            if (posPixel.y > 0 && posPixel.y < Camera.allCameras[0].pixelHeight)
            {
                Vector3 pos = transform.position;
                pos.x = pos.x * (-0.98f);
                transform.position = pos;
            }
            else if (posPixel.y < 0)
                Invoke("Reset", 0.7f);
        }
        else
            Invoke("Reset", 0.7f);
	}

    void ChangeDirection()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    IEnumerator Spawn()
    {
        Color tmp = GetComponent<SpriteRenderer>().color;
        tmp.a = 0;
        GetComponent<SpriteRenderer>().color = tmp;
        transform.Find("Spawn Particle System").GetComponent<ParticleSystem>().Emit(50);
        audioPlayer.PlayOneShot(spawnAudio);
        yield return new WaitForSeconds(0.3f);
        tmp.a = 1.0f;
        GetComponent<SpriteRenderer>().color = tmp;
    }
}
