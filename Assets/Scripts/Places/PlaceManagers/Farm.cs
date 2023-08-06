using System.Collections;
using System.Collections.Generic;
using Tsiunas.SistemaDialogos;
using UnityEngine;

public class Farm : PlaceManager
{

    #region Enums


    #endregion

    #region Atributos y Propiedades
    public GameObject iconoPuerta;
    public GameObject pnjMamaTule;
    

    #endregion

    #region Eventos    


    #endregion

    #region Mensajes Unity
    #endregion

    #region Métodos
    internal override void ConfigurePlace()
    {
        if (iconoPuerta != null)
            if (GameManager.Instance.GetGameState == GameState.InGame)
            {
                iconoPuerta.SetActive(true);
                if (pnjMamaTule == null)
                    throw new TsiunasException("PNJ MAmaTule en Farm es nulo");
                pnjMamaTule.GetComponent<PNJMamaTule>().enabled = true;
                pnjMamaTule.GetComponent<AlertasMamaTule>().enabled = true;
            }
    }

    private void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.T))
        {
            // TimeManager.Instance.SetCurrentTime(new GameTime(55, 0, 4, 55));
            StoreManager.ObtainSeed(TypesGameElement.Seeds.Tsiuna, 10);
            StoreManager.ObtainTool(TypesGameElement.Tools.Hoe, 1);
            PersistenceManager.Instance.GuardarPerfil();

        }
        */
        


    }

    #endregion



    #region CoRutinas


    #endregion
}
