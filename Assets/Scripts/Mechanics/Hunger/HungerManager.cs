using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tsiunas.Mechanics
{
    public class HungerManager : Singleton<HungerManager>, IHungerObserved
    {
        #region Event Actions
        public event Action<int> OnHungerLevelChange = delegate { };
        public event Action<HungerStates> OnHungerStateChange = delegate { };
        #endregion

        #region Attributes
        private int hungry = 100;
        public static readonly int[] HUNGER_LEVELS = { 1, 10, 25, 50, 75, 100 };
        public static readonly string HUNGER_BACKGROUND_TEXTURE = "HungerBackground";
        public static readonly string HUNGER_STOMACH_TEXTURE = "HungerStomach";

        public enum HungerStates { Death, VeryHungry, ModerateHungry, LittleHungry, WithoutHunger };
        private HungerStates hungerState;
        #endregion

        #region Properties
        public int Hungry
        {
            get { return hungry; }
            set
            {
                if (value > 100) hungry = 100; else if (value < 0) hungry = 0; else hungry = value;
                if (CalculateState(hungry) == HungerStates.Death)
                {
                    GameManager.Instance.ActivarMuertePorHambre();
                }

            }
        }

        public HungerStates HungerState
        {
            get { return this.hungerState; }
            set { this.hungerState = value; if (OnHungerStateChange != null) OnHungerStateChange(this.hungerState); }
        }
        #endregion

        #region Unity Messages
        private void Awake()
        {
            PersistenceManager.Instance.PerformProfileDataLoading((ProfileData pD) => {
                Hungry = pD.profile_Hungry;
            });
        }

        private void Start()
        {
            
           

            TimeManager.Instance.OnDecreaseSecondsInDay += DecreaseHungerByTime;
        }

        private void DecreaseHungerByTime()
        {
            DecreaseHungerLevel();
        }

        private new void OnDestroy()
        {
            if (TimeManager.Instance != null)
                TimeManager.Instance.OnDecreaseSecondsInDay -= DecreaseHungerByTime;
            base.OnDestroy();
        }
        #endregion

        #region Methods
        public HungerManager()
        {
            base.uniqueToAllApp = true;
        }
        /// <summary>
        /// Aumenta el valor del nivel de HAMBRE
        /// </summary>
        /// <param name="valueToIncrease">valor a incrementar.</param>
        public void IncreaseHungerLevel(int valueToIncrease)
        {
            this.Hungry += valueToIncrease;
             TrackerSystem.Instance.SendTrackingData("user", "increased", "health",""+ (this.Hungry)+"|user|éxito");
            HungerState = CalculateState(this.hungry);
            OnHungerLevelChange(this.Hungry);
        }

        internal static void EatFood(IEat currentElementToEat)
        {
            currentElementToEat.BeEaten();
            SoundManager.PlayComer();
        }

        /// <summary>
        /// Decrementa el valor del nivel de HAMBRE
        /// </summary>
        /// <param name="valueToDecrease">Value to decrease.</param>
        public void DecreaseHungerLevel(int valueToDecrease = 1)
        {
            
            this.Hungry -= valueToDecrease;
            TrackerSystem.Instance.SendTrackingData("user", "decreased", "health",""+ (this.Hungry)+"|user|éxito");
            HungerState = CalculateState(this.hungry);
            OnHungerLevelChange(this.Hungry);
        }

        private HungerStates CalculateState(int currentLvl)
        {
            int lvlToReturn = 0;

            if (currentLvl == 0)
            {
                lvlToReturn = 0;
                return (HungerStates)lvlToReturn;
            }
            else
            {
                for (int i = 1; i < HUNGER_LEVELS.Length; i++)
                {
                    if (currentLvl >= HUNGER_LEVELS[i] && currentLvl <= HUNGER_LEVELS[i + 1])
                    {
                        lvlToReturn = i;
                        return (HungerStates)lvlToReturn;
                    }
                }
            }

            return (HungerStates)lvlToReturn;
        }
        #endregion

        public override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            OnHungerLevelChange(this.Hungry);
            HungerState = CalculateState(this.hungry);
        }

    }
}
