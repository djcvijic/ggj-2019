using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour 
{
	public float moveSpeed = 1;
	public Vector2 screenBounds;
	public GameObject explosionPrefab;
	public int maxLives = 1;

	private int lives;

	// Use this for initialization
	void Start ()
	{
		lives = maxLives;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (GameController.Instance.state == GameController.State.Running)
		{
			transform.position += moveSpeed * transform.up;

			var posX = transform.position.x;
			var posY = transform.position.y;
			if (posX < -screenBounds.x || posX > screenBounds.x || posY < -screenBounds.y || posY > screenBounds.y)
			{
				Destroy(gameObject);
			}
		
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Earth"))
		{
			GameController.Instance.LifeLost();
			Die();
		}

		if (other.CompareTag("Bullet"))
		{
			GameController.Instance.EnemyDestroyed();
			Destroy(other.gameObject);
			LoseHealth();
		}
	}

	private void LoseHealth()
	{
		lives -= 1;
		if (lives <= 0)
		{
			Die();
		}
	}

	private void Die()
	{
		if (explosionPrefab != null)
		{
			Instantiate(explosionPrefab, transform.position, transform.rotation, GameController.Instance.transform);
		}
		Destroy(gameObject);
	}
}
