using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationsMechanics : MonoBehaviour {

    private SpriteRenderer sR;
    public Sprite spritePlowedGround;
    public Sprite spriteNotPlowedGround;
    public Sprite spritePlowable;
    public GameObject prefabSeed;

	// Use this for initialization
	void Awake () {
        NotificationCenter.DefaultCenter().AddObserver(this, "ChangeSprite");
        NotificationCenter.DefaultCenter().AddObserver(this, "ChangeSpritePlowable");
        NotificationCenter.DefaultCenter().AddObserver(this, "ChangeColor");
	}

    /// <summary>
    /// Sirve para añdir una planta al Tile que debe estar arado
    /// </summary>
    /// <returns>The sow.</returns>
    /// <param name="notification">Notification.</param>
    public void Sow(Notification notification)
    {
        if ((GameObject)notification.data != null)
        {
            GameObject fertileTile = (GameObject)notification.data;
            Instantiate(prefabSeed, fertileTile.transform, false);
        }
    }
	
    public void ChangeSprite(Notification notification)
    {
        if (notification.data == null)
            return;

        object[] data = (object[])notification.data;
        // Se realiza casteo de object a SpriteRenderer
        sR = (SpriteRenderer)data[0] as SpriteRenderer;
        // Se asigna un sprite a la propiedad sprite del SpriteRenderer
        // Puede ser de terreno plano o arado
        sR.sprite = (bool)data[1] ? spritePlowedGround : spriteNotPlowedGround;
    }

    public void ChangeSpritePlowable(Notification notification)
    {
        if (notification.data == null)
            return;

        object[] data = (object[])notification.data;
        // Se realiza casteo de object a SpriteRenderer
        sR = (SpriteRenderer)data[0] as SpriteRenderer;
        // Se asigna un sprite a la propiedad sprite del SpriteRenderer
        // Puede ser de terreno plano o arado
        sR.sprite = (bool)data[1] ? spritePlowable : spriteNotPlowedGround;
    }

    /// <summary>
    /// Cambia el color del SpriteRenderer pasado como parámetro
    /// </summary>
    /// <param name="notification">Objeto notificación (SpriteRenderer).</param>
    public void ChangeColor(Notification notification)
    {
        if (notification.data == null)
            return;

        object[] data = (object[])notification.data;
        // Se realiza casteo de object a SpriteRenderer
        sR = (SpriteRenderer)data[0] as SpriteRenderer;
        Debug.Log("Nombre SpriteRenderer: " + sR.name);
        // Se asigna un sprite a la propiedad sprite del SpriteRenderer
        // Puede ser de terreno plano o arado
        sR.color = (bool)data[1] ? new Color(233f / 255f, 233f / 255f, 233f / 255f) : Color.white;
    }
}
