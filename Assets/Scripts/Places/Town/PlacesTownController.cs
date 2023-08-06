using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Enums
#region Enums
public enum NamePlacesTown { Church, Store, TownHall, Bank, Agroinsumos, Pub, Bakery, Farm, tiendaDonJorge, ElRefugio, ELLago, HaciendaLaMarquesa, TiendaElBuenVivir, Out }
#endregion

public class PlacesTownController : MonoBehaviour
{
	// Attributes
	#region Attributes
	public List<Place> places;

    // Lugares Canvas
    public Animator animatorUIPlaceClosed;
    private bool isAnimatingUIPlace;
    public Text txtNombreLugar;
	#endregion

	// Methods
	#region Methods
	// Use this for initialization
	void Start ()
	{
		NotificationCenter.DefaultCenter ().AddObserver(this, "ChangePlaceState");

        if (GameManager.Instance.GetGameState == GameState.InGame)
        {
            OpenAllPlaces();
        }

        //Cerrar Hacienda La Marquesa si aún no se ha elevado la bandera del agua
        Place hacienda = places.Find(p => p.placeName == NamePlacesTown.HaciendaLaMarquesa);
        Place tienda = places.Find(p => p.placeName == NamePlacesTown.TiendaElBuenVivir);
       
        if (!PlaceFlags.Instance.IsTrue(PlaceFlags.INICIAR_HITO_AGUA))
        {           
            hacienda.SetStatePlace(PlaceState.Close);            
            tienda.SetStatePlace(PlaceState.Close);
        }

        if(!PlaceFlags.Instance.IsTrue(PlaceFlags.YA_HABLO_CON_MARGARITA))
        {
            tienda.SetStatePlace(PlaceState.Close);
        }

        places.ForEach(pl => pl.GetComponent<Button>().onClick.AddListener(() => SeleccionarLugar(pl)));
        
	}

    private void SeleccionarLugar(Place _pl)
    {
        if (!isAnimatingUIPlace)
        {
            // Verificar el estado del lugar
            switch (_pl.GetStatePlace())
            {
                case PlaceState.Open:
                    SoundManager.PlayAndThenInvoke(this, 0, () => SceneLoadManager.Instance.CargarEscena(_pl.GetNamePlace().ToString()));
                    Debug.Log("PlaceState: Open... ir a escena");
                    
                    

                    break;
                case PlaceState.Close:
                    SoundManager.PlaySound(this, 1);
                    animatorUIPlaceClosed.GetComponentInChildren<Text>().text = _pl.closeMessage;
                    txtNombreLugar.text = _pl.nombre;
                    Debug.Log("PlaceState: Close... tocar otro lugar");
                    isAnimatingUIPlace = true;
                    AnimUIPlaceClose(true);
                    AnimUIPlaceClose(false, 2.0f);
                    break;
            }
        }
    }

    #region Animación Lugares
    void AnimUIPlaceClose(bool anim)
    {
        animatorUIPlaceClosed.SetBool("UIPlaceClosed", anim);
    }

    void AnimUIPlaceClose(bool anim, float time)
    {
        StartCoroutine(AnimUIPlaceCloseWithDelay(anim, time));
    }

    IEnumerator AnimUIPlaceCloseWithDelay(bool anim, float time)
    {
        yield return new WaitForSecondsRealtime(time);
        AnimUIPlaceClose(anim);
        yield return new WaitForSecondsRealtime(animatorUIPlaceClosed.GetCurrentAnimatorStateInfo(0).length);
        isAnimatingUIPlace = false;
    }
    #endregion

    private void OpenAllPlaces()
    {
        foreach (Place pl in places )
        {
            pl.SetStatePlace(PlaceState.Open);
        }
    }

    /// <summary>
    /// Método de implementación de la Notificación: ChangePlaceState
    /// El método debe llevar el mismo nombre de la notificación
    /// </summary>
    /// <param name="notification">Objeto de notificación</param>
    void ChangePlaceState (Notification notification)
	{
		if (notification.data == null)
			return;

		object[] data = (object[])notification.data;
		NamePlacesTown _placeName = (NamePlacesTown)data [0];
		PlaceState _placeState = (PlaceState)data [1];
		 
		Place currentPlace = places.Find ((Place obj) => obj.placeName.Equals ((NamePlacesTown)_placeName));
		currentPlace.SetStatePlace (_placeState);
	}

	private void OnDestroy()
	{
        places.ForEach(pl => pl.GetComponent<Button>().onClick.RemoveAllListeners());
	}
	#endregion
}

