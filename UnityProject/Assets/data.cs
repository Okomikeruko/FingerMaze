using UnityEngine;
using System.Collections;

public class data : MonoBehaviour {

	public int width = 1, 
				height = 1, 
				widthIncrement = 1,  
				heightIncrement = 1, 
				moveRange = 5,
				moveRangeIncrement = 0;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this);
		Application.LoadLevel("main");
	}

	public void IncrementData(){
		width += widthIncrement;
		height += heightIncrement;
		moveRange += moveRangeIncrement;
	}
}
