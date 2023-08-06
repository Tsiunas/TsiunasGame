
#define DEBUG_REFLECTIONS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugreflections : MonoBehaviour {

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
	void Awake ()
    {
        GameManager.Instance.AddTool(new Hoe(10,10,false,new ToolTarget(), ToolType.Hoe, TargetType.GROUND));
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	#endregion
	
	
}
