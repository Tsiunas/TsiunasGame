using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;
using Tsiunas.Mechanics;
using Tsiunas.SistemaDialogos;

public class ConsequenceStore : ConsequenceDecision
{

    // Attributes
    #region Attributes
    public GameObject prefabHoe;
    private GameObject uiAnimatedHoe;
    public GameObject uiContainerHoe;
    public GameObject mensajeMamaTule;
    private DialogueUIController dialogUI;
    public DialogueSystemController dialogSystemController;
    public Animator animatorMamaTule;
    #endregion

    // Methods
    #region Methods
    private void Start()
    {
        GestorPNJ.Instance.ObtenerPNJActoresEnEscena();
        dialogUI = mensajeMamaTule.GetComponentInChildren<DialogueUIController>();
        dialogUI.OnDialogOutEnded += delegate {
            mensajeMamaTule.SetActive(false);
            animatorMamaTule.enabled = false;
            // Mostrar de nuevo globo de diálogo con las opciones
            dialogSystemController.currentDialogue.gameObject.SetActive(true);
        };
    }
    public override T ImplementConsequence<T>(T parameter)
    {
        if (parameter.GetType() == typeof(int))
        {
            int valueParameter = (int)(object)parameter;
            switch (valueParameter)
            {
                case 0:
                    Debug.Log("Compro azadón");
                    //SituationStore sStore = GameObject.FindObjectOfType<SituationStore> ();
                    //Util.CreateUIWithAnyPosition (prefabHoe, sStore.hoe.transform.position, 0, out uiAnimatedHoe, uiContainerHoe);
                    //Destroy (sStore.hoe);

                    // El primer parámetro debe ser donde toqué, en el inventario 

                    //StaticCoroutine.DoCoroutine(Util.CreateUILerpGameElement(FindObjectOfType<ControladorProductosParaVentaPersonalizado>().productosUI[0].gameObject, GameObject.Find("Canvas/Inventories/InventoryTools/PlaceholderToolActive"),
                    //                         TexturesManager.Instance.GetSpriteFromSpriteSheet("Hoe"),
                    //                                                         5, Consecuencia, 0.1f, true));

                    //GestorPNJ.Instance.ObtenerPNJActor("PNJ_DON JORGE").SubirAmistad();
                    //GestorPNJ.Instance.ObtenerPNJActor("PNJ_YURANI").BajarAmistad();

                    #region Nuevo Diseño Primer Ciclo Don Jorge
                    // Ocultar globo de diálogo con las opciones
                    dialogSystemController.currentDialogue.gameObject.SetActive(false);

                    // Mostrar mensaje Especial de MamaTule
                    mensajeMamaTule.SetActive(true);
                    animatorMamaTule.enabled = true;
                    string textoMamaTule = "Don Jorge se comporta como todo un machista al no respetar a Yurani. <b>No merece que le compres el azadón. </b>";
                    TrackerSystem.Instance.SendTrackingData("user", "selected", "dialog-tree", "machista|PNJ_DON JORGE|fallo");
                    dialogUI.SetConversationDialogue(textoMamaTule, 0);
                    dialogUI.SetBehaviourType(new string[] {});
                    #endregion


                    break;
                case 1:
                    Debug.Log("NO Compro azadón");
                    //Util.SetPlaceState(NamePlacesTown.Store, PlaceState.Close);
                    //Util.SetPlaceState(NamePlacesTown.Agroinsumos, PlaceState.Open);

                    //GestorPNJ.Instance.ObtenerPNJActor("PNJ_DON JORGE").BajarAmistad();
                    //GestorPNJ.Instance.ObtenerPNJActor("PNJ_YURANI").SubirAmistad();
                    break;
            }
        }
        return default(T);
    }

    public void Consecuencia()
    {
        Util.SetPlaceState(NamePlacesTown.Farm, PlaceState.Open);
        Util.SetPlaceState(NamePlacesTown.Store, PlaceState.Close);

        // Obtiene el azadón
        //GameManager.Instance.AddTool (new Hoe(false));

        StoreManager.BuyTool(TypesGameElement.Tools.Hoe, 15, false);

        // Compró en la tienda de Don Jorge
        GameManager.Instance.BoughtInStore = true;
        GameManager.Instance.compraAzadon.comproEnTiendaDonJorge = true;
    }
    #endregion
}
