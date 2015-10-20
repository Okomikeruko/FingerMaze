using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
//using System.Xml;
//using System.Xml.Serialization;
using Newtonsoft.Json;
using AOT;

public class GamePlay : MonoBehaviour {

	public delegate void Counters();
	public static event Counters counters;
	public static event Counters gameOver;

	public delegate void Bools(bool b);
	public static event Bools victory;

	private static int counter = 0,	remaining = 0, score = 0;
	public string FileName;



	void Awake()
	{
		SaveData.SetupSave(FileName);
	}

	void OnLevelWasLoaded(int l) {
		counter = 0;
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
		int output = remaining - counter;
		if (output < 0 && gameOver != null){
			ColorPallet.CallRecolor();
			gameOver();
		}
	}

	public static void CallVictory(bool b) {
		if (victory != null) {
			ColorPallet.CallRecolor();
			victory(b);
		}
	}

	public static void ClearVictory() {
		victory = null;
	}

	public static void setRemaining(int r) {
		float multiplier = Mathf.Max ((data.slope * data.level) + data.intercept, 1);
		remaining += (int)((Mathf.Ceil(r/data.moveRange) + 1) * multiplier);
	}

	public static void setRemainingFromSave(int r) {
		remaining = r;
	}

	public static void scoreUp(int s)
	{
		score += s;
	}

	public static void setScore(int s)
	{
		score = s;
	}

	public static void ZeroCounter()
	{
		counter = 0;
		remaining = 0;
		score = 0;
	}

	public static int getCounter() {
		int output = remaining - counter;
		return output; 
	}

	public static int getScore() {
		return score;
	}

	public static int getHighScore()
	{
		return Mathf.Max(SaveData.GetSave().HighScore, score);
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
#if  UNITY_EDITOR_OSX 
		_Location = Path.Combine(Application.dataPath, "Resources");
#elif UNITY_IOS
		_Location = Application.persistentDataPath;
#else
		_Location = Application.dataPath + slash + "Resources";
#endif
		_FileName = fileName;
		if (File.Exists (Path.Combine (_Location, _FileName))) {
			LoadFromJson();
			if(_Data.ToString () != "")
			{
				save = (Save)JavaScriptConvert.DeserializeObject(_Data, typeof(Save));
			}else{
				save = new Save();
			}
		} else {
			File.Create (Path.Combine (_Location, _FileName));
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
		SaveToJson();
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
		SaveToJson();
	}
	public static void saveColor(int colorIndex)
	{
		save.ColorIndex = colorIndex;
		SaveToJson();
	}

	public static void reset()
	{
		int h = save.HighScore;
		int i = save.ColorIndex;
		save = new Save();
		save.HighScore = h;
		save.ColorIndex = i;
		SaveToJson();
	}

	public static void SaveToJson (){
		JsonSerializer serializer = new JsonSerializer ();
		serializer.NullValueHandling = NullValueHandling.Ignore;

		using (StreamWriter sw = new StreamWriter(@""+ Path.Combine(_Location,_FileName)))
		using (JsonWriter writer = new JsonTextWriter(sw))

		serializer.Serialize (writer, save);
/*		string saveData = JavaScriptConvert.SerializeObject (save);
		StreamWriter writer;
		FileInfo t = new FileInfo(Path.Combine (_Location, _FileName));
		if (!t.Exists) {
				writer = t.CreateText ();
		} else {
				t.Delete ();
				writer = t.CreateText ();
		}
		writer.Write (saveData);
		writer.Close ();*/
	}

	public static void LoadFromJson(){
		StreamReader reader = File.OpenText (Path.Combine (_Location, _FileName));
		string info = reader.ReadToEnd ();
		reader.Close ();
		_Data = info;
	}
/*
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
		FileInfo t = new FileInfo(Path.Combine (_Location, _FileName));
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
		StreamReader r = File.OpenText(Path.Combine (_Location, _FileName));
		string info = r.ReadToEnd();
		r.Close();
		_Data = info;
	}
	*/
}

//[XmlRoot("root")]
[JsonObject(MemberSerialization.OptIn)]
public class Save {

//	[XmlAttribute("C")]
	[JsonProperty] 
	public int ColorIndex { get; set;}

//	[XmlElement("MN")]
	[JsonProperty]
	public List<MazeNodeData> CurrentMaze { get; set;  }

//	[XmlAttribute("Lvl")]
	[JsonProperty]
	public int CurrentLevel { get; set; }

//	[XmlAttribute("w")]
	[JsonProperty]
	public int Width { get; set; }

//	[XmlAttribute("h")]
	[JsonProperty]
	public int Height { get; set; }

//	[XmlElement("SN")]
	[JsonProperty]
	public List<MazeNodeData> CurrentSolution { get; set; }

//	[XmlElement("P")] 
	[JsonProperty]
	public MazeNodeData CurrentPosition { get; set; }

//	[XmlElement("R")]
	[JsonProperty]
	public int CurrentRemainingMoves { get; set; }

//	[XmlElement("S")]
	[JsonProperty]
	public int CurrentScore { get; set; }

//	[XmlElement("H")]
	[JsonProperty]
	public int HighScore { get; set; }
	
	public Save(){ 
		ColorIndex = 1;
		CurrentMaze = new List<MazeNodeData>();
		CurrentSolution = new List<MazeNodeData>();
		CurrentPosition = new MazeNodeData();
	}
}
