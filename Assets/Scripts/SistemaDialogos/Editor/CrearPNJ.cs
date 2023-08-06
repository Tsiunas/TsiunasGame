using UnityEngine;
using UnityEditor;

public class CrearPNJ
{
    [MenuItem("Assets/Create/PNJ")]
    public static PNJDatos Create()
    {
        return Create("Assets/PNJ/NuevoPNJ.asset");
    }

    public static PNJDatos Create(string path)
    {
        PNJDatos asset = ScriptableObject.CreateInstance<PNJDatos>();

        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.SaveAssets();
        return asset;
    }
}