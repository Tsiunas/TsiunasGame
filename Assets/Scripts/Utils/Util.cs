using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using Tsiunas.Mechanics;

public class Util : MonoBehaviour
{
    // Methods
    #region Methods
    /// <summary>
    /// Crea un elemento de Interfaz de usuario en cualquier posición tomando como referencia un objeto 3d en el mundo
    /// </summary>
    /// <param name="prefab">prefab del objeto a crear</param>
    /// <param name="wordPosition">Posición en el mundo</param>
    /// <param name="offsetY">offset en el eje Y</param>
    /// <param name="varContainer">contenedor del objeto instanciado.</param>
    /// <param name="parent">objeto padre del objeto a instanciar</param>
    public static void CreateUIWithAnyPosition(GameObject prefab, Vector2 wordPosition, float offsetY, out GameObject varContainer, GameObject parent = null) {
		varContainer = null;
		varContainer = (GameObject)Instantiate (prefab, Vector2.zero, Quaternion.identity);

		if (parent == null)
			parent = FindObjectOfType<Canvas>().gameObject;
			
		varContainer.transform.SetParent (parent.transform, false);
		Vector2 pos = wordPosition;
		pos.y -= offsetY;
		pos = Camera.main.WorldToScreenPoint (pos);
        RectTransform rt = varContainer.GetComponent<RectTransform>();
        if(rt != null)
            rt.position = pos;
	}

	/// <summary>
	/// Establece el estado del lugar por medio de la constante de la enumeración: NamePlacesTown, nombre del lugar y estado
	/// Haciendo usa de persistencia simple con la clase PlayerPrefs
	/// </summary>
	/// <param name="placeName">Place name.</param>
	/// <param name="placeState">Place state.</param>
	public static void SetPlaceState(NamePlacesTown placeName, PlaceState placeState) {
		PlayerPrefs.SetString(Enum.GetName(typeof(NamePlacesTown), placeName), Enum.GetName(typeof(PlaceState), placeState));
	}

	/// <summary>
	/// Regresa el estado del lugar por medio de la constante de la enumeración: NamePlacesTown, nombre del lugar
	/// Haciendo usa de persistencia simple con la clase PlayerPrefs
	/// </summary>
	/// <returns>Estado del lugar</returns>
	/// <param name="placeName">Nombre del lugar</param>
	/// <param name="placeState">Estado del lugar</param>
	public static PlaceState GetPlaceState(NamePlacesTown placeName, PlaceState placeState) {		
		string valueRetrived = PlayerPrefs.GetString(Enum.GetName(typeof(NamePlacesTown), placeName), Enum.GetName(typeof(PlaceState), placeState));
		PlaceState _state;
		switch (valueRetrived) {
		case "Open":
			_state = PlaceState.Open;
			break;
		case "Close":
			_state = PlaceState.Close;
			break;
		default:
			_state = PlaceState.UnStated;
			break;
		}
		return _state;
	}

	/// <summary>
	/// Permite pasar del diálogo actual al siguiente.
	/// Cumpliendo la condición de la coroutina y activando las propiedades del CanvasGroup 
	/// </summary>
	public static void PassToAnotherDialog() {
        FindObjectOfType<Condition>().meetCondition = true;
        FindObjectOfType<DialogueUIController>().ShowDialogue ();
	}

    public static IEnumerator PassToAnotherDialog(float delay) {
        yield return new WaitForSeconds(delay);
        FindObjectOfType<Condition>().meetCondition = true;
        FindObjectOfType<DialogueUIController>().ShowDialogue ();
	}

	/// <summary>
	/// Cast a ray to test if Input.mousePosition is over any UI object in EventSystem.current. This is a replacement
	/// for IsPointerOverGameObject() which does not work on Android in 4.6.0f3
	/// </summary>
	public static bool IsPointerOverUIObject() {
		// Referencing this code for GraphicRaycaster https://gist.github.com/stramit/ead7ca1f432f3c0f181f
		// the ray cast appears to require only eventData.position.
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
		eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
		return results.Count > 0;
	}

    public static IEnumerable<T> GetValuesOfEnum<T>()
    {
        return Enum.GetValues(typeof(T)).Cast<T>();
    }

    public static IEnumerable<T> EnumToList<T>()
    where T : struct
    {
        return Enum.GetValues(typeof(T)).Cast<T>();
    }

    /// <summary>
    /// Se utiliza para crear un image (de Canvas), posicionarlo en base a la posición de un Gameobject en el world space
    /// Asignarle un sprite.
    /// TODO ESTO SE HARÁ EL NÚMERO DE VECES QUE LE INDIQUEMOS Y CON EL INTERVALO DE TIEMPO QUE LE INDIQUEMOS (estos son parámetros opcionales)
    /// </summary>
    /// <param name="go">GameObject del que se toma la posición del mundo.</param>
    /// <param name="parent">GameObject padre</param>
    /// <param name="sprite">Sprite a establecer</param>
    /// <param name="amountUILerpGameElement">*Parámetro Opcional* la cantidad de veces que se crearán los Image</param>
    /// <param name="callback">*Parámetro Opcional* método anónimo que se ejecuta una vez se han creado todos los Image</param>
    /// <param name="delay">*Parámetro Opcional* intervalo de tiempo con que se creararn los Image</param>
    public static IEnumerator CreateUILerpGameElement(GameObject go, GameObject parent, Sprite sprite, int amountUILerpGameElement = 1, Action callback = null, float delay = 0.1f, bool callbackWhenEndLerp = false)
    {
        YieldInstruction yieldInstruction = null;
        for (int i = 0; i < amountUILerpGameElement; i++)
        {
            yieldInstruction = i == 0 ? (YieldInstruction)new WaitForEndOfFrame() : (YieldInstruction)new WaitForSeconds(delay);
            yield return yieldInstruction;
            GameObject prefab = (GameObject)Resources.Load("Prefabs/LerpUIGameElement");
            GameObject goInstantiated = (GameObject)Instantiate(prefab, Camera.main.WorldToScreenPoint(go.transform.position), Quaternion.identity);
            goInstantiated.GetComponent<RectTransform>().SetParent(parent.transform);
            LerpUIElementGameElement lerpUI = goInstantiated.GetComponent<LerpUIElementGameElement>();
            lerpUI.SetSprite(sprite);

            if (callback != null)
            {
                (callbackWhenEndLerp ? new Action(() => { lerpUI.OnEndLerp += callback; }) : callback)();
            }

        }

    }

    public static void FeedbackToolUsed(ToolTarget target, ToolType type)
    {
        GameObject prefab = (GameObject)Resources.Load("Prefabs/ToolUsed");
        GameObject goInstantiated = (GameObject)Instantiate(prefab, Camera.main.WorldToScreenPoint(target.transform.position), Quaternion.identity);
        RectTransform rTransform = goInstantiated.GetComponent<RectTransform>();
        rTransform.SetParent(GameObject.FindWithTag("Canvas").transform);
        Vector2 posC = rTransform.anchoredPosition;
        posC.y += 50;
        rTransform.anchoredPosition = posC;
        goInstantiated.GetComponent<Image>().sprite = TexturesManager.Instance.GetSpriteFromSpriteSheet(type.ToString());
    }
	#endregion
}

