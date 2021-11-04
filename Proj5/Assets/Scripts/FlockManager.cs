using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public GameObject flocker;
    public int numFlockers;
    public GameObject flockCenter;
    public GameObject altFlockCenter; //for front following
    public static FlockManager instance = null;
    [HideInInspector]
    public Vehicle[] flock;
    [HideInInspector]
    public bool debugLines;
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
        debugLines = false;
        flock = new Vehicle[numFlockers];
        float angle = Random.Range(0, 2 * Mathf.PI);
        for (int i = 0; i < numFlockers; i++)
        {
            flock[i] = Instantiate(flocker, Vector3.zero, Quaternion.identity).GetComponent<Flocker>();
            float nangle = angle + Random.Range(-1f, 1f); //new angle
            flock[i].ApplyForce(4*new Vector3(Mathf.Cos(nangle), 0, Mathf.Sin(angle)));
            Vector3 pos = flock[i].gameObject.transform.position;
            pos.x = angle;
            pos.z = Mathf.Sin(angle);
            flock[i].gameObject.transform.position = pos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 center = Vector3.zero;
        Vector3 forward = Vector3.zero;
        foreach(Vehicle flocker in flock)
        {
            center += flocker.gameObject.transform.position;
            forward += flocker.transform.forward;
        }
        center /= flock.Length;
        forward /= flock.Length;
        flockCenter.transform.position = center;
        flockCenter.transform.forward = forward;
        altFlockCenter.transform.position = center;
        altFlockCenter.transform.forward = -forward;
        if (Input.GetKeyDown(KeyCode.D))
        {
            debugLines ^= true;
        }
    }

    private void OnDrawGizmos()
    {
        if (!debugLines)
        {
            return;
        }
        Gizmos.color = new Color(0, 0, 0.8f);
        Gizmos.DrawCube(flockCenter.transform.position, new Vector3(1, 1, 1));
        Gizmos.color = new Color(1, 0.5f, 0);
        Gizmos.DrawLine(flockCenter.transform.position, flockCenter.transform.position + 4*flockCenter.transform.forward);
    }
}
