using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : Vehicle
{
    public GameObject[] pathNodes;
    public float closeEnough = 0.5f;
    private int nodeIndex; //the node to seek next
    protected override void Start()
    {
        base.Start();
        nodeIndex = 0;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (Vector3.Distance(position, pathNodes[nodeIndex].transform.position) < closeEnough)
        {
            nodeIndex = (nodeIndex + 1) % pathNodes.Length;
        }
    }

    protected override void CalcSteeringForces()
    {
        ApplyForce(Vector3.ClampMagnitude(Seek(pathNodes[nodeIndex]), maxForce));
    }

    private void OnDrawGizmos()
    {
        if (!FlockManager.instance.debugLines)
        {
            return;
        }
        Gizmos.color = Color.green;
        for (int i = 0; i < pathNodes.Length; i++)
        {
            Gizmos.DrawLine(pathNodes[i].transform.position, pathNodes[(i + 1) % (pathNodes.Length)].transform.position);
        }
    }
}
