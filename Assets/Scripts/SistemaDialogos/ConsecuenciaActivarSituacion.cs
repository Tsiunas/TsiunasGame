
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tsiunas.Mechanics;
using Tsiunas.SistemaDialogos;

/// <summary>
/// Consecuencia que hace que la situación actual encole la intervención dada
/// </summary>
public class ConsecuenciaActivarSituacion : Consecuencia {

    public string idSituacion;
    public string idPNJ;

    public override void Ejecutar()
    {
        PNJActor actor = GestorPNJ.Instance.EncontrarActor(idPNJ);
        actor.EstablecerSituacionActiva(idSituacion);

    }
}