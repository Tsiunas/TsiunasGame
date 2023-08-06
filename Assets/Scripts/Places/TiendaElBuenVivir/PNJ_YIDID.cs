using System.Collections;
using System.Collections.Generic;
using Tsiunas.SistemaDialogos;
using UnityEngine;

namespace Tsiunas.Places
{
    public class PNJ_YIDID : PNJ_ACTUACION
    {
        
        public override void Actuar(string arg1, object arg2)
        {
            PNJActor pNJActor = GestorPNJ.Instance.EncontrarActor("PNJ_ANUAR");
            if (pNJActor.NivelAmistad < NivelesAmistad.REGULAR)
                pNJActor.EstablecerSituacionActiva("SIT_COBRA");
            else if (pNJActor.NivelAmistad > NivelesAmistad.REGULAR)
                pNJActor.EstablecerSituacionActiva("SIT_REGALO");
            PNJ_ANUAR.EstarPendiente();
            TiendaElBuenVivir.haHabladoConYidid = true;
        }
    }
}