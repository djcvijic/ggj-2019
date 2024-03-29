﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour 
{
	public float moveSpeed = 1;
	public Vector2 screenBounds;
	public int maxLives = 1;
	public SpriteRenderer spriteRenderer;
	public Sprite[] deathSprites;
	public float deathAnimationLength = 1;

	private int lives;
	private bool isDead;

	// Use this for initialization
	void Start ()
	{
		lives = maxLives;
		var rendererTransform = spriteRenderer.transform;
		rendererTransform.rotation = Quaternion.identity;
		if (transform.position.x > 0)
		{
			rendererTransform.localScale = new Vector3(-rendererTransform.localScale.x, rendererTransform.localScale.y, rendererTransform.localScale.z);
		}
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
		if (isDead) return;

		if (other.CompareTag("Earth"))
		{
			GameController.Instance.LifeLost();
			Die();
		}

		if (other.CompareTag("Bullet"))
		{
			Destroy(other.gameObject);
			LoseHealth();
		}

		if (other.CompareTag("Ulti"))
		{
			Die();
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
		StartCoroutine(DieRoutine());
	}

	private IEnumerator DieRoutine()
	{
		isDead = true;
		GameController.Instance.PlayExplosion();
		var frameLength = deathAnimationLength / deathSprites.Length;
		for (var i = 0; i < deathSprites.Length; i++)
		{
			spriteRenderer.sprite = deathSprites[i];
			yield return new WaitForSeconds(frameLength);
		}
		Destroy(gameObject);
	}
}
