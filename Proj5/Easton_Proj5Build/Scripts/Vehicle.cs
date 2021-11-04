using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Vehicle : MonoBehaviour
{
    // Vectors for the physics
    protected Vector3 position;
    protected Vector3 direction;
    protected Vector3 velocity;
    protected Vector3 acceleration;

    public float maxSpeed = 4;
    public float maxForce = 4;
    public float safeDistance;

    protected Vector3 extents;
    protected MeshRenderer meshRenderer;

    // The mass of the object. Note that this can't be zero
    public float mass = 1;
    public float radius;

    private float wanderAngle;
    private const float MIN_VELOCITY = 0.1f;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        acceleration = new Vector3(0, 0, 0);
        velocity = new Vector3(0, 0, 0);
        position = gameObject.transform.position;
        wanderAngle = Random.Range(0, Mathf.PI / 2);
        extents.x = 50;
        extents.z = 50;
        extents.y = 25;
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        acceleration = new Vector3(0, 0, 0);
        CalcSteeringForces();
        velocity += acceleration*Time.deltaTime;
        //velocity.y = 0;
        velocity.Normalize();
        gameObject.transform.forward = velocity;
        velocity *= maxSpeed;
        position += velocity*Time.deltaTime;
        gameObject.transform.position = position;
    }

    protected abstract void CalcSteeringForces();

    /// <summary>
    /// Applies friction to the vehicle
    /// </summary>
    /// <param name="coeff">The coefficient of friction</param>
    protected void ApplyFriction(float coeff)
    {
        // If the velocity is below a minimum value, just stop the vehicle
        if (velocity.magnitude < MIN_VELOCITY)
        {
            velocity = Vector3.zero;
            return;
        }

        Vector3 friction = velocity * -1;
        friction.Normalize();
        friction = friction * coeff;
        acceleration += friction;
    }


    /// <summary>
    /// Applies a force to the vehicle
    /// </summary>
    /// <param name="force">The force to be applied</param>
    public void ApplyForce(Vector3 force)
    {
        // Make sure the mass isn't zero, otherwise we'll have a divide by zero error
        if (mass == 0)
        {
            Debug.LogError("Mass cannot be zero!");
            return;
        }

        // Add our force to the acceleration for this frame
        acceleration += force / mass;
    }

    protected Vector3 AvoidObstacle(GameObject target)
    {
        return AvoidObstacle(target.transform.position, target.transform.localScale.x);
    }
    /// <summary>
    /// Returns a vector to avoid an object at tagetPosition with radius otherRadius
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <param name="otherRadius"></param>
    /// <returns></returns>
    private Vector3 AvoidObstacle(Vector3 targetPosition, float otherRadius)
    {
        Vector3 meToOther = targetPosition - position;
        float fwdMeToOtherDot = Vector3.Dot(transform.forward, meToOther);
        if (fwdMeToOtherDot < 0)
        {
            return Vector3.zero;
        }
        float rightMeToOtherDot = Vector3.Dot(transform.right, meToOther);
        if (Mathf.Abs(rightMeToOtherDot) > otherRadius + meshRenderer.bounds.extents.magnitude)
        {
            return Vector3.zero;
        }
        float distance = meToOther.magnitude - otherRadius;
        if (distance > safeDistance)
        {
            return Vector3.zero;
        }
        float weight;
        if (distance <= 0)
        {
            weight = float.MaxValue;
        }
        else
        {
            weight = Mathf.Pow(safeDistance / distance, 2);
        }
        weight = Mathf.Max(10000, weight);
        return -weight * meToOther.normalized;
    }

    protected Vector3 Seek(GameObject target)
    {
        return Seek(target.transform.position);
    }

    protected Vector3 Pursue(GameObject target, float foresight = 1)
    {
        return Seek(target.transform.position + foresight*target.transform.forward);
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

    /// <summary>
    /// returns a vector away from a position in front of target
    /// </summary>
    /// <param name="target">the object to move away from</param>
    /// <param name="foresight">the number of units ahead of target to evade</param>
    /// <returns>a vector away from a position in front of target</returns>
    protected Vector3 Evade(GameObject target, float foresight = 1)
    {
        return Flee(target.transform.position + foresight*target.transform.forward);
    }

    private Vector3 Flee(Vector3 targetPosition)
    {
        Vector3 desiredVelocity = position - targetPosition;
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;
        Vector3 fleeForce = desiredVelocity - velocity;
        return fleeForce;
    }

    protected Vector3 StayInBounds(float sharpness = 2)
    {
        Vector3 force = Vector3.zero;
        if (position.x > extents.x)
            force.x = -Mathf.Pow(position.x - extents.x, sharpness);
        else if (position.x < -extents.x)
            force.x = Mathf.Pow(-extents.x - position.x, sharpness);
        if (position.y > extents.y)
            force.y -= Mathf.Pow(position.y - extents.y, sharpness);
        else if (position.y < -extents.y)
            force.y += Mathf.Pow(-extents.y - position.y, sharpness);
        if (position.z > extents.z)
            force.z -= Mathf.Pow(position.z - extents.z, sharpness);
        else if (position.z < -extents.z)
            force.z += Mathf.Pow(-extents.z - position.z, sharpness);
        return force;
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
        foreach(Vehicle vehicle in vehicles)
        {
            if (vehicle != this)
            {
                //Yes, I could have done this whole if ladder in one line, but checking these cases prevents unnecessary expensive operations
                if (power == 0)
                    result += Flee(vehicle.position);
                else if (power == 2)
                    result += Flee(vehicle.position) / (vehicle.position - position).sqrMagnitude;
                else if (power % 2 == 0)
                    result += Flee(vehicle.position) / Mathf.Pow((vehicle.position - position).sqrMagnitude, power/2);
                else
                    result += Flee(vehicle.position) / Mathf.Pow((vehicle.position - position).magnitude, power);
            }
        }
        return result;//.normalized;
    }

    protected Vector3 Cohere(Vehicle[] vehicles, float power = 1)
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
                dividend += multiplier * vehicle.gameObject.transform.forward;
                divisor += multiplier;
            }
        }
        Vector3 desiredVelocity = dividend * maxSpeed / divisor;
        return (desiredVelocity - this.velocity);
    }

    protected Vector3 Wander(float distAhead, float radius, float maxVariation = 0.2f) 
    {
        wanderAngle += Random.Range(-maxVariation, maxVariation);
        return gameObject.transform.forward * distAhead + radius * gameObject.transform.forward * Mathf.Sin(wanderAngle) + radius * gameObject.transform.right * Mathf.Cos(wanderAngle);
    }


}
