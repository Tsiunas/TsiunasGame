using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductoUI : MonoBehaviour {
    #region Attributes
    public Image imagenProducto;
    public Text precioProducto;
    private const string STR_PRECIO = "Precio: ";
    public ProductoDatos.Tipo tipoProducto;
    private int diasParaEstarDisponible;
    private string nombre;
    private int precio;
    private float diaDeVenta;
    private IEnumerator corutinaEstarDisponibleDeNuevo;
    private bool esProductoDeOro;

    #endregion

    #region Properties
    public string Nombre
    {
        get { return nombre; }
        set { nombre = value; }
    }

    public int Precio
    {
        get { return precio; }
        set { precio = value; }
    }

    public bool EsProductoDeOro
    {
        get { return esProductoDeOro; }
        set { esProductoDeOro = value;  }
    }
    #endregion

    #region Methods
    public void RestablecerProductoUI (Sprite sprite, int precio) {
        imagenProducto.sprite = sprite;
        EstalecerTextoPrecio(precio);
    }

    public void Vender(Action callback = null) {
        diaDeVenta = TimeManager.Instance.Dia;
        this.gameObject.SetActive(false);
        corutinaEstarDisponibleDeNuevo = EstarDisponibleDeNuevo(callback);
        StaticCoroutine.DoCoroutine(corutinaEstarDisponibleDeNuevo);

       
    }

    public IEnumerator EstarDisponibleDeNuevo(Action callback = null)
    {
        bool reaparecer = false;
        while (!reaparecer)
        {
            yield return null;
            if (TimeManager.Instance.Dia - diaDeVenta >= diasParaEstarDisponible)
            {
                diaDeVenta = 0;

                this.gameObject.SetActive(true);
                reaparecer = true;
            }
        }

        if (callback != null)
            callback();
        
        StopCoroutine(corutinaEstarDisponibleDeNuevo);
        corutinaEstarDisponibleDeNuevo = null;
    }

    public void EstalecerTextoPrecio(int precio)
    {
        this.precio = precio;
        precioProducto.text = precio.ToString();
    }

    public void EstalecerTextoNombreProducto(string nombre)
    {
        this.nombre = nombre;
    }

    public void EstablecerImagen(Sprite sprite)
    {
        imagenProducto.sprite = sprite;
    }

    public void EstablecerTipoProducto(ProductoDatos productoDatos)
    {
        tipoProducto = productoDatos.tipo;
        EstablecerDiasParaEstarDisponible((int)tipoProducto);
    }

    private void EstablecerDiasParaEstarDisponible(int dias)
    {
        this.diasParaEstarDisponible = dias;
    }
    #endregion

}
