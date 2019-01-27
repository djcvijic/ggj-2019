using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
	public AudioSource source;
	public AudioClip intro;
	public AudioClip loop;

	private float introDuration;

	// Use this for initialization
	void Start ()
	{
		StartCoroutine(IntroThenLoop());
	}

	// Update is called once per frame
	void Update () {
		
	}

	private IEnumerator IntroThenLoop()
	{
		source.clip = intro;
		source.Play();
		while (source.isPlaying)
		{
			yield return null;
		}
		source.clip = loop;
		source.loop = true;
		source.Play();
	}
}
