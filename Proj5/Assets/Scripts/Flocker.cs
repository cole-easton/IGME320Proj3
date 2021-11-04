using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocker : Vehicle
{
    private const float AdjSpeed = 0.1f;
    protected override void Update()
    {
        base.Update();
        position.y = position.y*(1-AdjSpeed) + AdjSpeed*Terrain.activeTerrain.SampleHeight(position)*1.1f;
    }
    protected override void CalcSteeringForces()
    {
        Vector3 total = Vector3.zero;
        total += 1.5f*Separate(FlockManager.instance.flock, 2); //2
        total += Align(FlockManager.instance.flock, 0);//1
        total += Cohere(FlockManager.instance.flock, 0);//.5
        total += Wander(3, 2);
        total -= gameObject.transform.position / 50;
        foreach(GameObject obstacle in ObstacleManager.instance.obstacles)
        {
            total += .01f*AvoidObstacle(obstacle);
        }
        total.y = 0;
        Vector3.ClampMagnitude(total, maxForce);
        ApplyForce(total);
    }
}
