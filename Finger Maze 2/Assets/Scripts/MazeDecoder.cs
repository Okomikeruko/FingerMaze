using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


public class MazeDecoder : MonoBehaviour {

    public Sprite[] sprites;
    public GameObject button, goal;
    public HeroMotionController hmc;

    private string code;
    private List<Button> buttons;
    private int height, length;

	void Start () {
        buttons = new List<Button>();
	}
	
    public int LastIndex(){
        return code.Length - 1;
    }
    
    public void GenerateMaze (string mazeCode, int h, int l){
        code = mazeCode;
        height = h;
        length = l;
        for (int i = 0; i < code.Length; i++)
        {
            GameObject btn = Instantiate(button);
            btn.transform.SetParent(transform);
            btn.GetComponent<Image>().sprite = sprites[HexToInt(code[i])];
            Button b = btn.GetComponent<Button>();
            b.GetComponent<RectTransform>().localScale = Vector3.one;
            int index = i;
            b.onClick.AddListener(() => SetPath(index, 4));
            if (i == code.Length - 1)
            {
                b.onClick.AddListener(() => Victory());
                GameObject g = Instantiate(goal);
                g.GetComponent<RectTransform>().localScale = Vector3.one;
                g.transform.SetParent(btn.transform);
            }
            buttons.Add(b);
        }
        SetPath(0, 4);
    }

    public static int HexToInt(char hex) {
        return hex < 'A' ? 
               hex - '0' : 
          10 + hex - 'A' ;
    }

    public static char IntToHex(int i)
    {
        char output = (char)(i < 10 ? (char)(i) + '0' : (char)(i - 10) + 'A');
        return output;
    }

    public static string HexToDirections(char c)
    {
        return IntToDirections(HexToInt(c));
    }

    public static string IntToDirections(int i)
    {
        string output = "";
        if (i - 8 >= 0)
        {
            output += "N";
            i -= 8;
        }
        if (i - 4 >= 0)
        {
            output += "E";
            i -= 4;
        }
        if (i - 2 >= 0)
        {
            output += "W";
            i -= 2;
        }
        if (i - 1 >= 0)
        {
            output += "S";
            i -= 1;
        }
        return output;
    }

    public static char GetOpposite(char c)
    {
        char output = c;
        switch (c)
        {
            case 'N':
                output = 'S';
                break;
            case 'E':
                output = 'W';
                break;
            case 'W':
                output = 'E';
                break;
            case 'S':
                output = 'N';
                break;
        }
        return output;
    }

    public int GetMapIndex(int origin, char direction)
    {
        int output = origin;
        switch (direction)
        {
            case 'N':
                output += length;
                break;
            case 'E':
                output += 1;
                break;
            case 'W':
                output -= 1;
                break;
            case 'S':
                output -= length;
                break;
        }
        return output;
    }

    public void SetPath(int origin, int distance = 0, char prev = '0', string path = "")
    {
        if (prev == '0')
        {
            ResetPath();
        } else {
            buttons[origin].interactable = true;
        }
        buttons[origin].onClick.AddListener(() => MoveHero(path, origin));
        string directions = HexToDirections(code[origin]);
        if (distance > 0)
        {
            for (int i = 0; i < directions.Length; i++)
            {
                char dir = directions[i];
                if (dir != prev)
                {
                    SetPath(GetMapIndex(origin, dir), 
                        distance - 1, 
                        GetOpposite(dir),
                        path + dir);
                }
            }
        }
    }

    public void ResetPath()
    {
        foreach (Button button in buttons)
        {
            button.onClick.RemoveAllListeners();
            button.interactable = false;
        }
    }
    
    public void MoveHero(string path, int i)
    {
        StartCoroutine(hmc.MoveMe(path, i));
    }

    public void Victory()
    {
        Debug.Log("You Win!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
