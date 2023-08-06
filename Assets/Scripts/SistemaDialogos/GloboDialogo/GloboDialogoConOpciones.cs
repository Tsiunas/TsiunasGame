using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class GloboDialogoConOpciones : ControladorGloboDialogo
{
    private List<Button> botonesDeOpcion = new List<Button>();
    public GameObject prefabBotonDeOpcion;
    public Transform gridLayoutGroup;

    public List<Button> BotonesDeOpcion { get { return botonesDeOpcion; } }

    public override void EstablecerComportamientoSegunTipo(string[] sentencias, Action callback = null)
    {
        foreach (string sentenciaDeOpcion in sentencias)
        {
            BotonesDeOpcion.Add(CrearYConfigurarBotonesDeOpcion(sentenciaDeOpcion));
        }

        if (callback != null)
            callback();
    }

    public override void EstablecerLineaDialogo(string _texto, bool opciones)
    {
        letreroGloboDialogo.text = _texto;
    }

    public Button CrearYConfigurarBotonesDeOpcion(string sentenciaAEstablecer) {
        GameObject nuevaOpcion = Instantiate(prefabBotonDeOpcion, gridLayoutGroup);
        nuevaOpcion.GetComponentInChildren<Text>().text = sentenciaAEstablecer;
        Button componenteBoton = nuevaOpcion.GetComponent<Button>();
        componenteBoton.onClick.AddListener(ActivarAnimacionGloboDialogoSale);
        return componenteBoton;
    }

    void OnDestroy()
    {
        foreach (Button boton in BotonesDeOpcion)
        {
            boton.onClick.RemoveAllListeners();
        }
    }
}
