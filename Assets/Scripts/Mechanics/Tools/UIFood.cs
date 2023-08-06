using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Esta clase se agrega al UI deuna herramienta
/// </summary>
public class UIFood : MonoBehaviour
{
    // Attributes
    #region Attributes
    // Imagen de la herramienta
    public Image iconFood;
    // Tipo de la herramienta
    public FoodType foodType = FoodType.None;
    // Texto de cantidad de ésta herramienta
    public Text textAmount;
    #endregion
}