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
	public GameObject startCanvas;
	public GameObject endCanvas;
	public AudioClip bulletSound;
	public AudioClip explosionSound;
	public AudioClip bigExplosionSound;

	public enum State {Start, Running, GameOver}
	[NonSerialized]
	public State state;

	public static GameController Instance;

	private AudioSource audioSource;
	private float timeSinceInitialization;
	private float timeSinceLastEnemy;
	private int currentLives;

	// Use this for initialization
	void Start ()
	{
		Instance = this;
		audioSource = GetComponent<AudioSource>();
		timeSinceInitialization = 0;
		timeSinceLastEnemy = 0;
		currentLives = EarthLives;
		state = State.Start;
		startCanvas.SetActive(true);
		endCanvas.SetActive(false);
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
		else if (Instance.state == State.GameOver && Input.GetButtonDown("Pew"))
		{
			Restart();
		}
		else if (Instance.state == State.Start && Input.GetButtonDown("Pew"))
		{
			RunTheAction();
		}
	}

	public void LifeLost()
	{
		if (audioSource != null && bigExplosionSound != null)
		{
			audioSource.Stop();
			audioSource.clip = bigExplosionSound;
			audioSource.Play();
		}

		currentLives-=1;
		if(currentLives<=0)
		{
			GameOver();
		} 
	}

	private void GameOver() {

		state=State.GameOver;		// game over screen
		startCanvas.SetActive(false);
		endCanvas.SetActive(true);
	}

	public void BulletFired()
	{
		if (audioSource != null && bulletSound != null)
		{
			audioSource.clip = bulletSound;
			audioSource.Play();
		}
	}

	public void EnemyDestroyed()
	{
		if (audioSource != null && explosionSound != null)
		{
			audioSource.clip = explosionSound;
			audioSource.Play();
		}
	}

	private void Restart()
	{
		if (earth != null) earth.GetComponent<Earth>().Restart();
		if (robit != null) robit.Restart();
		timeSinceInitialization = 0;
		timeSinceLastEnemy = 0;
		currentLives = EarthLives;
		RunTheAction();
	}

	private void RunTheAction()
	{
		state = State.Running;
		startCanvas.SetActive(false);
		endCanvas.SetActive(false);
	}
}
