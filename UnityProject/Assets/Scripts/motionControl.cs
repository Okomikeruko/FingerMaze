using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class motionControl : MonoBehaviour {

	public float speed = 1;
	private static float s;
	private static Transform tr;

	void Start(){
		s = speed;
		tr = transform;
	}

	public static IEnumerator move(List<MazeNode> Path){
		GamePlay.counterUp();
		GamePlay.scoreUp(10);
		GamePlay.updateCounters();
		MazeBuilder.ClearMaze();
		int x = Path[Path.Count - 1].x;
		int y = Path[Path.Count - 1].y;
		ColorPallet.CallRecolor ();
		while(Path.Count > 1){
			Vector3 last = new Vector3(Path[0].x, Path[0].y, 0) * 10;
			Vector3 next = new Vector3(Path[1].x, Path[1].y, 0) * 10;
			float tween = 0;
			while(Vector3.Distance(motionControl.tr.position, next) > 0.5f){
				motionControl.tr.position = Vector3.Lerp(last, next, tween);
				tween += Time.deltaTime * motionControl.s;
				yield return new WaitForEndOfFrame();
			}
			Path.RemoveAt (0);
		}
		SaveData.saveMove (new MazeNodeData(x, y), GamePlay.getCounter(), GamePlay.getScore());
		MazeBuilder.SetClickZone(x, y);
	}
}