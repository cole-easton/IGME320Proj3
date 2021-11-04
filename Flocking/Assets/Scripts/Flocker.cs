using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocker : Vehicle
{
    protected override Vector3 CalcSteeringForces()
	{
		Vector3 force = Vector3.zero;
		force += 0.1f*Separate(FlockManager.instance.flock, 3);
		force += Align(FlockManager.instance.flock, 4f);
		force += Cohere(FlockManager.instance.flock, 4f);
		force += 3*Wander(3, 2);
		force += 2*AvoidObstacles(ObstacleManager.instance.obstacles);
		force -= position / 25;
		return force;
	}
}
