using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Tsiunas.Mechanics;

public class EscenaMuerte : MonoBehaviour 
{
    #region Enums


    #endregion

    #region Atributos y Propiedades
    public UnityEvent evento;
	
    #endregion
	
    #region Eventos    
	
	
    #endregion
	
    #region Mensajes Unity
	
	void Start () {
		
	}

    // Update is called once per frame
    bool played = false;
	void Update ()
    {
        if (!played)
        {
            bool clicked = Input.touchCount > 0 || Input.GetMouseButton(0);
            if (clicked)
            {
                evento.Invoke();
                played = false;
            }
        }
		
	}
    #endregion

    #region Métodos

    public void Salir()
    {
        //TEDE muerte
        //Recuperar la intensidad de las flamas a 50
        if (HarmonyFlamesManager.Instance.CurrentFAState == FAStates.Muerte)
            HarmonyFlamesManager.Instance.IncreaseIntenistyHarmonyFlamesLevel(50);
        else
            if (HungerManager.Instance.HungerState == HungerManager.HungerStates.Death)
            HungerManager.Instance.Hungry = 50;

        //Quitarle todo el dinero
        GameManager.Instance.Money = 0;
        //Perder todas las frutas
        StoreManager.LoseAllFruits();
        //perder todas las semillas
        StoreManager.LoseAllSeeds();

        // Regresar de la muerte con el azadón con 50 % de Durabilidad
        if (!GameManager.Instance.WasObtainedTool(ToolType.Hoe))
        {
            Tool herramientaDada = StoreManager.ObtainTool(TypesGameElement.Tools.Hoe);
            int durabilidad = herramientaDada._filledDurability;
            herramientaDada.Durability = durabilidad / 2;
        }

        // Regresar de la muerte con2 semillas de Tsiunas
        StoreManager.ObtainSeed(TypesGameElement.Seeds.Tsiuna, 2);
        
        //Marcar la granja como que debe cargar la granja por defecto
        TilesJuego.CargarPorDefecto = true;
        //Volver a la granja
        GameManager.Instance.SetGameState(GameState.InGame);
        SceneLoadManager.Instance.CargarEscena("Farm");
    }
    #endregion
    #region CoRutinas
	
	
	#endregion
}
