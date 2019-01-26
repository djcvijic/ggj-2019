using System;
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
	public float difficultyCurveSpeed = 1;
	public AnimationCurve difficultyCurve;

	public static GameController Instance;

	private float timeSinceInitialization;
	private float timeSinceLastEnemy;

	// Use this for initialization
	void Start ()
	{
		Instance = this;
		timeSinceInitialization = 0;
		timeSinceLastEnemy = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		var actualSpawnPeriod = enemySpawnPeriod;
		if (difficultyCurve != null)
		{
			timeSinceInitialization += Time.deltaTime;
			var difficultyTime = timeSinceInitialization * difficultyCurveSpeed;
			actualSpawnPeriod *= 1 - difficultyCurve.Evaluate(difficultyTime);
		}

		timeSinceLastEnemy += Time.deltaTime;
		if (timeSinceLastEnemy >= actualSpawnPeriod && enemyPrefab != null && earth != null)
		{
			timeSinceLastEnemy -= actualSpawnPeriod;
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
	}

	public void EnemyDestroyed()
	{
		// TODO
		Debug.Log("EnemyDestroyed");
	}
}
