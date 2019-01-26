using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public float enemySpawnPeriod = 1;
	public GameObject enemyPrefab;
	public float spawnDistance = 1;
	public Transform earth;

	private float timeSinceLastEnemy;

	// Use this for initialization
	void Start () {
		
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
			Instantiate(enemyPrefab, position3, rotation);
		}
	}
}
