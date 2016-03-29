using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class EditModeSnapGrid : MonoBehaviour {
    //Changable values
    public float snapValue = 0.5f;
    public float depth = 0;
    
	void Update ()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            float snapInverse = 1 / snapValue;

            float x, y, z;

            x = Mathf.Round(transform.position.x * snapInverse) / snapInverse;
            y = Mathf.Round(transform.position.y * snapInverse) / snapInverse;
            z = depth;

            transform.position = new Vector3(x, y, z);
        }
    }
}
