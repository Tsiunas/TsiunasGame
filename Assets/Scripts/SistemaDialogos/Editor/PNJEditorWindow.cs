using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Tsiunas.SistemaDialogos;

public class PNJEditorWindow : EditorWindow
{
    
    public static PNJDatos pnj;
    private static int situacionIndex = 1;
    //private int intervencionIndex = 1;
    private EditorHijos<Situacion,Intervencion> intervencionesEditor;
    //private int lineaIndex = 1;
    
    


    [MenuItem("Window/Editor de PNJ %#e")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(PNJEditorWindow));        
    }

    void OnEnable()
    {

        if (EditorPrefs.HasKey("ObjectPath"))
        {
            string objectPath = EditorPrefs.GetString("ObjectPath");
            pnj = AssetDatabase.LoadAssetAtPath(objectPath, typeof(PNJDatos)) as PNJDatos;
            
            intervencionesEditor = new EditorIntervenciones(null);
            CrearEditorIntervencionesDeAcuerdoASituaciones();
            
        }

       

    }

    public static string GetCurrentSitID()
    {
        return pnj.situaciones[situacionIndex - 1].id;
    }

    public static string[] GetCurrentIntervenciones()
    {
        if (pnj == null || pnj.situaciones == null || pnj.situaciones.Count == 0 || situacionIndex < 0)
            return null;
        Situacion s = pnj.situaciones[situacionIndex - 1];
        string[] rta = new string[s.intervenciones.Count];

        for (int i = 0; i < s.intervenciones.Count; i++)
        {
            rta[i] = s.intervenciones[i].id;
        }
        return rta;

    }


    void OnGUI()
    {
        
        GUILayout.BeginHorizontal();
            GUILayout.Label("Editor de PNJ", EditorStyles.boldLabel);
            if (pnj != null)
            {
                
                if (GUILayout.Button("Mostrar PNJ"))
                {                             
                    //Esta línea saca un error después en los BeginHorizontal. No sé por qué...
                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = pnj;                    
                }            
            }

            if (GUILayout.Button("Abrir PNJ"))
            {
                AbrirPNJ();
            }
            if (GUILayout.Button("Nuevo PNJ"))
            {
                pnj = null;
                //EditorUtility.FocusProjectWindow();
                //Selection.activeObject = pnj;
            }
        GUILayout.EndHorizontal();

        if (pnj == null)
        {
            GUILayout.BeginHorizontal();
                GUILayout.Space(10);
                if (GUILayout.Button("Crear Nuevo PNJ", GUILayout.ExpandWidth(false)))
                {
                    CrearNuevoPNJ();
                }
                if (GUILayout.Button("Abrir PNJ Existente", GUILayout.ExpandWidth(false)))
                {
                    AbrirPNJ();
                }
            GUILayout.EndHorizontal();
        }

        try
        {
            GUILayout.BeginHorizontal();
        }
        catch (NullReferenceException e)
        {
            if (e.Source == "UnityEngine.IMGUIModule")
            {
                Debug.LogWarning("Hay un error al Mostrar PNJ en pestaña project");
            }
            GUILayout.BeginHorizontal();
            /*
            try
            {

                GUILayout.EndHorizontal();
            }
            catch (InvalidOperationException)
            {
                Debug.LogWarning("Error operación inválida en Editor PNJ");
            }
            */
        }
                GUILayout.Space(20);
            GUILayout.EndHorizontal();
        
        
        


        if (pnj != null)
        {
            
            GUILayout.BeginHorizontal();
                pnj.id = EditorGUILayout.TextField("ID:", pnj.id);
                pnj.nombre = EditorGUILayout.TextField("Nombre:", pnj.nombre);
                pnj.avatar = (Sprite) EditorGUILayout.ObjectField("Avatar:", pnj.avatar, typeof(Sprite), false);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
                pnj.esAnonimo = EditorGUILayout.Toggle("Es Anónimo:", pnj.esAnonimo);                
                pnj.estadoMachismo = (PNJDatos.EstadosMachismo)EditorGUILayout.EnumPopup("Machismo", pnj.estadoMachismo);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
                pnj.speechPorDefecto.texto = EditorGUILayout.TextField("Texto por Defecto:", pnj.speechPorDefecto.texto);
            GUILayout.EndHorizontal();
            if (pnj.estadoMachismo != PNJDatos.EstadosMachismo.Corresponsable)
            {
                GUILayout.BeginHorizontal();
                pnj.mensajeMamaTule = EditorGUILayout.TextField("Mensaje de MamaTule:", pnj.mensajeMamaTule);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                pnj.textoFlamado = EditorGUILayout.TextField("Texto Tsiunado:", pnj.textoFlamado);
                GUILayout.EndHorizontal();
                
            }
            GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Speeches por Nivel de Amistad");
            GUILayout.EndHorizontal();
            if(pnj.speechesPorAmistad == null || pnj.speechesPorAmistad.Count == 0)
            {
                int c = Enum.GetNames(typeof(NivelesAmistad)).Length;
                pnj.speechesPorAmistad = new List<Speech>(new Speech[c]);
                for (int i = 0; i < c; i++)
                {                    
                    pnj.speechesPorAmistad[i] = new Speech();
                }
                pnj.speechesPorAmistad[(int)NivelesAmistad.AMISTAD_ENTABLADA].texto = GestorPNJ.TEXTO_AMISTOSO;
                pnj.speechesPorAmistad[(int)NivelesAmistad.DESDEN].texto = GestorPNJ.TEXTO_DESDEN;
                pnj.speechesPorAmistad[(int)NivelesAmistad.DISGUSTO].texto = GestorPNJ.TEXTO_DISGUSTO;
                pnj.speechesPorAmistad[(int)NivelesAmistad.EMPATICO].texto = GestorPNJ.TEXTO_EMPATICO;                
            }
            int lvl = ++EditorGUI.indentLevel;
            for (int i = 0; i < pnj.speechesPorAmistad.Count; i++)
            {
                NivelesAmistad na = (NivelesAmistad)i;
                //No se pintan los estados regular y Desdén
                //En Desdén, el personaje no habla
                //En Regular, el texto es el mismo que por defecto
                if (na == NivelesAmistad.REGULAR || na == NivelesAmistad.DESDEN) continue;
                GUILayout.BeginHorizontal();                
                    pnj.speechesPorAmistad[i].texto = EditorGUILayout.TextField(na.ToString(), pnj.speechesPorAmistad[i].texto);
                GUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel = lvl;




            if (!pnj.esAnonimo)
            {
                #region Botones de Situación
                GUILayout.BeginHorizontal();
                GUILayout.Space(10);

                if (GUILayout.Button("Prev", GUILayout.ExpandWidth(false)))
                {
                    if (situacionIndex > 1)
                        situacionIndex--;
                }
                GUILayout.Space(5);
                if (GUILayout.Button("Sig.", GUILayout.ExpandWidth(false)))
                {
                    if (situacionIndex < pnj.situaciones.Count)
                    {
                        situacionIndex++;
                    }
                }

                GUILayout.Space(60);

                if (GUILayout.Button("Agregar Situación", GUILayout.ExpandWidth(false)))
                {
                    AgregarSituacion();
                }

                if (pnj.situaciones.Count > 0)
                {
                    if (GUILayout.Button("Borrar", GUILayout.ExpandWidth(false)))
                    {
                        BorrarSituacion(situacionIndex - 1);
                    }
                }
                GUILayout.EndHorizontal();
                #endregion

            }


            if (pnj.situaciones == null)
                Debug.Log("wtf");
            if (pnj.situaciones.Count > 0)
            {
                GUILayout.BeginHorizontal();
                    situacionIndex = Mathf.Clamp(EditorGUILayout.IntField("Situación Actual", situacionIndex, GUILayout.ExpandWidth(false)), 1, pnj.situaciones.Count);
                    //Mathf.Clamp (viewIndex, 1, inventorysituaciones.situaciones.Count);
                    EditorGUILayout.LabelField("de   " + pnj.situaciones.Count.ToString() + "  situaciones", "", GUILayout.ExpandWidth(false));
                    pnj.situaciones[situacionIndex - 1].id = EditorGUILayout.TextField("ID Situacion", pnj.situaciones[situacionIndex - 1].id as string, GUILayout.ExpandWidth(false));
                    pnj.situaciones[situacionIndex-1].noConsumir =  EditorGUILayout.Toggle("No Consumir: ", pnj.situaciones[situacionIndex - 1].noConsumir);
                GUILayout.EndHorizontal();

                PintarEditorIntervenciones(pnj.situaciones[situacionIndex - 1]);


                //pnj.situaciones[situacionIndex - 1].itemIcon = EditorGUILayout.ObjectField("Item Icon", pnj.situaciones[situacionIndex - 1].itemIcon, typeof(Texture2D), false) as Texture2D;
                //pnj.situaciones[situacionIndex - 1].itemObject = EditorGUILayout.ObjectField("Item Object", pnj.situaciones[situacionIndex - 1].itemObject, typeof(Rigidbody), false) as Rigidbody;

                /*GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                pnj.situaciones[situacionIndex - 1].isUnique = (bool)EditorGUILayout.Toggle("Unique", pnj.situaciones[situacionIndex - 1].isUnique, GUILayout.ExpandWidth(false));
                pnj.situaciones[situacionIndex - 1].isIndestructible = (bool)EditorGUILayout.Toggle("Indestructable", pnj.situaciones[situacionIndex - 1].isIndestructible, GUILayout.ExpandWidth(false));
                pnj.situaciones[situacionIndex - 1].isQuestItem = (bool)EditorGUILayout.Toggle("QuestItem", pnj.situaciones[situacionIndex - 1].isQuestItem, GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                pnj.situaciones[situacionIndex - 1].isStackable = (bool)EditorGUILayout.Toggle("Stackable ", pnj.situaciones[situacionIndex - 1].isStackable, GUILayout.ExpandWidth(false));
                pnj.situaciones[situacionIndex - 1].destroyOnUse = (bool)EditorGUILayout.Toggle("Destroy On Use", pnj.situaciones[situacionIndex - 1].destroyOnUse, GUILayout.ExpandWidth(false));
                pnj.situaciones[situacionIndex - 1].encumbranceValue = EditorGUILayout.FloatField("Encumberance", pnj.situaciones[situacionIndex - 1].encumbranceValue, GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                GUILayout.Space(10);
                */

            }
            else
            {
                GUILayout.Label("Este Personaje no tiene situaciones");
            }
        }
        if (GUI.changed)
        {
            if (pnj != null)
            {
                EditorUtility.SetDirty(pnj);
                SetDirtyLineasDialogos(pnj);
            }
        }

        
    }

    private void SetDirtyLineasDialogos(PNJDatos pnj)
    {
        if(pnj != null)
        {
            if(pnj.situaciones!= null)
            {
                foreach(Situacion s in pnj.situaciones)
                {
                    if(s.intervenciones!= null)
                    {
                        foreach(Intervencion i in s.intervenciones)
                        {
                            if(i.lineas != null)
                            {
                                foreach(LineaDialogo l in i.lineas)
                                {
                                    if(l!=null)
                                        EditorUtility.SetDirty(l);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void PintarEditorIntervenciones(Situacion situacion)
    {
        if (intervencionesEditor == null)
            RecrearIntervencionEditor(null);
        intervencionesEditor.PintarHijos(situacion);       
    }

    private void RecrearIntervencionEditor(Situacion s)
    {
        intervencionesEditor = new EditorIntervenciones(s);
    }

    void CrearNuevoPNJ()
    {
        // There is no overwrite protection here!
        // There is No "Are you sure you want to overwrite your existing object?" if it exists.
        // This should probably get a string from the user to create a new name and pass it ...
        string absPath = EditorUtility.SaveFilePanel("Crear PNJ", "PNJ", "Nuevo PNJ", "asset");
        if (absPath.StartsWith(Application.dataPath))
        {
            string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
            situacionIndex = 1;
            pnj = CrearPNJ.Create(relPath);
            pnj.nombre = pnj.name;
            pnj.id = "PNJ_" + pnj.nombre.ToUpper();
        }

        
        if (pnj)
        {
            pnj.situaciones = new List<Situacion>();
            string relPath = AssetDatabase.GetAssetPath(pnj);
            EditorPrefs.SetString("ObjectPath", relPath);
            RecrearIntervencionEditor(null);

            

        }
    }

    void AbrirPNJ()
    {
        string absPath = EditorUtility.OpenFilePanel("Seleccionar PNJ", "", "");
        if (absPath.StartsWith(Application.dataPath))
        {
            string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
            pnj = AssetDatabase.LoadAssetAtPath(relPath, typeof(PNJDatos)) as PNJDatos;
            CrearEditorIntervencionesDeAcuerdoASituaciones();

            if (pnj)
            {
                EditorPrefs.SetString("ObjectPath", relPath);                
            }
            

        }
    }

    private void CrearEditorIntervencionesDeAcuerdoASituaciones()
    {
        if (pnj.situaciones == null)
            pnj.situaciones = new List<Situacion>();
        else
        {
            if (pnj.situaciones.Count > 0)
            {
                situacionIndex = 1;
                RecrearIntervencionEditor(pnj.situaciones[situacionIndex - 1]);
            }
        }
    }

    void AgregarSituacion()
    {
        Situacion newItem = new Situacion();
        newItem.id = "SIT_";
        pnj.situaciones.Add(newItem);
        situacionIndex = pnj.situaciones.Count;        
        RecrearIntervencionEditor(pnj.situaciones[situacionIndex - 1]);
    }

    void BorrarSituacion(int index)
    {
        if (pnj.situaciones.Count == 0)
            return;
        pnj.situaciones.RemoveAt(index);
    }
}