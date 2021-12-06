using System;
using System.Collections.Generic;
using UnityEngine;

//namespace Assets.Scripts
//{
[RequireComponent(typeof(Collider2D))]
public class Receptor : MonoBehaviour
{
	private Collider2D collider;
	private CannonController cannon;
	/// <summary>
	/// Keeps track of how long particle stream has been colliding with this receptor (anti-spam).
	/// </summary>
	private float collisionTimer = float.MinValue;
	/// <summary>
	/// How long the particle stream must be colliding with this receptor to activate (anti-spam).
	/// </summary>
	public const float collisionLength = 1.5f;

	public event EventHandler OnReceptorReached;

	public event EventHandler OnReceptorActivated;

	public event EventHandler OnReceptorDeactivated;

	public Action<object, EventArgs> receptorActivationAction;

	public Action<object, EventArgs> receptorDeactivationAction;

	private void Start()
	{
		gameObject.tag = "Receptor";
		cannon = FindObjectOfType<CannonController>();
	}

	private void FixedUpdate()
	{
		if (cannon.OverlapsCollider(collider))
		{
			if (collisionTimer < 0f)
			OnReceptorReached?.Invoke(this, new EventArgs());
			collisionTimer = 0f;
		}
	}
}
//}
