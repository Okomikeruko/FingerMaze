using UnityEngine;
using System.Collections;

public class ScrollToScale : MonoBehaviour {

    public float min, max;
	
	// Update is called once per frame
	void Update () {
	    float x = transform.localScale.x + Input.GetAxis("Mouse ScrollWheel");
        x = Mathf.Min(max, x);
        x = Mathf.Max(min, x);
        transform.localScale = new Vector3(x, x, x);
	}
}
