using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeBuilder : MonoBehaviour {

	[SerializeField] private int width = 1, height = 1;
	[SerializeField] private GameObject floor, wall, parent, goal, camera;

	private GameObject o;
	private Camera cam;
	private MazeNode start;

	private List<List<GameObject>> ground, vertWalls, horiWalls; 
	private List<MazeNode> maze;

	void Start () {

		maze = new List<MazeNode>();
		ground = new List<List<GameObject>> ();
		vertWalls = new List<List<GameObject>> ();
		horiWalls = new List<List<GameObject>> ();
		cam = camera.GetComponent<Camera> ();

		// ******************** Size Camera **********************

		camera.transform.position = new Vector3 ((width - 1) * 5, (height - 1) * 5, -10);
		cam.orthographicSize = height * 5;

		// ******************** Full Grid Build *************************

		if (width > 0 && height > 0) {
			o = Instantiate (goal, new Vector3((width - 1) * 10, (height -1) * 10, 0), Quaternion.identity) as GameObject;

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


		start = maze[Random.Range(0, maze.Count - 1)];
		start.start = true;
		start.makeMaze ();
	
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

	}
}

public class MazeNode {

	enum direction
	{
		north,
		east,
		south,
		west,
	}

	public int x, y;
	public string xy {
		get{ return "(" + x + ", " + y + ")"; }
	} 
	public bool active = true;
	public bool start = false;

	public MazeNode[] AdjacentNodes;
	public MazeNode back;

	public MazeNode() {
		AdjacentNodes = new MazeNode[4];
	}

	public MazeNode(int a, int b) {
		x = a;
		y = b;
		AdjacentNodes = new MazeNode[4];
	}

	public void makeMaze() {
		active = false;

		List<MazeNode> availableNodes = new List<MazeNode>();
		foreach (MazeNode node in AdjacentNodes){
			if (node != null && node.active){
				availableNodes.Add (node);
			}
		}
		if (availableNodes.Count > 0)
		{
			MazeNode next = availableNodes[Random.Range(0, availableNodes.Count)];
			next.back = this;
			next.makeMaze();
		}
		else if (back != null)
		{
			back.makeMaze ();
		}
	}

	public void FindAdjacentNodes(List<MazeNode> maze) {
		foreach (MazeNode node in maze) {
			if(isAdjacent(node))
			{
				AdjacentNodes[(int)AdjacentNodeDirection(node)] = node;
			}
		}
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
