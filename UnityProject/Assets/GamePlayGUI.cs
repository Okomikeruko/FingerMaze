using UnityEngine;
using System.Collections;

public class GamePlayGUI : MonoBehaviour {

	public gui Back, Title, Level, Score, Moves, Pause;

	void Awake () {
		ColorPallet.coloring += setColor;
	}
	
	void OnGUI(){
		GUI.color = Back.color;
		GUI.Box(Back.AnchoredRect(), Back.content, Back.style);

		GUI.color = Title.color;
		GUI.Box(Title.AnchoredRect(), Title.content, Title.style);

		GUI.color = Level.color;
		GUI.Box(Level.AnchoredRect(), Level.content, Level.style);

		GUI.color = Score.color;
		GUI.Box(Score.AnchoredRect(), Score.content, Score.style);

		GUI.color = Moves.color;
		GUI.Box(Moves.AnchoredRect(), Moves.content, Moves.style);

		GUI.color = Pause.color;
		if(GUI.Button(Pause.AnchoredRect(), Pause.content, Pause.style)){
			Pause.menuObject.GetComponent<ButtonBehavior>().push();
		}
	}

	void setColor()
	{
		 Back.color = ColorPallet.pallet[ColorPallet.i].colors[(int) Back.shade].color;
		Title.color = ColorPallet.pallet[ColorPallet.i].colors[(int)Title.shade].color; 
		Level.color = ColorPallet.pallet[ColorPallet.i].colors[(int)Level.shade].color;
		Score.color = ColorPallet.pallet[ColorPallet.i].colors[(int)Score.shade].color; 
		Moves.color = ColorPallet.pallet[ColorPallet.i].colors[(int)Moves.shade].color;
		Pause.color = ColorPallet.pallet[ColorPallet.i].colors[(int)Pause.shade].color;
	}
}

[System.Serializable]
public class gui {
	[SerializeField]
	public Shade shade;
	public Color color;
	[SerializeField]
	public Anchor anchorPoint;
	public Rect rect;
	public float scale;
	public GUIContent content;
	public GUIStyle style;
	public GameObject menuObject;

	public Rect AnchoredRect()
	{
		Rect output = rect;
		output.width *= scale * (Screen.width / 814.0F);
		output.height *= scale * (Screen.height / 458.0F);
		
		switch (anchorPoint)
		{
		case Anchor.TopLeft:
			output.x = (Screen.width * rect.x);
			output.y = (Screen.height * rect.y);
			break;
		case Anchor.TopCenter:
			output.x = ((Screen.width - output.width )/ 2) + (Screen.width * rect.x);
			output.y = (Screen.height * rect.y);
			break;
		case Anchor.TopRight:
			output.x = (Screen.width - output.width ) + (Screen.width * rect.x);
			output.y = (Screen.height * rect.y);
			break;
		case Anchor.MiddleLeft:
			output.x = (Screen.width * rect.x);
			output.y = (Screen.height / 2) + (Screen.height * rect.y);
			break;
		case Anchor.MiddleCenter:
			output.x = ((Screen.width - output.width )/ 2) + (Screen.width * rect.x);
			output.y = (Screen.height / 2) + (Screen.height * rect.y);
			break;
		case Anchor.MiddleRight:
			output.x = (Screen.width - output.width ) + (Screen.width * rect.x);
			output.y = (Screen.height / 2) + (Screen.height * rect.y);
			break;
		case Anchor.BottomLeft:
			output.x = (Screen.width * rect.x);
			output.y = (Screen.height - output.height) + (Screen.height * rect.y);
			break;
		case Anchor.BottomCenter:
			output.x = ((Screen.width - output.width )/ 2) + (Screen.width * rect.x);
			output.y = (Screen.height - output.height) + (Screen.height * rect.y);
			break;
		case Anchor.BottomRight:
			output.x = (Screen.width - output.width ) + (Screen.width * rect.x);
			output.y = (Screen.height - output.height) + (Screen.height * rect.y);
			break;
		default:
			break;
		}
		return output;
	}

}

public enum Anchor{
	TopLeft,
	TopCenter,
	TopRight,
	MiddleLeft,
	MiddleCenter,
	MiddleRight,
	BottomLeft,
	BottomCenter,
	BottomRight
}