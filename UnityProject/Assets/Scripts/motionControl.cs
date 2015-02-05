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

	public static IEnumerator move(List<MazeNode> Path/*, Transform t = motionControl.tr*/){
		MazeBuilder.ClearMaze();
		int x = Path[Path.Count - 1].x;
		int y = Path[Path.Count - 1].y;
	//	shade = clickingShade;
		ColorPallet.CallRecolor ();
		while(Path.Count > 1){
			Vector3 last = new Vector3(Path[0].x, Path[0].y, 0) * 10;
			Vector3 next = new Vector3(Path[1].x, Path[1].y, 0) * 10;
			float tween = 0;
			while(Vector3.Distance(motionControl.tr.position, next) > 0.5f){
				motionControl.tr.position = Vector3.Lerp(last, next, tween);
				yield return new WaitForEndOfFrame();
				tween += Time.deltaTime * motionControl.s;
			}
			Path.RemoveAt (0);
		}
		MazeBuilder.SetClickZone(x, y);
	}
}