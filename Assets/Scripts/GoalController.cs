using UnityEngine;
using System.Collections;

public class GoalController : MonoBehaviour {
	public Vector3 MoveVector = Vector3.up; //unity already supplies us with a readonly vector representing up and we are just chaching that into MoveVector
	public float MoveRange; //change this to increase/decrease the distance between the highest and lowest points of the bounce
	public float MoveSpeed; //change this to make it faster or slower
								   // Use this for initialization

	private Vector3 startPosition;
	void Start () {
		startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = startPosition + MoveVector * (MoveRange * Mathf.Sin(Time.timeSinceLevelLoad * MoveSpeed));
	}
}
