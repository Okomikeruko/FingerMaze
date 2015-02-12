using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

public class GamePlay : MonoBehaviour {

	public delegate void Counters();
	public static event Counters counters;

	private static int counter = 0,	remaining = 0;
	public string FileName;

	void Start()
	{
		SaveData.SetupSave(FileName);
	}

	void OnLevelWasLoaded(int l) {
		counter = 1;
		updateCounters ();
	}
	
	public static void updateCounters() {
		if (counters!= null) {
			counters();
		}
	}
	
	public static void ClearCounters() {
		counters = null;
	}

	public static void counterUp(){
		counter++;
	}

	public static void setRemaining(int r) {
		remaining = (int)Mathf.Ceil(r/data.moveRange) + 1;
	}

	public static int getCounter() {
		return remaining - counter; 
	}

	public static int getScore() {
		return 500;
	}
}

public static class SaveData {
	
	private static Save save;

	private static string _Location, _Data, _FileName;

	public static Save GetSave()
	{
		return save;
	}

	public static void SetupSave(string fileName)
	{
		_Location = Application.dataPath + "\\XML";
		_FileName = fileName;
		LoadFromXml();
		if(_Data.ToString () != "")
		{
			save = (Save)DeserializeObject(_Data);
		}else{
			save = new Save();
		}
	}

	public static void savePuzzle(List<MazeNodeData> maze, int level, List<MazeNodeData> solution, int width, int height)
	{
		save.CurrentMaze = maze;
		save.CurrentLevel = level;
		save.CurrentSolution = solution;
		save.Width = width;
		save.Height = height;
		SaveToXml();
	}
	
	public static void saveMove(MazeNodeData position, int moves, int score)
	{
		save.CurrentPosition = position;
		save.CurrentRemainingMoves = moves;
		save.CurrentScore = score;
		if (save.HighScore < save.CurrentScore)
		{
			save.HighScore = save.CurrentScore;
		}
		SaveToXml();
	}

	public static void reset()
	{
		int h = save.HighScore;
		save = new Save();
		save.HighScore = h;
		SaveToXml();
	}

	static byte[] StringToUTF8ByteArray(string pXmlString)
	{
		UTF8Encoding encoding = new UTF8Encoding();
		byte[] byteArray = encoding.GetBytes(pXmlString);
		return byteArray;
	}
	
	static string UTF8ByteArrayToString( byte[] characters )
	{
		UTF8Encoding encoding = new UTF8Encoding();
		string constructedString = encoding.GetString (characters);
		return constructedString;
	}
	
	static object DeserializeObject (string pXmlizedString)
	{
		XmlSerializer xs = new XmlSerializer(typeof(Save));
		MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
		return xs.Deserialize(memoryStream);
	}
	
	static string SerializeObject (object pObject)
	{
		string XmlizedString = null;
		MemoryStream memoryStream = new MemoryStream();
		XmlSerializer xs = new XmlSerializer(typeof(Save));
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xs.Serialize (xmlTextWriter, pObject);
		memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
		XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
		return XmlizedString;
	}

	static void SaveToXml()
	{
		string saveData = SerializeObject (save);
		StreamWriter writer;
		FileInfo t = new FileInfo(_Location + "\\" +_FileName);
		if (!t.Exists){
			writer = t.CreateText();
		} else {
			t.Delete();
			writer = t.CreateText();
		}
		writer.Write(saveData);
		writer.Close();
	}

	static void LoadFromXml()
	{
		StreamReader r = File.OpenText(_Location + "\\" + _FileName);
		string info = r.ReadToEnd();
		r.Close();
		_Data = info;
	}
}

[XmlRoot("root")]
public class Save {

	[XmlElement("MazeNode")]
	public List<MazeNodeData> CurrentMaze { get; set;  }

	[XmlAttribute("Level")]
	public int CurrentLevel { get; set; }

	[XmlAttribute("Width")]
	public int Width { get; set; }

	[XmlAttribute("Height")]
	public int Height { get; set; }

	[XmlElement("SolutionNode")]
	public List<MazeNodeData> CurrentSolution { get; set; }

	[XmlElement("CurrentPosition")] 
	public MazeNodeData CurrentPosition { get; set; }

	[XmlElement("RemainingMoves")]
	public int CurrentRemainingMoves { get; set; }

	[XmlElement("CurrentScore")]
	public int CurrentScore { get; set; }

	[XmlElement("HighScore")]
	public int HighScore { get; set; }
	
	public Save(){ 
		CurrentMaze = new List<MazeNodeData>();
		CurrentSolution = new List<MazeNodeData>();
		CurrentPosition = new MazeNodeData();
	}
}