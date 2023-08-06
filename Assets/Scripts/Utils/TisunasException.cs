using UnityEngine;
using System;

/// <summary>
/// Excepción propia del juego, mecanicas y módulos internos.
/// </summary>
public class TsiunasException : Exception
{
    /// <summary>
    /// El módulo de Tsiunas donde ocurrió la excepción
    /// </summary>
    string modulo;

    /// <summary>
    /// Atribuyto opcional que representa el responsable de esta excepción
    /// </summary>
    string responsable;

    /// <summary>
    /// Es una excepción que debería lanzar un error? Por defecto es falso (es decir, que por defecto esto debería ser tratado como un warning) 
    /// </summary>
    bool error = false;

    public TsiunasException(string mensaje, bool error = false, string modulo = "SIN_MODULO", string responsable = "NADIE" ):base(mensaje)
    {
        this.modulo = modulo;
        this.responsable = responsable;
        this.error = error;
    }

    public override string Message
    {
        get
        {
            return "Error en el módulo de Tsiunas " + modulo + "\n"
                    + "Error: " + base.Message + " - "
                    + "Responsable: " + responsable;

        }
    }

    internal void Tratar()
    {
        string m = "Error Obteniendo Personaje:" + this.Message;
        if (error)
            Debug.LogError(m);
        else
            Debug.LogWarning(m);
    }
}