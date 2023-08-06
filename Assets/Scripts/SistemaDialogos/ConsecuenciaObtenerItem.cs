
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tsiunas.Mechanics;
using Tsiunas.SistemaDialogos;

/// <summary>
/// Consecuencia que hace que la situaci�n actual encole la intervenci�n dada
/// </summary>
public class ConsecuenciaObtenerItem : Consecuencia {

    public TypeInventory tipoItem;
    public int idItem;
    public bool isGold;

    public override void Ejecutar()
    {
        StoreManager.ObtainItem(tipoItem, idItem);
    }
}