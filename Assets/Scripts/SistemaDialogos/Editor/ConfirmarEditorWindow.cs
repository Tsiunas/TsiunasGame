using UnityEngine;
using System.Collections;
using UnityEditor;
using Tsiunas.SistemaDialogos;

public class ConfirmarEditorWindow : ModalWindow
{

    public const float BUTTONS_HEIGHT = 30;

    public const float HEIGHT = 100;
    public const float WIDTH = 200;


    

    protected ConfirmarEditorWindow()
    {
    }

   
    public static ConfirmarEditorWindow Create(IModal owner)
    {
        ConfirmarEditorWindow cew = ConfirmarEditorWindow.CreateInstance<ConfirmarEditorWindow>();
        cew.owner = owner;
        cew.title = "Confirmar";
        

        float halfWidth = WIDTH / 2;

        Vector2 position = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);

        float x = position.x - halfWidth;
        float y = position.y;

        float height = HEIGHT;

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
        if (GUILayout.Button("Ok"))
            Ok();

        //GUI.enabled = true;

        if (GUILayout.Button("Cancelar"))
            Cancel();

        EditorGUILayout.EndHorizontal();
        GUILayout.EndArea();
        
    }
}
