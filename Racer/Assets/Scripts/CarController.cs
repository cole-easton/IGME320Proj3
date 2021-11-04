using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class CarController : MonoBehaviour
{
	public float maxAcceleration;
	public float maxBrake;
	public float maxTurnAngle;
	public float autoSpeed;
	public float autoBrake;
	public float dragCoefficient;
	public float axleSeparation;

	private Vector3 velocity;
	private Vector3 frontAxlePos;
	private Vector3 backAxlePos;

	void Start()
    {
		velocity = Vector3.zero;
		frontAxlePos = transform.position + transform.forward*axleSeparation/2;
		backAxlePos = transform.position - transform.forward*axleSeparation/2;
    }

	// Update is called once per frame
	void Update()
	{
		float acceleration = (Gamepad.current.rightTrigger.ReadValue()*maxAcceleration - Gamepad.current.leftTrigger.ReadValue()*maxBrake - autoBrake - dragCoefficient*velocity.sqrMagnitude)*Time.deltaTime;
		float wheelAngle = Gamepad.current.leftStick.x.ReadValue() * maxTurnAngle;
		velocity += acceleration * transform.forward * Time.deltaTime;
		velocity = Mathf.Cos(wheelAngle * Mathf.PI / 180)*( Quaternion.EulerAngles(0, wheelAngle, 0)*velocity);
		if (Vector3.Dot(transform.forward, velocity) < 0) 
		{
			velocity = Vector3.zero;
			acceleration = 0;
		}
		frontAxlePos += velocity;
		backAxlePos = backAxlePos + (frontAxlePos - backAxlePos) * axleSeparation;
		transform.position = (frontAxlePos + backAxlePos) / 2;
		transform.rotation = Quaternion.LookRotation(frontAxlePos - backAxlePos);
		Debug.Log("FOrward: " + transform.forward);
		Debug.Log("V: " + velocity);
		
    }
}
