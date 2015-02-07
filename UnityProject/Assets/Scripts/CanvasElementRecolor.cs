using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasElementRecolor : MonoBehaviour {

	public enum CanvasElement{
		Text,
		Image
	}

	[SerializeField]
	public Shade shade;

	[SerializeField]
	public CanvasElement canvasElement;


	void Awake(){
		ColorPallet.coloring += setColor;
	}

	void setColor()
	{
		switch(canvasElement)
		{
		case CanvasElement.Text:
			Text text = GetComponent<Text>();
			text.color = ColorPallet.pallet[ColorPallet.i].colors[(int)shade].color;
			break;
		case CanvasElement.Image:
			Image image = GetComponent<Image>();
			image.color = ColorPallet.pallet[ColorPallet.i].colors[(int)shade].color; 
			break;
		default:
			break;
		}
	}
}