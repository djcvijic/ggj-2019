using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	public Transform camera;
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
	public SpriteRenderer earthRenderer;
	public List<Sprite> earthSprites;
	public SpriteRenderer cover;
	public float coverSustain = 1;
	public float coverDecay = 1;

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
		earthRenderer.sprite = earthSprites[currentLives];
		state = State.Start;
		startCanvas.SetActive(false);
		endCanvas.SetActive(false);
		if (cover != null)
		{
			StartCoroutine(FadeOutCover());
		}
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

	public void PlayExplosion()
	{
		if (audioSource != null) audioSource.Play();
	}

	public void LifeLost()
	{
		earth.GetComponent<Earth>().PlayExplosion();


		currentLives-=1;
		earthRenderer.sprite = earthSprites[currentLives];
		if(currentLives<=0)
		{
			GameOver();
		} 
	}

	private IEnumerator FadeOutCover()
	{
		yield return new WaitForSeconds(coverSustain);
		var fadeTime = 0f;
		while (fadeTime < coverDecay)
		{
			fadeTime += Time.deltaTime;
			var alpha = Mathf.Lerp(1, 0, fadeTime / coverDecay);
			cover.color = new Color(cover.color.r, cover.color.g, cover.color.b, alpha);
			yield return null;
		}
		startCanvas.SetActive(true);
	}

	private void GameOver() {

		state=State.GameOver;		// game over screen
		startCanvas.SetActive(false);
		endCanvas.SetActive(true);
	}

	private void Restart()
	{
		if (earth != null) earth.GetComponent<Earth>().Restart();
		if (robit != null) robit.Restart();
		timeSinceInitialization = 0;
		timeSinceLastEnemy = 0;
		currentLives = EarthLives;
		earthRenderer.sprite = earthSprites[currentLives];
		RunTheAction();
	}

	private void RunTheAction()
	{
		state = State.Running;
		startCanvas.SetActive(false);
		endCanvas.SetActive(false);
	}
}
