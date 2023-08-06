
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Tsiunas.SistemaDialogos
{
    public class ConsecuenciaSubirBajarAmistad : Consecuencia
    {

        public ConsecuenciaSubirBajarAmistad()
        {

        }

        public bool subir;
        public string idPNJ;

        public override void Ejecutar()
        {
            //Se busca el GameObject con el nombre del idPNJ y se le sube la amistad
            PNJActor actorASubir = GestorPNJ.Instance.EncontrarActor(idPNJ);
            if (actorASubir != null)
            {
                (subir ? new Action(() => { actorASubir.SubirAmistad(); } ): () => { actorASubir.BajarAmistad(); })();  
            }
            else
                throw new TsiunasException("Actor a subir amistad no encontrado",false,"Consecuencias", "Hendrys");
        }
    }
}