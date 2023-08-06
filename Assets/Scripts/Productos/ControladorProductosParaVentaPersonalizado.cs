using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class ControladorProductosParaVentaPersonalizado : ControladorProductosParaVenta
{
    public Action<ProductoUI> OnProdcutoTocado;
    public Action<List<ProductoUI>> OnProductosAgregados;

    public override void TocarProducto(ProductoUI ui, ProductoDatos datos)
    {
        if (OnProdcutoTocado != null) OnProdcutoTocado(ui);      
    }

    public override void AgregarComidas()
    {

    }

    public override void ProductosUIAgregados()
    {
        if (OnProductosAgregados != null)
            OnProductosAgregados(this.productosUI);
        
        HacerProductosInteractuables(false);
    }

    public void HacerProductosInteractuables(bool interactuables) {
        productosUI.ForEach(productoActual => { productoActual.GetComponent<Button>().interactable = interactuables; });   
    }
}
