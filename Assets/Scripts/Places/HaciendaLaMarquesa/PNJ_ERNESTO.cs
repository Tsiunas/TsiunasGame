using System.Collections;
using System.Collections.Generic;
using Tsiunas.Places;
using Tsiunas.SistemaDialogos;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PNJ_ERNESTO : PNJ_ACTUACION
{
    private static readonly string ACT_BANDERA = "PonerBandera";
    private static readonly string ACT_AMISTADES = "DMargarita_AErnesto";
    private static readonly string ACT_IRSE = "Irse";
    private static readonly string ACT_FINAL = "Final";
    public PNJActor margarita;
    public PNJActor arcenio;
    

    public override void Actuar(string mensaje, object datos)
    {
        //Si el mensaje es el de Poner bandera, entonces se eleva la bandera de poner bandera de la campaña Cero Cambios
        //TODO: Se debe poner la bandera en la granja
        if (mensaje.CompareTo(ACT_BANDERA) == 0)
        {
            PlaceFlags.Instance.RaiseFlag(PlaceFlags.PONER_BANDERA);
            Actor.SubirAmistad();
            Actor.SubirAmistad();
        }
        if (mensaje.CompareTo(ACT_AMISTADES) == 0)
        {
            Actor.NivelAmistad = NivelesAmistad.DESDEN;
            margarita.NivelAmistad = NivelesAmistad.AMISTAD_ENTABLADA;

        }
        if (mensaje.CompareTo(ACT_IRSE) == 0)
        {
            Actor.NivelAmistad = NivelesAmistad.DESDEN;
            margarita.NivelAmistad = NivelesAmistad.AMISTAD_ENTABLADA;
            
            GetComponent<Animator>().Play("Irse");
            PNJActor.AnimarCaminar(Actor.id);
        }

        if (mensaje.CompareTo(ACT_FINAL) == 0)
        {
            margarita.EliminarSituacionActiva();
            arcenio.NivelAmistad = NivelesAmistad.AMISTAD_ENTABLADA;
            //TODO: Obtener dos flamas de la armonía para la colección
            StoreManager.ObtainTool(TypesGameElement.Tools.Watering_Can, 1, true);
            PlaceFlags.Instance.RaiseFlag(PlaceFlags.SEGUNDO_HITO_FINALIZADO);
        }
    }

    
}
