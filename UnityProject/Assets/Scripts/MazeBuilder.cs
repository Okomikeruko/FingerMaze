using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MazeBuilder : MonoBehaviour {

	[SerializeField] private GameObject floor, wall, parent, goal, camera;

	public int width = 1, height = 1, moveRange = 5;
	private GameObject o;
	private CameraControl cam;
	private MazeNode start, current;

	public List<List<GameObject>> ground, vertWalls, horiWalls; 
	private List<MazeNode> maze;

	public List<MazeNode> solution;


	void Start () {

		setValues();

		maze = new List<MazeNode>();
		solution = new List<MazeNode>();
		ground = new List<List<GameObject>> ();
		vertWalls = new List<List<GameObject>> ();
		horiWalls = new List<List<GameObject>> ();
		cam = camera.GetComponent<CameraControl> ();

		// ******************** Size Camera **********************

		camera.transform.position = new Vector3 ((width - 1) * 5, (height - 1) * 5, -10);
		cam.setMax(height * 5);

		// ******************** Full Grid Build *************************

		if (width > 0 && height > 0) {
			o = Instantiate (goal, new Vector3((width - 1) * 10, (height -1) * 10, -1), Quaternion.identity) as GameObject;

			for (int i = 0; i <= width; i++)
			{
				List<GameObject> column = new List<GameObject>();
				List<GameObject> vert = new List<GameObject>();
				List<GameObject> hori = new List<GameObject>();
				for (int j = 0; j < height; j++)
				{

					if (i < width){
			// Floors
						o = Instantiate (floor, 
					                     new Vector3(i * 10, j * 10, 0), 
					                     Quaternion.Euler(270, 0, 0)) as GameObject;
						o.GetComponent<FloorController>().x = i;
						o.GetComponent<FloorController>().y = j;
						column.Add (o);
						maze.Add (new MazeNode(i,j));

			// Horizontal Walls
						o = Instantiate (wall, 
						                 new Vector3(i * 10, (j * 10) - 5, 0), 
						                 Quaternion.Euler(0, 0, 90)) as GameObject;
						hori.Add (o);
					}
			// Vertical Walls
					o = Instantiate (wall, 
					                 new Vector3((i * 10) - 5, j * 10, 0), 
					                 Quaternion.Euler(0, 0, 0)) as GameObject;
					vert.Add (o);
				}
			// Top Horizontal Walls
				if (i < width){
					o = Instantiate (wall, 
					                 new Vector3(i * 10, (height * 10) - 5, 0), 
					                 Quaternion.Euler(0, 0, 90)) as GameObject;
				}
				ground.Add( column );
				vertWalls.Add ( vert );
				horiWalls.Add ( hori );
			}
		}

		foreach (MazeNode node in maze){
			node.FindAdjacentNodes(maze);
		}

		// ***************** Create Maze *******************
		
		start = maze[0];
		start.start = true;
		start.makeMaze ();
		Debug.Log (solution.Count);

		// **************** Delete Walls to Form Maze *******************

		foreach (MazeNode node in maze){
			if (node.back != null){
				if (node.back.x != node.x){
					Destroy (vertWalls[Mathf.Max (node.x, node.back.x)][node.y]);
				}
				if (node.back.y != node.y){
					Destroy (horiWalls[node.x][Mathf.Max (node.y, node.back.y)]);
				}
			}
		}

		// ***************** Set Remaining Object to Maze Parent Object ***************

		foreach (GameObject item in GameObject.FindGameObjectsWithTag("Maze")){
			item.transform.parent = parent.transform;
		}
		current = maze.Find (n => n.xy == "(0, 0)");
		ClearMaze ();
		SetClickZone(0, 0);

	}

	public void SetClickZone(int x, int y)
	{
		List<MazeNode> clickArea = new List<MazeNode>();
		current = maze.Find (n => n.xy == "("+x+", "+y+")");
		clickArea = MazeNode.getRange (current, moveRange);
		foreach (MazeNode area in clickArea){
			ground[area.x][area.y].GetComponent<FloorController>().makeClickable ();
		}
	}

	public List<MazeNode> GetPath(int x, int y)
	{
		List<List<MazeNode>> PathFinder = new List<List<MazeNode>>();
		for (int l = 0; l <= moveRange; l++){
			if(PathFinder.Count == 0){
				List<MazeNode> FirstPath = new List<MazeNode>();
				FirstPath.Add (current);
				PathFinder.Add (FirstPath);
			}else{
				List<List<MazeNode>> temp = new List<List<MazeNode>>();
				foreach (List<MazeNode> path in PathFinder){
					List<MazeNode> forks = new List<MazeNode>();
					foreach (MazeNode way in path.Last().GetAllConnections()){
						forks.Add (way);
					}
					if(forks.Count > 0) {
						foreach(MazeNode way in forks){
							if(!path.Contains (way)){
								temp.Add (new List<MazeNode>(path));
								temp.Last().Add(way);
							}
						}
					}
				}
				if (temp.Count > 0)
				{
					foreach (List<MazeNode> newPath in temp){
						PathFinder.Add (newPath);
					}
				}
				foreach (List<MazeNode> output in PathFinder) {
					if (output.Exists(n => n.xy == "(" + x + ", " + y + ")"))
					{
						return output;
						break;
					}
				}
			}
		}
		return null;
	}

	public void ClearMaze(){
		foreach (MazeNode node in maze){
			ground[node.x][node.y].GetComponent<FloorController>().restore ();
		}
	}

	private void setValues()
	{
		data d = GameObject.Find ("Data").GetComponent<data>();
		width = d.width;
		height = d.height;
		moveRange = d.moveRange;
	}
}

public class MazeNode {

	enum direction { north, east, south, west }

	public int x, y;
	public string xy { get{ return "(" + x + ", " + y + ")"; } } 
	public bool active = true, start = false;
	public MazeNode[] AdjacentNodes;
	public List<MazeNode> forwards;
	public MazeNode back;

	public MazeNode() {
		AdjacentNodes = new MazeNode[4];
		forwards = new List<MazeNode> ();
	}

	public MazeNode(int a, int b) {
		x = a;
		y = b;
		AdjacentNodes = new MazeNode[4];
		forwards = new List<MazeNode> ();
	}

	public void makeMaze() {
		active = false;
		List<MazeNode> availableNodes = new List<MazeNode>();
		List<MazeNode> solution = GameObject.Find("Origin").GetComponent<MazeBuilder>().solution;
		int lastX = GameObject.Find("Origin").GetComponent<MazeBuilder>().width - 1;
		int lastY = GameObject.Find("Origin").GetComponent<MazeBuilder>().height - 1;

		foreach (MazeNode node in AdjacentNodes){
			if (node != null && node.active){
				availableNodes.Add (node);
			}
		}
		if (availableNodes.Count > 0){
			MazeNode next = availableNodes[Random.Range(0, availableNodes.Count)];
			next.back = this;
			forwards.Add(next);
			if(!solution.Exists(n => n.xy == "(" + lastX + ", " + lastY + ")")){
				solution.Add (next);
			}
			next.makeMaze();
		} else if (back != null) {
			if(!solution.Exists(n => n.xy == "(" + lastX + ", " + lastY + ")")){
				solution.RemoveAt(solution.Count - 1);
			}
			back.makeMaze ();
		}
	}

	public void FindAdjacentNodes(List<MazeNode> maze) {
		foreach (MazeNode node in maze) {
			if(isAdjacent(node)) {
				AdjacentNodes[(int)AdjacentNodeDirection(node)] = node;
			}
		}
	}

	public List<MazeNode> GetAllConnections(){
		List<MazeNode> output = new List<MazeNode>();
		if (back != null) {
			output.Add (back);
		}
		if (forwards.Count > 0){
			foreach(MazeNode forward in forwards){
				output.Add (forward);
			}
		}
		return output;
	}

	static public List<MazeNode> getRange (MazeNode origin, int range){
		List<MazeNode> output = new List<MazeNode> ();
		output.Add (origin);
		for (int i = 0; i < range; i++){
			List<MazeNode> temp = new List<MazeNode> ();
			foreach (MazeNode node in output){
				if (node.back != null) {
					temp.Add (node.back);
				}
				if (node.forwards.Count > 0){
					foreach (MazeNode n in node.forwards){
						temp.Add (n);
					}
				}
			}
			foreach (MazeNode node in temp)
			{
				if (!output.Contains(node)){
				    output.Add (node);
				}
			}
		}
		return output;
	}

	private bool isAdjacent (MazeNode node)
	{
		return (node.x == x && Mathf.Abs (node.y - y) == 1) || (node.y == y && Mathf.Abs (node.x - x) == 1);
	}

	private direction AdjacentNodeDirection (MazeNode node)
	{
		if ( x == node.x ){
			if (y < node.y){
				return direction.north;
			}else{
				return direction.south;
			}
		} else {
			if ( x < node.x ){
				return direction.east;
			}else{
				return direction.west;
			}
		}
	}
}