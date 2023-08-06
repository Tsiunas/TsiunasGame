using System;
using System.Collections;
using System.Collections.Generic;
using Tsiunas.Mechanics;
using UnityEngine;

public partial class HarmonyFlamesManager : Singleton<HarmonyFlamesManager>, IFAObserved
{

    #region Enums


    #endregion

    #region Atributos y Propiedades


    #endregion

    #region Eventos    
    public event Action<FAStates> OnFlamasChanged = delegate { };  

    #endregion

    #region Mensajes Unity


    #endregion

    #region Métodos
    private FAStates CalcularFAState(int harmonyFlames)
    {
        if (harmonyFlames <= 0)return FAStates.Muerte;
        if (harmonyFlames > 0 && harmonyFlames <=29) return FAStates.Muriendo;
        if (harmonyFlames >= 30 && harmonyFlames <=49) return FAStates.Mal;
        if (harmonyFlames >= 50 && harmonyFlames <=79) return FAStates.Regular;
        if (harmonyFlames >= 80 && harmonyFlames <=100) return FAStates.Beneficio;
        return FAStates.Regular;

    }

    
    public FAStates CurrentFAState
    {
        get
        {
            return CalcularFAState(this.IntensityHarmonyFlames);
        }
    }

    #endregion

    #region CoRutinas


    #endregion
}
