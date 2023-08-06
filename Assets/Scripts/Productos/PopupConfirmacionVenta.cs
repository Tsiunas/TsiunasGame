using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PopupConfirmacionVenta : MonoBehaviour
{
    public Image imagenProducto;
    public Text textoConfirmacionVenta;
    public Button botonSI;
    public Button botonNO;
    public Text textoDineroDisponible;
    public GameObject fondoTextoDineroDisponible;

    public void EstablecerTextoDineroDisponible(int dineroDisponible, float tiempoRetrasoLimpiarTexto = 2.0f) {
        StopAllCoroutines();
        textoDineroDisponible.text = "";
        fondoTextoDineroDisponible.SetActive(false);
        StartCoroutine(RetrasoTextoDineroDisponible(dineroDisponible, tiempoRetrasoLimpiarTexto));
    }

    private IEnumerator RetrasoTextoDineroDisponible(int dineroAAsignar, float tiempoRetraso) {
        string textoCantidad = "";
        fondoTextoDineroDisponible.SetActive(true);
        switch (dineroAAsignar)
        {
            case 0:
                textoCantidad = "no tienes monedas ";
                break;
            case 1:
                textoCantidad = "solo tienes " + dineroAAsignar + " moneda ";
                break;
            default:
                textoCantidad = "solo tienes " + dineroAAsignar + " monedas ";
                break;
        }
        textoDineroDisponible.text = "No se pudo realizar la compra\n" + textoCantidad + " de oro";
        yield return new WaitForSeconds(tiempoRetraso);
        textoDineroDisponible.text = "";
        fondoTextoDineroDisponible.SetActive(false);
    }

    private void Awake()
    {
        botonNO.onClick.AddListener(DestroyPopup);
    }

    public void DestroyPopup() {
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        botonNO.onClick.RemoveAllListeners();
        botonSI.onClick.RemoveAllListeners();
    }

}
