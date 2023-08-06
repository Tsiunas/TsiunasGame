
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tsiunas.Mechanics;
using Tsiunas.SistemaDialogos;

/// <summary>
/// Consecuencia que hace que la situaci�n actual encole la intervenci�n dada
/// </summary>
public class ConsecuenciaActivarIntervencion : Consecuencia {

    public string idIntervencion;
    
    public override void Ejecutar()
    {
        if (Situacion.SituacionActual != null)
            Situacion.SituacionActual.EncolarIntervencion(idIntervencion);
        else
            throw new TsiunasException("No se pudo encolar la intervenci�n"+idIntervencion,false, "Consecuencias", "Hendrys");
    }
}