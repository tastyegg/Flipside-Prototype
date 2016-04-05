using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RepeatSprite : MonoBehaviour {
	SpriteRenderer sr;
	Queue<GameObject> q;

	// Use this for initialization
	void Start () {
		q = new Queue<GameObject>();
		sr = GetComponent<SpriteRenderer>();
		for (int i = 0; i < transform.localScale.x; i++)
			for (int j = 0; j < transform.localScale.y; j++)
			{
				GameObject g = new GameObject(gameObject.name + " " + i + "" + j);
				g.transform.SetParent(transform);
				g.transform.localScale = new Vector3(1.0f / transform.localScale.x, 1.0f / transform.localScale.y, transform.localScale.z);
				g.transform.localPosition = new Vector3((i - transform.localScale.x / 2 + 0.5f) / transform.localScale.x, (transform.localScale.y / 2 - 0.5f - j) / transform.localScale.y, 0);

				SpriteRenderer gsr = g.AddComponent<SpriteRenderer>();
				gsr.sortingLayerID = sr.sortingLayerID;
				gsr.sprite = sr.sprite;
				gsr.color = sr.color;

				q.Enqueue(g);
			}
		sr.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		foreach (GameObject g in q)
		{
			SpriteRenderer gsr = g.GetComponent<SpriteRenderer>();
			gsr.color = sr.color;
		}
	}
}
