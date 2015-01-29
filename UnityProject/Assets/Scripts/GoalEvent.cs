using UnityEngine;
using System.Collections;

public class GoalEvent : MonoBehaviour {

	void OnTriggerEnter(Collider col) {
		Application.LoadLevel (Application.loadedLevel);
	}
}