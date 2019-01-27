using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	public float enemySpawnPeriod = 1;
	public GameObject enemyPrefab;
	public float spawnDistance = 1;
	public Transform earth;
	public Robit robit;
	public int EarthLives = 3;
	public float difficultyCurveSpeed = 1;
	public AnimationCurve difficultyCurve;
	public Text title;

	public enum State {Start, Running, GameOver}
	[NonSerialized]
	public State state;

	public static GameController Instance;

	private float timeSinceInitialization;
	private float timeSinceLastEnemy;
	private int currentLives;

	// Use this for initialization
	void Start ()
	{
		Instance = this;
		timeSinceInitialization = 0;
		timeSinceLastEnemy = 0;
		currentLives = EarthLives;
		state = State.Start;
		title.text = "Play";
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Instance.state == State.Running)
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
		else if (Instance.state == State.GameOver && Input.GetButtonDown("Fire1"))
		{
			Restart();
		}
		else if (Instance.state == State.Start && Input.GetButtonDown("Pew"))
		{
			Instance.state = State.Running;
			title.text = "";
		}
	}

	public void LifeLost()
	{
		// TODO
		Debug.Log("LifeLost");
		currentLives-=1;
		if(currentLives<=0)
		{
			GameOver();
		} 
	}

	private void GameOver() {

			state=State.GameOver;		// game over screen
			title.text = "Game Over. Restart?";
	}



	public void EnemyDestroyed()
	{
		// TODO
		Debug.Log("EnemyDestroyed");
	}

	private void Restart()
	{
		if (earth != null) earth.GetComponent<Earth>().Restart();
		if (robit != null) robit.Restart();
		timeSinceInitialization = 0;
		timeSinceLastEnemy = 0;
		currentLives = EarthLives;
		state = State.Running;
		title.text = "";
	}
}
