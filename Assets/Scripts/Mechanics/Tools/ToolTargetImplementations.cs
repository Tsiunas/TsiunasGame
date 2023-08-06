using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Esta clase se usa para suscribir los métodos a los eventos de la clase: ToolTarget
/// </summary>
public class ToolTargetImplementations : MonoBehaviour
{

    private ToolTarget _toolTarget;
    private Collider _collider;

    private int resistance;
    private float result;
    private const int AMOUNT_STATES = 5;
    private readonly int[] PERCENTS = { 100, 81, 61, 41, 21, 0 };
    public Sprite[] states;
    // Methods
    #region Methods
    // Use this for initialization
    void Awake()
    {
        // Almacenar en caché los componentes
        _collider = GetComponent<Collider>();
        _toolTarget = GetComponent<ToolTarget>();

        // Se suscriben algunos métodos a los eventos de la clase ToolTarget
        _toolTarget.EventFinishedResistance += DestroyToolTarget;
        _toolTarget.EventSpriteChange += ChangeSprite;
        _toolTarget.EventSpriteChangePlowable += ChangeSpritePlowable;
        _toolTarget.EventColorChange += ColorChange;
        _toolTarget.EventChangeOccupationStatus += SetOccupationStatus;
        _toolTarget.EventResistanceChange += ResistanceChange;

        _toolTarget.OnSetResistance += (int filledResistance) => { resistance = filledResistance; };
    }



    void ResistanceChange(int _remainingAccuracy)
    {
        double percent = (double)(_remainingAccuracy * 100 / resistance);
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = CalculateState(percent);
    }

    private Sprite CalculateState(double percent)
    {
        int index = 0;
        for (int i = 0; i < PERCENTS.Length; i++)
        {
            if (percent <= PERCENTS[i] && percent >= PERCENTS[i + 1])
            {
                index = i;
                return states[index];
            }
		}
        return states[index];
    }

    /// <summary>
    /// Imnplementa la lógica del cambio del estado de ocupación XD (si este Tile está o no ocupado)
    /// </summary>
    /// <param name="occupied">si se setea en <c>true</c> esu color cambiara de blanco a gris </param>
    /// <param name="sameObject">si se setea en <c>true</c> no se desactivara el collider (ya que es el mismo objeto)</param>
    void SetOccupationStatus(bool occupied, bool sameObject)
    {
        _toolTarget.Occupied = occupied;
        // Cachñe color
        Color _color = GetComponent<SpriteRenderer>().color;
        // Cambio de color dependiendo del valor de _toolTarget.Occupied
        //_color = !_toolTarget.Occupied ? Color.white : new Color(197.0f / 255.0f, 197.0f / 255.0f, 197.0f / 255.0f);
        _color = !_toolTarget.Occupied ? Color.white : new Color(248.0f / 255.0f, 248.0f / 255.0f, 248.0f / 255.0f);
        // Asignación del nuevo color
        GetComponent<SpriteRenderer>().color = _color;     

        // Si sameObject es false, habilite o inhabilite el Collider dependiendo de !_toolTarget.Occupied
        if (!sameObject) _collider.enabled = !_toolTarget.Occupied;
    }

    /// <summary>
    /// Destruye los hijos de este objeto
    /// </summary>
    void DestroyToolTarget()
    {
        if (transform.childCount > 0) {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        } 
    }

    /// <summary>
    /// Envía notificación para cambiar el sprite de este GameObject
    /// </summary>
    /// <param name="_spriteRenderer">Sprite renderer.</param>
    void ChangeSprite(SpriteRenderer _spriteRenderer, bool isPlowed)
    {
        NotificationCenter.DefaultCenter().PostNotification(this, "ChangeSprite", new object[] {_spriteRenderer, isPlowed });
    }

    void ChangeSpritePlowable(SpriteRenderer _spriteRenderer, bool isPlowable)
    {
        NotificationCenter.DefaultCenter().PostNotification(this, "ChangeSpritePlowable", new object[] { _spriteRenderer, isPlowable });
    }

    /// <summary>
    /// Envía notificación para cambiar el color del sprite de este GameObject
    /// </summary>
    /// <param name="_spriteRenderer">Sprite renderer.</param>
    void ColorChange(SpriteRenderer _spriteRenderer, bool isWet)
    {
        NotificationCenter.DefaultCenter().PostNotification(this, "ChangeColor", new object[] { _spriteRenderer, isWet });
    }

    /// <summary>
    /// Cuando este objeto sea destriudo 
    /// </summary>
    void OnDestroy()
    {
        // Desuscribe los métodos suscritos a los eventos de la clase ToolTarget
        _toolTarget.EventFinishedResistance -= DestroyToolTarget;
        _toolTarget.EventSpriteChange -= ChangeSprite;
        _toolTarget.EventChangeOccupationStatus -= SetOccupationStatus;
    }
	#endregion
}
