using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	[SerializeField]
	private Shade shade;

	private float cameraSizeMin, cameraSizeMax, zoomSpeed;
	private Vector3 cameraPosMin, cameraPosMax, cameraPosMid, cameraPosMinScale, cameraPosMaxScale, startPoint;
	private int level;

	public float sensitivity;

	void Awake () {
		ColorPallet.coloring += Recolor;
	}

	void Recolor(){
		this.camera.backgroundColor = ColorPallet.pallet[ColorPallet.i].colors[(int)shade].color;
	}

	void Start () {
		level = data.level;
		cameraSizeMin = data.cameraSizeMin;
		zoomSpeed = data.zoomSpeed;

		if(SaveData.GetSave().CurrentLevel >= data.level){
			setCameraPos(SaveData.GetSave().Width,SaveData.GetSave().Height);
		}else{
			setCameraPos(data.width, data.height);
		}
	}

	void setCameraPos(int w, int h)
	{
		cameraPosMin = new Vector3 (0, 0, -10);
		cameraPosMax = new Vector3 (w * 10, h * 10, -10);
		cameraPosMid = new Vector3 ((w - 1) * 5, (((17F / 3F) * h) - 6), -10);
		cameraPosMinScale = cameraPosMaxScale = cameraPosMid;
	}

	void Update () {
		if (Input.touchCount == 2)
		{
			Touch touchZero = Input.GetTouch (0);
			Touch touchOne = Input.GetTouch (1);

			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

			Vector3 center = new Vector3((touchOne.position.x + touchZero.position.x)/2,
			                             (touchOne.position.y + touchZero.position.y)/2,
			                             0);
			Vector3 prevCenter = new Vector3((touchOnePrevPos.x + touchZeroPrevPos.x)/2,
			                             (touchOnePrevPos.y + touchZeroPrevPos.y)/2,
			                             0);
			Vector3 centerDiff = (prevCenter - center) * (sensitivity * this.camera.orthographicSize);

			zoom (deltaMagnitudeDiff * -zoomSpeed, centerDiff);
		}
	}
	
	void zoom(float input, Vector3 Center)
	{
		input *= level;
		float change = this.camera.orthographicSize - input;
		this.camera.orthographicSize = Mathf.Clamp(change, cameraSizeMin, cameraSizeMax);
		cameraPosMinScale = Vector3.Lerp (cameraPosMin, 
		                               cameraPosMid, 
		                               zoomRatio (this.camera.orthographicSize, cameraSizeMin, cameraSizeMax));
		cameraPosMaxScale = Vector3.Lerp (cameraPosMax, 
		                                  cameraPosMid, 
		                                  zoomRatio (this.camera.orthographicSize, cameraSizeMin, cameraSizeMax));
		pan(Center);
	}

	float zoomRatio(float current, float min, float max){
		return (current - min)/(max - min);
	}

	void pan (Vector3 input)
	{
		Vector3 newPosition = transform.position + input;
		newPosition = new Vector3 (Mathf.Clamp (newPosition.x, cameraPosMinScale.x, cameraPosMaxScale.x),
		                           Mathf.Clamp (newPosition.y, cameraPosMinScale.y, cameraPosMaxScale.y),
		                           Mathf.Clamp (newPosition.z, cameraPosMinScale.z, cameraPosMaxScale.z));
		transform.position = newPosition;
	}

	public void setMax(float max)
	{
		this.camera.orthographicSize = cameraSizeMax = max + data.cameraZoomOffset;
	}
}
