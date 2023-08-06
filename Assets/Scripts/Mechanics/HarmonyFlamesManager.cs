using System;
using System.Collections;
using System.Collections.Generic;
using Tsiunas.Mechanics;
using UnityEngine;

public partial class HarmonyFlamesManager : Singleton<HarmonyFlamesManager>
{

    #region Attributes
    private int harmonyFlames = 0;
    private int intensityHarmonyFlames = 70;

    public Action<int> onCambioColorCirculoFA = delegate { };
    public Action<int, bool> OnCambioNivelFlamasArmonia = delegate { };
    public Action<int> OnCambioIntensidadFlamasArmonia = delegate { };
    public event Action OnJuegoGanado = delegate { };


    public const int FLAMAS_OBJETIVO = 9;
    #endregion

    protected HarmonyFlamesManager()
    {
        uniqueToAllApp = true;
    }

    #region Properties
    public int HarmonyFlames
    {
        get { return harmonyFlames; }
        set
        {
            if (value > 9)
                harmonyFlames = 9;
            else if (value < 0)
                harmonyFlames = 0;
            else harmonyFlames = value;            
            
        }
    }

    public int IntensityHarmonyFlames
    {
        get { return intensityHarmonyFlames; }
        private set
        {
            if (value > 100) intensityHarmonyFlames = 100;
            else if (value < 0) intensityHarmonyFlames = 0;
            else intensityHarmonyFlames = value;
            OnFlamasChanged(CalcularFAState(intensityHarmonyFlames));
            if (intensityHarmonyFlames <= 0)
            {
                GameManager.Instance.ActivarMuerteMamaTule();
            }
        }
    }

    public bool CreceRapido { get { return this.CurrentFAState == FAStates.Beneficio; } }

    public bool MalEstado { get { return CurrentFAState.EsMalo(); } }

    
    #endregion

    private void Awake()
    {
        // cargar los datos de perfil guardados
        PersistenceManager.Instance.PerformProfileDataLoading((ProfileData pD) => {
            HarmonyFlames = pD.profile_HarmonyFlames;
            IntensityHarmonyFlames = pD.profile_IntensityHarmonyFlames;
        });

        TimeManager.Instance.OnDecreaseSecondsInDayIntensityFA += DecreaseIntensityFA;
        OnJuegoGanado += TerminarJuegoPorFlamas;
    }

    private void TerminarJuegoPorFlamas()
    {
        GameManager.Instance.TerminarJuego();
    }

    void DecreaseIntensityFA()
    {
        DecreaseIntenistyHarmonyFlamesLevel();
    }

    private new void OnDestroy()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.OnDecreaseSecondsInDayIntensityFA -= DecreaseIntensityFA;
    }

    #region Methods
    /// <summary>
    /// Incrementa el valor del nivel de FLAMAS DE LA ARMONÍA en 1
    /// </summary>
    public void IncreaseHarmonyFlamesLevel(int valueToIncrease = 1)
    {
        this.HarmonyFlames += valueToIncrease;
        OnCambioNivelFlamasArmonia(valueToIncrease, true);

        OnCambioIntensidadFlamasArmonia(this.IntensityHarmonyFlames);
        onCambioColorCirculoFA(this.IntensityHarmonyFlames);

        if (this.HarmonyFlames == 9)
            OnJuegoGanado();
        //Se incrementa la intensidad a 100 cuando se gana una nueva flama
        IncreaseIntenistyHarmonyFlamesLevel(100);
    }

    /// <summary>
    /// Decrementa el valor del nivel de FLAMAS DE LA ARMONÍA
    /// </summary>
    public void DecreaseHarmonyFlamesLevel(int valueToDecrease = 1)
    {
        this.HarmonyFlames -= valueToDecrease;
        OnCambioNivelFlamasArmonia(valueToDecrease, false);
    }

    /// <summary>
    /// Decrementa el valor de intensidad de las FLAMAS DE LA ARMONÍA
    /// </summary>
    public void DecreaseIntenistyHarmonyFlamesLevel(int valueToDecrease = 1)
    {
        this.IntensityHarmonyFlames -= valueToDecrease;
        OnCambioIntensidadFlamasArmonia(this.IntensityHarmonyFlames);
        onCambioColorCirculoFA(this.IntensityHarmonyFlames);
    }

    /// <summary>
    /// Incrementa el valor de intensidad de las FLAMAS DE LA ARMONÍA
    /// </summary>
    public void IncreaseIntenistyHarmonyFlamesLevel(int valueToIncrease = 1)
    {
        this.IntensityHarmonyFlames += valueToIncrease;
        OnCambioIntensidadFlamasArmonia(this.IntensityHarmonyFlames);
        onCambioColorCirculoFA(this.IntensityHarmonyFlames);
    }

    public override void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        OnFlamasChanged(CalcularFAState(IntensityHarmonyFlames));
        OnCambioIntensidadFlamasArmonia(this.IntensityHarmonyFlames);
        onCambioColorCirculoFA(this.IntensityHarmonyFlames);
    }
    #endregion

    


}

public static class FAUtil
{
    /// <summary>
    /// Método de extensón para saber si un estado de FA es malo
    /// </summary>
    public static bool EsMalo(this FAStates estado)
    {
        return estado == FAStates.Muriendo || estado == FAStates.Muerte || estado == FAStates.Mal;
    }
}
