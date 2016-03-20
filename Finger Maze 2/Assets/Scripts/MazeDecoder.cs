using UnityEngine;
using UnityEngine.UI;

public class MazeDecoder : MonoBehaviour {

    public string code;
    public Sprite[] sprites;
    public GameObject button;

	// Use this for initialization
	void Start () {
	    for (int i = 0; i < code.Length; i++)
        {
            GameObject btn = Instantiate(button);
            btn.transform.SetParent(transform);
            btn.GetComponent<Image>().sprite = sprites[HexToInt(code[i])];
        }
	}
	
    int HexToInt(char hex)
    {
        return hex < 'A' ? 
            hex - '0' : 
            10 + hex - 'A';
    }
}
