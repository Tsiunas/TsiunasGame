using System.Collections.Generic;
using Tsiunas.SistemaDialogos;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AyudasMamaTule : MonoBehaviour
{
    public PNJMamaTule mamaTule;
    public GameObject prefabBoton;
    public GameObject panel;
    public GameObject botonAyuda;

    public Dictionary<string, string> preguntasYRespuestas = new Dictionary<string, string>() {
        { "¿Qué tengo que hacer?", "Consigue 9 Flamas de la armonía antes de 30 días" },
        { "¿Cómo consigo Flamas?", "Regala Tsiunas a la gente que se comporte de manera machista. Algunas personas requieren más de una Tsiuna" },
        { "¿Cómo consigo Tsiunas?", "Ara el prado, siembra semillas y espera a que maduren. Luego puedes cosechar" },
        { "¿Cómo cosecho?", "Selecciona la herramienta mano y toca una planta" },
        { "¿Qué es el estómago?", "Debes alimentarte bien para no desmayarte. Compra comida en el pueblo y arrástrala al estómago" }
    };

    void CerrarPanelAyuda() {
        var pointer = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(botonAyuda, pointer, ExecuteEvents.pointerClickHandler);
    }

    public void Start()
    {
        GetComponent<Button>().onClick.AddListener(CerrarPanelAyuda);

        foreach (var item in preguntasYRespuestas)
        {
            GameObject go = Instantiate(prefabBoton);
            go.transform.SetParent(panel.transform);
            go.transform.localScale = Vector3.one;

            // Establece la pregunta al texto del botón
            go.GetComponentInChildren<Text>().text = item.Key;
            // Obtiene el componente botón
            Button b = go.GetComponent<Button>();

            // Añade listener a cada botón 
            b.onClick.AddListener(() => DesplegadorDialogo.Instance.DesplegarLinea(mamaTule.datosPNJ, new Speech(item.Value, null)));
        }
    }

    private void OnDestroy()
    {
        if (GetComponent<Button>() != null)
            GetComponent<Button>().onClick.RemoveListener(CerrarPanelAyuda);
    }
}