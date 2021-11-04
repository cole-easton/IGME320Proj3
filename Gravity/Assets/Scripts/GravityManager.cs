using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : MonoBehaviour
{
	public static GravityManager instance = null;
	private List<Planet> planets;
	
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
		}
		planets = new List<Planet>();
    }

	public void AddPlanet(Planet p)
	{
		planets.Add(p);
	}

	public void ClearPlanets()
	{
		planets.Clear();
	}

	public int CountPlanets()
	{
		return planets.Count;
	}

	public Vector3 FieldAt(Vector3 position)
	{
		return Vector3.zero;
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
