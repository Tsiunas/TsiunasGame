using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Tsiunas.Mechanics;
using Tsiunas.SistemaDialogos;
using System;

public class SituationStore : MonoBehaviour {

	// Attributes
	#region Attributes
	public GameObject speechPrice;
	public GameObject hoe;
	public GameObject donJorge;
	int indexEvent;
	public GameObject uiPriceHoe;
	public GameObject handHelp;
	public Animator animatorYurani;
	public Animator animatorDonJorge;
	public Animator animatorHoe;
	public string[] nameAnims;
    public ControladorProductosParaVentaPersonalizado controlador;
    public GameObject handForTool;

    public UIInventory inventarioFrutos;
    public GameObject manoSenalaFrutos;
    public GameObject particulasTsiunas;
    public SpriteRenderer caraDonJorge;
    public Sprite imagenRostroCambiado;
    public GameObject manoSenalaDonJorge;
    private int tisunasDadas;
    public GameObject flamaDonJorge;

    private List<UIDragFruit> frutosTsiunasDisponibles = new List<UIDragFruit>();
    public AudioClip sonidoDonJorge;

    public PNJActor pnjActorYurani;
    public PNJActor pnjActorDonJorge;
    #endregion

    // Methods
    #region Methods
    private void Awake()
    {
        controlador.OnProdcutoTocado += (ui) => {
            Util.PassToAnotherDialog();
            controlador.HacerProductosInteractuables(false);
            handForTool.SetActive(false);
            SoundManager.PlayExito();
        };


    }



    // Use this for initialization
    void Start () {
		hoe.SetActive (true);
		NotificationCenter.DefaultCenter().AddObserver(this, "EventLaunched");

        // Test: quitar comentarios si se pureba desde este misma escena: "Store" - Don Jorge Primer Ciclo
        // StoreManager.ObtainFruit(TypesGameElement.Fruits.Tsiuna, 2);
        foreach (Transform fruto in inventarioFrutos.scrollContent)
        {
            if (fruto.GetComponent<UIDragFruit>() != null)
            {
                frutosTsiunasDisponibles.Add(fruto.GetComponent<UIDragFruit>());
            }
        }

        frutosTsiunasDisponibles.ForEach(tsiuna => tsiuna.enabled = false);
        inventarioFrutos.ClosePanelFromOtherGO();

        donJorge.GetComponentInChildren<PNJActor>().EstablecerSiPuedeSerTsiunado(true);

       
	}

	public void AnimatePNJ(Animator animator, int indexNameCondition, bool value) {
		animator.SetBool (nameAnims[indexNameCondition], value);
	}


    IEnumerator Esperar(Action callback = null) {
        yield return new WaitForSeconds(0.05f);
        PNJActor.AnimarCaminar(pnjActorDonJorge.id);
        DonJorgeAnimacionesGO d = FindObjectOfType<DonJorgeAnimacionesGO>();
        yield return new WaitUntil(() => d.flag);
        if (callback != null) callback();
    }

	public void EventLaunched (Notification notification) {
		indexEvent++;

		if (indexEvent == 1) {
			hoe.GetComponent<Collider> ().enabled = true;
            //Se activa la mando indicadora
            handForTool.SetActive(true);
			// Animar a Yurani
			AnimatePNJ(animatorYurani, 0, true);
            PNJActor.AnimarCaminar(pnjActorYurani.id);
			// Animar Azadon
			//AnimatePNJ(animatorHoe, 1, true);

            controlador.HacerProductosInteractuables(true);

			//Util.CreateUIWithAnyPosition (speechPrice, hoe.transform.position, -2.0f, out uiPriceHoe);

		}

		// Aparece Don Jorge
		if (indexEvent == 2)
        {
            //Se desactiva la mano indicadora
            handForTool.SetActive(false);
            SoundManager.PlaySound(sonidoDonJorge);
            //AnimatePNJ(animatorHoe, 1, false);
            donJorge.SetActive (true);
			AnimatePNJ(animatorDonJorge, 2, true);


			hoe.GetComponent<Collider> ().enabled = true;
			GameObject.FindObjectOfType<Condition> ().meetCondition = true;
			GameObject.FindObjectOfType<DialogueUIController> ().ShowDialogue ();

            StartCoroutine(Esperar(delegate {
                PNJActor.AnimarHablar(pnjActorDonJorge.id);               
            }));

		}

		if (indexEvent == 3) {

            Debug.Log("gtgtgt");
            FindObjectOfType<ControladorProductosParaVentaPersonalizado>().productosUI[0].EstalecerTextoPrecio(15);
			//uiPriceHoe.GetComponentInChildren<Text>().text = "<b>Precio:</b> 15 monedas";
			GameObject.FindObjectOfType<Condition> ().meetCondition = true;
			GameObject.FindObjectOfType<DialogueUIController> ().ShowDialogue ();
		}

		if (indexEvent == 4) {
            //SceneManager.LoadScene ("Town");
            //SceneLoadManager.Instance.CargarEscena("Town");

            // Se debe abrir el inventario de Frutos para mostrar que se deba arrastrar fruto de tsiunas a Don Jorge
           
            inventarioFrutos.OpenPanelFromOtherGO();
            manoSenalaFrutos.SetActive(true);
            frutosTsiunasDisponibles.ForEach(tsiuna => tsiuna.enabled = true);
            frutosTsiunasDisponibles.ForEach(tsiuna => tsiuna.OnFruitGived += OnFruitGived);

        }

        if (indexEvent == 5) {

            /// Agregar componente para tocar a Don Jorge y que pase al siguiente diálogo que sería el texto de Tsiunado
            donJorge.GetComponentInChildren<Tsiunas.SistemaDialogos.PNJ>().gameObject.AddComponent<TocarDonJorge>();


            // Se bloquea los frutos de tsiunas para no poderlos arrastrar / esperar a que se toque a don Jorge
            frutosTsiunasDisponibles.ForEach(tsiuna => tsiuna.enabled = false);

            // Activar mano de Don Jorge
            manoSenalaDonJorge.SetActive(true);
        }

        if (indexEvent == 6) {
            

            // Activar mano de Don Jorge
            manoSenalaDonJorge.SetActive(false);
            frutosTsiunasDisponibles.ForEach(tsiuna => tsiuna.enabled = true);
        }

        if (indexEvent == 7)
        {
            // Don Jorge regala el Azadón
            StaticCoroutine.DoCoroutine(Util.CreateUILerpGameElement(FindObjectOfType<ControladorProductosParaVentaPersonalizado>().productosUI[0].gameObject, GameObject.Find("Canvas/Inventories/InventoryTools/PlaceholderToolActive"),
                                     TexturesManager.Instance.GetSpriteFromSpriteSheet("Hoe"),
                                                                     1, Consecuencia, 0.1f, true));
        }
	}
    #endregion

    public void Consecuencia()
    {
        // "Tsinuar" a Don Jorge, cambiar su estado para que el GestorPNJ lo guarde como ya tsiunado
        pnjActorDonJorge.CambiarEstadoMachismoActual = PNJDatos.EstadosMachismo.Corresponsable;
        pnjActorDonJorge.isMerchant = true;

        Util.SetPlaceState(NamePlacesTown.Farm, PlaceState.Open);
        Util.SetPlaceState(NamePlacesTown.Store, PlaceState.Close);

        // Don Jorge regala el Azadón como agradecimiento
        StoreManager.ObtainTool(TypesGameElement.Tools.Hoe);

        // Compró en la tienda de Don Jorge
        GameManager.Instance.BoughtInStore = true;
        GameManager.Instance.compraAzadon.comproEnTiendaDonJorge = true;

        SceneLoadManager.Instance.CargarEscena("Town", 1.0f);
    }

    void OnFruitGived(GameObject obj)
    {
        tisunasDadas++;
        // Cuando le doy la tsiuna, desactivo el drag para evitar bugs
        frutosTsiunasDisponibles.ForEach(tsiuna => tsiuna.enabled = false);
        Debug.Log("Fruto de Tsiuna dado a " + obj.name);
        // Se oculta la mano que señala los frutos de tsiunas luego de arrastrar una a Don Jorge
        manoSenalaFrutos.SetActive(false);


        StaticCoroutine.DoCoroutine(Util.PassToAnotherDialog(2.0f));
        // Sonido de "objetivo" cumplido
        SoundManager.PlaySound(this, 0);

        if (tisunasDadas >= 2) {
            particulasTsiunas.GetComponent<Animator>().enabled = true;
            donJorge.GetComponentInChildren<Tsiunas.SistemaDialogos.PNJ>().gameObject.GetComponent<Collider2D>().enabled = false;
            CrearTsiunaDonJorge();
        }
        else {
            // Se activa el sistema de partículas
            particulasTsiunas.SetActive(true);
           
        }

    }

    private void CrearTsiunaDonJorge()
    {
        flamaDonJorge.SetActive(true);
        TrackerSystem.Instance.SendTrackingData("user", "earned", "item", "flama|PNJ_DON JORGE|éxito");
        TrackerSystem.Instance.SendTrackingData("user", "increased", "item", "flama|user|éxito");
        TrackerSystem.Instance.SendTrackingData("user", "progressed", "serious-game", "flama|user|éxito");
        HarmonyFlamesManager.Instance.IncreaseHarmonyFlamesLevel();
    }
}
