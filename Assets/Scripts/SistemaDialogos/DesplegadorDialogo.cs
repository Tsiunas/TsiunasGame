
using System;
using UnityEngine;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

namespace Tsiunas.SistemaDialogos
{
    public class DesplegadorDialogo : Singleton<DesplegadorDialogo>
    {
        #region Attributes
        public GameObject prefabGloboDialogo;
        public GameObject prefabGloboDialogoPJ;
        public GameObject prefabGloboDialogoConOpciones;
        private ControladorGloboDialogo controladorGlogoDialogo;
        private Queue<Speech> speechesEncolados;
        private Queue<PNJDatosSpeeches> pnjDatosSpeechesEncolados;
        private PNJDatos pnjActual;
        private PNJDatos _pnj;
        #endregion


        protected DesplegadorDialogo()
        {

        }

        private bool desplegando = false;

        public event Action OnDesplegado;

        public event Action OnCerrado = delegate { };

        private Action callback;

        private Action callBackTodos;

        public event Action<int> OnClicOpcion;

        /// <summary>
        /// Despliega el Speech dado con el PNJdatos dado (avatar y nombre)
        /// </summary>
        /// <param name="pnj"></param>
        /// <param name="speech"></param>
        public void DesplegarLinea(PNJDatos pnj, Speech speech)
        {
            // TODO implement here
            /* Implementación para crear y configurar el globo de diálogo de un PNJ */
            ConfigurarGloboDeDialogo(pnj, speech);
        }

        /// <summary>
        /// Despliega el Speech dado con el PNJdatos dado (avatar y nombre).
        /// Cuando se cierra el Desplegado ejecuta el callback dado
        /// </summary>
        /// <param name="pnj"></param>
        /// <param name="speech"></param>
        public void DesplegarLinea(PNJDatos pnj, Speech speech, Action callBack)
        {
            this.callback = callBack;
            DesplegarLinea(pnj, speech);
        }

        /// <summary>
        /// Ejecuta todos los speeches con su respectivo PNJDatos
        /// </summary>
        /// <param name="datos"></param>
        public void DesplegarTodos(params PNJDatosSpeeches[] datos)
        {
            if (datos.Length == 0)
                return;
            if (datos.Length == 1)
            {
                DesplegarTodos(datos[0].pnjDatos, datos[0].speeches);
                return;
            }
            pnjDatosSpeechesEncolados = new Queue<PNJDatosSpeeches>(datos.Length);
            Array.ForEach<PNJDatosSpeeches>(datos, p => pnjDatosSpeechesEncolados.Enqueue(p));
            ContinuarColaPNJSpeeches();


        }

     

        /// <summary>
        /// Se llama para continuar la cola de PNJ con Speeches
        /// </summary>
        private void ContinuarColaPNJSpeeches ()
        {
            //Si ya no quedan PNJ  con Speeches
            if(pnjDatosSpeechesEncolados == null)
                return;
            if (pnjDatosSpeechesEncolados.Count == 0)
            {
                //salir
            }
            else//Si quedabn
            {
                PNJDatosSpeeches datos = pnjDatosSpeechesEncolados.Dequeue();
                DesplegarTodos(datos.pnjDatos, datos.speeches);
            }


        }

        /// <summary>
        /// Ejecuta todoslos speeches dados en secuencia
        /// </summary>
        /// <param name="pnj"></param>
        /// <param name="speech"></param>
        public void DesplegarTodos(PNJDatos pnj, params Speech[] speeches)
        {
            if (speeches.Length == 0)
                return;
            /*Sw comentarea para poder soportar el encolado de varios speeches con sus respectivos PNJ. Sin esto el comportamiento es igual
            if (speeches.Length == 1)
            {
                DesplegarLinea(pnj, speeches[0]);
                return;
            }*/
            speechesEncolados = new Queue<Speech>();
            Array.ForEach<Speech>(speeches, s => speechesEncolados.Enqueue(s));
            pnjActual = pnj;
            this.OnCerrado += ContinuarColaSpeeches;
            ContinuarColaSpeeches();
        }
        /// <summary>
        /// Ejecuta todoslos speeches dados en secuencia. Al finalizarlos todos ejecuta el Callback dado
        /// </summary>
        /// <param name="pnj"></param>
        /// <param name="speech"></param>
        internal void DesplegarTodos(PNJDatos pnj, Speech[] speeches, Action callBack)
        {
            callBackTodos = callBack;
            DesplegarTodos(pnj, speeches);
        }

        private void ContinuarColaSpeeches()
        {
            if (speechesEncolados.Count > 0)
            {
                DesplegarLinea(pnjActual, speechesEncolados.Dequeue());
            }
            else
            {


                this.OnCerrado -= ContinuarColaSpeeches;
                if (callBackTodos != null)
                    callBackTodos();
                callBackTodos = null;
                ContinuarColaPNJSpeeches();
            }
        }

        public void DesplegarLinea(string pregunta, params string[] opciones)
        {
            // TODO implement here
            /* Implementación para crear y configurar el globo de diálogo del PJ */
            ConfigurarGloboDeDialogo(pregunta, opciones);
        }

        /// <summary>
        /// Configura los datos del globo de diálogo con opciones
        /// </summary>
        /// <param name="pregunta">Enunciado que va encima de los botones de opción</param>
        /// <param name="opciones">las sentencias de las opciones a colocar en cada botón de opción</param>
        private void ConfigurarGloboDeDialogo(string pregunta, string[] opciones)
        {
            GameObject globoDialogoInstanciado = CrearGloboDialogo(prefabGloboDialogoConOpciones);
            ConfigurarTransformada(globoDialogoInstanciado);

            controladorGlogoDialogo.EstablecerNombrePNJ();
            controladorGlogoDialogo.EstablecerLineaDialogo(pregunta, true);
            controladorGlogoDialogo.EstablecerImagenAvatar();
            controladorGlogoDialogo.EstablecerComportamientoSegunTipo(opciones);

            int i = 0;
            foreach (Button botonActual in (controladorGlogoDialogo as GloboDialogoConOpciones).BotonesDeOpcion)
            {
                int _i = i++;
                botonActual.onClick.AddListener(delegate { ClicOnOpcion(_i); });
            }
        }

        internal void DesplegarMensajeMamaTuleAlTsiunar(PNJDatos datos)
        {
            string t = datos.mensajeMamaTule;
            if (string.IsNullOrEmpty(datos.mensajeMamaTule))
                t = GestorPNJ.MENSAJE_MAMATULE_DEFAULT;
            DesplegarLinea(GestorPNJ.Instance.GetMamaTule(), new Speech(datos.name+": "+t));
        }

        /// <summary>
        /// Configura los datos del globo de diálogo sin opciones (línea de diálogo simple)
        /// </summary>
        /// <param name="pnj">Datos de PNJ</param>
        /// <param name="speech">Speech del PNJActor</param>
        private void ConfigurarGloboDeDialogo(PNJDatos pnj, Speech speech)
        {
            
            GameObject globoDialogoInstanciado = CrearGloboDialogo(pnj.id == "PJ" ? prefabGloboDialogoPJ : prefabGloboDialogo);
            ConfigurarTransformada(globoDialogoInstanciado);

            controladorGlogoDialogo.EstablecerNombrePNJ(pnj.nombre);
            controladorGlogoDialogo.EstablecerLineaDialogo(speech.texto, false);
            controladorGlogoDialogo.EstablecerImagenAvatar(pnj.avatar);
            controladorGlogoDialogo.EstablecerComportamientoSegunTipo();
        }

        /// <summary>
        /// Crea un globo de diálogo, lo asigna como hijo del este GameObject y obtiene el componente ControladorGloboDialogo
        /// </summary>
        /// <returns>El globo de diálogo instanciado</returns>
        /// <param name="original">El prefab a instanciar, puede ser el globo de diálogo con o sin opciones</param>
        private GameObject CrearGloboDialogo(GameObject original)
        {
            GameObject globoDialogoInstanciado = Instantiate(original);
            globoDialogoInstanciado.transform.SetParent(this.transform, false);
            controladorGlogoDialogo = globoDialogoInstanciado.GetComponent<ControladorGloboDialogo>();
            return globoDialogoInstanciado;
        }

        /// <summary>
        /// Configura aspectos del RectTransform del globo de diálogo para que se adecue al Canvas
        /// </summary>
        /// <param name="globoDialogoInstanciado">Globo dialogo instanciado.</param>
        private void ConfigurarTransformada(GameObject globoDialogoInstanciado)
        {
            RectTransform rectT = globoDialogoInstanciado.GetComponent<RectTransform>();
            rectT.offsetMin = Vector2.zero;
            rectT.offsetMax = Vector2.zero;
            rectT.localScale = Vector2.one;
        }

        public void CerrarDialogo()
        {
            if (OnCerrado != null)
                OnCerrado();
            if (callback != null)
            {
                callback();
                callback = null;
            }
            
        }

        public void ClicOnOpcion(int numOpcion)
        {
            // TODO implement here
            Debug.Log("numOpcion: " + numOpcion + ", del botón tocado");
            if (OnClicOpcion != null) OnClicOpcion(numOpcion);

        }

        public class PNJDatosSpeeches
        {
            public PNJDatos pnjDatos;
            public Speech[] speeches;

            public PNJDatosSpeeches(PNJDatos pnjDatos, params Speech[] speeches)
            {
                this.pnjDatos = pnjDatos;
                this.speeches = speeches;
            }
        }


    }
   

}