using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	private float cameraSizeMin, cameraSizeMax, zoomSpeed;
	private Vector3 cameraPosMin, cameraPosMax, startPoint;

	void Start () {
		data d = GameObject.Find ("Data").GetComponent<data>();
		cameraSizeMin = d.cameraSizeMin;
		zoomSpeed = d.zoomSpeed;
		cameraPosMin = new Vector3 (0, 0, -10);
		cameraPosMax = new Vector3 (d.width * 10, d.height * 10, -10);
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
		float change = this.camera.orthographicSize - input;
		this.camera.orthographicSize = Mathf.Clamp(change, cameraSizeMin, cameraSizeMax);
	}

	void pan (Vector3 input)
	{
		Vector3 newPosition = transform.position + input;
		newPosition = new Vector3 (Mathf.Clamp (newPosition.x, cameraPosMin.x, cameraPosMax.x),
		                           Mathf.Clamp (newPosition.y, cameraPosMin.y, cameraPosMax.y),
		                           Mathf.Clamp (newPosition.z, cameraPosMin.z, cameraPosMax.z));
		transform.position = newPosition;
	}

	public void setMax(float max)
	{
		this.camera.orthographicSize = cameraSizeMax = max;
	}
}
