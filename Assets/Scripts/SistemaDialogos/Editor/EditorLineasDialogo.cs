using System;
using Tsiunas.SistemaDialogos;
using UnityEditor;
using UnityEngine;

class EditorLineasDialogo : EditorHijos<Intervencion, LineaDialogo>,IModal
{
    LineaDialogo.TiposLineaDialogo op;
    
    
    public EditorLineasDialogo(Intervencion s) : base(s)
    {
    }


    protected override int SeleccionarOpcion()
    {
        op = (LineaDialogo.TiposLineaDialogo)PonerPopup<LineaDialogo.TiposLineaDialogo>("Tipo Línea: ", op);
        return (int)op;
    }

    protected override LineaDialogo AgregarHijo(int op)
    {

        string objectPath = EditorPrefs.GetString("ObjectPath");
        LineaDialogo.TiposLineaDialogo opcion = (LineaDialogo.TiposLineaDialogo)op;
        int nuevaCantidad = padre.GetCantidadHijos() + 1;
        switch (opcion)
        {
            case LineaDialogo.TiposLineaDialogo.Simple:
                LineaDialogoSimple lds =  UtilidadScriptableObject.CrearScriptableObject<LineaDialogoSimple>(nuevaCantidad, "LDS", objectPath);                
                lds.speeches.Add(new Speech());
                return lds;
            case LineaDialogo.TiposLineaDialogo.ConOpciones:
                LineaDialogoOpciones ldo = UtilidadScriptableObject.CrearScriptableObject<LineaDialogoOpciones>(nuevaCantidad, "LDO", objectPath);
                ldo.opciones.Add(new Opcion(ldo));
                return ldo;

        }
        return new LineaDialogoSimple();


    }

    protected override void PintarComponentesHijo(LineaDialogo lineaDialogo, int i)
    {
        if (lineaDialogo is LineaDialogoSimple)
            PintarLineaDialogo(lineaDialogo as LineaDialogoSimple);
        if (lineaDialogo is LineaDialogoOpciones)
            PintarLineaDialogo(lineaDialogo as LineaDialogoOpciones);

    }

    private void PintarLineaDialogo(LineaDialogoSimple lineaDialogoSimple)
    {
        if (GUILayout.Button("Agregar Speech", GUILayout.ExpandWidth(false)))
        {
            lineaDialogoSimple.speeches.Add(new Speech());
        }
        if (lineaDialogoSimple.speeches.Count > 0)
        {
            if (GUILayout.Button("Borrar último Speech", GUILayout.ExpandWidth(false)))
            {
                lineaDialogoSimple.speeches.RemoveAt(lineaDialogoSimple.speeches.Count - 1);
            }
        }
        foreach (Speech s in lineaDialogoSimple.speeches )
        {
            EditorGUILayout.BeginHorizontal();
            s.texto = EditorGUILayout.TextArea(s.texto, GUILayout.Height(20));
            EditorGUILayout.EndHorizontal();
        }
    }

    private void PintarLineaDialogo(LineaDialogoOpciones lineaDialogoOpciones)
    {
        if (GUILayout.Button("Agregar Opción", GUILayout.ExpandWidth(false)))
        {
            if(lineaDialogoOpciones.opciones.Count < 4)
            {
                lineaDialogoOpciones.opciones.Add(new Opcion(lineaDialogoOpciones));
            }            
        }
        if (lineaDialogoOpciones.opciones.Count > 0)
        {
            if (GUILayout.Button("Borrar última Opción", GUILayout.ExpandWidth(false)))
            {
                lineaDialogoOpciones.opciones.RemoveAt(lineaDialogoOpciones.opciones.Count - 1);
            }
        }
        foreach (Opcion o in lineaDialogoOpciones.opciones)
        {
            EditorGUILayout.BeginHorizontal();
            o.sentencia = EditorGUILayout.TextArea(o.sentencia, GUILayout.Height(15));
            if(GUILayout.Button("Abrir Opción",GUILayout.ExpandWidth(false)))
            {
                EditorGUILayout.EndHorizontal();
                AbrirVentanaOpcion(o);
                EditorGUILayout.BeginHorizontal();
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    private void AbrirVentanaOpcion(Opcion o)
    {
        OpcionEditorWindow oew = OpcionEditorWindow.Create(this, "Opción", GUIUtility.GUIToScreenPoint(Event.current.mousePosition), o);
    }

    internal override LineaDialogo AgregarHijo()
    {
        return AgregarHijo(0);
    }

    protected override void SetIDHijo(LineaDialogo nuevaH)
    {
        string prefijo = "LDD_";
        if (nuevaH is LineaDialogoSimple)
            prefijo = "LDS_";
        else if (nuevaH is LineaDialogoOpciones)
            prefijo = "LDO_";
        nuevaH.SetID(prefijo + padre.GetListaHijos().Count);
    }

    public void ModalRequest(bool shift)
    {
        //vacio, no se necesita
    }

    public void ModalClosed(ModalWindow window)
    {
        //Actualizar la opcion
    }

    protected override string NombrehijoInfoAdicional(LineaDialogo hijo)
    {
        if (hijo is LineaDialogoOpciones) return "(Opciones)";
        if (hijo is LineaDialogoSimple) return "(Simple)";
        return string.Empty;
    }

   
}