//Esta interfaz ha sido creada desde código bajado de: https://forum.unity.com/threads/editor-modal-window.222018/
/// <summary>
/// This EditorWindow can recieve and send Modal inputs.
/// </summary>
public interface IModal
{
    /// <summary>
    /// Called when the Modal shortcut is pressed.
    /// The implementation should call Create if the condition are right.
    /// </summary>
    void ModalRequest(bool shift);

    /// <summary>
    /// Called when the associated modal is closed.
    /// </summary>
    void ModalClosed(ModalWindow window);
}