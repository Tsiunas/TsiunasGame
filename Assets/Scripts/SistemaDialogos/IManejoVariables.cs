using System;
using System.Collections.Generic;
using UnityEngine;
//Creado por Hendrys Tobar 17/12/2017 1:53:08

namespace Tsiunas.Mechanics
{
    /// <summary>
    /// Interfaz que maneja (Sube y Baja) variables del juego
    /// </summary>
    public interface IManejoVariables
    {
        /// <summary>
        /// Sube la variable <paramref name="v"/> en una cantidad <paramref name="cantidad"/>
        /// </summary>
        /// <param name="v">La variable</param>
        /// <param name="cantidad">la cantidad a subir</param>
        /// <returns>La cantidad resultante</returns>
        int SubirVariable(Variables v, int cantidad);

        /// <summary>
        /// Baja la variable <paramref name="v"/> en una cantidad <paramref name="cantidad"/>
        /// </summary>
        /// <param name="v">La variable</param>
        /// <param name="cantidad">la cantidad a subir</param>
        /// <returns>La cantidad resultante</returns>
        int BajarVariable(Variables v, int cantidad);
    }
}
