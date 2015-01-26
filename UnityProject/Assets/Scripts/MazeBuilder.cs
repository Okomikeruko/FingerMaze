using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeBuilder : MonoBehaviour {

	[SerializeField] private int width = 1, height = 1;
	[SerializeField] private GameObject floor, wall;

	private Object o;

	private MazeNode start;

	private List<List<Object>> ground, vertWalls, horiWalls;

	// Use this for initialization
	void Start () {

		start = new MazeNode (Random.Range (0, width-1), Random.Range (0, height-1));

		ground = vertWalls = horiWalls = new List<List<Object>> ();

		// ******************** Full Grid Build *************************

		if (width > 0 && height > 0) {
			for (int i = 0; i <= width; i++)
			{
				List<Object> column = new List<Object>();
				List<Object> vert = new List<Object>();
				List<Object> hori = new List<Object>();
				for (int j = 0; j < height; j++)
				{

					if (i < width){
			// Floors
						o = Instantiate (floor, 
					                     new Vector3(i * 10, j * 10, 0), 
					                     Quaternion.Euler(270, 0, 0)) as Object;
						column.Add (o);

			// Horizontal Walls
						o = Instantiate (wall, 
						                 new Vector3(i * 10, (j * 10) - 5, 0), 
						                 Quaternion.Euler(0, 0, 90)) as Object;
						hori.Add (o);
					}
			// Vertical Walls
					o = Instantiate (wall, 
					                 new Vector3((i * 10) - 5, j * 10, 0), 
					                 Quaternion.Euler(0, 0, 0)) as Object;
					vert.Add (o);
				}
			// Top Horizontal Walls
				if (i < width){
					o = Instantiate (wall, 
					                 new Vector3(i * 10, (height * 10) - 5, 0), 
					                 Quaternion.Euler(0, 0, 90)) as Object;
				}
				ground.Add( column );
				vertWalls.Add ( vert );
				horiWalls.Add ( hori );
			}
		}

		// **************** Delete Walls to Form Maze *******************

	}
}

public class MazeNode {

	public int x, y;
	public List<MazeNode> nodes;

	public MazeNode(int a, int b) {
		x = a;
		y = b;
	}

}
