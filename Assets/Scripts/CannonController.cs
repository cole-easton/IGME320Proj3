using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(LineRenderer))]
public class CannonController : MonoBehaviour
{
	public GameObject tip;
	public float launchSpeed;
	public float electricForceConstant = 1;
	public float rotationSpeed = 0.1f;
	public bool allowPolaritySwitching = false;

	const int ParticleLimit = 250;

	private Vector3[] positions;
	private BoxCollider2D[] walls;
	private LineRenderer lr;
	private ChargedObject[] chargedObjects;
	private Vector3 bulletVelocity;
	private bool invertPolarity;

	private GameObject receptor;    // the goal

	public float cannonRotation = 0;
	/// <summary>
	/// Controls if the cannon's rotation is locked.
	/// </summary>
	[Tooltip("Controls if the cannon's rotation is locked.")]
	public bool canRotate = true;

	private float radius;

	public event EventHandler OnReceptorReached;

	// Start is called before the first frame update
	void Start() { 
        chargedObjects = FindObjectsOfType(typeof(ChargedObject)) as ChargedObject[];
		//Wall[] wallScripts = FindObjectsOfType(typeof(Wall)) as Wall[];
		Wall[] wallScripts = FindObjectsOfType<Wall>();
		walls = new BoxCollider2D[wallScripts.Length];
		for (int i = 0; i < wallScripts.Length; i++) {
			walls[i] = wallScripts[i].gameObject.GetComponent<BoxCollider2D>();
		}
		lr = GetComponent<LineRenderer>();
		lr.useWorldSpace = true;

		receptor = GameObject.FindGameObjectWithTag("Receptor");
		radius = Vector3.Distance(tip.transform.position, transform.position); //class wide so we don't recalculate every frame
		invertPolarity = false;
		lr.startColor = lr.endColor = new Color(.38f, .51f, .87f);
	}

	void Update()
	{

		if (Input.GetKeyDown(KeyCode.Space))
		{
			invertPolarity = !invertPolarity;
			if (invertPolarity)
			{
				lr.startColor = lr.endColor = new Color(.93f, 0f, 0f);
			}
			else
			{
				lr.startColor = lr.endColor = new Color(.38f, .51f, .87f);
			}
		}
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
			if (useMouseAngle)
			{
				Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				mousePos.z = 0;
				cannonRotation = 180 / (Mathf.PI) * Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x);
				// TODO: Fix rotation bounds
			}
		}

		transform.rotation = Quaternion.Euler(0, 0, cannonRotation);
		/*Vector3[] */
		positions = new Vector3[ParticleLimit];
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
				//velocity += electricForceConstant * chargedObject.gameObject.transform.localScale.x * chargedObject.transform.localScale.y
				// * displacement / Mathf.Pow(displacement.magnitude, 3) * (chargedObject.positive ? -1 : 1) * (posPolarity? -1 : 1) ;
				velocity += electricForceConstant * chargedObject.effectiveLocalScale.x * chargedObject.effectiveLocalScale.y
				 * displacement / Mathf.Pow(displacement.magnitude, 3) * (chargedObject.positive ^ invertPolarity ? -1 : 1);
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

	/// <summary>
	/// Checks if the given collider overlaps any particles in <see cref="positions"/>.
	/// </summary>
	/// <param name="collider">The collider to check.</param>
	/// <returns><c>true</c> if any of the <see cref="positions"/> overlap, <c>false</c> otherwise.</returns>
	/// <remarks>Doesn't check if the line between points overlap, just the points themselves.</remarks>
	public bool OverlapsCollider(Collider2D collider)
	{
		foreach (var p in positions)
		{
			if (collider.OverlapPoint(p))
				return true;
		}
		return false;
	}

	private bool useMouseAngle = false;
	private void OnMouseDown()
	{
		useMouseAngle = true;
	}
	private void OnMouseUp()
	{
		useMouseAngle = false;
	}
}
