using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

public class MazeBuilder : MonoBehaviour {
	
	public GameObject floor, baseFloor, wall, wallParent, floorParent, goal, _camera, hero;

	public static int width = 1, height = 1, moveRange = 5, level = 1;
	private GameObject o;
	private CameraControl cam;
	private static MazeNode start, current;

	public static List<List<GameObject>> ground, vertWalls, horiWalls; 
	private static List<MazeNode> maze;

	public static List<MazeNode> solution, toBeginning, toEnd;


	void Start () {

		if(SaveData.GetSave().CurrentLevel >= data.level){
			BuildTheMaze (SaveData.GetSave ());
		} else {
			BuildTheMaze();
		}
		GamePlay.updateCounters();
	}

	public void BuildTheMaze(){
		setValues();
		
		maze = new List<MazeNode>();
		solution = new List<MazeNode>();
		toBeginning = new List<MazeNode>();
		toEnd = new List<MazeNode>();
		ground = new List<List<GameObject>> ();
		vertWalls = new List<List<GameObject>> ();
		horiWalls = new List<List<GameObject>> ();
		cam = _camera.GetComponent<CameraControl> ();
		
		// ******************** Size Camera **********************
		
		_camera.transform.position = new Vector3 ((width - 1) * 5, (((17F / 3F) * height) - 6), -10);
		cam.setMax(((17f / 3f) * height) + 1);
		
		// ******************** Full Grid Build *************************
		
		if (width > 0 && height > 0) {
			o = Instantiate (goal, new Vector3((width - 1) * 10, (height - 1) * 10, -1), Quaternion.identity) as GameObject;
			o = Instantiate (baseFloor, new Vector3((width - 1) * 5, (height - 1 ) * 5, 3), Quaternion.Euler(270, 0, 0)) as GameObject;
			o.transform.localScale = new Vector3 (width, 1, height);
			o.transform.parent = floorParent.transform;
			
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
						                 new Vector3(i * 10, j * 10, 1), 
						                 Quaternion.Euler(270, 0, 0)) as GameObject;
						o.GetComponent<FloorController>().x = i;
						o.GetComponent<FloorController>().y = j;
						o.transform.parent = floorParent.transform;
						column.Add (o);
						maze.Add (new MazeNode(i,j));
						
						// Horizontal Walls
						o = Instantiate (wall, 
						                 new Vector3(i * 10, (j * 10) - 5, 0), 
						                 Quaternion.Euler(0, 0, 90)) as GameObject;
						o.transform.parent = wallParent.transform;
						hori.Add (o);
					}
					// Vertical Walls
					o = Instantiate (wall, 
					                 new Vector3((i * 10) - 5, j * 10, 0), 
					                 Quaternion.Euler(0, 0, 0)) as GameObject;
					o.transform.parent = wallParent.transform;
					vert.Add (o);
				}
				// Top Horizontal Walls
				if (i < width){
					o = Instantiate (wall, 
					                 new Vector3(i * 10, (height * 10) - 5, 0), 
					                 Quaternion.Euler(0, 0, 90)) as GameObject;
					o.transform.parent = wallParent.transform;
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
		
		start = maze[Random.Range(0,maze.Count)];
		start.start = true;
		start.makeMaze ();
		solution = GetSolution(toBeginning, toEnd);
		
		// **************** Delete Walls to Form Maze *******************
		
		foreach (MazeNode node in maze){
			if (node.back != null){
				if (node.back.x != node.x){
					vertWalls[Mathf.Max (node.x, node.back.x)][node.y].transform.parent = null;
					vertWalls[Mathf.Max (node.x, node.back.x)][node.y].GetComponent<CustomColoring>().removeMe();
					Destroy (vertWalls[Mathf.Max (node.x, node.back.x)][node.y]);
				}
				if (node.back.y != node.y){
					horiWalls[node.x][Mathf.Max (node.y, node.back.y)].transform.parent = null;
					horiWalls[node.x][Mathf.Max (node.y, node.back.y)].GetComponent<CustomColoring>().removeMe();
					Destroy (horiWalls[node.x][Mathf.Max (node.y, node.back.y)]);
				}
			}
		}
		
		// ***************** Set Remaining Object to Maze Parent Object ***************
		
		current = maze[0];
		ClearMaze ();
		SetClickZone(0, 0);

		ColorPallet.SetIndex (SaveData.GetSave().ColorIndex);
		ColorPallet.CallRecolor();
		GamePlay.setRemaining(solution.Count);
		
		MeshFilter[] meshFilters = wallParent.GetComponentsInChildren<MeshFilter>();
		CombineInstance[] combine = new CombineInstance[meshFilters.Length-1];
		int z = 0;
		for (int n = 0; n < meshFilters.Length; n++)
		{
			if(meshFilters[n].sharedMesh == null) continue;
			combine[z].mesh = meshFilters[n].sharedMesh;
			combine[z++].transform = meshFilters[n].transform.localToWorldMatrix;
			meshFilters[n].gameObject.SetActive(false);
		}
		wallParent.GetComponent<MeshFilter>().mesh = new Mesh();
		wallParent.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
		wallParent.gameObject.SetActive (true);
		
		// ****************** Save Maze ***********************************************
		
		List<MazeNodeData> mazeData = new List<MazeNodeData>();
		foreach (MazeNode node in maze){
			mazeData.Add (node.convertToMazeNodeData());
		}
		List<MazeNodeData> solutionData = new List<MazeNodeData>();
		foreach (MazeNode node in solution){
			solutionData.Add (node.convertToMazeNodeData());
		}
		SaveData.savePuzzle (mazeData, level, solutionData, width, height);
		SaveData.saveMove (new MazeNodeData(0, 0), GamePlay.getCounter(), GamePlay.getScore());
	}
	
	public void BuildTheMaze(Save saveData){
		data.LoadSavedData(saveData);
		setValues();
		
		maze = new List<MazeNode>();
		solution = new List<MazeNode>();
		toBeginning = new List<MazeNode>();
		toEnd = new List<MazeNode>();
		ground = new List<List<GameObject>> ();
		vertWalls = new List<List<GameObject>> ();
		horiWalls = new List<List<GameObject>> ();
		cam = _camera.GetComponent<CameraControl> ();
		
		// ******************** Size Camera **********************
		
		_camera.transform.position = new Vector3 ((width - 1) * 5, (((17F / 3F) * height) - 6), -10);
		cam.setMax(((17f / 3f) * height) + 1);
		
		// ******************** Full Grid Build *************************
		
		if (width > 0 && height > 0) {
			o = Instantiate (goal, new Vector3((width - 1) * 10, (height - 1) * 10, -1), Quaternion.identity) as GameObject;
			o = Instantiate (baseFloor, new Vector3((width - 1) * 5, (height - 1 ) * 5, 3), Quaternion.Euler(270, 0, 0)) as GameObject;
			o.transform.localScale = new Vector3 (width, 1, height);
			o.transform.parent = floorParent.transform;
			
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
						                 new Vector3(i * 10, j * 10, 1), 
						                 Quaternion.Euler(270, 0, 0)) as GameObject;
						o.GetComponent<FloorController>().x = i;
						o.GetComponent<FloorController>().y = j;
						o.transform.parent = floorParent.transform;
						column.Add (o);
						maze.Add (new MazeNode(i,j));
						
						// Horizontal Walls
						o = Instantiate (wall, 
						                 new Vector3(i * 10, (j * 10) - 5, 0), 
						                 Quaternion.Euler(0, 0, 90)) as GameObject;
						o.transform.parent = wallParent.transform;
						hori.Add (o);
					}
					// Vertical Walls
					o = Instantiate (wall, 
					                 new Vector3((i * 10) - 5, j * 10, 0), 
					                 Quaternion.Euler(0, 0, 0)) as GameObject;
					o.transform.parent = wallParent.transform;
					vert.Add (o);
				}
				// Top Horizontal Walls
				if (i < width){
					o = Instantiate (wall, 
					                 new Vector3(i * 10, (height * 10) - 5, 0), 
					                 Quaternion.Euler(0, 0, 90)) as GameObject;
					o.transform.parent = wallParent.transform;
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

		for(int i = 0; i < maze.Count; i++){
			foreach (direction dir in saveData.CurrentMaze[i].dir) {
				maze[i].forwards.Add ( maze[i].AdjacentNodes[(int)dir] );
			}
		}

		foreach (MazeNodeData node in saveData.CurrentSolution){
			solution.Add(new MazeNode(node.x, node.y));
		}

		// **************** Delete Walls to Form Maze *******************
		
		foreach (MazeNode node in maze){
			if (node.forwards.Count > 0){
				foreach (MazeNode forward in node.forwards){
					if (node.x != forward.x){
						int x = Mathf.Max(node.x, forward.x);
						if (vertWalls[x][node.y] != null){ 
							vertWalls[x][node.y].transform.parent = null;
							vertWalls[x][node.y].GetComponent<CustomColoring>().removeMe();
							Destroy (vertWalls[x][node.y]);
						}
					}
					if (node.y != forward.y){
						int y = Mathf.Max(node.y, forward.y);
						if (horiWalls[node.x][y] != null){ 
							horiWalls[node.x][y].transform.parent = null;
							horiWalls[node.x][y].GetComponent<CustomColoring>().removeMe();
							Destroy (horiWalls[node.x][y]);
						}
					}
				}
			}
		}
		
		// ***************** Set Remaining Object to Maze Parent Object ***************
		
		current = maze[(saveData.CurrentPosition.x * height) + saveData.CurrentPosition.y];
		ClearMaze ();
		SetClickZone(saveData.CurrentPosition.x, saveData.CurrentPosition.y);

		ColorPallet.SetIndex (SaveData.GetSave().ColorIndex);
		ColorPallet.CallRecolor();
		GamePlay.setRemainingFromSave(SaveData.GetSave().CurrentRemainingMoves); 
		GamePlay.setScore(SaveData.GetSave().CurrentScore);
		
		MeshFilter[] meshFilters = wallParent.GetComponentsInChildren<MeshFilter>();
		CombineInstance[] combine = new CombineInstance[meshFilters.Length-1];
		int z = 0;
		for (int n = 0; n < meshFilters.Length; n++)
		{
			if(meshFilters[n].sharedMesh == null) continue;
			combine[z].mesh = meshFilters[n].sharedMesh;
			combine[z++].transform = meshFilters[n].transform.localToWorldMatrix;
			meshFilters[n].gameObject.SetActive(false);
		}
		wallParent.GetComponent<MeshFilter>().mesh = new Mesh();
		wallParent.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
		wallParent.gameObject.SetActive (true);

		// **************** Set Player Starting Position ******************************

		hero.transform.position = new Vector3(current.x, current.y, 0) * 10;
	}

	public static void SetClickZone(int x, int y)
	{
		List<MazeNode> clickArea = new List<MazeNode>();
		MazeBuilder.current = maze.Find (n => n.xy == "("+x+", "+y+")");
		clickArea = MazeNode.getRange (MazeBuilder.current, MazeBuilder.moveRange);
		foreach (MazeNode area in clickArea){
			ground[area.x][area.y].GetComponent<FloorController>().makeClickable ();
		}
	}

	private List<MazeNode> GetSolution(List<MazeNode> Begin, List<MazeNode> End){
		while(Begin.Count > 1 && End.Count > 1 && Begin[1] == End [1]) { 
			Begin.RemoveAt (0);
			End.RemoveAt (0);
		}
		if(Begin.Count > 1){
			Begin.RemoveAt(0);
		}else{
			End.RemoveAt (0);
		}
		Begin.Reverse();
		Begin.AddRange(End);
		return Begin;
	}

	public static List<MazeNode> GetPath(int x, int y)
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
					}
				}
			}
		}
		return null;
	}

	public static void ClearMaze(){
		foreach (MazeNode node in MazeBuilder.maze){
			ground[node.x][node.y].GetComponent<FloorController>().restore ();
		}
	}

	public void ResetGame(){
		ColorPallet.Clear ();
		GamePlay.ClearCounters();
		GamePlay.ZeroCounter();
		SaveData.reset();
		data.CallReset();
		Application.LoadLevel (Application.loadedLevel);
	}

	private void setValues()
	{
		width = data.width;
		height = data.height;
		moveRange = data.moveRange;
		level = data.level;
	}
}

public enum direction { north, east, south, west } 

public class MazeNode {


	public bool active = true;
	public string xy { get { return "(" + x + ", " + y + ")"; } } 
	public int x {get; set;} 
	public int y {get; set;}	
	public bool start = false; 
	public MazeNode[] AdjacentNodes {get; set;}
	public List<MazeNode> forwards {get; set;}
	public MazeNode back {get; set;}

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
		List<MazeNode> toBeginning = MazeBuilder.toBeginning;
		List<MazeNode> toEnd = MazeBuilder.toEnd;
		int lastX = MazeBuilder.width - 1;
		int lastY = MazeBuilder.height - 1;

		foreach (MazeNode node in AdjacentNodes){
			if (node != null && node.active){
				availableNodes.Add (node);
			}
		}
		if (availableNodes.Count > 0){
			MazeNode next = availableNodes[Random.Range(0, availableNodes.Count)];
			next.back = this;
			forwards.Add(next);

			if(!toBeginning.Exists(n => n.xy == "(0, 0)")){
				toBeginning.Add (next);
			}
			if(!toEnd.Exists(n => n.xy == "(" + lastX + ", " + lastY + ")")){
				toEnd.Add (next);
			}
			next.makeMaze();
		} else if (back != null) {
			if(!toBeginning.Exists(n => n.xy == "(0, 0)")){
				toBeginning.RemoveAt(toBeginning.Count - 1);
			}
			if(!toEnd.Exists(n => n.xy == "(" + lastX + ", " + lastY + ")")){
				toEnd.RemoveAt(toEnd.Count - 1);
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

	public MazeNodeData convertToMazeNodeData()
	{
		MazeNodeData output = new MazeNodeData();
		output.x = x;
		output.y = y;
		List<MazeNode> directions = new List<MazeNode>();
		directions.Add(back);
		if (forwards.Count > 0){
			directions.AddRange(forwards);
		}
		foreach (MazeNode node in directions){
			if (node != null) {
			    if (output.x == node.x ){
					if (output.y < node.y){
						output.dir.Add ( direction.north );
					}else{
						output.dir.Add ( direction.south );
					}
				} else {
					if ( output.x < node.x ){
						output.dir.Add ( direction.east );
					}else{
						output.dir.Add ( direction.west );
					}
				}
			}
		}
		return output;
	}
}

public class MazeNodeData{

	[XmlAttribute("x")]
	public int x { get; set; }

	[XmlAttribute("y")]
	public int y { get; set; }

	[XmlElement("D")]
	public List<direction> dir { get; set; }

	public MazeNodeData(){
		dir = new List<direction>();
	}

	public MazeNodeData(int a, int b){
		dir = new List<direction>();
		x = a;
		y = b;
	}
}