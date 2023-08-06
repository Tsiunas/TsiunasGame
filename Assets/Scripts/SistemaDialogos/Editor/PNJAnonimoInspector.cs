using UnityEngine;
using UnityEditor;
using Tsiunas.SistemaDialogos;
using System;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(PNJAnonimo))]
public class PNJAnonimoInspector : Editor
{
    PNJAnonimo thePNJAnonimo;
    private string id;
    private bool firstDraw = true;

    private void Awake()
    {
        thePNJAnonimo = target as PNJAnonimo;
    }
    public override void OnInspectorGUI()
    {
        
        DrawDefaultInspector();
        if (thePNJAnonimo.datosPNJ == null)
        {
            //El Id del nuevo PNJDatos será PNJA_+<nombre de escena>+número
            string nomEscena = EditorSceneManager.GetActiveScene().name.ToUpper().Replace(' ', '_');
            int cant = FindObjectsOfType<PNJAnonimo>().Length;
            id = "PNJA_" + nomEscena + "_" + (cant).ToString();
            if (firstDraw) firstDraw = false;
            if (GUILayout.Button("Crear PNJ Datos"))
            {
                thePNJAnonimo.datosPNJ = CrearPNJAnonimoAsset(id);
                thePNJAnonimo.datosPNJ.speechPorDefecto = new Speech("!Hola!");
                thePNJAnonimo.name = id;
                thePNJAnonimo.datosPNJ.id = id;
                EditorUtility.SetDirty(thePNJAnonimo);
                EditorUtility.SetDirty(thePNJAnonimo.datosPNJ);
                
                AssetDatabase.SaveAssets();
            }
            
        }
        else
        {
            thePNJAnonimo.datosPNJ.id = EditorGUILayout.TextField("ID", thePNJAnonimo.datosPNJ.id);                    
            thePNJAnonimo.datosPNJ.speechPorDefecto.texto = EditorGUILayout.TextField("Texto", thePNJAnonimo.datosPNJ.speechPorDefecto.texto);
            thePNJAnonimo.name = thePNJAnonimo.id = thePNJAnonimo.datosPNJ.id;
        }

       
        if (GUI.changed)
        {
            if (thePNJAnonimo.datosPNJ != null)
            { 
                EditorUtility.SetDirty(thePNJAnonimo.datosPNJ);
                EditorUtility.SetDirty(thePNJAnonimo);
            }
        }

    }

    private PNJDatos CrearPNJAnonimoAsset(string id)
    {
        PNJDatos rta = null;
        string absPath = EditorUtility.SaveFilePanel("Crear PNJ", "PNJ", id, "asset");
        if (absPath.StartsWith(Application.dataPath))
        {
            string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);            
            rta  = CrearPNJ.Create(relPath);
            rta.id = id;            
        }
        return rta;
    }
}