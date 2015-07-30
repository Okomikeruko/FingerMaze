using UnityEngine;
using System.Collections;

public class VictoryEvent : MonoBehaviour {

	public GameObject VictoryScreen;

	void Awake(){
		GamePlay.victory += Victory;
	}
	
	void Victory(bool set) {
		VictoryScreen.SetActive(set);
	}
}
