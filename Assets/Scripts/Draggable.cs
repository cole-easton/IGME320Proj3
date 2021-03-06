using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
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
		myRB.freezeRotation = true;

		origScale = transform.localScale;

		// Distingish Locked And Unlocked elements
		var rend = GetComponent<SpriteRenderer>();
		if (rend != null)
			rend.color = Color.white;

		SpriteRenderer[] axesRenderers = GetComponentsInChildren<SpriteRenderer>(true);
		if (axesRenderers != null && axesRenderers.Length > 1)
			axesRenderers[1].enabled = true;

		//var l = gameObject.AddComponent<Light>();
		//l.type = LightType.Point;
		//float r = Math.Max(GetComponent<Collider2D>().bounds.extents.x, GetComponent<Collider2D>().bounds.extents.y);
		//r *= 2f;
		//l.range = r;
	}

	private void FixedUpdate()
	{
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
			myRB.velocity = Vector2.zero;
		}
	}

	private void OnMouseUp()
	{
		myRB.velocity = Vector2.zero;
		mouseOffset = null;
		transform.localScale = origScale;
	}

	/// <summary>
	/// Used to store scale to revert to after mouse exits collider.
	/// </summary>
	private Vector3 origScale;
	/// <summary>
	/// The amount to scale the object by when mouse is hovering.
	/// </summary>
	[SerializeField]
	[Tooltip("The amount to scale the object by when mouse is hovering.")]
	private float hoverScaleFactor = 1.08f;
	private void OnMouseEnter()
	{
		//origScale = transform.localScale;
		transform.localScale = origScale * hoverScaleFactor;
	}
	private void OnMouseExit()
	{
		// If it's not being dragged, revert scale
		if (mouseOffset == null)
			transform.localScale = origScale;
	}
}