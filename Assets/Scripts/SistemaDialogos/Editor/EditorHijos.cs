using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;




public abstract class EditorHijos<P,H>:IModal where P: IContenedorPadre<P,H> where H : IWithID
{

    protected P padre;
    private List<bool> foldouts;

    public EditorHijos(P s)
    {
        padre = s;
        ResetFoldouts();
    }

    private void ResetFoldouts()
    {
        if (padre != null)
        {
            List<H> hh = padre.GetListaHijos();
            foldouts = new List<bool>(new bool[hh.Count]);
        }
    }
    public void PintarBotones(P s)
    {
        if (s == null)
            return;

        
        if (!padre.Equals(s))
        {
            padre = s;
            ResetFoldouts();
        }

        
        int op = SeleccionarOpcion();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Agregar "+ padre.NombreHijos, GUILayout.ExpandWidth(false)))
            {
                H nuevaH;
                if (op == -1)
                    nuevaH = AgregarHijo();
                else
                    nuevaH = AgregarHijo(op);

                padre.GetListaHijos().Add(nuevaH);
                SetIDHijo(nuevaH);                
                foldouts.Add(false);
            }        
        EditorGUILayout.EndHorizontal();

    }

    protected virtual void SetIDHijo(H nuevaH)
    {
        nuevaH.SetID(padre.PrefijoHijos + padre.GetListaHijos().Count);
    }

    protected virtual H AgregarHijo(int op)
    {
        return default(H);
    }

    protected virtual int SeleccionarOpcion()
    {
        return -1;
    }
    internal abstract H AgregarHijo();

    public void PintarHijos(P s)
    {
        if(!s.Equals(padre))
        {
            padre = s;
            ResetFoldouts();
            
            
        }

        PintarBotones(s);

        if(s != null)
        {
            
            if (s.GetListaHijos().Count == 0)
                GUILayout.Label("Esta "+padre.NombreContenedorPadre+ " no tiene "+padre.NombreHijosPlural);
            else
            {
                int i = 0;
                foreach(H hijo in s.GetListaHijos())
                {
                    PintarH(hijo, i);
                    i++;
                }
            }
            
        }
    }
    
    private void PintarH(H hijo, int i)
    {
        EditorGUILayout.BeginHorizontal();
        foldouts[i] =  EditorGUILayout.Foldout(foldouts[i], padre.NombreHijos +NombrehijoInfoAdicional(hijo)+": "+ hijo.GetID());



        //Pintar Botón Borrar
        if (GUILayout.Button("Borrar", GUILayout.ExpandWidth(false)))
        {
            ConfirmarEditorWindow.Create(this);
            hABorrar = i;
        }
        
        

        if(hijo.GetID() !=null)
            hijo.SetID(EditorGUILayout.TextField("", hijo.GetID(), GUILayout.ExpandWidth(false)));
        EditorGUILayout.EndHorizontal();
        
        if (foldouts[i])
        {
            //EditorGUILayout.BeginHorizontal();
            int level = EditorGUI.indentLevel;
            EditorGUI.indentLevel++;

            PintarComponentesHijo(hijo,i);

            EditorGUI.indentLevel = level;
            //EditorGUILayout.EndHorizontal();
        }
        
        
    }

    protected virtual string NombrehijoInfoAdicional(H hijo) { return string.Empty; }

    public Enum PonerPopup<T>(string label, T op)where T:struct,IConvertible
    {

        //LineaDialogo.TiposLineaDialogo op = LineaDialogo.TiposLineaDialogo.ConOpciones;
        Enum o = null;        
        if (typeof(T).IsEnum)
            o = op as Enum;
        else
            throw new Exception("T debe ser enum");

        EditorGUILayout.BeginHorizontal();
        o = EditorGUILayout.EnumPopup(label, o, GUILayout.ExpandWidth(false));
        //T a = oa;
        EditorGUILayout.EndHorizontal();
        return o;
    }

    protected abstract void PintarComponentesHijo(H hijo,int i);

    void IModal.ModalRequest(bool shift)
    {
        //vacio
    }

    int hABorrar = -1;
    void IModal.ModalClosed(ModalWindow window)
    {
        if (hABorrar != -1 && window.Result == WindowResult.Ok)
        {
            BorrarHijo(hABorrar);
            hABorrar = -1;
        }
    }

    internal void BorrarHijo(int hABorrar)
    {
        padre.GetListaHijos().RemoveAt(hABorrar);
    }
}