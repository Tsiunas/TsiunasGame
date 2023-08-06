using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tsiunas.Mechanics
{
    
    public class Plant : MonoBehaviour
    {
        #region Enums
        //Tipos posibles de plantas, esto también se usa para tipos de semillas
        public enum PlantTypes { Tsiuna, Maiz };
        //Estados posibles de la planta
        public enum PlantStates { Retoño, Pequeña, UnFruto, DosFrutos, Madura, Seca, Marchita };
        #endregion
        #region Atributos y Propiedades

        public PlantTypes tipo;
        public PlantTypes Tipo
        {
            get
            {
                return tipo;
            }

            set
            {
                tipo = value;
                //Si el objeto no está agregado auna escena, entonces no actualiza los gráficos
                if(this.gameObject.scene.name != null)
                    UpdateGraphics();
                UpdateDatos();
                if (onPlantChanged != null)
                    onPlantChanged(this);
            }
        }

        public PlantStates estado;
        public PlantStates Estado
        {
            get
            {
                return estado;
            }

            set
            {
                bool esNuevoEstado = estado != value;
                estado = value;
                //Si el objeto no está agregado auna escena, entonces no actualiza los gráficos
                if (this.gameObject.scene.name != null && esNuevoEstado)
                    UpdateGraphics();
                
                if (onPlantChanged != null)
                    onPlantChanged(this);
                if (Estado < PlantStates.UnFruto || Estado == PlantStates.Marchita || Estado == PlantStates.Seca)
                {
                    ActivarColisionador(false);
                }
                else
                {
                    ActivarColisionador(true);
                }

            }
        }

        private void ActivarColisionador(bool activado)
        {
            Collider c = GetComponent<Collider>();
            if(c != null)
                c.enabled = activado;
        }

        public PlantaDatos datos;
        /// <summary>
        /// El día (tiempo) en el que fue sembrada.
        /// </summary>
        #pragma warning disable 414
        float timestampSowed;
        #pragma warning restore 414

        public int nivelCrecimiento = 0;
        public int NivelCrecimiento
        {
            get
            {
                return nivelCrecimiento;
            }

            set
            {
                if (value < 0)
                    nivelCrecimiento = 0;
                else
                    nivelCrecimiento = value;
                
                Estado = CalcularEstado(nivelCrecimiento);            
            }
        }

        
        public bool regada;

        /// <summary>
        /// Establece si esta planta debe o no crecer (por cualquier razón)
        /// </summary>
        public bool crecer = true;

        /// <summary>
        /// La velocidad de crecimiento en puntos de crecimiento por día
        /// </summary>
        public float VCrecimiento
        {
            get
            {
                return PlantManager.Instance.CalcularVCrecimiento(this.Tipo, regada);
            }
        }

        internal void SetFrom(PlantToSave plantaARecuperar)
        {
            Tipo = plantaARecuperar.tipo;
            NivelCrecimiento = plantaARecuperar.nivelCrecimiento;
            TimestampSowed = plantaARecuperar.timeStampSowed;
            regada = plantaARecuperar.regada;
            crecer = plantaARecuperar.debeCrecer;
            NivelMarchitacion = plantaARecuperar.nivelMarchitacion;
            tiempoCuandoCrecio = plantaARecuperar.tiempoCuandoCrecio;
            JustSowed = false;
        }

        /// <summary>
        /// La velocidad de crecimiento en puntos de crecimiento por segundo
        /// </summary>
        public float VCrecimientoSeg
        {
            get
            {
                //Se multiplica por 60 el tiempo de maduración porque hay 60 segundos en un minuto. Se divide el 60 por los "dias por minuto" porque entre más dias por minuto haya menor es el día en segundos. P.e. Si hay 4 días en un minuto entonces 
                //un día pasa en 15 segundos (60/4)
                if (Math.Round((decimal)datos.tiempoMaduración, 2) != 0)
                    return PlantManager.CRECIMIENTO_MADURACION / (datos.tiempoMaduración * (60 / TimeManager.DAYS_PER_MINUTE));
                else
                {
                    throw new FieldAccessException("El campo de tiempo Maduración es 0. La velocidad no existe");
                }
            }
        }

        

        /// <summary>
        /// El tiempo que tarda la planta en subir un punto de crecimiento (en segundos) según la velocidad en segundos(VCrecimientoSeg)
        /// Esto se usa para la corutina que actualiza el crecimiento de la planta
        /// </summary>
        public float secondsToGrow
        {
            get
            {
                return 1 / VCrecimientoSeg;
            }
        }
        /// <summary>
        /// El tiempo que tarda la planta en subir un punto de crecimiento (en días) según la velocidad en sías(VCrecimiento)
        /// Esto se usa para la corutina que actualiza el crecimiento de la planta
        /// </summary>
        public float daysToGrow
        {
            get
            {
                return 1f / VCrecimiento;
            }
        }
        //Constantes



        /// <summary>
        /// El estadio es el nivel de la planta según su estado
        /// </summary>
        public int Estadio
        {
            get
            {
                return (int)estado + 1;
            }

        }

        public float TimestampSowed
        {
            get
            {
                return timestampSowed;
            }

            set
            {
                timestampSowed = value;
            }
        }

        public bool JustSowed { get; set; }

        

        public Transform trPadreDePlanta;
        public bool seEstaMarchitando;
        public bool SeEstaMarchitando
        {
            get
            {
                return seEstaMarchitando;
            }

            set
            {
                seEstaMarchitando = value;
                MarchitarGraphics(seEstaMarchitando);
            }
        }

        

        private int nivelMarchitacion = 0;
        public int NivelMarchitacion
        {
            get
            {
                return nivelMarchitacion;
            }

            set
            {
                nivelMarchitacion =  value < 0 ? 0 : value;
            }
        }
        private bool creceRapido;
        public bool CreceRapido
        {
            set
            {
                creceRapido = value;
                CrecimientoGraphics(creceRapido);
            }
            get
            {
                return creceRapido;
            }
        }

      


        /// <summary>
        /// El sistema de particulas que se activa cuando la planta está crecienco con beneficio
        /// </summary>
        public GameObject bloomingPS;

        #endregion
        #region Eventos

        //Evento que se lanza cuando la planta cambia su estado o tipo
        public Action<Plant> onPlantChanged;
        #endregion
        #region Mensajes Unity
        // Use this for initialization
        void Start()
        {
            UpdateDatos();            
            UpdateGraphics();
            if(this.JustSowed)
                SetCrecimiento();            
            StartCoroutine(ActualizarCrecimiento());
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        /// <summary>
        /// Cuando se da clic en la planta se puede cosechar
        /// </summary>
        private void OnMouseDown()
        {
            Debug.Log("Clic en Planta");
            ToolTarget tt = GetComponentInParent<ToolTarget>();
            if(tt != null)
            {
                tt.UseTool();
            }

        }




        public void OnValidate()
        {
            //UpdateDatos();
            if (Application.isPlaying && DebugGlobals.canDebug)
            {
                Estado = estado;
                Tipo = tipo;
                SetCrecimiento();
            }
            //SetCrecimiento();
        }
        

        /// <summary>
        /// Actualiza el nivel del crecimiento den acuerdo al estado de esta planta
        /// </summary>
        private void SetCrecimiento()
        {
            //Lista de valores que son el nivel más bajo para cada Estadio
            
            //Se obtiene el valor de la lista según el estadio (menos 1 por que es un array, mientras que el Estado parte en 1)
            NivelCrecimiento = PlantManager.NIVELES_SEGUN_ESTADIO[Estadio - 1];
        }

        private void UpdateDatos()
        {
            datos = PlantManager.Instance.GetDatos(this.tipo);
        }

        /// <summary>
        /// Sirve para comprobar estado de juego y dar cantidad de cosecha adecuada
        /// si el estado de juego es InIntro va a dar 2 frutos y 5 semillas
        /// </summary>
        /// <returns>The cosecha.</returns>
        public Cosecha DarCosecha() {
            if (GameManager.Instance.GetGameState == GameState.InIntro)
                return Cosechar(2, 5);
            else
                return Cosechar();
        }

        #endregion
        #region Métodos
        /// <summary>
        /// Devuelde un objeto Frutos, que contiene los frutos a dar depende del estado actual de la planta y su tipo (el tipo del fruto)
        /// </summary>
        /// <returns>Estructura que contiene la cantidad de frutos a dar y su tipo.</returns>
        public Cosecha Cosechar(params int[] cantidadCosecha) {
            Cosecha frutosADar;
            TypesGameElement.Fruits tipoFruto = this.Tipo == PlantTypes.Maiz ? TypesGameElement.Fruits.Corn : TypesGameElement.Fruits.Tsiuna;
            TypesGameElement.Seeds tipoSemilla = this.Tipo == PlantTypes.Maiz ? TypesGameElement.Seeds.Corn : TypesGameElement.Seeds.Tsiuna;

            if (cantidadCosecha.Length == 0)
            {
                frutosADar = new Cosecha(0, 0, TypesGameElement.Fruits.Corn, TypesGameElement.Seeds.Corn);
               
                switch (this.Estado)
                {
                    case PlantStates.UnFruto:
                        frutosADar = new Cosecha(1, 1, tipoFruto, tipoSemilla);
                        break;
                    case PlantStates.DosFrutos:
                        frutosADar = new Cosecha(1, 1, tipoFruto, tipoSemilla);
                        break;
                    case PlantStates.Madura:
                        int n = UnityEngine.Random.Range(1, datos.N + 1) + 2;
                        int s = UnityEngine.Random.Range(1, datos.N + 1) + 2;
                        frutosADar = new Cosecha((int)tipoFruto == 1 ? 2 : n, (int)tipoSemilla == 1 ? 1 : s, tipoFruto, tipoSemilla);
                        break;
                }
                return frutosADar;
            }
            else {
                frutosADar = new Cosecha(cantidadCosecha[0], cantidadCosecha[1], tipoFruto, tipoSemilla);
                return frutosADar;
            }
        }

        /// <summary>
        /// Actualizad el sprite de este objeto dependiendo de su tipo y estado
        /// </summary>
        
        private void UpdateGraphics()
        {
            
            GameObject prefabInstanciar = PlantManager.Instance.GetGraphics(this);
            //trPadreDePlanta = this.transform.GetChild();                          
            

            if (trPadreDePlanta != null && prefabInstanciar != null)
            {
                //Hay que eliminar todos los hijos del objeto
                
                foreach (Transform t in trPadreDePlanta)
                {
                    t.gameObject.SetActive(false);
                    Destroy(t.gameObject);
                }
                GameObject go = Instantiate<GameObject>(prefabInstanciar, trPadreDePlanta);
            }
            
            MarchitarGraphics(SeEstaMarchitando);
            CrecimientoGraphics(CreceRapido);

            
        }

        private void MarchitarGraphics(bool seEstaMarchitando)
        {
            //lo siguiente pone una animación en el material del objeto Mesh
            //TRUCO: Se busca un objeto con SkinnedMeshRenderer
            SkinnedMeshRenderer[] smr = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
            if (smr.Length == 0) throw new TsiunasException("Intentando marchitar planta. No se ha encontrado Skinned Meshrenderer", true, "PLANTA", "Hendrys");
            SkinnedMeshRenderer theSmr = null;
            Animator a = null;
            //Se busca el que tenga el tag "mesh"
            foreach (SkinnedMeshRenderer item in smr)
            {
                if (!(item.transform.parent.gameObject == null) && item.tag == "mesh")
                {
                    theSmr = item;
                    break;
                }
            }
            if (theSmr == null) throw new TsiunasException("Intentando cargar SkinnedMeshRenderer pero no se encontró objeto con el tag 'mesh'. El objeto debe ser marcado con este tag", true, "Plantas", "Hendrys");
            a = theSmr.GetComponent<Animator>();

            if (a != null)
            {
                a.enabled = true;
                if (seEstaMarchitando)
                {
                    //a.enabled = true;
                    a.Play("Marchitar");
                }
                else
                {
                    a.Play("Idle");
                    //a.enabled = false;                    
                }
            }
            else
            {
                throw new TsiunasException("No se encontró el animador de la planta para marchitarla", true, "Plantas", "Hendrys");
            }



        }

        private void CrecimientoGraphics(bool creceRapido)
        {
            bloomingPS.SetActive(creceRapido);
        }

        public void EstablecerEstado(PlantStates value)
        {
            estado = value;
            //Si el objeto no está agregado auna escena, entonces no actualiza los gráficos
            if (this.gameObject.scene.name != null)
                UpdateGraphics();
            SetCrecimiento();
        }
        #endregion
        #region CoRutinas
        //La última vez que creció  (actualizó su nivel de crecimiento) la planta en días
        float tiempoCuandoCrecio;
        public static readonly int NIVEL_PARA_MARCHITAR = 100;

        public IEnumerator ActualizarCrecimiento()
        {
            if(JustSowed)
                tiempoCuandoCrecio = TimestampSowed;
            while (true)
            {
                
                yield return new WaitUntil(TiempoEnDiasParaCrecerPasado);
                PlantStates estadoTmp = Estado;
                int nivelMarchitacionTmp = NivelMarchitacion;
                int nivelCrecimientoTmp = NivelCrecimiento;

                PlantManager.Instance.ActualizarCrecimiento(crecer, ref estadoTmp, SeEstaMarchitando, ref nivelMarchitacionTmp, ref nivelCrecimientoTmp, ref tiempoCuandoCrecio);
                if (estadoTmp != Estado)
                    Estado = estadoTmp;
                if(nivelMarchitacionTmp != NivelMarchitacion)
                    NivelMarchitacion = nivelMarchitacionTmp;
                if (nivelCrecimientoTmp != NivelCrecimiento)
                    NivelCrecimiento = nivelCrecimientoTmp;

               
            }
        }
        private bool TiempoEnDiasParaCrecerPasado()
        {
            return PlantManager.Instance.YaDebeCrecer(tiempoCuandoCrecio, Tipo, CreceRapido, regada);
                
        }


        private PlantStates CalcularEstado(int nivelCrecimiento)
        {

            return PlantManager.Instance.CalcularEstado(nivelCrecimiento);
        }
        #endregion
        #region Clase Para guardar
        /// <summary>
        /// Clase para guardar datos de plantas.
        /// Nótese que no se necesitan todos los campos. P.e. El campo Estado se puede calcular a partir del campo nivelCrecimiento
        /// </summary>
        [Serializable]
        public class PlantToSave
        {
            public float timeStampSowed;
            public int nivelCrecimiento;
            public Plant.PlantTypes tipo;
            public bool regada;
            public bool debeCrecer;
            public int nivelMarchitacion;
            public float tiempoCuandoCrecio;

            public PlantToSave(Plant original)
            {
                this.timeStampSowed = original.TimestampSowed;
                this.nivelCrecimiento = original.nivelCrecimiento;
                this.tipo = original.tipo;
                this.regada = original.regada;
                this.debeCrecer = original.crecer;
                this.nivelMarchitacion = original.NivelMarchitacion;
                this.tiempoCuandoCrecio = original.tiempoCuandoCrecio;
            }

           
            
        }
        #endregion

    }
    
}
