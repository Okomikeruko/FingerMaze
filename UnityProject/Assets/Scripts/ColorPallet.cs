using UnityEngine;
using System;

public class ColorPallet : MonoBehaviour {
	public int index = 0;
	public ColorSwatch[] colorPallet;
}

[Serializable] 
public class ColorSwatch{
	public string name;
	public ColorElement[] colors = new ColorElement[5];
}

[Serializable]
public class ColorElement{
	public Shade shade;
	public Color color;
}

public enum Shade { 
	darkest,
	dark,
	mid,
	light,
	lightest
}