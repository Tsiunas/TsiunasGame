using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateDialogEngine
{
	[MenuItem("Assets/Create/Dialoge Engine")]
	public static DialogEngine Create() {
		DialogEngine asset = ScriptableObject.CreateInstance<DialogEngine> ();
		AssetDatabase.CreateAsset (asset, "Assets/DialogEngine.asset");
		AssetDatabase.SaveAssets ();
		return asset;
	}
}

