using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	public float animationSpeed = 11.0f;
	public float previewAnimationSpeed = 5.0f;
	public float blinkSpeed = 6.0f;
    public float walkVelocity = 5.5f;
    public float jumpForce = 7.5f;
    public float FOCUS_TIMER = 10.0f;
    public float rateOfDecay = 1.0f;

	Rigidbody2D playerRB;
    private Animator animator;
    private AudioSource audioPlayer;

	bool grounded;
    bool facingRight;
    public Transform groundCheckLeft;
    public Transform groundCheckRight;
    public LayerMask whatIsGround;
    float jumpTimer = 2.0f;
    float jumpHoldTimer = 0.0f;
    bool jumpHoldCanceled = true;
	Vector3 recordedPosition;
	Vector2 recordedVelocity;
    public static bool enteringFocus;
    public static bool inFocus;
    public static bool exitingFocus;

    public static bool axisButtonDownFlipX;
    public static bool axisButtonFlipX;
    public static bool axisButtonUpFlipX;

    public static bool axisButtonDownFlipY;
    public static bool axisButtonFlipY;
    public static bool axisButtonUpFlipY;

    static bool axisButtonDownJump;
    static bool axisButtonJump;
    static bool axisButtonUpJump;
           
    public AudioClip smokeAudio;
    public AudioClip spawnAudio;
    public AudioClip jumpAudio;

    //more audio
    public AudioClip walkingAudio;
    public AudioClip focusAudio;
    public AudioClip bgm; //notreally used

	float walkingTimer;
    float focusTimer;
    bool dropFocus;

    //for axis check
    public bool axisX;
    public bool axisY;
   
    public float acceleration_speed;
    public float velocity_cap;
    public float stopping_power;
    public float immediate_stop_cutoff;
    public float air_stopping_power;
    public float jump_hold_max;
    public float jump_hold_min;
    public float run_factor;

    private Vector3 spawnPosition;
    private bool isDead;

	ParticleSystem dustSystem;
	FlipMechanic flip;

    // Use this for initialization
    void Start ()
	{
		flip = GetComponent<FlipMechanic>();

        spawnPosition = transform.position;
        playerRB = GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
        audioPlayer = this.GetComponent<AudioSource>();
        recordedPosition = transform.position;
        facingRight = true;
        StartCoroutine(Spawn(0.0f));
        focusTimer = FOCUS_TIMER;
        dropFocus = false;
        axisX = false;
        axisY = false;
		dustSystem = GetComponentsInChildren<ParticleSystem>()[3];
    }
	
	public void Reset()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	// Update is called once per frame
	void Update ()
	{
		walkingTimer += 8.0f * Time.deltaTime / Time.timeScale;
        convertAxisToButton(Input.GetAxis("Focus"), ref enteringFocus, ref inFocus, ref exitingFocus);
        convertAxisToButton(Input.GetAxis("Jump"), ref axisButtonDownJump, ref axisButtonJump, ref axisButtonUpJump);
        convertAxisToButton(Input.GetAxis("FlipX"), ref axisButtonDownFlipX, ref axisButtonFlipX, ref axisButtonUpFlipX);
        convertAxisToButton(Input.GetAxis("FlipY"), ref axisButtonDownFlipY, ref axisButtonFlipY, ref axisButtonUpFlipY);

        animator.SetBool("walking", false);

        //player move left
        if (Input.GetAxis("Horizontal") < 0 && (playerRB.velocity.x <= 1.0f || !grounded))
        {
            if (facingRight && playerRB.velocity.x <= 0.0f) ChangeDirection();
            animator.SetBool("walking", true);
			if (walkingTimer >= 1.0f && grounded)
			{
				audioPlayer.Play();
				walkingTimer = 0;
			}
            playerRB.AddForce(new Vector2(Input.GetAxis("Horizontal") * acceleration_speed, 0.0f));

            //if (walkSoundTimer > WALKTIME-1 && grounded)
            //{
            //    audioPlayer.volume = 2.0f;
            //    audioPlayer.PlayOneShot(walkingAudio);
            //    walkFlag = true;
            //    audioPlayer.volume = 1.0f;
            //}
        }
        //player move right
        else if (Input.GetAxis("Horizontal") > 0 && (playerRB.velocity.x >= -1.0f || !grounded))
        {
            if (!facingRight && playerRB.velocity.x >= 0.0f) ChangeDirection();
            animator.SetBool("walking", true);
            
			if (walkingTimer >= 1.0f && grounded)
			{
				audioPlayer.Play();
				walkingTimer = 0;
			}
			playerRB.AddForce(new Vector2(Input.GetAxis("Horizontal") * acceleration_speed, 0.0f));
        }
        else
		{
			audioPlayer.Stop();

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
       
        //jump
        if (Input.GetButton("Jump") || axisButtonJump)
        {
            jumpHoldTimer += Time.fixedDeltaTime;
            //hold down jump to jump higher
            if (jumpHoldTimer < jump_hold_max && !jumpHoldCanceled)
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
            }
            //initial press
            if ((Input.GetButtonDown("Jump") || axisButtonDownJump) && grounded && jumpTimer > 0.2f)
            {
                jumpTimer = 0.0f;
                jumpHoldCanceled = false;
                audioPlayer.PlayOneShot(jumpAudio);
                //audioPlayer.volume = 0.5f;
                animator.SetBool("jumping", true);
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
            }

            if (Input.GetButtonUp("Jump") || axisButtonUpJump)
            {
                jumpHoldCanceled = true;
            }
        }
        else
        {
            if (playerRB.velocity.y > 0.0f && jumpTimer > jump_hold_min)
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, 0.0f);
            }
            jumpHoldTimer = 0.0f;
            jumpHoldCanceled = true;
        }
		

		if (Input.GetButtonDown("Exit"))
		{
			//SceneManager.LoadScene(0);
            LoadScene("TitleScreen"); //temporary
		} else if (Input.GetButtonDown("Reset"))
		{
			Reset();
		}

        if (inFocus)
		{
			Time.timeScale = 0.1f;
			Time.fixedDeltaTime = 0.02f * Time.timeScale;
			audioPlayer.pitch = Time.timeScale * 1.0f;
        } else
		{
			Time.timeScale = 1.0f;
			Time.fixedDeltaTime = 0.02f * Time.timeScale;
			audioPlayer.pitch = 1.0f;
        }
    }

    void FixedUpdate()
    {
        //cap velocity
        float cur_velocity_cap = velocity_cap;

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
		Color dc = dustSystem.startColor;
		dc.a = grounded || jumpTimer <= 0.11f ? 0.3f : 0;
		dustSystem.startColor = dc;

		if (!animator.GetBool("jumping") != grounded)
            animator.SetBool("jumping", !grounded);

        //Sync walk animation with velocity
        animator.SetFloat("movementSpeed", Mathf.Clamp(Mathf.Abs(playerRB.velocity.x)/2.0f, 0.0f, 1.5f));
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
            playerRB.isKinematic = true;
            Color tmp = GetComponent<SpriteRenderer>().color;
            tmp.a = 0;
            GetComponent<SpriteRenderer>().color = tmp;
            collider.GetComponent<SpriteRenderer>().color = tmp;
            audioPlayer.PlayOneShot(smokeAudio);
            GetComponent<ParticleSystem>().Emit(500);
			GetComponentsInChildren<ParticleSystem>()[2].gameObject.SetActive(false);
			GetComponentsInChildren<ParticleSystem>()[2].gameObject.SetActive(false);
			Invoke("LoadNextLevel", 2.1f);
            if (GameObject.Find("StarBox"))
            {
                GameObject.Find("StarBox").GetComponent<Timer>().stop();
            }
        }
        if (collider.CompareTag("Star"))
        {
            Color tmp = GetComponent<SpriteRenderer>().color;
            tmp.a = 0;
            collider.GetComponent<SpriteRenderer>().color = tmp;
            //GetComponentsInChildren<ParticleSystem>()[3].gameObject.SetActive(false);
            GameObject.Find("StarBox").GetComponent<Timer>().bonusStar();
            Destroy(collider.gameObject);
        }
	}


	void OnBecameInvisible()
	{
        if (Camera.allCamerasCount > 0)
        {
            Vector3 posPixel = Camera.allCameras[0].WorldToScreenPoint(transform.position);
            if (!(posPixel.y > 0 && posPixel.y < Camera.allCameras[0].pixelHeight) || !(posPixel.x > 0 && posPixel.x < Camera.allCameras[0].pixelHeight))
                Die();
        }
        else
            Die();
	}

    void ChangeDirection()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void Die()
	{
        if (!isDead)
        {
            isDead = true;
            GameObject starbox = GameObject.Find("StarBox");
            if (starbox)
                starbox.GetComponent<Timer>().restart();
            Color tmp = GetComponent<SpriteRenderer>().color;
            tmp.a = 0;
            GetComponent<SpriteRenderer>().color = tmp;
            audioPlayer.PlayOneShot(smokeAudio);
            GetComponent<ParticleSystem>().Emit(20);

			flip.Reset();

            if (gameObject.activeSelf) StartCoroutine(Spawn(1.5f));
        }
    }

    IEnumerator Spawn(float delayStart)
    {
        transform.Find("Mirage Particle System").GetComponent<ParticleSystem>().Stop();
        transform.Find("Dust Particle System").GetComponent<ParticleSystem>().Stop();
        yield return new WaitForSeconds(delayStart);
        isDead = false;
        transform.position = spawnPosition;
        transform.Find("Mirage Particle System").GetComponent<ParticleSystem>().Play();
        transform.Find("Dust Particle System").GetComponent<ParticleSystem>().Play();
        playerRB.velocity = new Vector2(0.0f, 0.0f);
        Color tmp = GetComponent<SpriteRenderer>().color;
        tmp.a = 0;
        GetComponent<SpriteRenderer>().color = tmp;
        transform.Find("Spawn Particle System").GetComponent<ParticleSystem>().Emit(50);
        audioPlayer.PlayOneShot(spawnAudio);
        yield return new WaitForSeconds(0.3f);
        tmp.a = 1.0f;
        GetComponent<SpriteRenderer>().color = tmp;
    }

    void convertAxisToButton(float axisValue, ref bool buttonDown, ref bool button, ref bool buttonUp)
    {
        buttonDown = false;
        buttonUp = false;
        if (axisValue != 0)
        {

            if (!button)
            {
                buttonDown = true;
                button = true;
            }
        }
        else
        {
            if (button)
            {
                buttonUp = true;
                button = false;
            }
        }
    }

    public void LoadScene(string level)
    {
        SceneManager.LoadScene(level);
    }
}
