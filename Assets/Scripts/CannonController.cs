using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct Particle {
	public Vector3 Position { get; set; }
	public Vector3 Velocity { get; set; }
	public Particle(Vector3 position, Vector3 velocity)
	{
		Position = position;
		Velocity = velocity;
	}
}

[RequireComponent(typeof(LineRenderer))]
public class CannonController : MonoBehaviour
{
	public GameObject tip;
	public float launchSpeed;
	public float electricForceConstant = 1;
	public float rotationSpeed = 0.1f;

	const int ParticleLimit = 250;
	const float FirePeriod = 0.02f; //seconds between each paerticle shot 

	private Particle[] particles;
	private BoxCollider2D[] walls;
	private int particleIndex;
	private bool recycling;
	private LineRenderer lr;
	private ChargedObject[] chargedObjects;
	private Vector3 bulletVelocity;

	private GameObject receptor;    // the goal

	public float cannonRotation = 0;
	public bool canRotate = true;

	public event EventHandler OnReceptorReached;

	// Start is called before the first frame update
	void Start() { 
        chargedObjects = FindObjectsOfType(typeof(ChargedObject)) as ChargedObject[];
		Wall[] wallScripts = FindObjectsOfType(typeof(Wall)) as Wall[];
		walls = new BoxCollider2D[wallScripts.Length];
		for (int i = 0; i < wallScripts.Length; i++) {
			walls[i] = wallScripts[i].gameObject.GetComponent<BoxCollider2D>();
		}
		particles = new Particle[ParticleLimit];
		recycling = false;
		lr = GetComponent<LineRenderer>();
		lr.useWorldSpace = true;
		particleIndex = 0;

		receptor = GameObject.FindGameObjectWithTag("Receptor");
	}

    // Update is called once per frame
    void Update()
    {
		bool shooting = true; // Input.GetMouseButton(0);

		/*
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0;
		transform.rotation = Quaternion.AngleAxis(180/(Mathf.PI)*Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x), Vector3.forward);
		if (shooting && Time.time % FirePeriod - Time.deltaTime < 0)
		{
			particles[particleIndex++] = new Particle(tip.transform.position,
				launchSpeed * new Vector3(mousePos.x - transform.position.x, mousePos.y - transform.position.y).normalized);
			if (particleIndex >= ParticleLimit)
			{
				particleIndex = 0;
				recycling = true;
			}
			
		}
		*/
		// new code starts
		if (canRotate)
		{
			if (Input.GetKey(KeyCode.UpArrow) && cannonRotation < 90)
			{
				cannonRotation+=rotationSpeed;
			}
			if (Input.GetKey(KeyCode.DownArrow) && cannonRotation > -90)
			{
				cannonRotation-=rotationSpeed;
			}
		}

		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0;
		transform.rotation = Quaternion.Euler(0, 0, cannonRotation);
		float radius = Vector3.Distance(tip.transform.position, transform.position);
		if (shooting && Time.time % FirePeriod - Time.deltaTime < 0)
		{
			particles[particleIndex++] = new Particle(tip.transform.position,
				launchSpeed * new Vector3(radius * Mathf.Cos(Mathf.Deg2Rad * cannonRotation), radius * Mathf.Sin(Mathf.Deg2Rad * cannonRotation)).normalized);
			if (particleIndex >= ParticleLimit)
			{
				particleIndex = 0;
				recycling = true;
			}

		}
		// new code ends
		for (int i = 0; i < (recycling?ParticleLimit:particleIndex); i++)
		{
			Vector3 displacement;
			foreach (ChargedObject chargedObject in chargedObjects)
			{
				displacement =  particles[i].Position - chargedObject.gameObject.transform.position;
				displacement.z = 0;
				particles[i].Velocity += electricForceConstant * chargedObject.gameObject.transform.localScale.x *  chargedObject.transform.localScale.y 
					* displacement / Mathf.Pow(displacement.magnitude, 3) * (chargedObject.positive?-1:1);

				if ((particles[i].Position - chargedObject.transform.position).sqrMagnitude 
					< chargedObject.gameObject.transform.localScale.x * chargedObject.transform.localScale.y/4)
				{
					//if particles get inside the charged objects, chaotic and framerate-dependent behavior can result.  This is therefore forbidden.
					particleIndex = 0;
					recycling = false;
				}
				
			}

			foreach(BoxCollider2D wall in walls)
			{
				if (wall.OverlapPoint(particles[i].Position))
				{
					particleIndex = 0;
					recycling = false;
				}
			}

			// check the particle against the goal receptor
			if ((particles[i].Position - receptor.transform.position).sqrMagnitude
				< receptor.transform.localScale.x * receptor.transform.localScale.y / 4)
			{
				Debug.Log("You win!!!!!!!");
				OnReceptorReached?.Invoke(this, new EventArgs());
			}

			particles[i].Position += particles[i].Velocity * Time.deltaTime;
		}

		Vector3[] positions = new Vector3[(recycling?ParticleLimit:particleIndex)+ (shooting ? 1 : 0)];
		if (particleIndex>0 && shooting)
			positions[0] = tip.transform.position;
		int j = particleIndex-1;
		while (j >= 0)
		{
			try
			{
				positions[particleIndex - j - 1 + (shooting ? 1 : 0)] = particles[j].Position;
			}
			catch (Exception e)
			{
				Debug.LogError($"Out of bounds: j = {j}, particleIndex = {particleIndex}");
				Debug.LogError($"Accessing index {particleIndex - j - 1 + (shooting ? 1 : 0)} in positions, whose length is {positions.Length}");
				Debug.LogError($"Accessing index {j} in particles, whose length is {positions.Length}");

				UnityEditor.EditorApplication.isPlaying = false;
			}
			j--;
		}
		if (recycling)
		{
			j = ParticleLimit-1;
			while (j >= particleIndex)
			{
				positions[particleIndex + ParticleLimit - 1 - j + (shooting ? 1 : 0)] = particles[j].Position;
				j--;
			}
		}
		lr.positionCount = positions.Length;
		lr.SetPositions(positions);
	}
}
