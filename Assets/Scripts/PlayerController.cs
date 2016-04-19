using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    public float walkVelocity = 5.5f;
    public float jumpForce = 7.5f;
    //public float FOCUS_TIMER = 10.0f;
    //public float rateOfDecay = 1.0f;

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
    float jumpTimer = 2.0f;
    float jumpHoldTimer = 0.0f;
    bool jumpHoldCanceled = true;
	Vector3 recordedPosition;
	Vector2 recordedVelocity;
    bool running;
    public static bool enteringFocus;
    public static bool inFocus;
    public static bool exitingFocus;
           
    public AudioClip smokeAudio;
    public AudioClip spawnAudio;
    public AudioClip jumpAudio;

    //float focusTimer;
    //bool dropFocus;

    //for axis check
    public bool axisX;
    public bool axisY;
   

    public float acceleration_speed;
    public float velocity_cap;
    public float stopping_power;
    public float immediate_stop_cutoff;
    public float air_stopping_power;
    public float jump_hold_max;
    public float run_factor;

    public GameObject focusBox;
    Focus fScript;
    public bool focusAllow; //refactor code later maybe
    public bool held = false;

    // Use this for initialization
    void Start ()
	{
		playerRB = GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
        audioPlayer = this.GetComponent<AudioSource>();
        recordedPosition = transform.position;
        facingRight = true;
        StartCoroutine("Spawn");
        //focusTimer = FOCUS_TIMER;
        //dropFocus = false;
        axisX = false;
        axisY = false;
        fScript = focusBox.GetComponent<Focus>();
        focusAllow = true;
    }
	
	public void Reset()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	// Update is called once per frame
	void Update ()
	{
        //axis check
        //if (Input.GetAxis("FlipAxisX") > 0.2f)
        //{
        //    axisX = true;
        //}
        //else if (Input.GetAxis("FlipAxisY") > 0.2f)
        //{
        //    axisY = true;
        //}

        //if (Input.GetAxis("FlipAxisX") < 0.2f && Input.GetAxis("FlipAxisX") > -0.2f)
        //{
        //    axisX = false;
        //}

        //if (Input.GetAxis("FlipAxisY") < 0.2f && Input.GetAxis("FlipAxisY") > -0.2f)
        //{
        //    axisY = false;
        //}

        animator.SetBool("walking", false);
        //playerRB.velocity = new Vector2(0, playerRB.velocity.y);
        if (!playerRB.isKinematic)
        {
            //player move left
            if (Input.GetAxis("Horizontal") < 0 && (playerRB.velocity.x <= 0.0f || !grounded))
            {
                if (facingRight && playerRB.velocity.x <= 0.0f) ChangeDirection();
                animator.SetBool("walking", true);
                playerRB.AddForce(new Vector2(Input.GetAxis("Horizontal") * acceleration_speed, 0.0f));
            }
            //player move right
            else if (Input.GetAxis("Horizontal") > 0 && (playerRB.velocity.x >= 0.0f || !grounded))
            {
                if (!facingRight && playerRB.velocity.x >= 0.0f) ChangeDirection();
                animator.SetBool("walking", true);
                playerRB.AddForce(new Vector2(Input.GetAxis("Horizontal") * acceleration_speed, 0.0f));
            }
            else
            {
                //ground friction
                if (grounded)
                {
                    //player stops immediately once velocity is low enough
                    if (playerRB.velocity.x > immediate_stop_cutoff || playerRB.velocity.x < -immediate_stop_cutoff)
                    {
                        playerRB.velocity = new Vector2(playerRB.velocity.x / stopping_power, playerRB.velocity.y);
                    }
                    else
                    {
                        playerRB.velocity = new Vector2(0.0f, playerRB.velocity.y);
                    }
                }
                //air friction
                else
                {
                    playerRB.velocity = new Vector2(playerRB.velocity.x / air_stopping_power, playerRB.velocity.y);
                }
            }
        }

        //jump
        if (Input.GetButton("Jump"))
        {
            jumpHoldTimer += Time.fixedDeltaTime;
            //hold down jump to jump higher
            if (jumpHoldTimer < jump_hold_max && !jumpHoldCanceled)
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
            }
            //initial press
            if (Input.GetButtonDown("Jump") && grounded && jumpTimer > 0.2f)
            {
                jumpTimer = 0.0f;
                jumpHoldCanceled = false;
                audioPlayer.PlayOneShot(jumpAudio);
                animator.SetBool("jumping", true);
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
            }

            if (Input.GetButtonUp("Jump"))
            {
                jumpHoldCanceled = true;
            }
        }
        else
        {
            if (playerRB.velocity.y > 0.0f && jumpTimer > 0.15f)
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, 0.0f);
            }
            jumpHoldTimer = 0.0f;
            jumpHoldCanceled = true;
        }
        //run
        if (Input.GetAxis("Run") > 0.0f)
        {
            running = true;
        }
        else
        {
            running = false;
        }
        exitingFocus = false;
        enteringFocus = false;
        if (Input.GetAxis("Focus") > 0)
        {
            if (!inFocus)
            {
                enteringFocus = true;
                inFocus = true;
            }
        }
        else
        {
            if (inFocus)
            {
                exitingFocus = true;
                inFocus = false;
            }
        }

        if (FlipMechanic.aniTime <= 1.0f)
			FlipMechanic.aniTime += 6.0f * Time.deltaTime / Time.timeScale;
		if (enteringFocus)
		{
			Time.timeScale = 0.2f;
			Time.fixedDeltaTime = 0.02f * Time.timeScale;
            //startFocus();
            fScript.startFocus();
		}
        /*
        if (fScript.getFocus() <= 0)
        {
            exitingFocus = true;
        }*/
        /*if (Input.GetButtonUp("Focus"))
        {
            held = false;
        }*/
		if (exitingFocus || (!inFocus && (Input.GetButtonDown("FlipX") || Input.GetButtonDown("FlipY"))))
		{
			inSequence = true;
			recordedPosition = transform.position;
			recordedVelocity = playerRB.velocity;
			playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
			playerRB.isKinematic = true;
			
			Time.timeScale = 1.0f;
			Time.fixedDeltaTime = 0.02f * Time.timeScale;
            //resetFocus();
            fScript.resetFocus();
		}
		if (inSequence && FlipMechanic.aniTime >= 1.0f && FlipMechanic.done)
		{
			inSequence = false;
			transform.position = recordedPosition;
			playerRB.velocity = recordedVelocity;
			playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
			playerRB.isKinematic = false;
            FlipMechanic.done = false;
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
        //for focus
        //if (dropFocus)
        //{
        //    focusTimer -= rateOfDecay;
        //}
        if (fScript.getFocus() < 0.0f)
        {
            focusAllow = false;
            fScript.resetFocus();
        }

        if (Input.GetButtonUp("Focus") && ! focusAllow)
        {
            focusAllow = true;
        }
    }

    //for the focus mode
    //void startFocus()
    //{
    //    dropFocus = true;
    //}

    //void resetFocus()
    //{
    //    dropFocus = false;
    //    focusTimer = 10.0f;
    //}

    //public float getFocus() { return focusTimer; }
    
    void FixedUpdate()
    {
        //cap velocity
        float cur_velocity_cap = velocity_cap;
        if (running) cur_velocity_cap *= run_factor;

        if (playerRB.velocity.x >= cur_velocity_cap)
        {
            playerRB.velocity = new Vector2(cur_velocity_cap, playerRB.velocity.y);
        }
        else if (playerRB.velocity.x <= -cur_velocity_cap)
        {
            playerRB.velocity = new Vector2(-cur_velocity_cap, playerRB.velocity.y);
        }

        if (jumpTimer <= 2.0f) jumpTimer += Time.fixedDeltaTime;
        
        grounded = Physics2D.OverlapArea(groundCheckLeft.position, groundCheckRight.position, whatIsGround);
        if (!animator.GetBool("jumping") != grounded)
            animator.SetBool("jumping", !grounded);

        //Sync walk animation with velocity
        animator.SetFloat("movementSpeed", Mathf.Clamp(Mathf.Abs(playerRB.velocity.x)/2.0f, 0.0f, 1.5f));
        
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
