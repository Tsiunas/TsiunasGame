using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour {

    // Attributes
    #region Attributes
    public Slider clock;
    public Text clockText;
    public Text timeGeneralText;
    #endregion

    // Methods
    #region Methods
    void Awake()
    {
        // Se suscriben métodos a los eventos de la clase TimeManager
        TimeManager.Instance.secondsChange += SecondsChange;
        TimeManager.Instance.dayChange += DayChange;
        TimeManager.Instance.timeChange += TimeChange;
    }

    /// <summary>
    /// Actualiza la UI de tiempo general
    /// TODO: timeGeneralText es solo para probar la funcionalidad
    /// </summary>
    /// <param name="day">días transcurridos</param>
    /// <param name="minutes">minutos transcurridos.</param>
    /// <param name="seconds">segundos transcurridos.</param>
    void TimeChange(int day, int minutes, int seconds)
    {
        //timeGeneralText.text = day + " " + (day == 1 ? "día" : "días") + " - " + minutes + " minutos" + " - " + seconds + " segundos" + " dias (float)="+TimeManager.Instance.Dia;
    }

    /// <summary>
    /// Actualiza la UI del día actual
    /// </summary>
    /// <param name="currentDay">día actual</param>
    void DayChange(int currentDay)
    {
        clockText.text = currentDay.ToString() + " " + (currentDay == 1 ? "día" : "días");
    }

    /// <summary>
    /// Actualiza la UI del reloj
    /// </summary>
    /// <param name="currentSecond">segundo actual transcurrido</param>
    void SecondsChange(float currentSecond)
    {
        float normalizedSeconds = (float)currentSecond / 60f;
        clock.value = normalizedSeconds;
    }
    #endregion

    private void OnDestroy()
    {
        if (TimeManager.Instance == null)
            return;
        TimeManager.Instance.secondsChange -= SecondsChange;
        TimeManager.Instance.dayChange -= DayChange;
        TimeManager.Instance.timeChange -= TimeChange;
    }
}
