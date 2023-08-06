using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Esta clase se agrega al UI deuna herramienta
/// </summary>
public class UITool : MonoBehaviour
{
    // Attributes
    #region Attributes
    // Imagen de la herramienta
    public Image iconTool;
    // Tipo de la herramienta
	public ToolType toolType = ToolType.None;
    // Texto de cantidad de ésta herramienta
    public Text textAmount;
    #endregion
}