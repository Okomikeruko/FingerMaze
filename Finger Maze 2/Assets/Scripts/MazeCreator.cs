using UnityEngine;
using UnityEngine.UI;

public class MazeCreator : MonoBehaviour {

    private bool maze = false;
    private string code, path;
    private Vector2 loc;
    public int height, length;
    private int[,] map;
    public Image progressBar;

    // Use this for initialization
    void Start () {
        code = new string('0', height * length);
        loc = new Vector2(Random.Range(0, length), Random.Range(0, height));
        map = ConvertToMap(code);
        path = "";
    }   
	
	// Update is called once per frame
	void Update ()
    {
        if (!maze)
        {
            if (code.Contains("0"))
            {
                SetStep();
            }
            else
            {
                GetComponent<MazeDecoder>().GenerateMaze(code, height, length);
                maze = true;
            }
            progressBar.fillAmount = (code.Length - code.Split('0').Length) / (float)code.Length;
        }
	}

    void SetStep()
    {
        string directions = GetDirections(map);
        if (directions.Length > 0)
        {
            char direction = directions[Random.Range(0, directions.Length)];
            path += direction;
            switch (direction)
            {
                case 'N':
                    map[(int)loc.x, (int)loc.y] += 8;
                    loc = new Vector2(loc.x, loc.y + 1);
                    map[(int)loc.x, (int)loc.y] += 1;
                    break;
                case 'E':
                    map[(int)loc.x, (int)loc.y] += 4;
                    loc = new Vector2(loc.x + 1, loc.y);
                    map[(int)loc.x, (int)loc.y] += 2;
                    break;
                case 'W':
                    map[(int)loc.x, (int)loc.y] += 2;
                    loc = new Vector2(loc.x - 1, loc.y);
                    map[(int)loc.x, (int)loc.y] += 4;
                    break;
                case 'S':
                    map[(int)loc.x, (int)loc.y] += 1;
                    loc = new Vector2(loc.x, loc.y - 1);
                    map[(int)loc.x, (int)loc.y] += 8;
                    break;
            }
        }
        else
        {
            BackTrack();
        }
        code = ConvertToCode(map);
    }

    void BackTrack()
    {
        char direction = path[path.Length - 1];
        switch (direction)
        {
            case 'N':
                loc = new Vector2(loc.x, loc.y - 1);
                break;
            case 'E':
                loc = new Vector2(loc.x - 1, loc.y);
                break;
            case 'W':
                loc = new Vector2(loc.x + 1, loc.y);
                break;
            case 'S':
                loc = new Vector2(loc.x, loc.y + 1);
                break;
        }
        path = path.Remove(path.Length - 1);
    }

    int[,] ConvertToMap(string c)
    {
        int[,] output = new int[length, height];
        for (int x = 0; x < length; x++)
        {
            for (int y = 0; y < height; y++)
            {
                output[x, y] = MazeDecoder.HexToInt(c[x + (y * length)]);
            }
        }
        return output;
    }

    string ConvertToCode(int[,] m)
    {
        string output = "";
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < length; x++)
            {
                output += MazeDecoder.IntToHex(m[x,y]);
            }
        }
        return output;
    }

    string GetDirections(int[,] map)
    {
        string output = "";
        
        if ((int)loc.y + 1 < height) {
            if (map[(int)loc.x, (int)loc.y + 1] == 0)
            {
                output += "N";
            }
        }
        if ((int)loc.x + 1 < length)
        {
            if (map[(int)loc.x + 1, (int)loc.y] == 0)
            {
                output += "E";
            }
        }
        if ((int)loc.x - 1 >= 0) {
            if (map[(int)loc.x - 1, (int)loc.y] == 0)
            {
                output += "W";
            }
        }
        if ((int)loc.y - 1 >= 0)
        {
            if (map[(int)loc.x, (int)loc.y - 1] == 0)
            {
                output += "S";
            }
        }

        return output;
    }
}
