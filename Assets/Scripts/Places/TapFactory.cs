using System;
using System.Collections;
using System.Collections.Generic;
using Tsiunas.Mechanics;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Impelmentación del manejado de Taps en la escena Factory
/// </summary>
public class TapFactory : Tap {

    public Action OnSemillaRecogida;
    // Methods
    #region Methods
    /// <summary>
    /// Implementación propia del método: TapGameobject
    /// Se llama cuando se toca un objeto en la escena con una Layer definida 
    /// </summary>
    /// <returns>Tipo genérico.</returns>
    /// <param name="parameter">El parámetro enviado (Genérico).</param>
    /// <typeparam name="T">Tipo genérico.</typeparam>
    public override T TapGameobject<T>(T parameter)
    {
        if (!Util.IsPointerOverUIObject())
        {
            if (parameter is GameObject)
            {
                StartCoroutine(tales());
                GameObject go = parameter as GameObject;

                if (go.tag.Equals("tsiuna"))
                {


                    StaticCoroutine.DoCoroutine(Util.CreateUILerpGameElement(go,
                                                                             GameObject.Find("Canvas/Inventories/InventorySeeds/PlaceholderSeedActive"),
                                                TexturesManager.Instance.GetSpriteFromSpriteSheet("TsiunaSeed"),
                                                                             1, delegate
                                                                             {
                                                                                 StoreManager.ObtainSeed(TypesGameElement.Seeds.Tsiuna, 1);

                                                                                 Destroy(go);
                                                                                 if (OnSemillaRecogida != null)
                                                                                     OnSemillaRecogida();
                                                                             }));

                }

                if (go.tag.Equals("door")) {

                    TapDoor();
                   
                }


                //if (GameManager.Instance.GetGameState == GameState.InGame)
                //{
                //    if (GameManager.Instance.currentToolActive != null)
                //    {
                //        if (!FindObjectOfType<CameraHandler>().BeginningPanning)
                //            // Si hay una herramienta activa actualmente se realiza la llamada de Use y se envía el componente ToolTarget del objeto tocado
                //            if (go.transform.parent.GetComponent<ToolTarget>())
                //                GameManager.Instance.currentToolActive.Use(go.transform.parent.GetComponent<ToolTarget>());
                //            else
                //                GameManager.Instance.currentToolActive.Use(go.GetComponent<ToolTarget>());
                //    }
                //}
            }
        }
        return default(T);
    }

    public void TapDoor()
    {
        if (GameManager.Instance.GetGameState == GameState.InIntro)
        {
            Util.SetPlaceState(NamePlacesTown.Farm, PlaceState.Close);
            Util.SetPlaceState(NamePlacesTown.Agroinsumos, PlaceState.Close);
            Util.SetPlaceState(NamePlacesTown.Store, PlaceState.Open);
        }

        SoundManager.PlaySound(this, 11);
        //SceneManager.LoadScene("Town");
        SceneLoadManager.Instance.CargarEscena("Town", 0.7f);
    }

    IEnumerator tales(){
        yield return new WaitForSeconds(1.0f);
    }
    #endregion
}
