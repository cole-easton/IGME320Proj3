using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Vehicle : MonoBehaviour
{
	public float mass = 1;
	public float maxSpeed = 4;
	public float maxForce = 10;
	private Vector3 velocity;
	private float wanderAngle;

	protected Vector3 position
	{
		get
		{
			return gameObject.transform.position;
		}
		set
		{
			gameObject.transform.position = value;
		}
	}

    // Start is called before the first frame update
    void Start()
    {
		velocity = Vector3.zero;
		wanderAngle = 0;
    }

    // Update is called once per frame
    void Update()
    {
		Vector3 force = Vector3.ClampMagnitude(CalcSteeringForces(), maxForce);
		Vector3 acc = force / mass;
		velocity += acc;
		velocity.z = 0;
		velocity.Normalize();
		//Vector3.ClampMagnitude(velocity, maxSpeed);
		gameObject.transform.right = velocity;
		velocity *= maxSpeed;
		//gameObject.transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(velocity.y, velocity.x)*(180/Mathf.PI));
		position += velocity*Time.deltaTime;	
    }

	protected abstract Vector3 CalcSteeringForces();

	protected Vector3 Seek(GameObject target)
	{
		return Seek(target.transform.position);
	}

	private Vector3 Seek(Vector3 targetPosition)
	{
		Vector3 desiredVelocity = targetPosition - position;
		desiredVelocity.Normalize();
		desiredVelocity *= maxSpeed;
		Vector3 steeringForce = desiredVelocity - velocity;
		return steeringForce;
	}

	protected Vector3 Flee(GameObject target)
	{
		return Flee(target.transform.position);
	}

	private Vector3 Flee(Vector3 targetPosition)
	{
		Vector3 desiredVelocity = position - targetPosition;
		desiredVelocity.Normalize();
		desiredVelocity *= maxSpeed;
		Vector3 fleeForce = desiredVelocity - velocity;
		return fleeForce;
	}

	/// <summary>
	/// Reuturns a unit vector away from vehicles
	/// </summary>
	/// <param name="vehicles">The flock to seperate form</param>
	/// <param name="power">The extent to which distance effects separation.  
	/// Zero means that distance does not matter at all. 1 recommended per class powerpoints</param>
	/// <returns>a unit vector away from the flock vehicles</returns>
	protected Vector3 Separate(Vehicle[] vehicles, float power = 1)
	{
		Vector3 result = Vector3.zero;
		foreach (Vehicle vehicle in vehicles)
		{
			if (vehicle != this)
			{
				//Yes, I could have done this whole if ladder in one line, but checking these cases prevents unnecessary expensive operations
				if (power == 0)
					result += Flee(vehicle.position);
				else if (power == 2)
					result += Flee(vehicle.position) / (vehicle.position - position).sqrMagnitude;
				else if (power % 2 == 0)
					result += Flee(vehicle.position) / Mathf.Pow((vehicle.position - position).sqrMagnitude, power / 2);
				else
					result += Flee(vehicle.position) / Mathf.Pow((vehicle.position - position).magnitude, power);
			}
		}
		return result;//.normalized;
	}

	protected Vector3 Cohere(Vehicle[] vehicles, float power = 1, bool frontOnly = false)
	{
		Vector3 dividend = Vector3.zero;
		float divisor = 0;
		foreach (Vehicle vehicle in vehicles)
		{
			if (vehicle != this && (!frontOnly || Vector3.Dot(this.transform.right, vehicle.position - position) > 0))
			{
				float multiplier;
				if (power == 0)
					multiplier = 1;
				else if (power == 2)
					multiplier = 1 / (vehicle.position - position).sqrMagnitude;
				else if (power % 2 == 0)
					multiplier = 1 / Mathf.Pow((vehicle.position - position).sqrMagnitude, power / 2);
				else
					multiplier = 1 / Mathf.Pow((vehicle.position - position).magnitude, power);
				dividend += multiplier * vehicle.position;
				divisor += multiplier;
			}
		}
		return Seek(dividend / divisor);
	}

	protected Vector3 Align(Vehicle[] vehicles, float power = 1)
	{
		Vector3 dividend = Vector3.zero;
		float divisor = 0;
		foreach (Vehicle vehicle in vehicles)
		{
			if (vehicle != this)
			{
				float multiplier;
				if (power == 0)
					multiplier = 1;
				else if (power == 2)
					multiplier = 1 / (vehicle.position - position).sqrMagnitude;
				else if (power % 2 == 0)
					multiplier = 1 / Mathf.Pow((vehicle.position - position).sqrMagnitude, power / 2);
				else
					multiplier = 1 / Mathf.Pow((vehicle.position - position).magnitude, power);
				dividend += multiplier * vehicle.gameObject.transform.right;
				divisor += multiplier;
			}
		}
		Vector3 desiredVelocity = dividend * maxSpeed / divisor;
		return (desiredVelocity - this.velocity);
	}

	protected Vector3 AvoidObstacles(GameObject[] obstacles)
	{
		Vector3 force = Vector3.zero;
		foreach(GameObject obstacle in obstacles)
		{
			Vector3 toObs = obstacle.transform.position - position; 
			if (Vector3.Dot(gameObject.transform.right, toObs) > 0)
			{
				force -= toObs.normalized / toObs.sqrMagnitude;
			}
		}
		return force;
	}

	protected Vector3 Wander(float distAhead, float radius, float maxVariation = 0.2f)
	{
		wanderAngle += Random.Range(-maxVariation, maxVariation);
		return gameObject.transform.forward * distAhead + radius * gameObject.transform.forward * Mathf.Sin(wanderAngle) + radius * gameObject.transform.right * Mathf.Cos(wanderAngle);
	}
}
