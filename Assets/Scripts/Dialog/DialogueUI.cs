using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : DialogueUIController {
	
	// Attributes
	#region Attributes
	public Button btnCloseDialogue;
    public AudioClip sonidoAlPresionar;
    #endregion

    // Methods
    #region Methods
   

    void OnDestroy () {
		btnCloseDialogue.onClick.RemoveAllListeners ();
       
	}

   
	public override void SetBehaviourType (string [] options)
	{
		btnCloseDialogue.onClick.AddListener (delegate() {
			TrackerSystem.Instance.SendTrackingData("user", "skiped", "dialog-tree", "dialogo_sin_respuesta|character|éxito");
            if (sonidoAlPresionar != null)
                SoundManager.PlaySound(sonidoAlPresionar);
            if (launchEvent)
            {
                HideDialogue();

               
            }
			else {
				DialogueOut();
              
			}

		});
	}

	public override void SecondTapOverSpeechBubble ()
	{
        if (sonidoAlPresionar != null)
            SoundManager.PlaySound(sonidoAlPresionar);
        base.SecondTapOverSpeechBubble ();
		if (launchEvent) 
			HideDialogue();
		else {
			DialogueOut();
		}

       
	}
	#endregion
}
