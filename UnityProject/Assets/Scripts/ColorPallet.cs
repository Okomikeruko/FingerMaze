using UnityEngine;
using System;

public class ColorPallet : MonoBehaviour {
	public static int i;
	public int index = 1;
	public ColorSwatch[] colorPallet;
	public static ColorSwatch[] pallet;

	public delegate void Delegate();  
	public static event Delegate coloring;

	void Start()
	{
		pallet = colorPallet;
		i = index;
	}

	public static void CallRecolor()
	{
		if(coloring != null){
			coloring();
		}
	}
	public static void Clear()
	{
		coloring = null;
	}
	public static void SetIndex(int x)
	{
		i = x;
	}
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