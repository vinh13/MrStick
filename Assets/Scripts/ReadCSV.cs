#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEngine;

public class ReadCSV
{
	[MenuItem ("TungnhEditor/ReadCSV")]
	public static void _ReadCSV ()
	{
		StreamReader strReader = new StreamReader ("D:\\DataExcel\\StickmanHero\\UpgradePower.csv");
		bool endOfFile = false;
		while (!endOfFile) {
			string data_string = strReader.ReadLine ();
			if (data_string == null) {
				endOfFile = true;
				break;
			}
		}

	}

	public static string[] DataCSV (string path)
	{
		List<string> listStr = new List<string> ();
		StreamReader strReader = new StreamReader (path);
		bool endOfFile = false;
		while (!endOfFile) {
			string data_string = strReader.ReadLine ();
			if (data_string == null) {
				endOfFile = true;
				break;
			}
			listStr.Add (data_string);
		}
		return listStr.ToArray ();
	}
}
#endif
