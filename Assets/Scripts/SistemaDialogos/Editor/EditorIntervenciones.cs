using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tsiunas.SistemaDialogos;
using UnityEditor;


public class EditorIntervenciones: EditorHijos<Situacion, Intervencion>
{
    List<EditorLineasDialogo> editoresLineasDialogo;

    public EditorIntervenciones(Situacion s) : base(s)
    {
        Recrear(s);
    }

    public void Recrear(Situacion s)
    {
        if (s == null)
            editoresLineasDialogo = new List<EditorLineasDialogo>();
        else
        {
            editoresLineasDialogo = new List<EditorLineasDialogo>(new EditorLineasDialogo[s.GetCantidadHijos()]);
            for (int i = 0; i < editoresLineasDialogo.Count; i++)
            {
                editoresLineasDialogo[i] = new EditorLineasDialogo(s.intervenciones[i]);
            }            

        }
    }

    protected override void PintarComponentesHijo(Intervencion intervecion, int i)
    {
        intervecion.actuarAlFinalizar = EditorGUILayout.Toggle("Actua al Finalizar?", intervecion.actuarAlFinalizar);
        if (intervecion.actuarAlFinalizar)
            intervecion.actuacion = EditorGUILayout.TextField("Actuacion: ", intervecion.actuacion, GUILayout.Width(500));
        intervecion.encolaIntervencionAlTerminar = EditorGUILayout.Toggle("Encola al Finalizar?", intervecion.encolaIntervencionAlTerminar);
        if (intervecion.encolaIntervencionAlTerminar)
            intervecion.idIntervencionAEncolarAlTerminar = EditorGUILayout.TextField("Encolar: ", intervecion.idIntervencionAEncolarAlTerminar, GUILayout.Width(500));

        editoresLineasDialogo[i].PintarHijos(intervecion);
    }

    internal override Intervencion AgregarHijo()
    {
        Intervencion rta = new Intervencion();        
        editoresLineasDialogo.Add(new EditorLineasDialogo(rta));
        return rta;
    }

    protected override Intervencion AgregarHijo(int op)
    {
        Intervencion interv = AgregarHijo();
        interv.actuarAlFinalizar = (Intervencion.TiposIntervenciones)op == Intervencion.TiposIntervenciones.ActuarAlFinal;
        return interv;
    }

    Intervencion.TiposIntervenciones op;
    protected override int SeleccionarOpcion()
    {
        op = (Intervencion.TiposIntervenciones)PonerPopup<Intervencion.TiposIntervenciones>("Tipo Intervencion Agregar: ", op);
        return (int)op;
    }

    protected override string NombrehijoInfoAdicional(Intervencion hijo)
    {
        if (hijo.actuarAlFinalizar)
            return "(actua al finalizar)";
        return base.NombrehijoInfoAdicional(hijo);
    }

   
}
