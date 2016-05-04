using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SortTileMap : MonoBehaviour
{
	public bool StoreAllWallObjects = false;
	public bool Sort = false;

	// Use this for initialization
	void Start()
	{
		Sort = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (StoreAllWallObjects)
		{
			foreach (GameObject g in GameObject.FindGameObjectsWithTag("Wall"))
			{
				g.transform.SetParent(transform);
			}
			StoreAllWallObjects = false;
			Sort = true;
		}
		if (Sort)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				for (int j = 0; j < i; j++)
				{
					int compare = transform.GetChild(i).name.CompareTo(transform.GetChild(j).name);
					if (compare < 0)
					{
						transform.GetChild(i).SetSiblingIndex(j);
						break;
					}
				}
			}
			Sort = false;
		}
	}
}
