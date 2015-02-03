using UnityEngine;
using System.Collections;

public class data : MonoBehaviour {

	public int  level = 1,
			    width = 1, 
				height = 1, 
				maxWidth = 40,
				maxHeight = 30,
				widthIncrement = 1,  
				heightIncrement = 1, 
				moveRange = 5,
				maxMoveRange = 8,
				moveRangeIncrement = 0;

	public float cameraSizeMin = 5,
				 zoomSpeed = 5;

	void Start () {
		DontDestroyOnLoad(this);
		Application.LoadLevel("main");
	}

	public void IncrementData(){
		width += (width < maxWidth) ? widthIncrement : 0;
		height += (width < maxWidth) ? heightIncrement : 0;
		moveRange += (moveRange < maxMoveRange) ? moveRangeIncrement : 0;
		level++;
	}
}
