using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public int numObstacles;
    public GameObject obstacle;
    [HideInInspector]
    public static ObstacleManager instance;
    [HideInInspector]
    public GameObject[] obstacles;
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
        obstacles = new GameObject[numObstacles];
        for (int i = 0; i < numObstacles; i ++)
        {
            obstacles[i] = Instantiate(obstacle, new Vector3(Random.Range(-100f, 100f), 0, Random.Range(-100f, 100f)), Quaternion.Euler(0, Random.Range(0, 360), 0));
            Vector3 pos = obstacles[i].transform.position;
            pos.y = Terrain.activeTerrain.SampleHeight(pos) - 3;
            obstacles[i].transform.position = pos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
