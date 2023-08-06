using UnityEngine;
using System.Collections;
using UnityEditor;
using Tsiunas.SistemaDialogos;

public class OpcionEditorWindow : ModalWindow
{

    public const float BUTTONS_HEIGHT = 30;

    public const float FIELD_HEIGHT = 20;

    public const float HEIGHT = 600;
    public const float WIDTH = 600;


    public Opcion opcionEditable;
    EditorConsecuencias editorConsecuencias;

    protected OpcionEditorWindow()
    {
    }

   
    public static OpcionEditorWindow Create(IModal owner, string title, Vector2 position, Opcion laOpcion )
    {
        OpcionEditorWindow cew = OpcionEditorWindow.CreateInstance<OpcionEditorWindow>();
        cew.opcionEditable = laOpcion;
        cew.editorConsecuencias = new EditorConsecuencias(laOpcion);
        cew.owner = owner;
        cew.title = title;
        

        float halfWidth = WIDTH / 2;

        float x = position.x - halfWidth;
        float y = position.y;

        float height = HEIGHT + (laOpcion.consecuencias != null?laOpcion.consecuencias.Count:0 * FIELD_HEIGHT);

        Rect rect = new Rect(x, y, 0, 0);
        cew.position = rect;
        cew.ShowAsDropDown(rect, new Vector2(WIDTH, height));

        return cew;
    }

    protected override void Draw(Rect region)
    {
        if (Event.current.type == EventType.KeyDown)
        {
            if (Event.current.keyCode == KeyCode.Return)
                Ok();

            if (Event.current.keyCode == KeyCode.Escape)
                Cancel();
        }


        GUILayout.BeginArea(region);
        EditorGUILayout.BeginHorizontal();
        opcionEditable.sentencia = EditorGUILayout.TextField(opcionEditable.sentencia, GUILayout.ExpandWidth(false));
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(20);       

      
        editorConsecuencias.PintarHijos(opcionEditable);
        //GUI.enabled = valid;
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Ok"))
            Ok();

        //GUI.enabled = true;

        if (GUILayout.Button("Cancel"))
            Cancel();

        EditorGUILayout.EndHorizontal();
        GUILayout.EndArea();
        
    }
}
