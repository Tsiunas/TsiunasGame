using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tsiunas.Mechanics;
using UnityEngine;

/// <summary>
/// Tamaños de los objetivos
/// </summary>
public enum TargetSizes { DEFAULT = 1, SMALL = 5, MEDIUM = 10, LARGE = 15, LARGER = 20 }
/// <summary>
/// Tipos de objetivos
/// </summary>
public enum TargetType { GROUND, TREE, ROCK, PLANT, UNDERGROWTH, CUTTRUNK }

/// <summary>
/// Objetivo de herramienta: este componente es atachado a un Gameobject junto con ToolTargetImplementations
/// </summary>
[RequireComponent(typeof(ToolTargetImplementations))]
[System.Serializable]
public class ToolTarget : MonoBehaviour
{
    // Attributes
    #region Attributes
    public TargetType targetType;
    [SerializeField]
    private TargetSizes targetSize = TargetSizes.DEFAULT;
    private int resistance;
    public delegate void OnResistanceChange(int _remainingAccuracy);
    public event OnResistanceChange EventResistanceChange;

    public delegate void OnFinishedResistance();
    public event OnFinishedResistance EventFinishedResistance;

    public delegate void OnSpriteChange(SpriteRenderer _spriteRenderer, bool isPlowed);
    public event OnSpriteChange EventSpriteChange;
    public event OnSpriteChange EventSpriteChangePlowable;


    public delegate void OnColorChange(SpriteRenderer _spriteRenderer, bool isWet);
    public event OnColorChange EventColorChange;

    public delegate void OnChangeOccupationStatus(bool occupied, bool sameObject);
    public event OnChangeOccupationStatus EventChangeOccupationStatus;

    public Action<TargetType> OnCambioTipoDeObjetivo;
    public Action<TargetType> OnUseTool = delegate { };

    public List<GameObject> neighbors = new List<GameObject>();
    private bool occupied;
    private bool isPlanted;
    private bool isPlowed;
    [SerializeField]
    private bool isPlowable;
    private bool isWet;

    public Action<int> OnSetResistance;
    private string _name;
    #endregion

    // Properties
    #region Properties
    public int Resistance
    {
        get { return resistance; }
        set { resistance = value; if (OnSetResistance != null) OnSetResistance(resistance); }
    }

    public bool Occupied
    {
        get { return occupied; }
        set { occupied = value; }
    }

    public bool IsPlanted
    {
        get { return isPlanted; }
        set { isPlanted = value; }
    }

    public bool IsPlowed
    {
        get { return isPlowed; }
        set
        {
            isPlowed = value;
            ChangeSprite(isPlowed);
        }
    }

    public bool IsPlowable
    {
        get { return isPlowable; }
        set
        {
            isPlowable = value;
            if(isPlowable)
                ChangeSpritePlowable(isPlowable);
        }
    }

   

    public bool IsWet
    {
        get { return isWet; }
        set { isWet = value; }
    }
	public string Name
	{
		get { return _name; }
	}
    #endregion

    // Methods
    #region Methods
    void Start()
    {
        this._name = name;
        Resistance = (int)targetSize;

        // Se evalua el tipo de objetivo que es este objeto
        switch (this.targetType)
        {   // En caso de ser Árbol o Roca
            case TargetType.TREE: case TargetType.ROCK: case TargetType.CUTTRUNK: case TargetType.UNDERGROWTH:
                // Realiza llamada al método DetectateNeighbors
                DetectateNeighbors();
                break;
        }

       
    }

    /// <summary>
    /// Sirve para configurar el tipo y tamaño de este objeto
    /// El segundo parámetro es opcional, su valor por defecto: DEFAULT
    /// </summary>
    /// <param name="type">Tipo.</param>
    /// <param name="size">Tamaño.</param>
    public void ChangeType(TargetType type, TargetSizes size = TargetSizes.DEFAULT) {
        this.targetType = type;
        this.targetSize = size;

        if (OnCambioTipoDeObjetivo != null)
            OnCambioTipoDeObjetivo(this.targetType);
    }

    /// <summary>
    /// Sirve para cambiar el estado de ocupación XD (si este Tile está o no ocupado)
    /// </summary>
    /// <param name="occupied">si se setea en <c>true</c> estará ocupado.</param>
    /// <param name="sameObject">si se setea en <c>true</c> hará referencia a este mismo GameObject.</param>
    public void ChangeOccupationStatus(bool occupied, bool sameObject) {
        // Se dispara el evento EventChangeOccupationStatus y se pasan los parámetros del método
        if (EventChangeOccupationStatus != null)
            EventChangeOccupationStatus(occupied, sameObject);
    }

    /// <summary>
    /// Sirve para dibujar un Gizmo si este objeto está selecciondo
    /// </summary>
    void OnDrawGizmosSelected()
    {
        // Se verifica que este objeto sea Roca o Árbol para dibujar el Gizmo
        if (targetType == TargetType.ROCK || targetType == TargetType.TREE)
        {
            // Declaro una distancia para el Raycast
            float distance = 1.5f;
            // Configuro un color para los Gizmso
            Gizmos.color = Color.red;
            // Realizo un ciclo por cada vez que se deba lanzar el rayo
            for (int i = 0; i < GetTimesCastsRayAndHisAngle(this.targetSize)[0]; i++)
            {
                // transformación para el ángulo obtenido dependiendo el tamaño de este objeto
                float rad = Mathf.Deg2Rad * (i * GetTimesCastsRayAndHisAngle(this.targetSize)[1]);
                /* Solamente si el tamaño es Muy Grande se cambia la distancia del rayo
                   se verifica el indice del for.
                   Si es número par o impar
                   Para dar mayor o menor a la distancia*/
                if (this.targetSize == TargetSizes.LARGER)
                    distance = i % 2 == 0 ? 2.5f : 1.5f;

                // Cambio de coordenadas 2d a 3d, coseno: eje X, seno: eje Y
                Vector3 direction = transform.TransformDirection(new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0));
                // Dibuja un rayo que va desde la posición de este objeto mas la dirección por la distancia
                Gizmos.DrawRay(transform.position, direction * distance);
            }
        }
    }

    /// <summary>
    /// Sirve para obtener la cantidad de veces que se lanzara un rayo y el ángulo en que lo hará
    /// </summary>
    /// <returns>Un arreglo de enteros de 2 valores:
    /// indice[0] = la cantidad de veces que lanzará un rayo
    /// indice[1] = el ángulo del rayo.</returns>
    /// <param name="thisSize">El tamañao de este objeto</param>
    public int[] GetTimesCastsRayAndHisAngle(TargetSizes thisSize) {
        int[] valuesToReturn = new int[2];
        switch (thisSize)
        {
            case TargetSizes.SMALL:
                valuesToReturn[0] = 2;
                valuesToReturn[1] = 180;
                break;
            case TargetSizes.MEDIUM:
                valuesToReturn[0] = 4;
                valuesToReturn[1] = 90;
                break;
            case TargetSizes.LARGE:
                valuesToReturn[0] = 8;
                valuesToReturn[1] = 45;
                break;
            case TargetSizes.LARGER:
                valuesToReturn[0] = 15;
                valuesToReturn[1] = 45;
                break;
        }
        return valuesToReturn;
    }

    /// <summary>
    /// Sirve para detectar los Tiles vecinos en diferentes ángulos dependiendo del Tamaño
    /// </summary>
    public void DetectateNeighbors()
    {
        // Cambio el estado de ocupación de este objeto
        ChangeOccupationStatus(true, true);
        // Limpio la lista de vecinos
        neighbors.Clear();
        // Declaro una distancia para el Raycast
        float distance = 1.5f;
        // Realizo un ciclo por cada vez que se deba lanzar el rayo
        for (int i = 0; i < GetTimesCastsRayAndHisAngle(this.targetSize)[0]; i++)
        {
            // transformación para el ángulo obtenido dependiendo el tamaño de este objeto
            float rad = Mathf.Deg2Rad * (i * GetTimesCastsRayAndHisAngle(this.targetSize)[1]);
            // Cambio de coordenadas 2d a 3d, coseno: eje X, seno: eje Y
            Vector3 direction = transform.TransformDirection(new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0));

            /* Solamente si el tamaño es Muy Grande se cambia la distancia del rayo
               se verifica el indice del for.
               Si es número par o impar
               Para dar mayor o menor a la distancia*/
            if (this.targetSize == TargetSizes.LARGER)
                distance = i % 2 == 0 ? 2.5f : 1.5f;

            // Lanza un rayo y alamcena los objetos tocados
            RaycastHit[] hit = Physics.RaycastAll(transform.position, direction, distance);
            foreach (RaycastHit h in hit)
            {
                Physics.IgnoreCollision(GetComponent<Collider>(), h.transform.GetComponentInChildren<Collider>());
                // si el objeto tocado tiene Collider y no es este mism objeto
                if (h.collider != null && h.collider.gameObject != this.gameObject && !h.collider.gameObject.tag.Equals("ToolTarget") && h.collider.gameObject.layer != 9)
                {
                    // añado el objeto a la lista de vecinos
                    neighbors.Add(h.collider.gameObject);
                    // Obtengo el componente ToolTarget del vecino actaul y cambio su estado de ocupación
                    if (h.collider.gameObject.GetComponent<ToolTarget>() != null)
                        h.collider.gameObject.GetComponent<ToolTarget>().ChangeOccupationStatus(true, false);
                }
            }
        }
    }

    /// <summary>
    /// Envía notificación para reproducir sonido de objetivo de herramienta válido
    /// (cuando está activa alguna herramienta y se toca en la granja el objetivo válido)
    /// </summary>
    /// <param name="indexClip">índice del clip a reproducir.</param>
    public void PlaySoundValidatedTarget(int indexClip)
    {
        NotificationCenter.DefaultCenter().PostNotification(this, "PlaySound", indexClip);
    }

    /// <summary>
    /// Envía notificación para mostrar retroalimentación de objetivo de herramienta NO válido (UI de Alerta)
    /// Envía notificación para reproducir sonido de objetivo de herramienta NO válida
    /// (cuando está activa alguna herramienta y se toca en la granja el objetivo NO válido)
    /// </summary>
    public void ExecuteFeedbackToolTargetWrong()
    {
        /* Se comenta la llamada a la notificación PresentAlertToTapWrongToolTarget
           para no mostrar el ícono de alerta cada que us una herramienta en el objetivo incorrecto
           se mantiene el código por si se quiere utilizar luego */
        // NotificationCenter.DefaultCenter().PostNotification(this, "PresentAlertToTapWrongToolTarget");
        NotificationCenter.DefaultCenter().PostNotification(this, "PlaySound", 5);
    }

    /// <summary>
    /// Disminuye el valor de la resistencia
    /// </summary>
    public void DecreaseResistance()
    {
        resistance--;
        // Si la resistencia llega a 0 entonces se realiza llamada a EventFinishedResistance
        if (resistance == 0)
        {
            // Si este objeto tiene vecinos 
            if (neighbors.Count > 0) {
                // Cambio su estado de ocupación 
                ChangeOccupationStatus(false, true);
                foreach (GameObject neighbor in neighbors)
                {
                    // Cambio el estado de ocupación de cada vecino 
                    if (neighbor.GetComponent<ToolTarget>() != null)
                        neighbor.GetComponent<ToolTarget>().ChangeOccupationStatus(false, false);
                }
                // Limpio la lista de vecinos
                neighbors.Clear();
            }

            // Cambio el sprite de este objeto por el de terreno plano
            ChangeSprite(false);
            // Cambio el color del sprite de este objeto por el de No mojado
            ChangeColor(false);
            // Cambio el tipo de objetivo y su tamaño
            ChangeType(TargetType.GROUND);
            // Cambio la resistencia en base al tamaño
            resistance = (int)targetSize;

            this.name = "TileToolGround";
            this._name = this.name;
            
            if (EventFinishedResistance != null)
                EventFinishedResistance();
            return;
        }
        // Se realiza llamada a EventResistanceChange
        if (EventResistanceChange != null)
            EventResistanceChange(resistance);
    }

    /// <summary>
    /// Cambia el sprite de este GameObject
    /// </summary>
    public void ChangeSprite(bool isPlowed)
    {
        // Se realiza llamada a EventSpriteChange
        if (EventSpriteChange != null)
            EventSpriteChange(GetComponent<SpriteRenderer>(), isPlowed);
    }

    /// <summary>
    /// Cambia el Sprite al plowable
    /// </summary>
    /// <param name="isPlowable"></param>
    private void ChangeSpritePlowable(bool isPlowable)
    {
        // Se realiza llamada a EventSpriteChange
        if (EventSpriteChangePlowable != null)
            EventSpriteChangePlowable(GetComponent<SpriteRenderer>(), isPlowable);
    }

    /// <summary>
    /// Cambia el color del sprite de este GameObject
    /// </summary>
    public void ChangeColor(bool isWet)
    {
        // Se realiza llamada a EventSpriteChange
        if (EventColorChange != null)
            EventColorChange(GetComponent<SpriteRenderer>(), isWet);
    }

    public void SetFrom(ToolTargetToSave source)
    {
        this.IsPlanted = source.isPlanted;
        this.IsPlowed = source.isPlowed;
        this.IsWet = source.IsWet;
        this.occupied = source.occupied;
        this.resistance = source.resistance;
        this.targetType = source.targetType;
        this.targetSize = source.targetSize;
        this.IsPlowable = source.isPlowable;
    }
    #endregion

    #region Clase para guardar datos
    [Serializable]
    public class ToolTargetToSave
    {
        public bool isPlanted;
        public bool isPlowed;
        public bool isPlowable;
        public bool IsWet;
        public bool occupied;
        public int resistance;
        public TargetType targetType;
        public TargetSizes targetSize;
        public float x;
        public float y;
        public float z;
        public string nombre;
        public Plant.PlantToSave planta;
        


        public ToolTargetToSave(ToolTarget original)
        {
            isPlanted = original.isPlanted;
            isPlowed = original.isPlowed;
            isPlowable = original.isPlowable;
            IsWet = original.isWet;
            occupied = original.occupied;
            resistance = original.resistance;
            targetType = original.targetType;
            targetSize = original.targetSize;
            x = original.transform.localPosition.x;
            y = original.transform.localPosition.y;
            z = original.transform.localPosition.z;
            nombre = original._name;
            //revisamos si el tt está plantado, Si lo está le asignamos un plantdatos dependiendoe de la planta plantada
            if (original.IsPlanted)
            {
                Plant p = original.GetComponentInChildren<Plant>();
                if (p != null) this.planta = new Plant.PlantToSave(p);
            }


        }

    }
    #endregion

    private void OnMouseUp()
    {
        UseTool();
    }

    public void UseTool()
    {
        if (GameManager.Instance.currentToolActive != null && !Util.IsPointerOverUIObject())
        {
            // Si hay una herramienta activa actualmente se realiza la llamada de Use y se envía el componente ToolTarget del objeto tocado
            if (!FindObjectOfType<CameraHandler>().BeginningPanning)
            {
                // Si no se está paneando
                // nuevo: si es posible usar la herramienta (cualquiera) se dispar el evento OnUseTool
                if (GameManager.Instance.currentToolActive.Use(this))
                    OnUseTool(this.targetType);
            }
        }
    }
}



