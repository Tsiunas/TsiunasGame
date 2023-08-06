using System;
using System.Collections;
using System.Collections.Generic;
using Tsiunas.Mechanics;
using UnityEngine;

public class TestPlanta : MonoBehaviour, IFAObserved {


    public Tsiunas.Mechanics.Plant plant;

    public event Action<FAStates> OnFlamasChanged = delegate { };



    // Use this for initialization

    void Start ()
    {
        	
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.M))
        {
            OnFlamasChanged(FAStates.Mal);
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            OnFlamasChanged(FAStates.Beneficio);
        }

        
	}

    void OnGUI()
    {
        
    }
}
