using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GoalEvent : MonoBehaviour {

	private FloorController floorController;

	void Start()
	{
		int x = (int)transform.position.x / 10;
		int y = (int)transform.position.y / 10;
		floorController = MazeBuilder.ground[x][y].GetComponent<FloorController>();
	}

	void OnTriggerEnter(Collider col) 
	{
		GamePlay.scoreUp(GamePlay.getCounter() * 50);
		data.IncrementData();
		ColorPallet.Clear ();
		GamePlay.ClearCounters();
		Application.LoadLevel (Application.loadedLevel);
	}

	void OnMouseDown()
	{
		floorController.watch();
	}
}