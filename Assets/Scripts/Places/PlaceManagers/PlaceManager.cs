using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlaceManager : MonoBehaviour 
{
    #region Enums


    #endregion

    #region Atributos y Propiedades


    #endregion

    #region Eventos    


    #endregion

    #region Mensajes Unity
    private void Awake()
    {
        

    }

   

    public void Start ()
    {
        ConfigurePlace();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    #endregion

    #region Métodos
    internal abstract void ConfigurePlace();

    #endregion
    #region CoRutinas


    #endregion
}
