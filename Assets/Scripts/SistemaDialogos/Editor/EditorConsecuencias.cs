using System;
using Tsiunas.Mechanics;
using Tsiunas.SistemaDialogos;
using UnityEditor;
using UnityEngine;

class EditorConsecuencias : EditorHijos<Opcion, Consecuencia>
{
    Consecuencia.TiposConsecuencia op;
    public EditorConsecuencias(Opcion s) : base(s)
    {
    }

    protected override Consecuencia AgregarHijo(int op)
    {
        string objectPath = EditorPrefs.GetString("ObjectPath");
        int nuevaCantidad = padre.GetCantidadHijos() + 1;
        Consecuencia.TiposConsecuencia o = (Consecuencia.TiposConsecuencia)op;
        switch (o)
        {
            case Consecuencia.TiposConsecuencia.SBVariable:
                return UtilidadScriptableObject.CrearScriptableObject<ConsecuenciaSubirBajar>(nuevaCantidad, "CSB", objectPath);                
            case Consecuencia.TiposConsecuencia.SBAmistad:
                return UtilidadScriptableObject.CrearScriptableObject<ConsecuenciaSubirBajarAmistad>(nuevaCantidad, "CSBA", objectPath);
            case Consecuencia.TiposConsecuencia.Actuar:
                ConsecuenciaActuar ca = UtilidadScriptableObject.CrearScriptableObject<ConsecuenciaActuar>(nuevaCantidad, "CA", objectPath);
                ca.actuacion = "Actuar";
                ca.idPNJ = PNJEditorWindow.pnj.id;
                return ca;
            case Consecuencia.TiposConsecuencia.ActivarIntervencion:
                return UtilidadScriptableObject.CrearScriptableObject<ConsecuenciaActivarIntervencion>(nuevaCantidad, "CAI", objectPath);
            case Consecuencia.TiposConsecuencia.ActivarSituacion:
                ConsecuenciaActivarSituacion cas = UtilidadScriptableObject.CrearScriptableObject<ConsecuenciaActivarSituacion>(nuevaCantidad, "CAS", objectPath);
                cas.idPNJ = PNJEditorWindow.pnj.id;
                cas.idSituacion = PNJEditorWindow.GetCurrentSitID();
                EditorUtility.SetDirty(cas);
                return cas;
            case Consecuencia.TiposConsecuencia.ObtenerItem:
                return UtilidadScriptableObject.CrearScriptableObject<ConsecuenciaObtenerItem>(nuevaCantidad, "COI", objectPath);
            default:
                return UtilidadScriptableObject.CrearScriptableObject<ConsecuenciaSubirBajar>(nuevaCantidad, "CSB", objectPath);
                
        }
        
    }

    protected override void PintarComponentesHijo(Consecuencia hijo, int i)
    {
        
        if (hijo is ConsecuenciaSubirBajar)
        {
            ConsecuenciaSubirBajar csb = hijo as ConsecuenciaSubirBajar;
            EditorGUILayout.BeginHorizontal();
            csb.cantidad = EditorGUILayout.Popup("Cantidad:",csb.cantidad, new string[] { "0","1", "2", "3", "4", "5", "6", "7", "8", "9", "10" });
            csb.sube = EditorGUILayout.Toggle("Sube?",csb.sube);
            csb.varAModificar = (Variables) EditorGUILayout.EnumPopup("Variable:", csb.varAModificar);
            EditorGUILayout.EndHorizontal();

        }
        if(hijo is ConsecuenciaSubirBajarAmistad)
        {
            ConsecuenciaSubirBajarAmistad csb = hijo as ConsecuenciaSubirBajarAmistad;
            EditorGUILayout.BeginHorizontal();
            csb.subir = EditorGUILayout.Toggle("Sube?", csb.subir);
            csb.idPNJ = EditorGUILayout.TextField("ID PNJ:", csb.idPNJ);
            EditorGUILayout.EndHorizontal();
        }
        if (hijo is ConsecuenciaActuar)
        {
            ConsecuenciaActuar csb = hijo as ConsecuenciaActuar;
            EditorGUILayout.BeginHorizontal();
            csb.actuacion = EditorGUILayout.TextField("Actuacion:", csb.actuacion);
            csb.idPNJ = EditorGUILayout.TextField("ID PNJ:", csb.idPNJ);
            EditorGUILayout.EndHorizontal();
        }
        if (hijo is ConsecuenciaActivarIntervencion)
        {
            ConsecuenciaActivarIntervencion csb = hijo as ConsecuenciaActivarIntervencion;
            EditorGUILayout.BeginHorizontal();

            string[] intervenciones = PNJEditorWindow.GetCurrentIntervenciones();

            int selectedIndex;
            if (string.IsNullOrEmpty(csb.idIntervencion))
            {
                selectedIndex = 0;
            }
            else
            {
                selectedIndex = Array.IndexOf<string>(intervenciones, csb.idIntervencion);
            }
            if (selectedIndex == -1) selectedIndex = 0;
            csb.idIntervencion = intervenciones[EditorGUILayout.Popup("ID Intervención",selectedIndex, intervenciones)];           
            
            EditorGUILayout.EndHorizontal();
        }
        if (hijo is ConsecuenciaActivarSituacion)
        {
            ConsecuenciaActivarSituacion csb = hijo as ConsecuenciaActivarSituacion;
            EditorGUILayout.BeginHorizontal();
            csb.idPNJ = EditorGUILayout.TextField("ID PNJ:", csb.idPNJ);
            csb.idSituacion = EditorGUILayout.TextField("ID Situacion:", csb.idSituacion);            
            EditorGUILayout.EndHorizontal();
        }
        if (hijo is ConsecuenciaObtenerItem)
        {
            ConsecuenciaObtenerItem csb = hijo as ConsecuenciaObtenerItem;
            EditorGUILayout.BeginHorizontal();
            csb.tipoItem = (TypeInventory)EditorGUILayout.EnumPopup("Tipo: ", csb.tipoItem);            
            csb.idItem = ObtenerIdItemConsecuenciaObtenerItem(csb.tipoItem, csb.idItem);
            if (csb.tipoItem == TypeInventory.TOOL)
            {
                csb.isGold = EditorGUILayout.Toggle("De Oro?", csb.isGold);
            }
            EditorGUILayout.EndHorizontal();
        }


    }

    private int ObtenerIdItemConsecuenciaObtenerItem(TypeInventory tipoItem, int idItem)
    {
        int rta = 0;
        switch (tipoItem)
        {
            case TypeInventory.TOOL:
                return (int)(TypesGameElement.Tools)EditorGUILayout.EnumPopup("Ítem: ", (TypesGameElement.Tools)idItem);                
            case TypeInventory.FOOD:
                return (int)(TypesGameElement.Foods)EditorGUILayout.EnumPopup("Ítem: ", (TypesGameElement.Foods)idItem);                
            case TypeInventory.SEED:
                return (int)(TypesGameElement.Seeds)EditorGUILayout.EnumPopup("Ítem: ", (TypesGameElement.Seeds)idItem);
            case TypeInventory.FRUIT:
                return (int)(TypesGameElement.Fruits)EditorGUILayout.EnumPopup("Ítem: ", (TypesGameElement.Fruits)idItem);
            default:
                break;
        }

        return rta;
    }

    protected override int SeleccionarOpcion()
    {
        op = (Consecuencia.TiposConsecuencia)PonerPopup<Consecuencia.TiposConsecuencia>("Tipo Consecuencia: ", op);
        return (int)op;
    }

    internal override Consecuencia AgregarHijo()
    {
        string objectPath = EditorPrefs.GetString("ObjectPath");        
        int nuevaCantidad = padre.GetCantidadHijos() + 1;
        return UtilidadScriptableObject.CrearScriptableObject<ConsecuenciaSubirBajar>(nuevaCantidad, "CSB", objectPath);

    }

    protected override string NombrehijoInfoAdicional(Consecuencia hijo)
    {
        return "(" + hijo.GetType().ToString() + ")";
    }


}