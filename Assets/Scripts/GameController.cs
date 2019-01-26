﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
	public float enemySpawnPeriod = 1;
	public GameObject enemyPrefab;
	public float spawnDistance = 1;
	public Transform earth;
	public int EarthLives = 3;
	public bool GameOver = false;

	public static GameController Instance;

	private float timeSinceLastEnemy;

	// Use this for initialization
	void Start ()
	{
		Instance = this;
	}
	
	// Update is called once per frame
	void Update ()
	{
		timeSinceLastEnemy += Time.deltaTime;
		if (timeSinceLastEnemy >= enemySpawnPeriod && enemyPrefab != null && earth != null)
		{
			timeSinceLastEnemy -= enemySpawnPeriod;
			var position = spawnDistance * Random.insideUnitCircle.normalized;
			var position3 = new Vector3(position.x, position.y, 0);
			var rotation = Quaternion.FromToRotation(Vector3.up, earth.position - position3);
			Instantiate(enemyPrefab, position3, rotation, transform);
		}
	}

	public void LifeLost()
	{
		// TODO
		Debug.Log("LifeLost");
		EarthLives-=1;
		if(EarthLives<=0)
		{
			GameOver=true;		// game over screen
		} 
	}

	public void EnemyDestroyed()
	{
		// TODO
		Debug.Log("EnemyDestroyed");
	}
}
