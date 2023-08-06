using UnityEngine;
using System.Collections.Generic;

public interface IContenedorPadre<P,H>
{
    int GetCantidadHijos();
    List<H> GetListaHijos();
    string NombreContenedorPadre { get; }
    string NombreContenedorPadrePlural { get; }
    string NombreHijos { get; }
    string NombreHijosPlural { get; }
    string PrefijoHijos { get; }
    string IdHijo(int i);

}
