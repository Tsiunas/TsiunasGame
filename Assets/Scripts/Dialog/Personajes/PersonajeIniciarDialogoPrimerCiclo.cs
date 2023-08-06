using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using Tsiunas.SistemaDialogos;
using UnityEngine;

public class PersonajeIniciarDialogoPrimerCiclo : MonoBehaviour {
    public GameObject manoIndicadora;
    public PNJActor pnjActorYurani;

    private void OnMouseDown()
    {
        if (manoIndicadora != null)
        {
            manoIndicadora.SetActive(false);
        }
        DialogueSystemController dialogSC = FindObjectOfType<DialogueSystemController>();
        dialogSC.PresentSituation(dialogSC.indexPresentSituation);
        GetComponent<Collider>().enabled = false;
       
        SoundManager.PlayExito();
    }

    public void TerminoCaminarYuraniGO()
    {
        PNJActor.AnimarIdle(pnjActorYurani.id);
    }
}
