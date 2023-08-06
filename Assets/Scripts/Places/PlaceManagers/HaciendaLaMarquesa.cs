using System.Collections;
using System.Collections.Generic;
using Tsiunas.Places;
using Tsiunas.SistemaDialogos;
using UnityEngine;

public class HaciendaLaMarquesa : PlaceManager
{
    public PNJActor ernesto;
    public PNJActor margarita;
    public PNJActor arcenio;
    public SpriteRenderer srMargarita;
    internal override void ConfigurePlace()
    {
        //Si ya se ha lanzado el segundo hito, entonces aparece don Ernesto y se cambia el sprite de Doña Margarita
        if (PlaceFlags.Instance.IsTrue(PlaceFlags.INICIAR_SEGUNDO_HITO) && !PlaceFlags.Instance.IsTrue(PlaceFlags.SEGUNDO_HITO_FINALIZADO))
        {
            ernesto.gameObject.SetActive(true);
            PNJ_MARGARITA pnjMargarita = margarita.GetComponent<PNJ_MARGARITA>();
            pnjMargarita.CambiarGraficosSegundoHito();
            //Además, se establece el texto de Doña Margarita a un texto del hito
            margarita.EstablecerSituacionActiva("SIT_SEGUNDO_HITO");
            arcenio.EliminarSituacionActiva();
        }
        

    }
    
}
