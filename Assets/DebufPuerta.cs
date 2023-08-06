using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebufPuerta : MonoBehaviour {

	#region Enums
	#endregion

	#region Atributos y propiedades
	
	#endregion
	
	#region Eventos
	#endregion
	
	#region Métodos
	#endregion

	#region Mensajes Unity
	// Use this for initialization
	void Start ()
    {
       
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        TapFactory tf = GameObject.FindObjectOfType<TapFactory>();
        if (tf != null)
            tf.TapDoor();

    }
	#endregion
	
	
}
