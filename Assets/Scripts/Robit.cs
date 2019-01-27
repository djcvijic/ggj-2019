using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robit : MonoBehaviour
{
	public float moveSpeed = 1;
	public float SecBetweenBullets = 0.25f;
	public float ultiActive = 3f;
	public float ultiCoolDown = 12f;
	public bool ultiEnable = true;
	public GameObject bulletPrefab;
	public Transform gunBarrel;
	public GameObject ulti;
	
	private AudioSource audioSource;
	private float timerBullet;
	private float timerUlti;
	private Quaternion initialRotation;

	// Use this for initialization
	void Start ()
	{
		audioSource = GetComponent<AudioSource>();
		timerBullet = 0;
		timerUlti = ultiActive+ultiCoolDown;
		initialRotation = transform.rotation;
		ulti.SetActive(false);
	}

	public void Restart()
	{
		timerBullet = 0;
		timerUlti = ultiActive+ultiCoolDown;
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

			timerBullet -= Time.deltaTime;
			var shoot = Input.GetButton("Pew");
			if (shoot && (bulletPrefab != null) && (gunBarrel != null) && ((timerBullet <= 0) || Input.GetButtonDown("Pew")) )
			{
				Instantiate(bulletPrefab, gunBarrel.position, transform.rotation, GameController.Instance.transform);
				timerBullet = SecBetweenBullets;
				if (audioSource != null)
				{
					audioSource.Play();
				}
			}

			timerUlti+= Time.deltaTime;
			var laser = Input.GetButtonDown("Fire1");
			if (laser && (ulti != null) && ultiEnable==true ){
				ulti.SetActive(true);
				timerUlti=0;
				ultiEnable=false;
			}
			if ( (timerUlti < ultiActive+Time.deltaTime) && (timerUlti > ultiActive - Time.deltaTime) ){
				ulti.SetActive(false);
			}
			if ( timerUlti > (ultiCoolDown+ultiActive) ) {
				ultiEnable=true;
			}

		}
	}
}
