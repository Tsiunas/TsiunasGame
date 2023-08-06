//Esta clase ha sido creada desde código bajado de: https://forum.unity.com/threads/editor-modal-window.222018/
using UnityEditor;
using UnityEngine;

/// <summary>
/// Define a popup window that return a result.
/// Base class for IModal call implementation.
/// </summary>
/// 
public abstract class ModalWindow : EditorWindow
{
    public const float TITLEBAR = 18;

    protected IModal owner;

    protected new string title = "ModalWindow";

    protected WindowResult result = WindowResult.None;

    public WindowResult Result
    {
        get { return result; }
    }

    protected virtual void OnLostFocus()
    {
        result = WindowResult.LostFocus;

        if (owner != null)
            owner.ModalClosed(this);
    }

    protected virtual void Cancel()
    {
        result = WindowResult.Cancel;

        if (owner != null)
            owner.ModalClosed(this);

        Close();
    }

    protected virtual void Ok()
    {
        result = WindowResult.Ok;

        if (owner != null)
            owner.ModalClosed(this);

        Close();
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, position.width, position.height));
        GUILayout.BeginHorizontal(EditorStyles.toolbar);

        GUILayout.Label(title);

        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        Rect content = new Rect(0, TITLEBAR, position.width, position.height - TITLEBAR);
        Draw(content);
    }

    protected abstract void Draw(Rect region);
}



public enum WindowResult
{
    None,
    Ok,
    Cancel,
    Invalid,
    LostFocus
}

