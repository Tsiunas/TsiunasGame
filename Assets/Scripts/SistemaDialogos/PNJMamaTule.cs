using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tsiunas.SistemaDialogos
{
    public class PNJMamaTule : PNJActor
    {

        public List<string> reflexiones;
        public Queue<KeyValuePair<Speech[],Action>> alertas;
        public AudioClip sonidoAlerta;


        #region Enums
        #endregion

        #region Atributos y propiedades
        public Transform [] spawnsTsiuna;
        public GameObject prefabTsiuna;
        public GameObject alertaSprite;
        private readonly static string TEXTO_AGRADECIMIENTO = "Gracias por escuchar a esta anciana Jandy. Toma, te regalo una semilla de Tsiuna.";
        private readonly static int PROBABILIDAD_SEMILLA_TSIUNA = 50;
        private static bool  enPrimeraAlerta = false;


        #endregion

        #region Eventos
        #endregion

        #region Métodos

        public PNJMamaTule():base()
        {
            id = "PNJ_MAMATULE";
            alertas = new Queue<KeyValuePair<Speech[], Action>>();
        }
        
        /// <summary>
        /// MamaTule habla poniendo una de sus reflexiones
        /// </summary>        
        protected override void Hablar()
        {
            //Si es la primera alerta...
            if (enPrimeraAlerta)
            {
                DesplegarSituacionPrimeraAlertaSegunFA();
                EstablecerAlerta((enPrimeraAlerta = false));
            }
            else
            {
                //si aún hay alertas...
                if (alertas.Count > 0)
                {
                    //Disponer alerta
                    KeyValuePair<Speech[], Action> kvp = alertas.Dequeue();
                    DesplegadorDialogo.Instance.DesplegarTodos(this.datosPNJ, kvp.Key);
                    if (kvp.Value != null)
                    {
                        kvp.Value();
                    }
                    if(alertas.Count <= 0)
                    {
                        EstablecerAlerta(false);

                    }
                }
                else
                {
                    //Poner a hablar a MamaTule con sus reflexiones
                    Speech s = new Speech(reflexiones[UnityEngine.Random.Range(0, reflexiones.Count)]);
                    DesplegadorDialogo.Instance.DesplegarLinea(this.datosPNJ, s);
                    DesplegadorDialogo.Instance.OnCerrado += HandleDialogoCerrado;
                }
            }
            
        }

        private void DesplegarSituacionPrimeraAlertaSegunFA()
        {
            //mamaTule explica sobre las plantas, dependiendo de si esta o no en beneficio (normalmente lo estará)
            int sitActivar = 0;
            switch (HarmonyFlamesManager.Instance.CurrentFAState)
            {
                case Mechanics.FAStates.Beneficio: sitActivar = 1; break;
                case Mechanics.FAStates.Mal:
                case Mechanics.FAStates.Muriendo:
                case Mechanics.FAStates.Muerte: sitActivar = 2;break;
                default:
                    sitActivar = 0;
                    break;
            }
            
            EstablecerSituacionActiva(sitActivar);
            
            situacionActiva.IdPNJOwner = this.id;
            situacionActiva.Iniciar();
        }

        

        private static bool primeraVez = true;
        

        private void HandleDialogoCerrado()
        {
            DesplegadorDialogo.Instance.OnCerrado -= HandleDialogoCerrado;
            //Cuando se cierra el diálogo de la refléxión...
            //Hay un 10% de probabilidades de que salga una semilla tsiuna regalada por MamaTule


            //if (primeraVez || UnityEngine.Random.Range(0, 100) < PROBABILIDAD_SEMILLA_TSIUNA)
            if (primeraVez)
            {
                RegalarSemillaTsiuna();
                primeraVez = false;
            }
            else if (GameManager.Instance.seeds.Count == 0)
            {
                RegalarSemillaTsiuna(2);
            }
            else if (GameManager.Instance.seeds.Count == 1)
                RegalarSemillaTsiuna();
        }

        private void RegalarSemillaTsiuna(int cantidadDeSemillasARegalar = 1)
        {
            DesplegadorDialogo.Instance.DesplegarLinea(this.datosPNJ, new Speech(TEXTO_AGRADECIMIENTO));
            for (int i = 0; i < cantidadDeSemillasARegalar; i++)
            {
                int indiceAleatorio = UnityEngine.Random.Range(0, spawnsTsiuna.Length);
                GameObject tsiuna = (GameObject)Instantiate(prefabTsiuna, spawnsTsiuna[indiceAleatorio].position, spawnsTsiuna[indiceAleatorio].rotation);
                tsiuna.transform.parent = spawnsTsiuna[indiceAleatorio].transform;
            }
        }

        public void EstablecerAlerta(bool activar)
        {
            if (activar)
            {
                SoundManager.PlaySound(sonidoAlerta);
                alertaSprite.GetComponent<Animator>().Play("StartAlerta");
            }
            alertaSprite.SetActive(activar);

        }

        /// <summary>
        /// Establece a MamaTulepara cuando se le toque debe poner la primera alerta
        /// </summary>
        public void EstablecerPrimeraAlerta()
        {
            EstablecerAlerta((enPrimeraAlerta = true));
        }

        public void EncolarMensaje(string mensaje, Action callBackCuandoDespliegue)
        {
            alertas.Enqueue(new KeyValuePair<Speech[], Action>(new Speech[1] { new Speech(mensaje) }, callBackCuandoDespliegue));
            EstablecerAlerta(true);
        }

        public void EncolarMensaje(string mensaje)
        {
            EncolarMensaje(mensaje, (Action)null);
        }

        public void EncolarMensaje(Action callBackCuandoDespliegue, params string[] mensajes)
        {
            if (mensajes == null) throw new TsiunasException("Se intentó Encolar un Mensaje en mamaTule con valor nulo");
            if (mensajes.Length == 0) return;
            Speech[] speeches = new Speech[mensajes.Length];
            for (int i = 0; i < speeches.Length; i++)
            {
                speeches[i] = new Speech(mensajes[i]);
            }
            alertas.Enqueue(new KeyValuePair<Speech[], Action>(speeches, callBackCuandoDespliegue));
            EstablecerAlerta(true);
        }

        public void EncolarMensaje(params string[] mensajes)
        {
            EncolarMensaje(null, mensajes);
        }

        #endregion

        #region Mensajes Unity
        // Use this for initialization
        new void Start()
        {
            //Activar Alertas de MamaTule
            this.GetComponent<AlertasMamaTule>().enabled = true;
        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion


    }
}