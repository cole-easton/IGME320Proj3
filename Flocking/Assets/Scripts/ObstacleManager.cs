using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
	public static ObstacleManager instance = null;
	public int numObstacles;
	public GameObject obstacle;
	[HideInInspector]
	public GameObject[] obstacles;
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

		Camera cam = Camera.allCameras[0];
		Vector3 min = cam.ScreenToWorldPoint(Vector3.zero);
		Vector3 max = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, 0));
		obstacles = new GameObject[numObstacles];
		for (int i = 0; i < numObstacles; i++)
		{
			float x = Random.Range(min.x, max.x);
			float y = Random.Range(min.y, max.y);
			obstacles[i] = Instantiate(obstacle, new Vector3(x, y, 0), Quaternion.identity);
		}
    }

}
