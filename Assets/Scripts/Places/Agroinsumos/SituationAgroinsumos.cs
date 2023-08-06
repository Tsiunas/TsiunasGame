using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SituationAgroinsumos : MonoBehaviour
{
	// Attributes
	#region Attributes
	private int indexEventLaunched;
	public string[] nameAnims;
	public Animator animatorDayami;
	public Animator animatorHoe;
	public GameObject hoe;
	public GameObject speechPrice;
	public GameObject uiPriceHoe;
    public ControladorProductosParaVentaPersonalizado controlador; 
	#endregion

	// Methods
	#region Methods
    private void Awake()
    {
        controlador.OnProdcutoTocado += (ui) => {
            Util.PassToAnotherDialog();
            controlador.HacerProductosInteractuables(false);
        };

        controlador.OnProductosAgregados += (listaDeProductos) => {
            listaDeProductos[0].EstalecerTextoPrecio(20);
        };
    }

	// Use this for initialization
	void Start ()
	{
		NotificationCenter.DefaultCenter().AddObserver(this, "EventLaunched");


	}

	public void EventLaunched (Notification notification) {
		indexEventLaunched++;

		if (indexEventLaunched == 1) {
			Util.PassToAnotherDialog ();
			//hoe.GetComponent<Collider> ().enabled = true;

			// Animar dayami 
			AnimatePNJ(animatorDayami, 0, true);
		}

		if (indexEventLaunched == 2) {
			
			AnimatePNJ(animatorHoe, 1, true);

            controlador.HacerProductosInteractuables(true);
			//Util.CreateUIWithAnyPosition (speechPrice, hoe.transform.position, -2.0f, out uiPriceHoe);
			//uiPriceHoe.GetComponentInChildren<Text>().text = "<b>Precio:</b> 15 monedas";
		}

		if (indexEventLaunched == 3) {
			Util.PassToAnotherDialog ();

			//if (animatorHoe != null)
				//AnimatePNJ(animatorHoe, 1, false);
		}

		if (indexEventLaunched == 4) {
            
            Util.PassToAnotherDialog();
            //Al terminar de hablar con Dayami, ella le regala 4 semillas de maíz (Siempre y cuando le compre el Azadón)
            if (GameManager.Instance.compraAzadon.comproEnTiendaDayami)
                StoreManager.ObtainSeed(TypesGameElement.Seeds.Corn, 4);            
		}
        if (indexEventLaunched == 5)
        {
            SceneLoadManager.Instance.CargarEscena("Town");
        }
    }

	/// <summary>
	/// Activa el parámetro de condición para el cambio de estado de las animaciones
	/// </summary>
	/// <param name="animator">Componente Animator del Gameobject</param>
	/// <param name="indexNameCondition">índice del arreglo de nombres de parámetro</param>
	/// <param name="value">If set to <c>true</c>valor true o false de la condición</param>
	public void AnimatePNJ(Animator animator, int indexNameCondition, bool value) {
		animator.SetBool (nameAnims[indexNameCondition], value);
	}
	#endregion
}

