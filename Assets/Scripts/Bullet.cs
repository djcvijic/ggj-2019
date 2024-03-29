﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float moveSpeed = 1;
	public Vector2 screenBounds;

	// Use this for initialization
	void Start ()
	{
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
}
