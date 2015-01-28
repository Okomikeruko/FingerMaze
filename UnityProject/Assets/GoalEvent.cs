using UnityEngine;
using System.Collections;

public class GoalEvent : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		Application.LoadLevel (Application.loadedLevel);
	}
}
