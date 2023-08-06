using System.Collections;
using System.Collections.Generic;
using Tsiunas.Mechanics;
using Tsiunas.SistemaDialogos;
using UnityEngine;
using UnityEngine.UI;

public class UIAmistad : MonoBehaviour {

	// Attributes
	#region Attributes
    public Image imagenIconoAmistad;
    private PNJActor actor;
    private RuntimeAnimatorController amistadController;
    private Animator amistadAnimator;
    #endregion

    // Methods
    #region Methods
    // Use this for initialization
    void Awake()
    {
        

		try {
            actor = GetComponent<PNJActor>();
            actor.OnAmistadCambio += AmistadCambio;
		} catch (System.Exception ex) {
			Debug.Log ("Error: " + ex.Message);
		}

        if (amistadController == null)
        {
            
            amistadController = Resources.Load<RuntimeAnimatorController>("AmistadController");
            if (imagenIconoAmistad != null)
            if((amistadAnimator = imagenIconoAmistad.GetComponent<Animator>()) == null)
                amistadAnimator = imagenIconoAmistad.gameObject.AddComponent<Animator>();

            if (amistadAnimator != null)
                amistadAnimator.runtimeAnimatorController = amistadController;
        }
    }

    void AmistadCambio(int nivel)
    {
        if (imagenIconoAmistad == null)
            return;
        imagenIconoAmistad.sprite = TexturesManager.Instance.GetSpriteFromSpriteSheet("Amistad_" + nivel.ToString());
        if ((NivelesAmistad)nivel == NivelesAmistad.AMISTAD_ENTABLADA)
        {
            amistadAnimator.enabled = true;
            amistadAnimator.Play("AgrandarAmistad");
        }
    }
	#endregion
}
