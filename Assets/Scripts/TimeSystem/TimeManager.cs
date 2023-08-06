using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : Singleton<TimeManager>
{
    // Attributes
    #region Attributes

    // Referencia a la clase GameManager
   

    // Delegates & Events
    public delegate void OnSecondsChange(float currentSecond);
    
    public event OnSecondsChange secondsChange;

    public delegate void OnTimeChange(int day, int minutes, int seconds);


    internal void SetDaysPerMinute(int v)
    {
#if UNITY_EDITOR
        DAYS_PER_MINUTE = v;
#endif
    }

   

    public event OnTimeChange timeChange;

    public delegate void OnFinishedGameTime();
    public event OnFinishedGameTime finishedGameTime;

    public Action<int> dayChange = delegate { };

    public Action OnDecreaseSecondsInDay = delegate {};
    public Action OnDecreaseSecondsInDayIntensityFA = delegate { };

    public GameTime gameTime = new GameTime();

    private float timer = 0f;
    private int currentSecond = 0;
    private int secondInDay = 0;
    private int currentMinute = 0;
    private float secondsDecreaseHunger = 0;
    private float secondsDecreaseIntensityFA = 0;

    public const int LIMIT_DIAS = 30;
#if UNITY_EDITOR
    public static int DAYS_PER_MINUTE = 1;
#else
    public const int DAYS_PER_MINUTE = 1;
    
#endif
    public const int SECONDS_PER_DAY = 6;
    public const int SECONDS_PER_DAY_FA = 6;
    internal static readonly float DIA_PRIMER_HITO = 10;
    internal static readonly float DIA_SEGUNDO_HITO = 20;
    internal static readonly float DIA_HITO_AGUA = 5;
    private int days = 1;
    private bool paused;



    /// <summary>
    /// Retorna el día actual. el valor es un flotante para indicar la fracción del día actual.
    /// </summary>
    public float Dia
    {
        get
        {
            return days + (secondInDay * DAYS_PER_MINUTE)/60f;
        }
    }

    public float VDecreaseSeg
    {
        get { return ((float)60 / DAYS_PER_MINUTE) / SECONDS_PER_DAY; }
    }

    public float VDecreaseSegFA
    {
        get { return ((float)60 / DAYS_PER_MINUTE) / SECONDS_PER_DAY_FA; }
    }
#endregion



    public TimeManager() { base.uniqueToAllApp = true; }


    // Methods
#region Methods
    public void Start()
    {
        //GameManager.Instance.SetGameState(GameState.InGame);
        LoadTime();
       
    }

    /// <summary>
    /// Sirve para realizar la cuenta del tiempo que pasa
    /// </summary>
    public void CountUp()
    {
        if (timer >= 1f)
        {
            currentSecond++;
            secondInDay++;
            secondsDecreaseHunger++;
            secondsDecreaseIntensityFA++;
            timer = 0f;
        }

        if (secondsDecreaseHunger >= VDecreaseSeg) {
            OnDecreaseSecondsInDay();
            secondsDecreaseHunger = 0;
        }

        if (secondsDecreaseIntensityFA >= VDecreaseSegFA)
        {
            OnDecreaseSecondsInDayIntensityFA();
            secondsDecreaseIntensityFA = 0;
        }

        if(secondInDay >= 60/DAYS_PER_MINUTE)
        {
            this.days++;
            dayChange(this.days);
            secondInDay = 0;

            // Cada que pase un día se debe guardar los datos de perfil
            PersistenceManager.Instance.GuardarPerfil();

        }

        if (currentSecond >= 60)
        {
            currentMinute++;
            currentSecond = 0;                        
        }

        if (days >= LIMIT_DIAS)
        {
            currentMinute = LIMIT_DIAS/DAYS_PER_MINUTE;
            currentSecond = 0;
            secondInDay = 0;
            days = LIMIT_DIAS;
            
            if (finishedGameTime != null) finishedGameTime();
        }
        else
        {
            timer += Time.deltaTime;
            if (secondsChange != null) secondsChange(secondInDay * DAYS_PER_MINUTE);
            if (timeChange != null) timeChange(this.days, (int)this.currentMinute, (int)this.currentSecond);
        }


    }

    public GameTime GetGameTime() {
        gameTime = new GameTime(currentSecond, currentMinute, days, secondInDay);
        return gameTime;
    }

    private void Update()
    {
        // Solo se realiza el conteo si el estado de juego actual es: InGame
        if (GameManager.Instance.GetGameState == GameState.InGame)
            CountUp();
        
        // TODO: TEST - Se usa para pausar el tiempo
        if (Input.GetKeyDown(KeyCode.Space))
            paused = !paused;
        //GameManager.Instance.SetGameState(paused ? GameState.InPause : GameState.InGame);
    }

    /// <summary>
    /// Asigna a las respectivas variables de esta clase los datos de GameTime
    /// </summary>
    /// <param name="timeToSet">Estrcutura de tiempo a cargar</param>
    public void SetCurrentTime(GameTime timeToSet)
    {
        this.currentSecond = timeToSet.seconds;
        this.currentMinute = timeToSet.minutes;
        this.days = timeToSet.days;
        this.secondInDay = timeToSet.secondsinDay;
    }

    internal void IncreaseOneDay()
    {
        days++;
        dayChange(days);
    }

    /// <summary>
    /// Carga el tiempo actual jugado
    /// </summary>
    public void LoadTime()
    {
        
        PersistenceManager.Instance.PerformProfileDataLoading((ProfileData pD) => {
            GameTime time = pD.profile_GameTime;
            if (time == null)
                return;
            SetCurrentTime(timeToSet: time);
            timer = 0;
            dayChange(this.days);
        });

    }
#endregion

    public override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        dayChange(this.days);
    }
}

/// <summary>
/// Esta clase representa los datos de tiempo a almacenar / cargar
/// </summary>
[System.Serializable]
public class GameTime
{
    public int seconds;
    public int secondsinDay;
    public int minutes;
    public int days;

    public GameTime(int seconds, int minutes, int days, int secondsInDay)
    {
        this.seconds = seconds;
        this.minutes = minutes;
        this.days = days;
        this.secondsinDay = secondsInDay;
    }

    public GameTime() {       
        this.seconds = 0;
        this.minutes = 0;
        this.days = 0;
        this.secondsinDay = 0;
    }
}