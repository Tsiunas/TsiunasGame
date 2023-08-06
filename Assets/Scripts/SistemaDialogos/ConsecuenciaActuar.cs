
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tsiunas.Mechanics;
using Tsiunas.SistemaDialogos;

/// <summary>
/// Consecuencia que hace que un personaje actúe
/// El personaje buscado debe tener un script con un método Actuar (para eso se puede implementar la interfaz IActor)
/// </summary>
public class ConsecuenciaActuar : Consecuencia {

    public string idPNJ;
    public string actuacion;
    
    public override void Ejecutar()
    {
        //Se busca el GameObject con el nombre del idPNJ y se le sube la amistad
        PNJActor actor = GestorPNJ.Instance.EncontrarActor(idPNJ);
        if (actor != null)
        {
            if (!string.IsNullOrEmpty(actuacion))
                actor.ActuarActuacion(actuacion);
            else
                actor.ActuarActuacion();
        }
        else
            throw new TsiunasException("Actor buscado a Actuar encontrado", false, "Consecuencias", "Hendrys");
    }
}