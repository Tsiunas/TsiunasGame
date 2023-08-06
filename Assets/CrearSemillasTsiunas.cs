using System;
using System.Collections;
using System.Collections.Generic;
using Tsiunas.Mechanics;
using Tsiunas.SistemaDialogos;
using UnityEngine;
using UnityEngine.UI;

public class CrearSemillasTsiunas : MonoBehaviour {

    public Transform[] spawnersTsiuna;
    public GameObject prefabTsiuna;
    private int indexEvent;
    private int cantidadTsiunas;
    private int canidadTsiunasSembradas;
    public List<ToolTarget> espaciosArados;
    public GameObject flechaFA;
    public GameObject flechaReloj;
    public GameObject flechaHambre;
    public GameObject flechaHerramientas;
    public GameObject manoPlantaACosechar;
    public GameObject iconoPuerta;
    public PNJMamaTule mamaTule;
    public UIInventory inventarioFrutos;
    public Transform scrollContentSemillas;
    public ToolTarget espacioAradoConPlanta;
    public GameObject manoIndicadorEspaciosAradosDisponibles;

    public GameObject mensajeObjetivo;
    public GameObject flechaBotonAyuda;
    public GameObject botonAyuda;
    public GameObject panelAyuda;
    private bool panelAyudaAbierto;

    

    private void Awake()
    {
        if (GameManager.Instance.GetGameState == GameState.InIntro)
        {
            espaciosArados.ForEach(x => x.OnCambioTipoDeObjetivo += OnCambioTipoDeObjetivo);

            //espaciosArados.ForEach(x => x.OnUseTool += X_OnUseTool );
            espacioAradoConPlanta.OnUseTool += OnUseToolHand;
        }

       
    }
    private void Start()
    {
        if (GameManager.Instance.GetGameState == GameState.InIntro)
        {
            
            NotificationCenter.DefaultCenter().AddObserver(this, "EventLaunched");
            FindObjectOfType<TapFactory>().OnSemillaRecogida += Handle_OnSemillaRecogida;

            espaciosArados.ForEach(x => { x.IsPlowed = true; x.ChangeSprite(true); });
            espacioAradoConPlanta.IsPlowed = true;
            espacioAradoConPlanta.ChangeSprite(true);

        }

        botonAyuda.GetComponent<Button>().onClick.AddListener(delegate {
            //panelAyuda.SetActive(true);
            panelAyudaAbierto = !panelAyudaAbierto;
            // si se abre el panel de ayuda el juego se pausa, si se cierra se despausa
            GameManager.Instance.SetGameState(panelAyudaAbierto ? GameState.InPause : GameState.InGame);
            panelAyuda.SetActive(panelAyudaAbierto);
        });

        botonAyuda.SetActive(GameManager.Instance.GetGameState == GameState.InGame);

    }

    void OnUseToolHand(TargetType obj)
    {
        // Luego de cosechar la plata ya plantada, se pasa al siguiente globo de diálogo con un delay de 2 segundos
        ObjetivoCumplido(2f);
        GameManager.Instance.currentToolActive = null;
    }

    private void ObjetivoCumplido(float delayToPassToAnotherDialog = 0)
    {
        StaticCoroutine.DoCoroutine(Util.PassToAnotherDialog(delayToPassToAnotherDialog));
        SoundManager.PlaySound(this, 10);
    }

    void OnCambioTipoDeObjetivo(TargetType obj)
    {
        canidadTsiunasSembradas++;
        if (canidadTsiunasSembradas == 3)
        {
            manoIndicadorEspaciosAradosDisponibles.SetActive(false);
            // Luego de sembrar las 3 semillas de Tsiunas, se pasa al siguiente globo de diálogo con un delay de 2 segundos
            ObjetivoCumplido(2f);
        }
    }

    void Handle_OnSemillaRecogida()
    {
        cantidadTsiunas++;
        if (cantidadTsiunas == 5) {
            Util.PassToAnotherDialog();
        }
    }

    public void EventLaunched(Notification notification)
    {
        indexEvent++;
        // se lleva el conteo de un entero para ejecutar la lógica correspondiente

        Debug.Log("Index Event: " + indexEvent.ToString());
        if (indexEvent == 1)
        {
            
            (GameManager.Instance.compraAzadon.comproEnTiendaDonJorge || GameManager.Instance.compraAzadon.comproEnTiendaDayami ? new Action(Util.PassToAnotherDialog) : MostrarPlantaACosechar)();
            if (GameManager.Instance.ComproAzadonEnAlgunaTienda)
            {
                flechaFA.SetActive(true);

            }
        }

        if (indexEvent == 2)
        {
            if (!GameManager.Instance.ComproAzadonEnAlgunaTienda)
            {
                inventarioFrutos.ClosePanelFromOtherGO();
                manoIndicadorEspaciosAradosDisponibles.SetActive(true);
            }
            else {
                // Regreso a la granja - Señala el reloj
                flechaFA.SetActive(false);
                flechaReloj.SetActive(true);

                Camera.main.GetComponent<Animator>().enabled = false;
                Util.PassToAnotherDialog();
            }

            // Usar GameManager.Instance.compraAzadon.comproEnTiendaDonJorge o GameManager.Instance.compraAzadon.comproEnTiendaDayami
           // (GameManager.Instance.compraAzadon.comproEnTiendaDonJorge ? new Action(() =>
           // {
           //     // Compró el azadón en la tienda de Don Jorge
           //     GameManager.Instance.DecreaseHarmonyFlamesLevel(2);
           //     TerminarCicloIntro();
           // }) : () => { }
           //)();

           // (GameManager.Instance.compraAzadon.comproEnTiendaDayami ? new Action(() =>
           // {
           //     // Compró el azadón en la tienda de Dayami
           //     GameManager.Instance.IncreaseHarmonyFlamesLevel(2);
           //     TerminarCicloIntro();
           // }) : () => { }
           //)();


        }

        if (indexEvent == 3)
        {
            if (GameManager.Instance.GetGameState == GameState.InIntro)
                Util.PassToAnotherDialog();
            if (GameManager.Instance.ComproAzadonEnAlgunaTienda)
            {
               flechaReloj.SetActive(false);
                flechaHambre.SetActive(true);
                //UIToolInventory uitool = FindObjectOfType<UIToolInventory>();
                //if (uitool != null)
                    //uitool.Toggles[0].isOn = true;
                Util.PassToAnotherDialog();
            }


        }

        if (indexEvent == 4)
        {
            if (GameManager.Instance.GetGameState == GameState.InIntro)
            {
                //Camera.main.GetComponent<Animator>().SetBool("MostrarPuerta", true);
                //iconoPuerta.SetActive(true);

                Util.PassToAnotherDialog();

                Util.SetPlaceState(NamePlacesTown.Farm, PlaceState.Close);
                Util.SetPlaceState(NamePlacesTown.Store, PlaceState.Open);
            }
            if (GameManager.Instance.ComproAzadonEnAlgunaTienda)
            {             
                FindObjectOfType<UIToolInventory>().OpenPanelFromOtherGO();
                flechaHambre.SetActive(false);
                flechaHerramientas.SetActive(true);
                //UIToolInventory uitool = FindObjectOfType<UIToolInventory>();
                //if (uitool != null)
                    //uitool.Toggles[0].isOn = true;
                Util.SetPlaceState(NamePlacesTown.Farm, PlaceState.Open);
                Util.PassToAnotherDialog();
            }
            else
                iconoPuerta.SetActive(true);

        }

        if (indexEvent == 5) {
            if (GameManager.Instance.ComproAzadonEnAlgunaTienda)
            {
                Util.SetPlaceState(NamePlacesTown.Farm, PlaceState.Open);
                flechaHerramientas.SetActive(false);
                // TODO: Mostrar NUEVO botón de ayuda y flecha apuntando ese botón
                botonAyuda.SetActive(true);


                flechaBotonAyuda.SetActive(true);

            }
        }
           if (indexEvent == 5) {
            if (GameManager.Instance.ComproAzadonEnAlgunaTienda)
            {
                flechaBotonAyuda.SetActive(false);
                // Mostrar mensaje de objetivo
                mensajeObjetivo.SetActive(true);
                Util.SetPlaceState(NamePlacesTown.Farm, PlaceState.Open);

                mensajeObjetivo.GetComponent<Button>().onClick.AddListener(delegate {
                    TerminarCicloIntro();
                });
            }
        }
    }

    void TerminarCicloIntro() {
        mensajeObjetivo.GetComponent<Button>().onClick.RemoveAllListeners();
        mensajeObjetivo.SetActive(false);
        Camera.main.GetComponent<Animator>().enabled = false;
        Camera.main.GetComponent<CameraHandler>().CanPanCamera = true;
        GameManager.Instance.SetGameState(GameState.InGame);
        Util.PassToAnotherDialog();
        //Activar reflexiones de MamaTule...
        PNJMamaTule pnjMamaTule = GameObject.FindObjectOfType<PNJMamaTule>();
        pnjMamaTule.enabled = true;
        iconoPuerta.SetActive(true);
        mamaTule.EstablecerPrimeraAlerta();

        // Cuando termine el primer ciclo se deben guardar los datos de perfil
        PersistenceManager.Instance.GuardarPerfil();

    }

    /// <summary>
    /// Instancia las semillas de Tsiunas
    /// </summary>
    public void MostrarPlantaACosechar()
    {
        //for (int i = 0; i < spawnersTsiuna.Length; i++)
        //{
        //    GameObject tsiuna = (GameObject)Instantiate(prefabTsiuna, spawnersTsiuna[i].position, spawnersTsiuna[i].rotation);
        //    tsiuna.transform.parent = spawnersTsiuna[i].transform;
        //}

        manoPlantaACosechar.SetActive(true);
    }
}

public struct CompraAzadon
{
    public bool comproEnTiendaDonJorge;
    public bool comproEnTiendaDayami;
}
