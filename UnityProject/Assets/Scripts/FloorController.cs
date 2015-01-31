using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloorController : MonoBehaviour {

	public int x, y;

	[SerializeField] 
	private Color clickableColor, originalColor;

	public delegate void watcher();
	public watcher watch;

	private bool clickable = false;
	private MazeBuilder mazeBuilder;
	private motionControl mc;

	void Start() {
		mazeBuilder = GameObject.Find("Origin").GetComponent<MazeBuilder>();
		mc = GameObject.Find("Hero").GetComponent<motionControl>();
	}

	void OnMouseDown() {
		watch();
	}

	public void makeClickable() {
		clickable = true;
		renderer.material.color = clickableColor;
		watch = click;
	}

	public void restore() {
		clickable = false;
		renderer.material.color = originalColor;
		watch = empty;
	}

	void click() {
		StartCoroutine(move(mazeBuilder.GetPath(x, y)));
	}

	void empty() {}

	IEnumerator move(List<MazeNode> Path){
		mazeBuilder.ClearMaze();
		GameObject Hero = GameObject.Find ("Hero");
		while(Path.Count > 1){
			Vector3 last = new Vector3(Path[0].x, Path[0].y, 0) * 10;
			Vector3 next = new Vector3(Path[1].x, Path[1].y, 0) * 10;
			float tween = 0;
			while(Vector3.Distance(Hero.transform.position, next) > 0.5f){
				Hero.transform.position = Vector3.Lerp(last, next, tween);
				yield return new WaitForEndOfFrame();
				tween += Time.deltaTime * mc.speed;
			}
			Path.RemoveAt (0);
		}
		mazeBuilder.SetClickZone(x, y);
	}
}
