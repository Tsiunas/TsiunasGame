using System;
using System.Collections.Generic;
using System.Linq;
using Tsiunas.SistemaDialogos;


namespace Tsiunas.SistemaDialogos
{
    public partial class GestorPNJ : Singleton<GestorPNJ>
    {
        private List<PNJActor> pnjActores = new List<PNJActor>();
        internal static readonly string MENSAJE_MAMATULE_DEFAULT = "Debes aprender sobre el respeto, la equidad y los derechos de las mujeres";

        public void ObtenerPNJActoresEnEscena()
        {
            pnjActores.Clear();
            pnjActores = FindObjectsOfType<PNJActor>().ToList();
        }

        public PNJActor ObtenerPNJActor(string id)
        {
            PNJActor actorEncontrado = pnjActores.Find(actor => actor.id == id);
            if (actorEncontrado == null)
                throw new TsiunasException("No se encontró el PNJActor con ID: (" + id + ")", true, "AMISTAD_PRIMER_CICLO", "Eduardo Andrade");
            return actorEncontrado;
        }

        internal void TsiunarPNJ(PNJActor actor)
        {
            if (actor == null)
                throw new TsiunasException("Argumento actor Nulo");
            if(actor.datosPNJ != GetMamaTule())
                actor.Tsiunarse();
        }

        internal PNJDatos GetMamaTule()
        {
            return ObtenerPNJ("PNJ_MAMATULE");
        }

        internal bool EsMamaTule(PNJActor actor)
        {
            return actor.datosPNJ == GetMamaTule();
        }
    }
}
