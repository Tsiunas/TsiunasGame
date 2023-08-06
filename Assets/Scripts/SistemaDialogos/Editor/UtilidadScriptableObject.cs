using UnityEngine;
using System.Collections;
using UnityEditor;

public class UtilidadScriptableObject 
{
    public static T CrearScriptableObject<T>(int numero, string siglas, string objectPath) where T: ScriptableObject
    {         
        T so = ScriptableObject.CreateInstance<T>();
        so.name = numero + siglas;
        AssetDatabase.AddObjectToAsset(so, objectPath);
        AssetDatabase.SaveAssets();
        return so;
    }
    
}
