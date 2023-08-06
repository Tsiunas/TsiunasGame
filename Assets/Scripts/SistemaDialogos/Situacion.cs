
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tsiunas.SistemaDialogos;

[Serializable]
public class Situacion:IContenedorPadre<Situacion,Intervencion>, IWithID
{

    #region Atributos Y Propiedades
    public string id;
    public List<Intervencion> intervenciones;
    private Intervencion intervencionActual = null;
    public string IdPNJOwner { get; set; }
    public bool noConsumir = false;

    public event Action OnSituacionTerminada;

    /// <summary>
    /// La situación que se está ejecutando. Solo puede haber una al tiempo.
    /// </summary>
    public static Situacion SituacionActual { private set;  get; }
    
    
    #endregion
    #region Constructores
    public Situacion()
    {
        intervenciones = new List<Intervencion>();
        intervencionActual = null;
	}
    #endregion
    #region Métodos
    public void Iniciar()
    {
        if (intervenciones == null || intervenciones.Count == 0)
        {
            throw new TsiunasException("Situación sin Intervenciones", false, "Dialogos", "Hendrys");
        }
        else
        {


            //Si la intervencion actual no se encuentra en las intervenciones...
            //Se debe hacer esto porque al ser Intervencion una clase serializable 
            //el sistema de Unity la llena con una Intervención por defecto
            //por lo cual no se puede tener una comparación con null.
            if (!intervenciones.Contains(intervencionActual))
            {
                intervencionActual = intervenciones[0];
                idIntervencionActual = intervencionActual.id;
            }
            SituacionActual = this;
            intervencionActual.SetOwner(this);
            intervencionEncolada = null;
            intervencionActual.Iniciar();



        }
    }

    private string idIntervencionActual;
    private Intervencion intervencionEncolada = null;

    public string IDIntervencionActual
    {
        get
        {
            return idIntervencionActual;
        }
        set
        {
            idIntervencionActual = value;
            SetIntervencionActual(idIntervencionActual);
        }
    }

    internal void IntervencionTerminada()
    {
        if(intervencionActual.actuarAlFinalizar)
        {
            PNJActor owner = GestorPNJ.Instance.EncontrarActor(IdPNJOwner);
            owner.ActuarActuacion(intervencionActual.actuacion);
        }

        if (intervencionActual.encolaIntervencionAlTerminar)
        {
            EncolarIntervencion(intervencionActual.idIntervencionAEncolarAlTerminar);
        }

        //Si hay encolada una nueva intervencion
        if (intervencionEncolada != null)
        {
            //establece la intervención actual apuntando a la intervención encolada
            intervencionActual = intervencionEncolada;
            SituacionActual = this;
            //consume la intervencion encolada
            intervencionEncolada = null;
            intervencionActual.SetOwner(this);
            intervencionActual.Iniciar();
        }
        else
        {
            if (OnSituacionTerminada != null) OnSituacionTerminada();
            SituacionActual = null;
        }
    }

    private void SetIntervencionActual(string idIntervencionActual)
    {
        if (intervenciones != null)
            intervencionActual = intervenciones.Find(i => i.id == idIntervencionActual);
    }

    public void EncolarIntervencion(string idIntervencion)
    {
        if (intervenciones != null)
            intervencionEncolada = intervenciones.Find(i => i.id == idIntervencion);
    }

    #endregion
    #region Implementación IContenedor Padre
    public string NombreContenedorPadre{get{return "Situacion";}}

    public string NombreContenedorPadrePlural { get { return "Situaciones"; } }

    public string NombreHijos { get { return "Intervencion"; } }

    public string NombreHijosPlural { get { return "Intervenciones"; } }

    public string PrefijoHijos { get { return "INT_"; } }

    public bool TieneIntervenciones { get { return intervenciones != null && intervenciones.Count > 0; } }

    public int GetCantidadHijos()
    {
        if (intervenciones == null)
            return 0;
        return intervenciones.Count;
    }

    public List<Intervencion> GetListaHijos()
    {
        return intervenciones;
    }

    public string IdHijo(int i)
    {
        if (intervenciones == null)
            throw new Exception("Intervenciones vacio");
        try
        {
            return intervenciones[i].id;
        }
        catch (IndexOutOfRangeException)
        {
            throw new Exception("Inervencion solicitada inexistente");
        }
    }
    #endregion
    #region Implementación IWithID
    public void SetID(string id)
    {
        this.id = id;
    }

    public string GetID()
    {
        return id;
    }
    #endregion
}