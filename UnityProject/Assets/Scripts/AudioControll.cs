using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(AudioSource))]
public class AudioControll : MonoBehaviour {

	public List<AudioBit> AudioBits;

	private AudioSource audioSource;

	void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	public void PlayClip(string audioName)
	{
		AudioBit bit = AudioBits.Find (i => i.name == audioName);
		if (bit != null)
		{
			audioSource.clip = bit.audioClip;
			audioSource.Play();
		}
	}
}

[System.Serializable]
public class AudioBit 
{
	[SerializeField]
	public string name;

	[SerializeField]
	public AudioClip audioClip;
}