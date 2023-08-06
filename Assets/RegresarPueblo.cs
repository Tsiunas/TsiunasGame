using UnityEngine;

public class RegresarPueblo : MonoBehaviour {
    public string nombreEscenaACargar = "Town";

    public void IrAEscena() {
        SceneLoadManager.Instance.CargarEscena(nombreEscenaACargar);
    }
}
