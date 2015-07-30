using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GoalEvent : MonoBehaviour {

	private FloorController floorController;

	private AsyncOperation async;

	void Start()
	{
		int x = (int)transform.position.x / 10;
		int y = (int)transform.position.y / 10;
		floorController = MazeBuilder.ground[x][y].GetComponent<FloorController>();
	}

	void OnTriggerEnter(Collider col) 
	{
		GamePlay.CallVictory(true); 
		StartCoroutine("Reload");
	}

	void OnMouseDown()
	{
		floorController.watch();
	}

	IEnumerator Reload() {
		data.IncrementData ();
		ColorPallet.Clear ();
		GamePlay.ClearCounters ();
		GamePlay.scoreUp (GamePlay.getCounter () * 50);
		async = Application.LoadLevelAsync (Application.loadedLevel);
		async.allowSceneActivation = false;
		yield return new WaitForSeconds (5.0f);
		async.allowSceneActivation = true;
//		yield return async;
//		GamePlay.CallVictory (false);
		GamePlay.ClearVictory ();
//		yield return async;
	}

	public void startNextLevel(){
		async.allowSceneActivation = true;
	}
}