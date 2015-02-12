using UnityEngine;
using System.Collections;

public class data : MonoBehaviour {

	public delegate void Reset();
	public static event Reset reset; 

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
	private float Slope = -0.375F,
	Intercept = 4.375f,
	CameraSizeMin = 5,
	ZoomSpeed = 5,
	CameraZoomOffset = 5;

	[SerializeField]
	private Vector3 CameraOffset = new Vector3 (5, 5, 0);

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

	public static float slope,
				 intercept,
				 cameraSizeMin,
				 zoomSpeed,
				 cameraZoomOffset;

	public static Vector3 cameraOffset;

	void Awake () {
		reset += ResetData;
		CallReset();
	}

	void Start () {
		DontDestroyOnLoad(this);
		Application.LoadLevel("main");
	}

	public static void LoadSavedData(Save saveData) {
		width = saveData.Width;
		height = saveData.Height;
		level = saveData.CurrentLevel;
		for (int i = 1; i < level; i++)	{
			if (moveRange < maxMoveRange) {
				moveRange++;
			}
		}
	}

	public static void CallReset(){
		if(reset != null) { 
			reset(); 
		} 
	}

	private void ResetData(){
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
		slope = Slope;
		intercept = Intercept;
		zoomSpeed = ZoomSpeed;
		cameraZoomOffset = CameraZoomOffset;
		cameraOffset = CameraOffset;
	}

	public static void IncrementData(){
		width += (width < maxWidth) ? widthIncrement : 0;
		height += (height < maxHeight) ? heightIncrement : 0;
		moveRange += (moveRange < maxMoveRange) ? moveRangeIncrement : 0;
		level++;
	}
}
