﻿using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	private float cameraSizeMin, cameraSizeMax, zoomSpeed;
	private Vector3 cameraPosMin, cameraPosMax, cameraPosMid, cameraPosMinScale, cameraPosMaxScale, startPoint;
	private int level;

	void Start () {
		data d = GameObject.Find ("Data").GetComponent<data>();
		level = d.level;
		cameraSizeMin = d.cameraSizeMin;
		zoomSpeed = d.zoomSpeed;
		cameraPosMin = new Vector3 (0, 0, -10);
		cameraPosMax = new Vector3 (d.width * 10, d.height * 10, -10);
		cameraPosMid = Vector3.Scale(cameraPosMax, new Vector3(0.5f, 0.5f, 1));
		cameraPosMid -= new Vector3 (5, 5, 0);
		cameraPosMinScale = cameraPosMaxScale = cameraPosMid;
		Debug.Log (cameraPosMid);
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

			zoom (deltaMagnitudeDiff * -zoomSpeed);
		}


		zoom (Input.GetAxis ("Mouse ScrollWheel") * zoomSpeed);

		if (Input.GetMouseButtonDown (1) || Input.GetMouseButtonDown (2))
		{
			startPoint = Input.mousePosition;
		}

		if (Input.GetMouseButton (1) || Input.GetMouseButton (2))
		{
			pan ((startPoint - Input.mousePosition) * 0.1F);
			startPoint = Input.mousePosition;
		}
	}

	void zoom(float input)
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
		//pan(Vector3.zero);
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
		this.camera.orthographicSize = cameraSizeMax = max + 5;
	}
}
