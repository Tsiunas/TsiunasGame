using System;
using System.Collections;
using System.Collections.Generic;
using Tsiunas.Mechanics;
using Tsiunas.SistemaDialogos;
using UnityEngine;

[RequireComponent(typeof(PNJMamaTule))]
public class AlertasMamaTule : MonoBehaviour 
{
    #region Enums


    #endregion

    #region Atributos y Propiedades

    public string[] textoSobreElHambre;
    public string[] textoSobreIntensidadFA;
   
    
    private PNJMamaTule mamaTuleM;
    public GameObject flechaHambre;
    public GameObject flechaIntensidadFA;
    public string[] textoPrimerHito;
    public string[] textoSegundoHito;
    public string[] textoHitoAgua;

    public PNJMamaTule Actor
    {
        get
        {
            if (mamaTuleM == null)
                return (mamaTuleM = GetComponent<PNJMamaTule>());
            return mamaTuleM;
        }


    }

    #endregion

    #region Eventos    


    #endregion

    #region Mensajes Unity

    void Start ()
    {
        //Cuando arranca este comportamiento...
        //Pone en escucha los eventos necesarios
        if (!PlaceFlags.Instance.IsTrue(PlaceFlags.YA_EXPLICO_INTENSIDAD_FA)) {
            // Si aún no explica la intensidad de las FA
            HarmonyFlamesManager.Instance.OnCambioIntensidadFlamasArmonia += AlertarSobreIntensidadFA;
        }

        if (!PlaceFlags.Instance.IsTrue(PlaceFlags.YA_EXPLICO_HAMBRE))
        {
            HungerManager.Instance.OnHungerStateChange += AlertarSobreHambre;
        }
        /* FIXME: Se comenta mientras se ajustan los gráficos y se crean los recursos para el primer hito.
        if (TimeManager.Instance.Dia < TimeManager.DIA_PRIMER_HITO && !PlaceFlags.Instance.IsTrue(PlaceFlags.INICIAR_PRIMER_HITO))
        {
            TimeManager.Instance.dayChange += AlertarPrimerHito;
            AlertarPrimerHito((int)TimeManager.Instance.Dia);
        }
        */
        if (TimeManager.Instance.Dia <= TimeManager.DIA_HITO_AGUA || !PlaceFlags.Instance.IsTrue(PlaceFlags.INICIAR_HITO_AGUA))
        {
            TimeManager.Instance.dayChange += AlertarHitoAgua;
            AlertarHitoAgua((int)TimeManager.Instance.Dia);
        }

        if (TimeManager.Instance.Dia <= TimeManager.DIA_SEGUNDO_HITO && !PlaceFlags.Instance.IsTrue(PlaceFlags.INICIAR_SEGUNDO_HITO))
        {
            TimeManager.Instance.dayChange += AlertarSegundoHito;
            AlertarSegundoHito((int)TimeManager.Instance.Dia);
        }

        if (TimeManager.Instance.Dia <= TimeManager.LIMIT_DIAS)
        {
            TimeManager.Instance.dayChange += FinalizarJuego;
        }

    }

    void AlertarSobreIntensidadFA(int arg1)
    {
        if (HarmonyFlamesManager.Instance.CurrentFAState == FAStates.Mal)
        {
            this.Actor.EncolarMensaje(textoSobreIntensidadFA);
            PlaceFlags.Instance.RaiseFlag(PlaceFlags.YA_EXPLICO_INTENSIDAD_FA);
            HarmonyFlamesManager.Instance.OnCambioIntensidadFlamasArmonia -= AlertarSobreIntensidadFA;
            //Semuestra la flecha del hambre que señala al hambre
            flechaIntensidadFA.SetActive(true);
            //Y se desaparece en 5 segundos
            this.Invoke(() => flechaIntensidadFA.SetActive(false), 5.0f);
        }
    }


    private void FinalizarJuego(int currentDay)
    {
        if (currentDay >= TimeManager.LIMIT_DIAS)
        {
            //TODO Finalizar Juego
            Debug.LogWarning("TODO: Finalizar Juego");
        }
    }

    private void AlertarHitoAgua(int currentDay)
    {
        if (currentDay >= TimeManager.DIA_HITO_AGUA)
        {
            this.Actor.EncolarMensaje(ActivarBanderaAgua, textoHitoAgua);            
            //TODO: COnfigurar pueblo para cerrar hacienda
            TimeManager.Instance.dayChange -= AlertarHitoAgua;
        }
    }

    private void ActivarBanderaAgua()
    {
        PlaceFlags.Instance.RaiseFlag(PlaceFlags.INICIAR_HITO_AGUA);
    }

    private void AlertarSegundoHito(int currentDay)
    {
        if (currentDay >= TimeManager.DIA_SEGUNDO_HITO)
        {
            this.Actor.EncolarMensaje(textoSegundoHito);
            PlaceFlags.Instance.RaiseFlag(PlaceFlags.INICIAR_SEGUNDO_HITO);
            HarmonyFlamesManager.Instance.DecreaseIntenistyHarmonyFlamesLevel(30);
            //TODO: Configurar escena de Segundo hito
            Debug.LogWarning("Alerta de Primer Hito: TODO: Configurar escena de Segundo hito");
            TimeManager.Instance.dayChange -= AlertarSegundoHito;
        }
    }

    private void AlertarPrimerHito(int currentDay)
    {
        if(currentDay >= TimeManager.DIA_PRIMER_HITO)
        {
            this.Actor.EncolarMensaje(textoPrimerHito);
            PlaceFlags.Instance.RaiseFlag(PlaceFlags.INICIAR_PRIMER_HITO);
            HarmonyFlamesManager.Instance.DecreaseIntenistyHarmonyFlamesLevel(30);
            //TODO: Configurar escena de primer hito
            Debug.LogWarning("Alerta de Primer Hito: TODO: Configurar escena de primer hito");
            TimeManager.Instance.dayChange -= AlertarPrimerHito;
        }
    }

    private void AlertarSobreHambre(HungerManager.HungerStates obj)
    {

        if (obj == HungerManager.HungerStates.ModerateHungry)
        {
            this.Actor.EncolarMensaje(textoSobreElHambre);
            PlaceFlags.Instance.RaiseFlag(PlaceFlags.YA_EXPLICO_HAMBRE);
            HungerManager.Instance.OnHungerStateChange -= AlertarSobreHambre;
            //Semuestra la flecha del hambre que señala al hambre
            flechaHambre.SetActive(true);
            //Y se desaparece en 5 segundos
            this.Invoke(() => flechaHambre.SetActive(false), 5.0f);
        }
            
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnDestroy()
    {
        if(HungerManager.Instance != null)
            HungerManager.Instance.OnHungerStateChange -= AlertarSobreHambre;
        if (TimeManager.Instance != null)
        {
            //TimeManager.Instance.dayChange -= AlertarPrimerHito;
            TimeManager.Instance.dayChange -= AlertarHitoAgua;
            TimeManager.Instance.dayChange -= AlertarSegundoHito;
            TimeManager.Instance.dayChange -= FinalizarJuego;
        }
        if (HarmonyFlamesManager.Instance != null)
            HarmonyFlamesManager.Instance.OnCambioIntensidadFlamasArmonia -= AlertarSobreIntensidadFA;
    }


    #endregion

    #region Métodos


    #endregion
    #region CoRutinas


    #endregion
}
