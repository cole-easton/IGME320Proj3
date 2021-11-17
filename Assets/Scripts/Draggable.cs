using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//[Flags]
//public enum PointerState
//{
//	None = 0b0000,
//	Over = 0b0001,
//	Down = 0b0010,

//	Up = 0b0000,
//	Out = 0b0000,
//	UpAndOut = 0b0000,
//	UpAndOver = 0b0001,
//	DownAndOut = 0b0010,
//	DownAndOver = 0b0011,
//}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Draggable : MonoBehaviour
{
	#region Fields & Properties
	[SerializeField]
	private decimal slop = new decimal(1, 0, 0, false, 28); //1e-28m;(decimal)float.Epsilon;

	/// <summary>
	/// Represents the displacement between the object's origin and the mouse when dragging began. Used to preserve that displacement while dragging.
	/// </summary>
	[SerializeField()]
	private Vector2? mouseOffset = null;

	private Vector2 MouseOffset { get => (mouseOffset != null) ? (Vector2)mouseOffset : Vector2.zero; }

	private Rigidbody2D myRB;
	#endregion

	// Start is called before the first frame update
	void Start()
	{
		myRB = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		Debug.Log("mousePosWorld: " + GetMousePosWorld());
		if (mouseOffset != null)
			OnMouseDragPsuedo();
	}

	private Vector2 GetMousePosWorld()
	{
		var mousePosScreen = Input.mousePosition;
		mousePosScreen.z = 10;
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(mousePosScreen);
		return mousePos;
	}

	private void OnMouseDown()
	{
		Vector2 mousePos2D = GetMousePosWorld();
		mouseOffset = mousePos2D - myRB.position;
	}

	//private decimal executionCounter = 0;
	private void OnMouseDragPsuedo()
	{
		//executionCounter++;
		//Debug.Log($"Execution Counter: {executionCounter}");
		//Debug.Log($"MousePos: {Input.mousePosition.ToString()}");
		//Debug.Log($"MouseOffset: {MouseOffset.ToString()}");
		Vector2 mousePos2D = GetMousePosWorld();
		if (MouseOffset + myRB.position != mousePos2D/* && (mousePos2D - (MouseOffset + myRB.position)).magnitude > (float)slop*/)
		{
			//var destination = mousePos2D - MouseOffset;
			//var projectedDestination = myRB.velocity * Time.deltaTime + myRB.position;
			//var velocityAdjustment = (destination - projectedDestination) / Time.deltaTime;
			//var accel = velocityAdjustment / Time.deltaTime;
			//var force = accel * myRB.mass;
			//force *= slop;
			//Debug.Log($"Force: {force.ToString()}");
			//myRB.AddForce(force);

			var destination = mousePos2D - MouseOffset;
			var projectedDestination = myRB.velocity * Time.fixedDeltaTime + myRB.position;
			var velocityAdjustment = (destination - projectedDestination) / Time.fixedDeltaTime;
			myRB.velocity += velocityAdjustment * (1f - float.Epsilon);

			//var destination = new Vector2Decimal(mousePos2D) - new Vector2Decimal(MouseOffset);
			//var projectedDestination = new Vector2Decimal(myRB.velocity) * (decimal)Time.deltaTime + new Vector2Decimal(myRB.position);
			//var velocityAdjustment = (destination - projectedDestination) / (decimal)Time.deltaTime;

			//myRB.velocity += velocityAdjustment;

			//var accel = velocityAdjustment / (decimal)Time.deltaTime;
			//var force = accel * (decimal)myRB.mass;
			//force *= 1m - slop;
			//Debug.Log($"Slop: {slop.ToString()}");
			//Debug.Log($"Force: {force.ToString()}");
			//myRB.AddForce(force);
		}
		else
		{
			myRB.position = mousePos2D - MouseOffset;
		}
	}

	private void OnMouseUp()
	{
		myRB.velocity = Vector2.zero;
		mouseOffset = null;
	}
}