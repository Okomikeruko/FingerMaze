using UnityEngine;
using System.Collections;

public class data : MonoBehaviour {

	[SerializeField]
	private int Level = 1,
	Width = 1, 
	Height = 1, 
	MaxWidth = 40,
	MaxHeight = 30,
	WidthIncrement = 1,  
	HeightIncrement = 1, 
	MoveRange = 5,
	MaxMoveRange = 8,
	MoveRangeIncrement = 0;

	[SerializeField]
	private float CameraSizeMin = 5,
	ZoomSpeed = 5,
	CameraZoomOffset = 5;

	public static int level,
					  width, 
					  height, 
					  maxWidth,
					  maxHeight,
					  widthIncrement,  
					  heightIncrement, 
					  moveRange,
					  maxMoveRange,
					  moveRangeIncrement;

	public static float cameraSizeMin,
				 zoomSpeed,
				 cameraZoomOffset;

	public static Vector3 CameraOffset = new Vector3 (5, 5, 0);

	void Awake () {
		level = Level;
		width = Width;
		height = Height; 
		maxWidth = MaxWidth;
		maxHeight = MaxHeight;
		widthIncrement = WidthIncrement;
		heightIncrement = HeightIncrement; 
		moveRange = MoveRange;
		maxMoveRange = MaxMoveRange;
		moveRangeIncrement = MoveRangeIncrement;
		cameraSizeMin = CameraSizeMin;
		zoomSpeed = ZoomSpeed;
		cameraZoomOffset = CameraZoomOffset;
	}

	void Start () {
		DontDestroyOnLoad(this);
		Application.LoadLevel("main");
	}

	public static void IncrementData(){
		width += (width < maxWidth) ? widthIncrement : 0;
		height += (height < maxHeight) ? heightIncrement : 0;
		moveRange += (moveRange < maxMoveRange) ? moveRangeIncrement : 0;
		level++;
	}
}
