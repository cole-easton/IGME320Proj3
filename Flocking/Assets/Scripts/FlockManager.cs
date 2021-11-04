using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
	public GameObject flocker;
	public int numFlockers;
	public static FlockManager instance = null;
	[HideInInspector]
	public Vehicle[] flock;
	// Start is called before the first frame update
	void Start()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(this);
		}
		flock = new Vehicle[numFlockers];
		
		for (int i = 0; i < numFlockers; i++)
		{
			float angle = Random.Range(0, 2 * Mathf.PI);
			flock[i] = Instantiate(flocker, new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0), Quaternion.identity).GetComponent<Flocker>();
		}
	}

}
