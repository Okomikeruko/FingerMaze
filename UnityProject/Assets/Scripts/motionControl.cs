using UnityEngine;
using System.Collections;

public class motionControl : MonoBehaviour {

	[SerializeField] float speed = 1;

	void Update () {
		rigidbody.velocity = new Vector3 (Input.GetAxis ("Horizontal"),
										  Input.GetAxis ("Vertical"),
										  0) * speed;
	}
}