using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class DialogEngineEditor : EditorWindow {
	
	public static DialogEngineEditor window;

	[MenuItem("Window/Dialog Engine")]
	public static void ShowWindow() {
		window = GetWindow<DialogEngineEditor> ("Dialog Engine");
	}

	DialogEngine dialogEngine;

	void OnEnable() {
		dialogEngine = AssetDatabase.LoadAssetAtPath("Assets/ScriptableObjects.asset",
			typeof(DialogEngine)) as DialogEngine;
	}

	void OnGUI() {
		if (GUILayout.Button("Crear Motor de dialogo")) {
			dialogEngine = ScriptableObject.CreateInstance<DialogEngine>();
			dialogEngine.situations = new List<Situation> ();
			AssetDatabase.CreateAsset(dialogEngine, "Assets/MonsterDefnList.asset");
			AssetDatabase.SaveAssets();
		}

		if (GUILayout.Button ("Crear Situación")) {
			Situation situation = ScriptableObject.CreateInstance<Situation>();
			dialogEngine.situations.Add (situation);
			situation.name = "Situation";
			AssetDatabase.AddObjectToAsset (situation, dialogEngine);
			AssetDatabase.SaveAssets();
		}

		if (GUILayout.Button ("Crear Diálogo")) {
		}

		if (GUI.changed) EditorUtility.SetDirty(dialogEngine);
	}
}
