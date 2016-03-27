using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeroMotionController : MonoBehaviour {

    public int distance, speed;
    public MazeDecoder mazeDecoder;
    
    private RectTransform rect;
    
    void Start(){
        rect = GetComponent<RectTransform>();
    }
    
	public IEnumerator MoveMe(string path, int index){
        mazeDecoder.ResetPath();
        foreach (char step in path){
            float tween = 0;
            Vector3 start = rect.localPosition;
            Vector3 end = GetEnd(start, step);
            while(Vector3.Distance(rect.localPosition, end) > 0.5f){
                rect.localPosition = Vector3.Lerp(start, end, tween);
                tween += Time.deltaTime * speed;
                yield return new WaitForEndOfFrame();
            }
        }
        if (index >= mazeDecoder.LastIndex()){
            mazeDecoder.Victory();
        } else {
            mazeDecoder.SetPath(index, 4);
        }
    }
    
    private Vector3 GetEnd(Vector3 start, char step){
        Vector3 output = start;
        Vector3 direction = new Vector3();
        switch (step){
            case 'N':
                direction = Vector3.up;
                break;
            case 'E':
                direction = Vector3.right;
                break;
            case 'W':
                direction = Vector3.left;
                break;
            case 'S':
                direction = Vector3.down;
                break;
        }
        return output + (direction * distance);
    }
}
