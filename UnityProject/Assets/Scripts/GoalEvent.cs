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
		floorController = GameObject.Find ("Origin").GetComponent<MazeBuilder>().ground[x][y].GetComponent<FloorController>();
	}

	void OnTriggerEnter(Collider col) 
	{
		IncrementData();
		Application.LoadLevel (Application.loadedLevel);
	}

	void OnMouseDown()
	{
		floorController.watch();
	}

	void IncrementData()
	{
		GameObject.Find ("Data").GetComponent<data>().IncrementData();
	}
}