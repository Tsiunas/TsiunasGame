using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Tsiunas.Mechanics
{
    public class UIHungerController : MonoBehaviour
    {
        public Slider sliderHungerLevel;
        public Image hungerBackgroundImage;
        public Image hungerStomachImage;
        private Animator animator;
        public AudioClip sndHambre;
        private const float TIEMPO_SONIDO_HAMBRE = 10.0f;
        private float tiempoParaSonido;
        

        private void Awake()
        {
            if (HungerManager.Instance == null)
                return;
            HungerManager.Instance.OnHungerLevelChange += HungerLevelChange;
            HungerManager.Instance.OnHungerStateChange += Instance_HungerStateChange;
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                throw new TsiunasException("Este UI hunger no tiene animación, no se podrá reproducir la animación de alertar por el hambre", false, "UIHunger", "Hendrys");
            }

        }

        private void OnDestroy()
        {
            if (HungerManager.Instance == null)
                return;
            HungerManager.Instance.OnHungerLevelChange -= HungerLevelChange;
            HungerManager.Instance.OnHungerStateChange -= Instance_HungerStateChange;
        }

        void HungerLevelChange(int currentHungerLevel)
        {
            float normalizedFloat = (float)currentHungerLevel / 100f;
            sliderHungerLevel.value = normalizedFloat;
        }

        HungerManager.HungerStates oldState;
        private IEnumerator coroutine;
        void Instance_HungerStateChange(HungerManager.HungerStates currentState)
        {
            hungerBackgroundImage.sprite = TexturesManager.Instance.GetSpriteFromSpriteSheet(HungerManager.HUNGER_BACKGROUND_TEXTURE + (int)currentState);
            hungerStomachImage.sprite = TexturesManager.Instance.GetSpriteFromSpriteSheet(HungerManager.HUNGER_STOMACH_TEXTURE + (int)currentState);
            CheckAlertaHambre(currentState);
           
           
        }

        private void CheckAlertaHambre(HungerManager.HungerStates currentState)
        {
            //Si el hambre empieza a ser crítica
            if (oldState != currentState)
            {
                animator.StopPlayback();
                if(coroutine != null)
                    StopCoroutine(coroutine);
                if (currentState <= HungerManager.HungerStates.ModerateHungry)
                {
                    //Activar la animación de alerta
                    if (animator != null)
                    {
                        animator.Play("AlertarHambre");
                    }
                    //Y poner a reproducir sonido
                    //El tiempo de sonido es más largo entre mejor se esté
                    tiempoParaSonido = TIEMPO_SONIDO_HAMBRE * (int) currentState;
                    coroutine = ReproducirSonidoHambre();
                    StartCoroutine(coroutine);
                }
                else
                {
                    //Sino, detener
                    if (animator != null)
                    {
                        animator.Play("Idle");
                    }
                }
                oldState = currentState;
            }
        }

        private IEnumerator ReproducirSonidoHambre()
        {
            while (true)
            {
                SoundManager.PlaySound(sndHambre);
                yield return new WaitForSeconds(tiempoParaSonido);
            }
            
        }

        private void Start()
        {
            CheckAlertaHambre(HungerManager.Instance.HungerState);
        }



        
    }
}
