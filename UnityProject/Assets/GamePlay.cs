using UnityEngine;
using System.Collections;

public class GamePlay : MonoBehaviour {

	private static int counter = 0,
						remaining = 0;

	private static data d; 

	void OnLevelWasLoaded(int l)
	{
		counter = 0;
		d = GetComponent<data>();
		Debug.Log (d.moveRange);
	}

	public static void counterUp(){
		counter++;
		Debug.Log (remaining - counter);
	}

	public static void setRemaining(int r)
	{
		remaining = (int)Mathf.Ceil(r/d.moveRange) + 1;
	}
}