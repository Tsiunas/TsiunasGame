using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;
using Tsiunas.Mechanics;
using Tsiunas.SistemaDialogos;

public class ConsequenceAgroinsumos : ConsequenceDecision
{

    // Attributes
    #region Attributes
    public GameObject prefabHoe;
    private GameObject uiAnimatedHoe;
    public GameObject uiContainerHoe;


    #endregion

    // Methods
    #region Methods
    private void Start()
    {
        GestorPNJ.Instance.ObtenerPNJActoresEnEscena();
    }
    public override T ImplementConsequence<T>(T parameter)
    {
        if (parameter is int)
        {
            SituationAgroinsumos sAgroinsumos = FindObjectOfType<SituationAgroinsumos>();
            int valueParameter = (int)(object)parameter;
            switch (valueParameter)
            {
                case 0:
                    Debug.Log("Compro Arado");
                    StaticCoroutine.DoCoroutine(Util.CreateUILerpGameElement(FindObjectOfType<ControladorProductosParaVentaPersonalizado>().productosUI[0].gameObject, GameObject.Find("Canvas/Inventories/InventoryTools/PlaceholderToolActive"),
                                             TexturesManager.Instance.GetSpriteFromSpriteSheet("Hoe"),
                                                                             1, Consecuencia, 0.1f, true));
                    
                    GestorPNJ.Instance.ObtenerPNJActor("PNJ_DAYAMI").SubirAmistad();
                    break;
                case 1:
                    Debug.Log("No Compro Arado");
                    Util.SetPlaceState(NamePlacesTown.Agroinsumos, PlaceState.Open);
                    Util.SetPlaceState(NamePlacesTown.Store, PlaceState.Close);
                    Util.SetPlaceState(NamePlacesTown.Farm, PlaceState.Close);
                    Destroy(sAgroinsumos.uiPriceHoe);
                    break;
            }
        }
        return default(T);
    }

    void Consecuencia()
    {
        Util.SetPlaceState(NamePlacesTown.Farm, PlaceState.Open);
        Util.SetPlaceState(NamePlacesTown.Agroinsumos, PlaceState.Close);
        Util.SetPlaceState(NamePlacesTown.Store, PlaceState.Close);
        StoreManager.BuyTool(TypesGameElement.Tools.Hoe, false);
        GameManager.Instance.compraAzadon.comproEnTiendaDayami = true;

    }
    #endregion
}
