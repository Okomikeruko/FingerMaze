using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloorController : MonoBehaviour {

	public int x, y;

	[SerializeField]
	private Shade clickableShade = Shade.darkest, 
				  clickingShade = Shade.darkest, 
				  originalShade = Shade.darkest;
	private Shade shade;

	public delegate void watcher();
	public watcher watch;

//	private bool clickable = false;

	void Awake() {
		ColorPallet.coloring += Recolor;
	}

	void Recolor(){
		renderer.material.color = ColorPallet.pallet[ColorPallet.i].colors[(int)shade].color;
	}

	void Update(){

		bool ctrl = (Input.GetKey(KeyCode.LeftControl) || Input.GetKey (KeyCode.RightControl));
		if(Input.GetMouseButtonDown (1) && ctrl && x == 0 && y == 0 )
		{
			StartCoroutine(motionControl.move(MazeBuilder.solution));
			foreach (MazeNode n in MazeBuilder.solution)
			{
				MazeBuilder.ground[n.x][n.y].gameObject.GetComponent<FloorController>().shade = Shade.mid;
				MazeBuilder.ground[n.x][n.y].renderer.material.color = Color.cyan;
			}
		}

		this.gameObject.GetComponent<MeshRenderer>().enabled = (shade != originalShade);
	}

	void OnMouseDown() {
		if (Input.touchCount == 1){
			watch();
		}
	}

	public void makeClickable() {
//		clickable = true;
		shade = clickableShade;
		Recolor ();
		watch = click;
	}

	public void restore() {
//		clickable = false;
		shade = originalShade;
		Recolor ();
		watch = empty;
	}

	void click() {
		shade = clickingShade;
		StartCoroutine(motionControl.move(MazeBuilder.GetPath(x, y)));
	}

	void empty() {}
}
