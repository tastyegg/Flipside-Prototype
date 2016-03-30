using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnCannonball : MonoBehaviour {
	List<GameObject> cannonballs;
	float spawnTimer;
	public bool shootsLeft = true;
	public bool noFlipBall = false;
	public bool noFlipBallVelocity = false;
	public bool shootsKillerBalls = true;
	public bool shootsReflectiveBalls = true;
//	public bool shootsReverse = false;	//Affected by the FlipMechanic

	// Use this for initialization
	void Start () {
		spawnTimer = 0;
		cannonballs = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		if (FlipMechanic.aniTime >= 1.0f)
			spawnTimer += Time.deltaTime;
		if (spawnTimer > 0.9f)
		{
			GameObject cannonball = new GameObject("Cannonball");
			cannonball.tag = "Cannonball";
			cannonballs.Add(cannonball);
			Rigidbody2D cannonballRB = cannonball.AddComponent<Rigidbody2D>();
			SpriteRenderer cannonballSprite = cannonball.AddComponent<SpriteRenderer>();
			cannonballSprite.sprite = GetComponent<SpriteRenderer>().sprite;
			cannonball.transform.localScale *= 0.5f;
			cannonball.transform.position = transform.position + new Vector3((shootsLeft ? -1 : 1) * transform.localScale.x, 0, 0);
			cannonballRB.velocity = (!shootsLeft ? -1 : 1) * 4 * Vector3.left;
			cannonballRB.gravityScale = 0;
			Cannonball cb = cannonball.AddComponent<Cannonball>();
			cb.killsPlayer = shootsKillerBalls;
			cb.reflectOnWall = shootsReflectiveBalls;
			if (!noFlipBall)
				cannonball.AddComponent<FlipMechanic>();
			if (!noFlipBallVelocity)
				cannonball.AddComponent<FlipMechanicDynamic>().reverseVelocity = true;

			spawnTimer = 0;
		}
		cannonballs.RemoveAll(item => item == null);
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.CompareTag("Cannonball") && !collision.collider.GetComponent<Rigidbody2D>().isKinematic && collision.collider.GetComponent<Cannonball>().killsPlayer)
		{
			Destroy(gameObject);
		}
	}

	void OnBecameInvisible()
	{
		Destroy(gameObject);
	}
}
