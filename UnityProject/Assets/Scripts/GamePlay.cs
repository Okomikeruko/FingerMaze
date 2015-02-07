using UnityEngine;
using System.Collections;

public class GamePlay : MonoBehaviour {

	public delegate void Counters();
	public static event Counters counters;

	private static int counter = 0,	remaining = 0;

	public static void updateCounters()
	{
		if (counters!= null)
		{
			counters();
		}
	}

	public static void ClearCounters()
	{
		counters = null;
	}

	void OnLevelWasLoaded(int l)
	{
		counter = 1;
		updateCounters ();
	}

	public static void counterUp(){
		counter++;
	}

	public static void setRemaining(int r)
	{
		remaining = (int)Mathf.Ceil(r/data.moveRange) + 1;
	}

	public static int getCounter()
	{
		return remaining - counter; 
	}

	public static int getScore()
	{
		return 500;
	}
}