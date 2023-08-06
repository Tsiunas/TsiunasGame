using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Text;
using RedBlueGames.Tools.TextTyper;
using Tsiunas.SistemaDialogos;

// Struct
#region Struct
struct StructTypeText {
	public Coroutine coroutineSTT;
	public string textSTT;
}
#endregion

public abstract class DialogueUIController : MonoBehaviour, IPointerDownHandler {

	// Attributes
	#region Attributes
	public Text lblCharacterDialogue, lblConversationDialogue;
	public Image imgCharacter;
	private string tagSpeechBubble = "SpeechBubble";
	private StructTypeText structTypeText;
	public bool launchEvent;
	private Animator animator;
	public int timeTouchedSpeechBubble;
	private AudioClip clipSoundTypeText;
	private AudioSource audioSourceTypeText;
    private TextTyper componentTextTyper;
    public Action OnDialogOutEnded = delegate { };
	#endregion

	// Properties
	#region Properties
	public int TimeTouchedSpeechBubble {
		get {
			return timeTouchedSpeechBubble;
		}
	}
	#endregion

	// Methods
	#region Methods
	public void Awake() {
		try {
			animator = GetComponent<Animator>();
			clipSoundTypeText = (AudioClip) Resources.Load("Sounds/DialogSystem/TypeText");
			audioSourceTypeText = GetComponent<AudioSource>();

            componentTextTyper = GetComponentInChildren<TextTyper>();

		} catch (Exception ex) {
			Debug.LogError("Error: " + ex.Message);
		}
	}
	public void DialogueOut() {
        PNJActor.AnimarIdle(AnimationsManager.Instance.ObtenerIDPorNombre(this.lblCharacterDialogue.text));
		this.animator.SetBool ("DialogueOut", true);
	}

	public void DialogueOutEventLaunced() {
        PNJActor.AnimarIdle(AnimationsManager.Instance.ObtenerIDPorNombre(this.lblCharacterDialogue.text));
		this.animator.SetBool ("DialogueOutEventLaunched", true);
	}

	public void EventDialogueOutEnded() {
        
		GetComponent<Condition>().meetCondition = true;
        OnDialogOutEnded();
	}

	/// <summary>
	/// Método abstracto para establecer el comportamiento del DialogueUI según su tipo
	/// </summary>
	public abstract void SetBehaviourType (string [] options); 

	/// <summary>
	/// Establece el texto para la UI del personaje interlocutor
	/// </summary>
	/// <param name="_text">texto a establecer</param>
	public void SetCharacterDialogue(string _text) {
        string text = _text.Equals("Tu") ? GameManager.Instance.GetNamePJ : _text;
        this.lblCharacterDialogue.text = text;
	}

	/// <summary>
	/// Establece si el diálogo lanza evento
	/// </summary>
	/// <param name="_text">bool a establecer</param>
	public void SetLauncEvent(bool _launch) {
		this.launchEvent = _launch;
	}

	/// <summary>
	/// Oculta el díalogo y manda una notificación
	/// </summary>
	public void HideDialogue() {
		DialogueOutEventLaunced ();
	}

	public void WaitToDialogueOutToHideIt () {
		CanvasGroup canvasGroup = GetComponentInParent<CanvasGroup> ();
		canvasGroup.alpha = 0;
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
		NotificationCenter.DefaultCenter().PostNotification(this, "EventLaunched");
	}

	/// <summary>
	/// Muestra el díalogo actual y activa para recibir eventos
	/// </summary>
	public void ShowDialogue() {
		CanvasGroup canvasGroup = GetComponentInParent<CanvasGroup> ();
		canvasGroup.alpha = 1;
		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;
	}

	/// <summary>
	/// Establece el texto para la UI de la conversación
	/// </summary>
	/// <param name="_text">texto a establecer</param>
	public void SetConversationDialogue(string _text, int typeSpeechBubble) {
		structTypeText.textSTT = _text;
        if (typeSpeechBubble != 1)
            componentTextTyper.TypeText(_text, 0.05f);
        else
            this.lblConversationDialogue.text = _text;
		//structTypeText.coroutineSTT = StartCoroutine(TypeText(_text, this.lblConversationDialogue, 0.05f, AddTimesTouchedSpeechBubble));
	}

	/// <summary>
	/// Establece el sprite para la UI del personaje interlocutor
	/// * El nombre se toma del objeto "character" del archivo .json de Situaciones / Dialogos
	/// * Se hace uso de un método de extensión para quitar las tíldes en caso que las tenga
	/// </summary>
	/// <param name="_text">nombre del sprite</param>
	public void SetImageCharacter(string _nameSprite) {
		this.imgCharacter.sprite = Resources.Load<Sprite>("Characters/" + _nameSprite.RemoveDiacritics());
	}

	/// <summary>
	/// Muestra el texto asignado letra por letra con un intervalo de tiempo entre cada una
	/// </summary>
	/// <returns>The text.</returns>
	/// <param name="textComplete">texto a establecer</param>
	/// <param name="lblDialogue">UI de conversación</param>
	/// <param name="letterInterval">intervalo de tiempo entre cada letra presentada</param>
	IEnumerator TypeText (string textComplete, Text lblDialogue, float letterInterval, Action callback = null) {
        componentTextTyper.TypeText(textComplete, 0.05f);
        //foreach (char letter in textComplete.ToCharArray()) {
        //	lblDialogue.text += letter;
        //	//audioSourceTypeText.PlayOneShot(clipSoundTypeText);
        //	yield return new WaitForSeconds (letterInterval);
        //}
        //// Al terminar de mostrar letra por letra hace la llamada a un 'callback'
        if (callback != null) callback ();
        return null;
	}

	/// <summary>
	///  Implementación del método OnPointerDown de la interface IPointerDownHandler.
	///  Si el globo de texto es tocado (su tag es igual al dado)
	/// </summary>
	/// <param name="eventData">información del evento disparado.</param>
	public void OnPointerDown (PointerEventData eventData)
	{
		AddTimesTouchedSpeechBubble ();
        if (eventData.pointerEnter != null)
        {
            if (eventData.pointerEnter.tag.Equals(tagSpeechBubble)) if (timeTouchedSpeechBubble == 1) /*PresentCompleteText (this.lblConversationDialogue);*/ componentTextTyper.Skip(); else SecondTapOverSpeechBubble ();
        }
    }

	/// <summary>
	/// Aumenta en 1, la variable timeTouchedSpeechBubble
	/// </summary>
	public void AddTimesTouchedSpeechBubble() {
		timeTouchedSpeechBubble++;
	}

	/// <summary>
	/// Reinicia a 0 el valor de la variable timeTouchedSpeechBubble
	/// En clases derivadas se podrá reemplazar su implementación o añadir una nueva
	/// </summary>
	public virtual void SecondTapOverSpeechBubble () {
		timeTouchedSpeechBubble = 0;
	}

	/// <summary>
	/// Detiene la coroutina que muestra el texto de díalogo letra por letra y asigana el texto completo al instante
	/// </summary>
	/// <param name="uiText">UI del texto a mostrar completo</param>
	public void PresentCompleteText(Text uiText) {
		StopCoroutine (structTypeText.coroutineSTT);
		uiText.text = structTypeText.textSTT;
	}
	#endregion
}
