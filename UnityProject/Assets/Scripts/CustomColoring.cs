using UnityEngine;
using System.Collections;

public class CustomColoring : MonoBehaviour {

	[SerializeField]
	public Shade shade;

	void Awake () {
		ColorPallet.coloring += setColor;
	}

	public void removeMe(){
		ColorPallet.coloring -= setColor;
	}

	void setColor()
	{
		renderer.material.color = ColorPallet.pallet[ColorPallet.i].colors[(int)shade].color;
	}
}
