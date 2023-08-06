using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Tsiunas.Mechanics;
using UnityEngine;

public class PersistentFarm:PersistentInMemory<PersistentFarm>
{
    /// <summary>
    /// Esta vareiable indica que se han guardado las plantas en la lista. Esto indica que se salió de la escena de Granja
    /// </summary>
    public static bool PlantasActualmenteEnMemoria { set; get; }
    /// <summary>
    /// Guarda los ToolTargets
    /// </summary>
    public static List<ToolTarget.ToolTargetToSave> tooltargetsGuardados;

    public static List<Plant.PlantToSave> plantasGuardadas;

    public static void Save(List<ToolTarget.ToolTargetToSave> toSave)
    {
        //PersistentFarm.Instance.Save<List<ToolTarget.ToolTargetToSave>>(toSave);
        
        tooltargetsGuardados = new List<ToolTarget.ToolTargetToSave>(toSave);
        plantasGuardadas = tooltargetsGuardados.Select(t => t.planta).Where(p => p != null).ToList();           
        
    }
    public static bool primeraVezQueCarga = true;
    public static List<ToolTarget.ToolTargetToSave>  Load()
    {
        PlantasActualmenteEnMemoria = false;
        //return PersistentFarm.Instance.Load<List<ToolTarget.ToolTargetToSave>>();
        return tooltargetsGuardados;

    }

    public static List<ToolTarget.ToolTargetToSave>  LoadFromDisk()
    {
        if (primeraVezQueCarga)
        {
            if (GameManager.Instance.GetGameState != GameState.InIntro)
            {
                if (PersistenceManager.Instance.profileData != null)
                {
                    if (PersistenceManager.Instance.profileData.profile_FarmState != null)
                    {
                        primeraVezQueCarga = false;
                        Save(PersistenceManager.Instance.profileData.profile_FarmState);
                        PlantasActualmenteEnMemoria = false;
                        return PersistenceManager.Instance.profileData.profile_FarmState;
                    }
                }
            }
        }
        return null;
    }

    public static bool IsreadyToLoadFromDisk
    {
        get
        {
            return primeraVezQueCarga && PersistenceManager.Instance.profileData != null && PersistenceManager.Instance.profileData.profile_FarmState != null && PersistenceManager.Instance.profileData.profile_FarmState.Count > 0;
        }
    }

}
