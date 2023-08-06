using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu()]
public class ProductoDatos : ScriptableObject {
    
    public Sprite imagen;
    public Sprite imagenOro;
    public string nombre;
    public Tipo tipo;
    public int precio;
    public bool esProductoDeOro;

    public enum Tipo
    {
        FRUTO,
        HERRAMIENTA = 3,
        COMIDA = 1,
        SEMILLA = 1
    }
    private int diasParaEstarDisponible;

    public int DiasParaEstarDisponible
    {
        get { return diasParaEstarDisponible = (int)tipo; }
        set { diasParaEstarDisponible = value; }
    }
}
