using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robit : MonoBehaviour
{
	public float moveSpeed = 1;
	public float SecBetweenBullets = 0.25f;
	public GameObject bulletPrefab;
	public Transform gunBarrel;
	public GameObject ulti;
	
	private AudioSource audioSource;
	private float timer;
	private Quaternion initialRotation;

	// Use this for initialization
	void Start ()
	{
		audioSource = GetComponent<AudioSource>();
		timer = 0;
		initialRotation = transform.rotation;
		ulti.SetActive(false);
	}

	public void Restart()
	{
		timer = 0;
		transform.rotation = initialRotation;
		ulti.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (GameController.Instance.state == GameController.State.Running)
		{
			var move = moveSpeed * Input.GetAxis("Shimmy");
			transform.Rotate(Vector3.back, move);

			timer -= Time.deltaTime;
			var shoot = Input.GetButton("Pew");
			if (shoot && (bulletPrefab != null) && (gunBarrel != null) && ((timer <= 0) || Input.GetButtonDown("Pew")) )
			{
				Instantiate(bulletPrefab, gunBarrel.position, transform.rotation, GameController.Instance.transform);
				timer = SecBetweenBullets;
				if (audioSource != null)
				{
					audioSource.Play();
				}
			}
		}
	}
}
