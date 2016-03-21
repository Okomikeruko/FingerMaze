using UnityEngine;
using UnityEngine.UI;

public class MazeDecoder : MonoBehaviour {

    public string code;
    public Sprite[] sprites;
    public GameObject button;

	// Use this for initialization
	void Start () {
        
	}
	
    public void GenerateMaze (string mazeCode){
        for (int i = 0; i < mazeCode.Length; i++)
        {
            GameObject btn = Instantiate(button);
            btn.transform.SetParent(transform);
            btn.GetComponent<Image>().sprite = sprites[HexToInt(mazeCode[i])];
        }
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
}
