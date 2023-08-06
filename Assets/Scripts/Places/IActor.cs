using System;
using System.Collections.Generic;
using UnityEngine;
//Creado por Hendrys Tobar 10/01/2018 20:18:23

/// <summary>
/// interfaz que define métodos para los PNJActor cuando actúan despúes de unan consecuencia
/// </summary>
namespace Tsiunas.Places
{
    public interface IActor
    {
        void Actuar(string mensaje, object origen);
    }
}
