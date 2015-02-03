using UnityEngine;
using System.Collections;

public class CustomColoring : MonoBehaviour {

	[SerializeField]
	private Shade shade;

	private ColorPallet colorPallet;

	void Awake () {
		colorPallet = GameObject.Find ("Data").GetComponent<ColorPallet>();
		int index = colorPallet.index;
		renderer.material.color = colorPallet.colorPallet[index].colors[(int)shade].color;
	}
}
