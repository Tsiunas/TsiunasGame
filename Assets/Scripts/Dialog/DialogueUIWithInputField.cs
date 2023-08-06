using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUIWithInputField : DialogueUIController {
	
	// Attributes
	#region Attributes
	public InputField iptQuestion;
	#endregion

	// Methods
	#region Methods
	public override void SetBehaviourType (string [] options)
	{
		iptQuestion.onEndEdit.AddListener (delegate(string text) {
			if (text == null || text == "") return;
			/* Publica notificación y envía la siguiente estructura de datos
			 * object[0] = valor para interpretar el tipo de dato enviado
			 * object[1] = dato enviado (en este caso un string: texto del Input Field)
			*/
			NotificationCenter.DefaultCenter().PostNotification(this, "NotificationBetweenDialogueUI", new object[] { 1, text });			
			TrackerSystem.Instance.SendTrackingData("user", "wrote", "question", ""+text);
			CuestionarioManager.Instance.MostrarCuestionario("pretest.json");
			if (launchEvent)
				HideDialogue();
			else
				DialogueOut();
		});
	}

	void OnDestroy () {
		iptQuestion.onEndEdit.RemoveAllListeners ();
	}

	public override void SecondTapOverSpeechBubble ()
	{
		base.SecondTapOverSpeechBubble ();
	}
	#endregion
}
