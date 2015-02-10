using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

public static class SaveData {
	
	private static Save save;
	
	static void savePuzzle(List<MazeNode> maze, int level)
	{
		save.CurrentMaze = maze;
		save.CurrentLevel = level;
	}
	
	static void saveMove(MazeNode position, int moves, int score)
	{
		save.CurrentPosition = position;
		save.CurrentRemainingMoves = moves;
		save.CurrentScore = score;
		if (save.HighScore < save.CurrentScore)
		{
			save.HighScore = save.CurrentScore;
		}
	}
	
	static void reset()
	{
		int h = save.HighScore;
		save = new Save();
		save.HighScore = h;
	}
}

public class Save {
	
	public List<MazeNode> CurrentMaze;
	public int CurrentLevel;
	public MazeNode CurrentPosition;
	public int CurrentRemainingMoves;
	public int CurrentScore;
	public int HighScore;
	
	public Save(){ 
		CurrentMaze = new List<MazeNode>();
		CurrentPosition = new MazeNode();
	}
}