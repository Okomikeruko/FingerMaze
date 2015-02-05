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

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Alpha1)){
			ColorPallet.SetIndex(0);
			ColorPallet.CallRecolor();
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)){
			ColorPallet.SetIndex(1);
			ColorPallet.CallRecolor();
		}
		if(Input.GetKeyDown(KeyCode.Alpha3)){
			ColorPallet.SetIndex(2);
			ColorPallet.CallRecolor();
		}
		if(Input.GetKeyDown(KeyCode.Alpha4)){
			ColorPallet.SetIndex(3);
			ColorPallet.CallRecolor();
		}
		if(Input.GetKeyDown(KeyCode.Alpha5)){
			ColorPallet.SetIndex(4);
			ColorPallet.CallRecolor();
		}
		if(Input.GetKeyDown(KeyCode.Alpha6)){
			ColorPallet.SetIndex(5);
			ColorPallet.CallRecolor();
		}
		if(Input.GetKeyDown(KeyCode.Alpha7)){
			ColorPallet.SetIndex(6);
			ColorPallet.CallRecolor();
		}
		if(Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown (KeyCode.Keypad8)){
			ColorPallet.SetIndex(7);
			ColorPallet.CallRecolor();
		}
	}

	void OnTriggerEnter(Collider col) 
	{
		IncrementData();
		ColorPallet.Clear ();
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