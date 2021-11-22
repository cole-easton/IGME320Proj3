using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CannonController : MonoBehaviour
{
	public GameObject tip;
	public float launchSpeed;
	public float electricForceConstant = 1;
	public float rotationSpeed = 0.1f;

	const int ParticleLimit = 250;

	private Vector3 positions;
	private BoxCollider2D[] walls;
	private LineRenderer lr;
	private ChargedObject[] chargedObjects;
	private Vector3 bulletVelocity;

	private GameObject receptor;    // the goal

	public float cannonRotation = 0;
	public bool canRotate = true;

	private float radius;

	public event EventHandler OnReceptorReached;

	// Start is called before the first frame update
	void Start() { 
        chargedObjects = FindObjectsOfType(typeof(ChargedObject)) as ChargedObject[];
		Wall[] wallScripts = FindObjectsOfType(typeof(Wall)) as Wall[];
		walls = new BoxCollider2D[wallScripts.Length];
		for (int i = 0; i < wallScripts.Length; i++) {
			walls[i] = wallScripts[i].gameObject.GetComponent<BoxCollider2D>();
		}
		lr = GetComponent<LineRenderer>();
		lr.useWorldSpace = true;

		receptor = GameObject.FindGameObjectWithTag("Receptor");
		radius = Vector3.Distance(tip.transform.position, transform.position); //class wide so we don't recalculate every frame
	}

    // Update is called once per frame
    void FixedUpdate()
    {
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

		Vector3[] positions = new Vector3[ParticleLimit];
		bool collided = false;
		positions[0] = tip.transform.position;
		positions[0].z = 0;
		Vector3 velocity = launchSpeed * new Vector3(radius * Mathf.Cos(Mathf.Deg2Rad * cannonRotation), radius * Mathf.Sin(Mathf.Deg2Rad * cannonRotation)).normalized;
		int posIndex = 1;
		Vector3 displacement;
		do
		{
			positions[posIndex] = positions[posIndex - 1] + velocity * Time.fixedDeltaTime;
			foreach (ChargedObject chargedObject in chargedObjects)
			{
				displacement = positions[posIndex] - chargedObject.gameObject.transform.position;
				displacement.z = 0;
				velocity += electricForceConstant* chargedObject.gameObject.transform.localScale.x* chargedObject.transform.localScale.y
				 * displacement / Mathf.Pow(displacement.magnitude, 3) * (chargedObject.positive ? -1 : 1);
				if ((positions[posIndex] - chargedObject.transform.position).sqrMagnitude
					< chargedObject.gameObject.transform.localScale.x * chargedObject.transform.localScale.y / 4)
				{
					//if particles get inside the charged objects, chaotic behavior can result.  This is therefore forbidden.
					collided = true;
				}
			}
			foreach (BoxCollider2D wall in walls)
			{
				if (wall.OverlapPoint(positions[posIndex]))
				{
					collided = true;
				}
			}
			// check the particle against the goal receptor
			if ((positions[posIndex] - receptor.transform.position).sqrMagnitude
				< receptor.transform.localScale.x * receptor.transform.localScale.y / 4)
			{
				Debug.Log("You win!!!!!!!");
				OnReceptorReached?.Invoke(this, new EventArgs());
			}
			posIndex++;
		} while (posIndex < ParticleLimit && !collided);
		Debug.Log(posIndex);
		
		lr.positionCount = posIndex;
		lr.SetPositions(positions);
	}
}
