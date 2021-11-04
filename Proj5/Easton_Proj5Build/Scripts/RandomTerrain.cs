using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Cole Easton
/// Applies heights to terrain with 5 different Perlin noise "octaves"
/// </summary>
public class RandomTerrain : MonoBehaviour
{
    private TerrainData terrainData;
    public int resolution = 129;
    private Vector3 worldSize; 

    /// <summary>
    /// Applies heights to terrain with 5 different Perlin noise "octaves"
    /// </summary>
    void Start()
    {
        terrainData = gameObject.GetComponent<TerrainCollider>().terrainData;
        worldSize = new Vector3(200, 50, 200);
        terrainData.size = worldSize;
        terrainData.heightmapResolution = resolution;
        const int MaxPow = 6;
        float Max2ToPow = Mathf.Pow(2, MaxPow);
        float[,] heightArray = new float[resolution, resolution];
        for (int i = 0; i < resolution; i++)
        {
            for (int j = 0; j < resolution; j++)
            {
                heightArray[i, j] = 0;
                for (int power = 1; power < MaxPow; power++) {
                    float multiplier = Mathf.Pow(2, power);
                    heightArray[i, j] += Mathf.PerlinNoise(i / multiplier, j / multiplier) * multiplier / Max2ToPow;
                }
            }
        }
        terrainData.SetHeights(0, 0, heightArray);
    }

    // Update is called once per frame
    void Update()
    {
        
    }








}
