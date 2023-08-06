using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Tsiunas.Mechanics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TilesJuego : MonoBehaviour {

    #region Enums
    #endregion

    #region Atributos y propiedades
    private static bool tilesYaGuardados;
    
    public List<GameObject> prefabToolTargets;

    private static bool cargarPorDefecto = true;
    public static bool CargarPorDefecto { get { return cargarPorDefecto; } internal set { cargarPorDefecto = value; } }




    #endregion

    #region Eventos
    #endregion

    #region Métodos
    #endregion

    #region Mensajes Unity

    private void OnEnable()
    {

        if (PersistentFarm.IsreadyToLoadFromDisk)
        {
            DestroyTiles();
            LoadTiles(PersistentFarm.LoadFromDisk());
            PersistentFarm.primeraVezQueCarga = false;
        }
        else
        {
            if ((tilesYaGuardados && !CargarPorDefecto))
            {
                DestroyTiles();
                LoadTiles(PersistentFarm.Load());
                PersistentFarm.primeraVezQueCarga = false;
            }
        }
    }

    private void LoadTiles(List<ToolTarget.ToolTargetToSave> tilesRecuperados)
    {
        
        foreach (ToolTarget.ToolTargetToSave t in tilesRecuperados)
        {
            GameObject correctPrefab = GetToolTargetPrefab(t);
            if (correctPrefab != null)
            {
                GameObject ti = Instantiate(correctPrefab, this.transform);
                ti.name = correctPrefab.name;
                ti.transform.localPosition = new Vector3(t.x, t.y, t.z);
                ti.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                ti.GetComponent<ToolTarget>().SetFrom(t);
                
                //Plantar si tiene planta
                if(t.isPlanted)
                {
                    if (t.planta != null)
                    {
                        PlantManager.Instance.LoadSowFrom(t.planta, ti);                        
                    }
                }
            }
            
        }
    }

    public GameObject GetToolTargetPrefab(ToolTarget.ToolTargetToSave savedToolTarget) {
        return prefabToolTargets.Find(currentPrefab => currentPrefab.name == savedToolTarget.nombre);
    }

    private void DestroyTiles()
    {
        foreach(Transform c in transform)
        {
            c.gameObject.SetActive(false);
            //Recordatorio...
            //c.GetComponent<BoxCollider>().enabled = false;
            Destroy(c.gameObject);
        }
    }

    private void Awake()
    {

    }
    // Use this for initialization
    void Start ()
    {
        if (!tilesYaGuardados || cargarPorDefecto)
            Save();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnDisable()
    {
        Save();
        PersistentFarm.PlantasActualmenteEnMemoria = true;
    }

    private void Save()
    {
        Debug.Log("Guardando Tiles...");
        //Se preparan los datos a guardar
        List<ToolTarget.ToolTargetToSave> toolTargetsToSave = new List<ToolTarget.ToolTargetToSave>();        
        ToolTarget[] toolTargets = GetComponentsInChildren<ToolTarget>();        
        foreach (ToolTarget tt in toolTargets)
        {           
            toolTargetsToSave.Add(new ToolTarget.ToolTargetToSave(tt));
        }
        //Guardar
        PersistentFarm.Save(toolTargetsToSave);
        tilesYaGuardados = true;
        cargarPorDefecto = false;
        Debug.Log("Tiles guardados con éxito!");

    }




    #endregion


}
