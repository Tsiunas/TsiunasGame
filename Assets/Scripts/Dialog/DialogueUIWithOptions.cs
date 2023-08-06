using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUIWithOptions : DialogueUIController {

	// Attributes
	#region Attributes
	public Button[] btnsOption;
    private Image imagenBlur;
    public AudioClip sonidoAlPresionar;
    #endregion

    // Methods
    #region Methods
    // Remiendo
    private void Start()
    {
        
        GameObject blur = GameObject.Find("Canvas/BLUR");
        if (blur != null)
        {
            imagenBlur = blur.GetComponent<Image>();
            imagenBlur.enabled = true;
        }
    }


    public void SetOptionTextButton(int indexOption, string _textOption) {
		btnsOption [indexOption].name = indexOption.ToString();
		Text textButton = btnsOption [indexOption].GetComponentInChildren<Text> ();
		textButton.text = _textOption;
	}

	void OnDestroy () {
		foreach (Button btn in btnsOption) {
			btn.onClick.RemoveAllListeners ();
		}

        if (imagenBlur != null)
            imagenBlur.enabled = false;
	}

	public override void SetBehaviourType (string [] options) {
		foreach (Button currentBtn in btnsOption) {
			currentBtn.onClick.AddListener (delegate() {
				/* Publica notificación y envía la siguiente estructura de datos
				 * object[0] = valor para interpretar el tipo de dato enviado
				 * object[1] = dato enviado (en este caso un int: indice del botón tocado (obtenido del nombre))
				*/
				NotificationCenter.DefaultCenter().PostNotification(this, "NotificationBetweenDialogueUI", new object[] { 0, int.Parse(currentBtn.name) });
				TrackerSystem.Instance.SendTrackingData("user", "selected", "dialog-tree", options[int.Parse(currentBtn.name)]+"|character|éxito");
                if(sonidoAlPresionar != null)
                {
                    SoundManager.PlaySound(sonidoAlPresionar);
                }
				if (launchEvent)
					HideDialogue();
				else
					DialogueOut();
			});
		}

		for (int o = 0; o < options.Length; o++) {
			SetOptionTextButton (o, options[o]);
		}
	}

	public override void SecondTapOverSpeechBubble ()
	{
		base.SecondTapOverSpeechBubble ();
	}
	#endregion
}
