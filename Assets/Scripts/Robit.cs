using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robit : MonoBehaviour
{
	public float moveSpeed = 1;
	public GameObject bulletPrefab;
	public Transform gunBarrel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		var move = moveSpeed * Input.GetAxis("Shimmy");
		transform.Rotate(Vector3.back, move);

		
		var shoot = Input.GetButton("Pew");
		if (shoot && bulletPrefab != null && gunBarrel != null)
		{
			Instantiate(bulletPrefab, gunBarrel.position, transform.rotation);
		}
	}
}
