using UnityEngine;
using UnityEditor;
using Tsiunas.SistemaDialogos;
using System;

public class UtilidadPNJACtor : ScriptableObject
{

    
    [MenuItem("Tsiunas/Editor/CambiarPNJActor por Prefabs")]
    static void DoIt()
    {
        //Buscamos el asset prefab (el GUID)
        string pnjGUID = AssetDatabase.FindAssets("l:PNJ_ACTOR")[0];
        //Usamos el GUI para cargar un GameObject base
        GameObject prefabPNJActor = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(pnjGUID));

        //Buscamos todos los actores en la escena
        PNJActor[] actores = FindObjectsOfType<PNJActor>();

        //Por cada actor...
        foreach (PNJActor actor in actores)
        {
            //Clona el prefab. Con la posición y rotación  y escala del actor original
            GameObject goInstanciado = PrefabUtility.InstantiatePrefab(prefabPNJActor) as GameObject;
            goInstanciado.transform.position = actor.transform.position;
            goInstanciado.transform.rotation = actor.transform.rotation;
            goInstanciado.transform.localScale = actor.transform.localScale;
            goInstanciado.name = actor.name;

            PNJActor actorInstanciado = goInstanciado.GetComponent<PNJActor>();
            EditorUtility.CopySerialized(actor, actorInstanciado);

            //Copiamos todos los componentes del objeto original al nuevo
            foreach (Component c in actor.GetComponents(typeof(Component)))
            {
                //Se hace si no es el transform (ese ya lo tiene el nuevo objeto)
                //Lo mismo pasa para estos otros tipos porque ya los tiene el prefab
                if (c is Transform || c is PNJActor || c is UIAmistad || c is PNJModelController)
                    continue;
                //Se agrega un nuevo componente del tipo adecuado
                Component newComponent = goInstanciado.AddComponent(c.GetType());
                //Se copian los valores
                EditorUtility.CopySerialized(c, newComponent);
            }
            DestroyImmediate(actor.gameObject);
            
        }

        Debug.Log("Actores cambiados satisfactoriamente");
    }
}