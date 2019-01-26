using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robit : MonoBehaviour
{
	public float moveSpeed = 1;
	public float SecBetweenBullets= 0.25f;
	public GameObject bulletPrefab;
	public Transform gunBarrel;
	private float timer;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		var move = moveSpeed * Input.GetAxis("Shimmy");
		transform.Rotate(Vector3.back, move);

		timer -= Time.deltaTime;
		var shoot = Input.GetButton("Pew");
		if (shoot && (bulletPrefab != null) && (gunBarrel != null) && ((timer <= 0) || Input.GetButtonDown("Pew")) )
		{
			Instantiate(bulletPrefab, gunBarrel.position, transform.rotation, GameController.Instance.transform);
			timer = SecBetweenBullets;
		}
	}
}
