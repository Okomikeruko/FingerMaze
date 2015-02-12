using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUICounter : MonoBehaviour {

	public enum CounterType {
		Moves,
		Level,
		Score,
		HighScore
	}

	[SerializeField]
	private CounterType counterType;

	private Text text; 

	void Awake() {
		text = GetComponent<Text>();
	
		switch (counterType){
		case CounterType.Moves:
			GamePlay.counters += movesUpdate;
			break;
		case CounterType.Level:
			GamePlay.counters += levelUpdate;
			break;
		case CounterType.Score:
			GamePlay.counters += scoreUpdate;
			break;
		case CounterType.HighScore:
			GamePlay.counters += highScoreUpdate;
			break;
		default:
			break;
		}
	}

	public void levelUpdate()
	{
		text.text = string.Format("{0:0}", data.level);
	}

	public void scoreUpdate()
	{
		text.text = string.Format ("{0:00000}", GamePlay.getScore());
	}

	public void highScoreUpdate()
	{
		text.text = string.Format ("{0:00000}", GamePlay.getHighScore());
	}

	public void movesUpdate()
	{
		text.text = string.Format("{0:00}", GamePlay.getCounter());
	}
}
