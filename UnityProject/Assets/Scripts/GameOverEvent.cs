using UnityEngine;
using System.Collections;

public class GameOverEvent : MonoBehaviour {

	public GameObject GameOverScreen;

	void Awake(){
		GamePlay.gameOver += GameOver;
	}

	void GameOver()
	{
		GameOverScreen.SetActive(true);
	}
}
