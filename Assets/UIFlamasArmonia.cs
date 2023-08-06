using System;
using System.Collections;
using System.Collections.Generic;
using Tsiunas.Mechanics;
using UnityEngine;
using UnityEngine.UI;

public class UIFlamasArmonia : MonoBehaviour
{

    public Transform[] spawnPoints;
    public GameObject prefabFlama;
    public float tiempoIntervaloFlamas;
    public Image imagenFrontalCirculoFA;
    private Animator animatorSliderCircleFA;

    public float PorcentajeColor
    {
        get { return ((float)HarmonyFlamesManager.Instance.HarmonyFlames / 9); }
    }
    public float PorcentajeTamanoFlama
    {
        get { return ((float)45 * HarmonyFlamesManager.Instance.HarmonyFlames / 9); }
    }

    float ObtenerPorcentaje(int cantidadActual)
    {
        return ((float)cantidadActual / 100);
    }

    void EventOnCambioIntensidadFlamasArmonia(int obj)
    {
        float porcentajeColor = ObtenerPorcentaje(obj);
        float porcentajeTamano = (float)45 * obj / 100;

        ObtenerFlamas().ForEach(flama =>
        {
            flama.GetComponent<RectTransform>().sizeDelta = new Vector2(26, porcentajeTamano);
            flama.color = new Color(1, porcentajeColor, porcentajeColor, 1);
        });
    }

    // Use this for initialization
    void Awake()
    {
        HarmonyFlamesManager.Instance.OnCambioIntensidadFlamasArmonia += EventOnCambioIntensidadFlamasArmonia;
        HarmonyFlamesManager.Instance.OnCambioIntensidadFlamasArmonia += AlertarCambioIntensidadFlamasArmonia;
        HarmonyFlamesManager.Instance.OnCambioNivelFlamasArmonia += CambioNivelFlamasArmonia;
        //HarmonyFlamesManager.Instance.onCambioColorCirculoFA += OnCambio_Inicial_ColorCirculoFA;
        HarmonyFlamesManager.Instance.onCambioColorCirculoFA += CirculoFASlider;
        CrearFlamasIniciales(HarmonyFlamesManager.Instance.HarmonyFlames);
        animatorSliderCircleFA = imagenFrontalCirculoFA.GetComponent<Animator>();
        CheckAlertaIntensidad(HarmonyFlamesManager.Instance.CurrentFAState);
    }

    void AlertarCambioIntensidadFlamasArmonia(int obj)
    {
        CheckAlertaIntensidad(HarmonyFlamesManager.Instance.CurrentFAState);
    }


    private void CirculoFASlider(int currentIntensityLevel)
    {
        imagenFrontalCirculoFA.fillAmount = (float)currentIntensityLevel / 100f;
    }

    List<Image> ObtenerFlamas()
    {
        List<Image> flamasEncontradas = new List<Image>();
        foreach (var spawnPoint in spawnPoints)
        {
            if (spawnPoint.childCount > 0)
            {
                flamasEncontradas.Add(spawnPoint.GetChild(0).GetComponent<Image>());
            }
        }
        return flamasEncontradas;
    }

    IEnumerator CambiarColorCirculoFA(float alfaFin, float tiempo)
    {
        float progreso = 0;
        float incremento = 0.02f / tiempo;
        while (progreso < 1)
        {
            imagenFrontalCirculoFA.fillAmount = Mathf.Lerp(imagenFrontalCirculoFA.fillAmount, alfaFin, progreso);
            //Color nuevoColor = new Color(1, 1, 1, Mathf.Lerp(imagenFrontalCirculoFA.color.a, alfaFin, progreso));
            //imagenFrontalCirculoFA.color = nuevoColor;
            progreso += incremento;
            yield return new WaitForSeconds(0.02f);
        }
        yield return null;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            HarmonyFlamesManager.Instance.OnCambioIntensidadFlamasArmonia -= EventOnCambioIntensidadFlamasArmonia;
            HarmonyFlamesManager.Instance.OnCambioIntensidadFlamasArmonia -= AlertarCambioIntensidadFlamasArmonia;
            HarmonyFlamesManager.Instance.OnCambioNivelFlamasArmonia -= CambioNivelFlamasArmonia;
            //HarmonyFlamesManager.Instance.onCambioColorCirculoFA -= OnCambio_Inicial_ColorCirculoFA;
            HarmonyFlamesManager.Instance.onCambioColorCirculoFA -= CirculoFASlider;
        }
    }

    void CrearFlamasIniciales(int indice)
    {
        for (int i = 0; i < indice; i++)
        {
            CrearFlama(i);
        }

        //ObtenerFlamas().ForEach(flama => { flama.GetComponent<RectTransform>().sizeDelta = new Vector2(26, ((float)45 * GameManager.Instance.HarmonyFlames / 9));
        //    flama.color = new Color(1, PorcentajeColor, PorcentajeColor, 1);
        //});
    }

    void CrearFlama(int indice)
    {
        if (spawnPoints[indice].childCount == 0)
        {
            // Se crea un objeto de UI por cada flama, según la cantidad de flamas actuales
            GameObject go = (GameObject)Instantiate(prefabFlama, spawnPoints[indice].position, Quaternion.identity);
            go.transform.SetParent(spawnPoints[indice]);
            RectTransform rectT = go.GetComponent<RectTransform>();
            rectT.localScale = Vector2.one;
        }
    }

    void BorrarFlama(Transform padreFlama)
    {
        if (padreFlama.childCount > 0)
        {
            Destroy(padreFlama.GetChild(0).gameObject);
        }
    }

    IEnumerator FadeHarmonyFlame(int vecesQueCambio, bool subio)
    {
        if (subio)
        {
            List<int> indices = new List<int>();
            for (int i = HarmonyFlamesManager.Instance.HarmonyFlames - 1; i > (HarmonyFlamesManager.Instance.HarmonyFlames - 1) - vecesQueCambio; i--)
            {
                indices.Add(i);
            }

            for (int l = indices.Count - 1; l >= 0; l--)
            {
                CrearFlama(indices[l]);
                //ObtenerFlamas().ForEach(flama =>
                //{
                //    flama.GetComponent<RectTransform>().sizeDelta = new Vector2(26, PorcentajeTamanoFlama);
                //    flama.color = new Color(1, PorcentajeColor, PorcentajeColor, 1);
                //});
                EventOnCambioIntensidadFlamasArmonia(HarmonyFlamesManager.Instance.IntensityHarmonyFlames);
                //OnCambio_Inicial_ColorCirculoFA(HarmonyFlamesManager.Instance.IntensityHarmonyFlames);
                CirculoFASlider(HarmonyFlamesManager.Instance.IntensityHarmonyFlames);
                yield return new WaitForSeconds(tiempoIntervaloFlamas);
            }
        }
        else
        {
            List<int> lista = new List<int>();
            for (int i = (HarmonyFlamesManager.Instance.HarmonyFlames + vecesQueCambio) - 1; i >= HarmonyFlamesManager.Instance.HarmonyFlames; i--)
            {
                lista.Add(i);
            }

            for (int l = 0; l < lista.Count; l++)
            {
                BorrarFlama(spawnPoints[lista[l]]);
                //ObtenerFlamas().ForEach(flama =>
                //{
                //    flama.GetComponent<RectTransform>().sizeDelta = new Vector2(26, PorcentajeTamanoFlama);
                //    flama.color = new Color(1, PorcentajeColor, PorcentajeColor, 1);
                //});
                yield return new WaitForSeconds(tiempoIntervaloFlamas);
            }

        }
    }

    void CambioNivelFlamasArmonia(int cantidadActualFlamas, bool subio)
    {
        StartCoroutine(FadeHarmonyFlame(cantidadActualFlamas, subio));
        //StartCoroutine(CambiarColorCirculoFA(PorcentajeColor, .1f));
    }


    void OnCambio_Inicial_ColorCirculoFA(int currentIntensity)
    {
        StartCoroutine(CambiarColorCirculoFA(ObtenerPorcentaje(currentIntensity), .1f));
    }

    #region Alerta Intensidad Circulo FA
    FAStates oldState = FAStates.Beneficio;
    private IEnumerator coroutine;
    private const float TIEMPO_SONIDO_INTENSIDAD = 5.0f;
    private float tiempoParaSonido;
    public AudioClip sndIntensidad;

    private void CheckAlertaIntensidad(FAStates currentState)
    {
        //Si la intensidad de las FA empieza a ser crítica
        if (oldState != currentState)
        {
            animatorSliderCircleFA.StopPlayback();

            if (coroutine != null)
                StopCoroutine(coroutine);
            if (currentState <= FAStates.Mal)
            {
                //Activar la animación de alerta
                if (animatorSliderCircleFA != null)
                {
                    animatorSliderCircleFA.Play("AlertarIntensidad");
                }
                //Y poner a reproducir sonido
                //El tiempo de sonido es más largo entre mejor se esté
                tiempoParaSonido = TIEMPO_SONIDO_INTENSIDAD * (int)currentState;
                coroutine = ReproducirSonidoHambre();
                StartCoroutine(coroutine);
            }
            else
            {
                //Sino, detener
                if (animatorSliderCircleFA != null)
                {
                    animatorSliderCircleFA.Play("Idle");
                }
            }
            oldState = currentState;
        }
    }

    private IEnumerator ReproducirSonidoHambre()
    {
        while (true)
        {
            SoundManager.PlaySound(sndIntensidad);
            yield return new WaitForSeconds(tiempoParaSonido);
        }
    }
    #endregion
}


