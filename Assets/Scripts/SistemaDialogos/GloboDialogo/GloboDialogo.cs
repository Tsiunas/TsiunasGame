using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GloboDialogo : ControladorGloboDialogo
{
    #region Attributes
    public Button botonCerrarGloboDialogo;
    #endregion

    #region Unity Messages
    private void OnDestroy() { botonCerrarGloboDialogo.onClick.RemoveAllListeners(); }
    #endregion

    #region Methods
    public override void EstablecerComportamientoSegunTipo(string[] sentencias, Action callback = null) { botonCerrarGloboDialogo.onClick.AddListener(ActivarAnimacionGloboDialogoSale); }

    public override void SegundoToqueSobreGloboDialogo()
    {
        base.SegundoToqueSobreGloboDialogo();
        ActivarAnimacionGloboDialogoSale();
    }
    #endregion
}
