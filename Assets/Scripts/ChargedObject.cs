using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedObject : MonoBehaviour
{
	public bool positive;
	/// <summary>
	/// The local scale to use for force determination; sidedsteps conflict w/ hover size increase.
	/// </summary>
	public Vector3 effectiveLocalScale;

    // Start is called before the first frame update
    void Start()
    {
		effectiveLocalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
