
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tsiunas.Mechanics;

public class ConsecuenciaSubirBajar : Consecuencia {

	public ConsecuenciaSubirBajar()
    {
       
	}



    IManejoVariables manejadorVariables;

	public int cantidad;

	public bool sube;

	public Variables varAModificar;

    private void OnEnable()
    {
        
    }



    public override void Ejecutar()
    {
        manejadorVariables = GameManager.Instance;
        if (sube)
        {
            manejadorVariables.SubirVariable(varAModificar, cantidad);
        }
        else
        {
            manejadorVariables.BajarVariable(varAModificar, cantidad);
        }
    }
}