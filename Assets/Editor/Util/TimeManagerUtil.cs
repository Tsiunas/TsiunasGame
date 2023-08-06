using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class TsiunasUtil : ScriptableObject
{
    [MenuItem("Tsiunas/Time/Borra Archivo de Tiempo")]
    static void DoIt()
    {
        PersistenceManager.Delete(fileNameToDelete: PersistenceHelp.FileNames.time);
        Debug.Log("Archivo de tiempo borrado. El tiempo iniciarás desde 0");        
    }

    [MenuItem("Tsiunas/Correr desde MainMenu %#r")]
    static void Run()
    {
        
        EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");
        EditorApplication.isPlaying = true;
        EditorPrefs.SetBool("CanDebug", true);
        
    }
    [MenuItem("Tsiunas/Activar Debug")]
    static void CanDebug()
    {
        DebugGlobals.canDebug = true;
        
    }
}