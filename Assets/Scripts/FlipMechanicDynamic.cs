/*
	This script is for dynamic components during the use of the FlipMechanic script.
	Before the FlipMechanic animation, it stores the velocity and disables collision.
	After the FlipMechanic animation, it restores the velocity and re-enables collision.
*/

using UnityEngine;
using System.Collections;

public class FlipMechanicDynamic : MonoBehaviour
{
	Rigidbody2D rb;
	bool inSequence;    //Time for frozen animation
	public bool reverseVelocity;
	Vector3 recordedPosition;
	Vector2 recordedVelocity;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!inSequence && !rb.isKinematic && (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyUp(KeyCode.LeftShift)))
		{
			if (!Input.GetKey(KeyCode.LeftShift))
			{
				inSequence = true;
				recordedPosition = transform.position;
				recordedVelocity = rb.velocity;
				if (reverseVelocity)
				{
					if (Input.GetKeyDown(KeyCode.Z))
					{
						recordedVelocity = new Vector2(recordedVelocity.x, -recordedVelocity.y);
					}
					if (Input.GetKeyDown(KeyCode.X))
					{
						recordedVelocity = new Vector2(-recordedVelocity.x, recordedVelocity.y);
					}
				}
				rb.constraints = RigidbodyConstraints2D.FreezeAll;
				rb.isKinematic = true;
			}
		}
		if (inSequence && FlipMechanic.aniTime >= 1.0f)
		{
			inSequence = false;
			transform.position = recordedPosition;
			rb.velocity = recordedVelocity;
			rb.constraints = RigidbodyConstraints2D.FreezeRotation;
			rb.isKinematic = false;
		}
	}
}
