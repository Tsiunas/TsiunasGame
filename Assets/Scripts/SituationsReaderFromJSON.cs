using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SituationsReaderFromJSON : MonoBehaviour {

	// Attributes
	#region Attributes
	public string situationsDirectory = "Situations/";
	public string situationsFileName = "Dialogues";
	#endregion

	// Methods
	#region Methods
	public abstract void LoadNow(SimpleJSON.JSONNode situationsFile);

	// Use this for initialization
	void Start () {
		SetSituationFile();
	}

	public void SetSituationFile() {

		string filename = situationsDirectory + situationsFileName;
		TextAsset bindata = Resources.Load(filename) as TextAsset;
		if(bindata == null){
			Debug.LogError("Imposible load file " + filename + " be sure the file exists in the correct directory!");
			return;
		}

		SimpleJSON.JSONNode node = SimpleJSON.JSON.Parse(bindata.text);
		LoadNow (node);
	}
	#endregion
}
