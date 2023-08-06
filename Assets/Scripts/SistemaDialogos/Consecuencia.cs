
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public abstract class Consecuencia:ScriptableObject, IWithID
{
    public enum TiposConsecuencia { SBVariable, SBAmistad,Actuar,ActivarIntervencion, ActivarSituacion, ObtenerItem};
	public abstract void Ejecutar();

    public string GetID()
    {
        return null;
    }

    public void SetID(string id)
    {
        //vacio
    }
}