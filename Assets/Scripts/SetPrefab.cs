using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SetPrefab : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Vector3 pos = transform.position;
		pos.z = -45;
		transform.position = pos;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
