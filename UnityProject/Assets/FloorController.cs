using UnityEngine;
using System.Collections;

public class FloorController : MonoBehaviour {

	public int x, y;

	[SerializeField] 
	private Color clickableColor;

	delegate void watcher();
	watcher watch;

	private bool clickable = false;
	private Color originalColor;
	private MazeBuilder mazeBuilder;

	void Start() {
		originalColor = renderer.material.color;
		mazeBuilder = GameObject.Find("Origin").GetComponent<MazeBuilder>();
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

		Debug.Log ("(" + x + ", " + y + ")");
	}

	void empty() {}
}
