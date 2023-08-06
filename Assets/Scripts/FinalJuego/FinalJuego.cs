using System;
using System.Collections;
using System.Collections.Generic;
using Tsiunas.SistemaDialogos;
using UnityEngine;
using UnityEngine.Playables;

public class FinalJuego : MonoBehaviour 
{
    #region Enums


    #endregion

    #region Atributos y Propiedades
    public UnityEngine.UI.Text txtReloj;
    public UnityEngine.Playables.PlayableDirector directorPrincipal;
    public Transform contenedorFlamas;
    public List<Speeches> textosFinalJuego;
    public GameObject trofeo;
    public GameObject btnQuit;

    private void Awake()
    {
        txtReloj.text = (int) TimeManager.Instance.Dia + " Días";
        ActivarFlamas();
        StartCoroutine(PonerMensajes());
    }


    #endregion

    #region Eventos    


    #endregion

    #region Mensajes Unity


    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    #endregion

    #region Métodos

    private void ActivarFlamas()
    {
        for (int i = 0; i < HarmonyFlamesManager.Instance.HarmonyFlames; i++)
        {
            contenedorFlamas.GetChild(8 - i).gameObject.SetActive(true);
        }
     }

    #endregion
    #region CoRutinas
    bool esperarDialogo;
    IEnumerator PonerMensajes()
    {
        int flamas = HarmonyFlamesManager.Instance.HarmonyFlames;
        PNJDatos mamaTule = GestorPNJ.Instance.GetMamaTule();
        //Esperamos hasta que el director pare la animación de intro
        yield return new WaitWhile(() => directorPrincipal.state == PlayState.Playing);
        //Si se ha acabado el tiempo
        
        if (TimeManager.Instance.Dia >= TimeManager.LIMIT_DIAS)
        {
            DesplegadorDialogo.Instance.DesplegarLinea(mamaTule, new Speech("Se ha acabado el tiempo"), PasarDialogo);
            esperarDialogo = true;
        }
        yield return new WaitWhile(() => esperarDialogo);

        string texto = string.Empty;

        if (flamas < HarmonyFlamesManager.FLAMAS_OBJETIVO)
           { TrackerSystem.Instance.SendTrackingData("user", "failed", "serious-game",flamas+"|serious-game|éxito" );
            texto = "Has conseguido solo " + flamas + (flamas == 1 ? " Flama" : " Flamas") + " de la Armonía";}
        else
            {TrackerSystem.Instance.SendTrackingData("user", "passed", "serious-game",flamas+"|serious-game|éxito" );
            texto = "¡Has conseguido todas las Flamas!";}
        DesplegadorDialogo.Instance.DesplegarLinea(mamaTule, new Speech(texto), PasarDialogo);
        esperarDialogo = true;

        yield return new WaitWhile(() => esperarDialogo);

        Speech[] speechesAMostrar = null;
        //Si consiguio menos de 3 flamas 
        if (flamas <= HarmonyFlamesManager.FLAMAS_OBJETIVO / 3)
        {
            speechesAMostrar = textosFinalJuego[0].speeches;
        }
        //si consiguió más de tres pero no todas
        if (flamas > HarmonyFlamesManager.FLAMAS_OBJETIVO / 3 && flamas < HarmonyFlamesManager.FLAMAS_OBJETIVO)
        {
            speechesAMostrar = textosFinalJuego[1].speeches;
        }
        //Si las consiguió todas
        if(flamas >= HarmonyFlamesManager.FLAMAS_OBJETIVO)
        {
            speechesAMostrar = textosFinalJuego[2].speeches;
            SoundManager.PlayExito();
        }
        if (speechesAMostrar == null)
            throw new TsiunasException("Algo salió mal, no se pudieron cargar los speeches de Final de Juego de MamaTule", true, "Final de Juego", "Hendrys");
        DesplegadorDialogo.Instance.DesplegarTodos(mamaTule, speechesAMostrar, PasarDialogo);
        esperarDialogo = true;
        yield return new WaitWhile(() => esperarDialogo);
        
        Debug.Log("Salir");
        if (flamas >= HarmonyFlamesManager.FLAMAS_OBJETIVO)
        {
            trofeo.SetActive(true);
            
        }
        //Activar botón de salir
        
        btnQuit.SetActive(true);
CuestionarioManager.Instance.MostrarCuestionario("posttest.json");
    }

    private void PasarDialogo()
    {
        esperarDialogo = false;
    }

    public void Salir()
    {
        PersistenceManager.Instance.DeleteFileProfile(PersistenceManager.Instance.CurrentlyLoadedProfileNumber);
        SceneLoadManager.Instance.CargarEscena("MainMenu");
    }



    #endregion
}
