using System.Collections;
using System.Collections.Generic;
using Tsiunas.Places;
using Tsiunas.SistemaDialogos;
using UnityEngine;

namespace Tsiunas.Places
{
    public class TiendaElBuenVivir : PlaceManager
    {
        public static bool haHabladoConYidid = false;
        public static bool pendienteHablar = false;
        internal override void ConfigurePlace()
        {
            PNJActor yidid = GestorPNJ.Instance.EncontrarActor("PNJ_YIDID");
            PNJActor anuar = GestorPNJ.Instance.EncontrarActor("PNJ_ANUAR");
            if (yidid == null)
            {

                throw new TsiunasException("No se encontró a Yidid en la configuración de la escena");
            }
            if (anuar == null)
            {
                throw new TsiunasException("No se encontró a Anuar en la configuración de la escena");
            }
            if (!PlaceFlags.Instance.IsTrue(PNJ_MARGARITA.KEY_MARGARITA))
            {
                yidid.EstablecerSituacionActiva(string.Empty);
            }
            else
            {
                if(!haHabladoConYidid)
                    yidid.EstablecerSituacionActiva("SIT_MARIDO");
            }

        }
    }

}