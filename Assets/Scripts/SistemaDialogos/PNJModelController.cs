using System.Collections;
using System.Collections.Generic;
using Tsiunas.SistemaDialogos;
using UnityEngine;

public class PNJModelController : MonoBehaviour {

    private PNJActor actor;
    private Animator _animator;

    public Animator GetAnimator { get { return _animator; } }

    private void Start()
    {
        actor = GetComponent<PNJActor>();
        if (actor == null)
            throw new TsiunasException("No se encontrado componente _PNJActor_", true, "INTEGRACION_MODELOS_ANIMADOS", "Eduardo");
        else
            AgregarModelo();
    }

    void AgregarModelo() {
        GameObject goInstanciado = Instantiate(ObtenerModelo());
        goInstanciado.transform.SetParent(this.transform, false);
        _animator = goInstanciado.GetComponent<Animator>();
    }

    GameObject ObtenerModelo() {
        GameObject go = ModelsManager.Instance.modelos.Find(modelo => modelo.name.Equals(actor.id));
        if (go == null)
            throw new TsiunasException("Modelo con nombre: " + actor.id + " no encontrado", true, "INTEGRACION_MODELOS_ANIMADOS", "Eduardo");
        else
            return go;
    }
}

public class ModelsManager : Singleton<ModelsManager> {
    public List<GameObject> modelos;
    private void Awake() { modelos = new List<GameObject>(Resources.LoadAll<GameObject>("Models")); }
    public ModelsManager () { base.uniqueToAllApp = true; }
}
