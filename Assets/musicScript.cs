using UnityEngine;
using System.Collections;

public class musicScript : MonoBehaviour {
    public AudioClip[] soundtrack;
    public float slowSpeed = 0.1f;
    private AudioSource audioPlayer;
    // Use this for initialization
    void Start () {
        audioPlayer = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!audioPlayer.isPlaying)
        {
            audioPlayer.clip = soundtrack[Random.Range(0, soundtrack.Length)];
            audioPlayer.Play();
        }

        if (PlayerController.inFocus)
        {
            Time.timeScale = slowSpeed;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            //audioPlayer.PlayOneShot(focusAudio);
            audioPlayer.pitch = Time.timeScale * 5.0f;
        }
        if (!PlayerController.inFocus && FlipMechanic.aniTime < 1.0f)
        {
            Time.timeScale = Mathf.Clamp(FlipMechanic.aniTime * 2, 0.0001f, 1.0f);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            audioPlayer.pitch = Time.timeScale * 2.0f;
        }
        if (FlipMechanic.aniTime >= 1.0f && !PlayerController.inFocus || PlayerController.exitingFocus || (!PlayerController.inFocus && (PlayerController.axisButtonDownFlipX || PlayerController.axisButtonDownFlipY) && FlipMechanic.blinktime >= FlipMechanic.blinkmax))
        {
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            //audioPlayer.Stop();
            audioPlayer.pitch = Time.timeScale;
        }
    }

}
