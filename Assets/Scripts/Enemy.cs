using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour 
{
	public float moveSpeed = 1;
	public Vector2 screenBounds;
	public GameObject explosionPrefab;

	// Use this for initialization
	void Start () {
		
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
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Earth"))
		{
			if (explosionPrefab != null)
			{
				Instantiate(explosionPrefab, transform.position, transform.rotation, GameController.Instance.transform);
			}
			GameController.Instance.LifeLost();
			Destroy(gameObject);
		}

		if (other.CompareTag("Bullet"))
		{
			if (explosionPrefab != null)
			{
				Instantiate(explosionPrefab, transform.position, transform.rotation, GameController.Instance.transform);
			}
			GameController.Instance.EnemyDestroyed();
			Destroy(gameObject);
			Destroy(other.gameObject);
		}
	}
}
