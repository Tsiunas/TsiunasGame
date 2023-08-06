using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;

public class ConfigurarDebug : MonoBehaviour 
{
   
    #region Enums


    #endregion

    #region Atributos y Propiedades

    public bool canDebug;
    #endregion

    #region Eventos    


    #endregion

    #region Mensajes Unity

    void Start ()
    {
        DebugGlobals.canDebug = canDebug;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	#endregion
	
    #region Métodos
	
	
    #endregion
    #region CoRutinas
	
	
	#endregion
}
