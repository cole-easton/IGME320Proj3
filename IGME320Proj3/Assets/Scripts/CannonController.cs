using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
	public GameObject bullet;
	public GameObject tip;
	public float launchSpeed;
	public float electricForceConstant = 1;

	private ChargedObject[] chargedObjects;
	private Vector3 bulletVelocity;
	// Start is called before the first frame update
	void Start() { 
        chargedObjects = FindObjectsOfType(typeof(ChargedObject)) as ChargedObject[];
		bullet.SetActive(false);
	}

    // Update is called once per frame
    void Update()
    {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0;
		transform.rotation = Quaternion.AngleAxis(180/(Mathf.PI)*Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x), Vector3.forward);
		if (Input.GetMouseButtonDown(0))
		{
			bullet.SetActive(true);
			bullet.transform.position = tip.transform.position;
			bulletVelocity = launchSpeed * new Vector3(mousePos.x - transform.position.x, mousePos.y - transform.position.y).normalized;
		}
		if (bullet.activeSelf)
		{
			Vector3 displacement;
			foreach (ChargedObject chargedObject in chargedObjects)
			{
				displacement =  bullet.transform.position - chargedObject.gameObject.transform.position;
				displacement.z = 0;
				bulletVelocity += electricForceConstant * chargedObject.gameObject.transform.localScale.x *  chargedObject.transform.localScale.y 
					* displacement / Mathf.Pow(displacement.magnitude, 3) * (chargedObject.positive?-1:1);
			}
			bullet.transform.position += bulletVelocity * Time.deltaTime;
		}
    }
}
