using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {

	private bool paused = false;

	public void PauseToggle()
	{
		if(paused){
			Time.timeScale = 1;
		}else{
			Time.timeScale = 0;
		}
		paused = !paused;
	}
}
