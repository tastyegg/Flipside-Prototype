using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class lasers : MonoBehaviour
{
	public int direction;
	public GameObject player;
	FlipMechanic flip;
	PlayerController pc;
	LineRenderer lr;
	RaycastHit2D[] rchs2d;

    Vector3 lastHitPoint;

	// Use this for initialization
	void Start()
	{
		lr = gameObject.GetComponent<LineRenderer>();
		pc = player.GetComponent<PlayerController>();
		flip = player.GetComponent<FlipMechanic>();
        lastHitPoint = transform.position;
	}

	// Update is called once per frame
	void Update()
	{
		lr.SetPosition(0, gameObject.transform.position);
		float x = gameObject.transform.position.x;
		float y = gameObject.transform.position.y;
		if (direction == 0)
		{
			rchs2d = Physics2D.RaycastAll(transform.position, Vector2.up);
			y += 50;
		}
		else if (direction == 1)
		{
			rchs2d = Physics2D.RaycastAll(transform.position, Vector2.right);
			x += 50;
		}
		else if (direction == 2)
		{
			rchs2d = Physics2D.RaycastAll(transform.position, Vector2.down);
			y -= 50;
		}
		else
		{
			rchs2d = Physics2D.RaycastAll(transform.position, Vector2.left);
			x -= 50;
		}
		lr.SetPosition(1, new Vector3(x, y, 0));

		int laserHit = rchs2d.Length;
        Vector3 npos = transform.position;
        bool killPlayer = false;
		for (int i = rchs2d.Length - 1; i >= 0; i--)
		{
			RaycastHit2D rch2d = rchs2d[i];
			if (rch2d.collider != null)
			{
				//Checks if the first collision is the player, which would mean it's hitting the player.
                if (rch2d.collider.tag == "WallPreview")
                {
                    laserHit--;
                }
                else if (rch2d.collider.tag == "Player")
                {
                    killPlayer = true;
                }
				else
				{
                    killPlayer = false;
					if (direction == 0 || direction == 2)
					{
						y = rch2d.point.y;
						npos = new Vector3(transform.position.x, y, 1);
                    }
					else
					{
						x = rch2d.point.x;
						npos = new Vector3(x, transform.position.y, 1);
					}
                }
            }
		}
        if (killPlayer)
        {
            pc.Die();
        }

        lastHitPoint = Vector3.Lerp(lastHitPoint, npos, flip.aniTime);
        lr.SetPosition(1, lastHitPoint);
        //Disables rendering during flip
        if (laserHit <= 0) {
			if (direction == 0 || direction == 2)
			{
				npos = new Vector3(transform.position.x, transform.position.y + 50 * (direction == 0 ? 1 : -1), 0);
			}
			else
			{
				npos = new Vector3(transform.position.x + 50 * (direction == 1 ? 1 : -1), transform.position.y, 0);
			}
			lr.SetPosition(1, npos);
		}
	}
}