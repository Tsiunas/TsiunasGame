using System;
using UnityEngine;
using Tsiunas.Mechanics;

namespace Tsiunas.Mechanics
{
    public enum FAStates {Beneficio = 4, Regular = 3, Mal = 2, Muriendo = 1, Muerte = 0 }
    public class PlantManager : Singleton<PlantManager>
    {


        #region Inspector
        public GameObject plantPrefab;
        #endregion
        #region Atributos
        /// <summary>
        /// Almacena los datos de las plantas según tipo
        /// </summary>
        private PlantaDatos[] plantasDatos = new PlantaDatos[]
                            {
                                new PlantaDatos(5,3.5f,Plant.PlantTypes.Tsiuna,Color.magenta),
                                new PlantaDatos(7,2,Plant.PlantTypes.Maiz,Color.yellow)
                            };
        public static readonly int[] NIVELES_SEGUN_ESTADIO = {0, 11, 36, 61, 100, 150 };

        public static float CRECIMIENTO_MADURACION = 100;
        public IFAObserved fAObserved;
        private FAStates estadoFA;
        #endregion
        #region Constructores
        protected PlantManager()
        {
            uniqueToAllApp = true;
        } 
        #endregion
        #region Mensajes Unity
        // Use this for initialization
        void Awake()
        {
            plantPrefab = Resources.Load<GameObject>("SproutSeed");
            fAObserved = FindObjectOfType<TestPlanta>();
            if(fAObserved == null)
                fAObserved = HarmonyFlamesManager.Instance;
            if(fAObserved != null)  fAObserved.OnFlamasChanged += CambiarEstadoPlantas;
        }

        private new void Start()
        {
            
            base.Start();
        }

        /// <summary>
        /// cambiar el estado de las plantas dependiendo del estado dado
        /// Esto se hace cuando cambian las flamas para afectas a las plantas
        /// </summary>
        /// <param name="obj"></param>
        private void CambiarEstadoPlantas(FAStates obj)
        {
            estadoFA = obj;
                                

            Plant[] plantas = GameObject.FindObjectsOfType<Plant>();
            bool marchitar = false;
            bool beneficiar = false;
            if (obj == FAStates.Mal || obj == FAStates.Muriendo || obj == FAStates.Muerte)
                marchitar = true;
            if (obj == FAStates.Beneficio)
                beneficiar = true;

            Array.ForEach<Plant>(plantas, p => p.SeEstaMarchitando = marchitar);
            Array.ForEach<Plant>(plantas, p => p.CreceRapido = beneficiar);




        }

        // Update is called once per frame
        void Update()
        {
            //Lo siguiente es el Sistema para que las plantas crezcan por fuera de la granja
            //Si se está por fuera de la granja y se está en estado Ingame
            if (PersistentFarm.PlantasActualmenteEnMemoria && GameManager.Instance.GetGameState == GameState.InGame)
            {
                //Tomar todos los PlantToSave de PersistenFarm
                PersistentFarm.plantasGuardadas.ForEach(p =>
                {
                    if(YaDebeCrecer(p.tiempoCuandoCrecio, p.tipo, HarmonyFlamesManager.Instance.CreceRapido, p.regada))
                    {
                        //Y actualizar su estado de crecimiento
                        ActualizarCrecimiento(p);
                    }
                }
                );
            }
            
            
        }

        private void ActualizarCrecimiento(Plant.PlantToSave p)
        {
            Plant.PlantStates estado = CalcularEstado(p.nivelCrecimiento);
            bool seEstaMarchitando = HarmonyFlamesManager.Instance.MalEstado;
            ActualizarCrecimiento(p.debeCrecer, ref estado, HarmonyFlamesManager.Instance.MalEstado, ref p.nivelMarchitacion, ref p.nivelCrecimiento, ref p.tiempoCuandoCrecio);
        }

        internal float CalcularVCrecimiento(Plant.PlantTypes tipo, bool regada)
        {
            PlantaDatos datos = GetDatos(tipo);
            if (Math.Round((decimal)datos.tiempoMaduración, 2) != 0)
            {
                float vCrecimientoActual = PlantManager.CRECIMIENTO_MADURACION / datos.tiempoMaduración;
                return (float)(vCrecimientoActual * (regada ? 1.1 : 1));
            }
            else
            {
                throw new FieldAccessException("El campo de tiempo Maduración es 0. La velocidad no existe");
            }
        }

        /// <summary>
        /// Planta una semilla del tipo <paramref name="tipoPlantaASembrar"/>. La planta resulta siendo hija de <paramref name="parent"/>
        /// </summary>
        /// <param name="tipoPlantaASembrar">El tipo de planta a sembrar</param>
        /// <param name="parent">El padre del retoño</param> 
        #endregion
        #region Métodos


        public void SowSeed(Plant.PlantTypes tipoPlantaASembrar, GameObject parent)
        {
            
            GameObject plantaInstaciada = Instantiate(plantPrefab, parent.transform, false);
            Plant planta = plantaInstaciada.GetComponent<Plant>();
            if (planta != null)
            {
                planta.Tipo = tipoPlantaASembrar;
                planta.Estado = Plant.PlantStates.Retoño;
                planta.nivelCrecimiento = 0;
                planta.TimestampSowed = TimeManager.Instance.Dia;
                planta.JustSowed = true;
                SetPlantaBeneficioMarchitacion(planta);
            }
        }

        /// <summary>
        /// Planta una planta de acuerdo a los datos dados. Esto se usa cuando se carga la escena guardada
        /// </summary>
        /// <param name="planaARecuperar"></param>
        /// <param name="parent"></param>
        public void LoadSowFrom(Plant.PlantToSave plantaARecuperar, GameObject parent)
        {
            
            GameObject plantaInstanciada = Instantiate(plantPrefab, parent.transform, false);
            Plant planta = plantaInstanciada.GetComponent<Plant>();
            if (planta != null)
            {
                planta.SetFrom(plantaARecuperar);
                SetPlantaBeneficioMarchitacion(planta);
            }
            //TODO: configurar planta sumando el crecimiento que ha pasado usando el tiempo  desde que fue guardada.

        }

        private void SetPlantaBeneficioMarchitacion(Plant planta)
        {
            if (HarmonyFlamesManager.Instance.CurrentFAState == FAStates.Beneficio)
                planta.CreceRapido = true;
            if (HarmonyFlamesManager.Instance.CurrentFAState == FAStates.Mal || HarmonyFlamesManager.Instance.CurrentFAState == FAStates.Muriendo)
                planta.SeEstaMarchitando = true;
        }

        /// <summary>
        /// retorna un prefab adecuado para la planta dada, según si tipo y estado
        /// </summary>
        /// <param name="plant"></param>
        /// <returns>El prefab de la planta</returns>
        internal GameObject GetGraphics(Plant plant)
        {
            //El objeto a retornar
            GameObject rta;
            //Se cargan todos los sprites de la spritesheet en este array
            GameObject[] all = Resources.LoadAll<GameObject>(plant.Tipo.ToString());

            //El nombre del sprite buscado se arma así:
            //TipoPlanta+Estadio+Estado
            //El Estadio es un número que representa el nivel de la planta



            string nombreABuscar = plant.Estadio.ToString() + plant.Estado.ToString();            

            //Esta instrucción lambda dentro del método Find indica que se busque un objeto Sprite con este nombre (propiedad name)
            //Y se almacena en rta

            rta = Array.Find<GameObject>(all, (element => element.name.ToUpper().CompareTo(nombreABuscar.ToUpper()) == 0));
            //Si no se encuentra el gráfico prefab
            if (rta == null)
                throw new TsiunasException("Prefab de planta no encontrado Asegurese de que específico bien los datos de la planta. Planta=" + nombreABuscar,true, "PLANTAS", "Hendrys");
            
            
            //se retorna la rta
            return rta;
        }
        internal PlantaDatos GetDatos(Plant.PlantTypes tipo)
        {
            return Array.Find<PlantaDatos>(plantasDatos, p => p.tipo == tipo);
        }

        internal bool YaDebeCrecer(float tiempoCuandoCrecio, Plant.PlantTypes tipo, bool creceRapido, bool regada)
        {
            return TimeManager.Instance.Dia - tiempoCuandoCrecio > daysToGrow(tipo, regada) * (creceRapido ? 0.8 : 1);
        }

        private float daysToGrow(Plant.PlantTypes tipo, bool regada)
        {
            return 1f / CalcularVCrecimiento(tipo, regada);
        }

        internal Plant.PlantStates CalcularEstado(int nivelCrecimiento)
        {
            int estadioRta = 0;
            //caso especial para la planta seca
            if (nivelCrecimiento >= PlantManager.NIVELES_SEGUN_ESTADIO[(int)Plant.PlantStates.Seca])
            {
                nivelCrecimiento = PlantManager.NIVELES_SEGUN_ESTADIO[(int)Plant.PlantStates.Seca];
                return Plant.PlantStates.Seca;
            }
            for (int i = 0; i < PlantManager.NIVELES_SEGUN_ESTADIO.Length - 1; i++)
            {
                if (PlantManager.NIVELES_SEGUN_ESTADIO[i] <= nivelCrecimiento && nivelCrecimiento < PlantManager.NIVELES_SEGUN_ESTADIO[i + 1])
                {
                    estadioRta = i;
                    break;
                }
            }
            return (Plant.PlantStates)estadioRta;
        }

        internal void ActualizarCrecimiento(bool crecer, ref Plant.PlantStates estado, bool seEstaMarchitando, ref int nivelMarchitacion, ref int nivelCrecimiento, ref float tiempoCuandoCrecio)
        {
            //Si la variable crecer es false entonces no se hace nada
            if (!crecer)
                return;
            //Se crece solo si no está en estado marchita o seca
            if (estado != Plant.PlantStates.Marchita && estado != Plant.PlantStates.Seca)
            {
                if (!seEstaMarchitando)
                {
                    if (nivelMarchitacion > 0)
                        nivelMarchitacion--;
                    else
                        nivelCrecimiento++;

                }
            }
            if (seEstaMarchitando)
            {
                nivelMarchitacion++;
                if (nivelMarchitacion >= Plant.NIVEL_PARA_MARCHITAR)
                {
                    seEstaMarchitando = false;
                    estado = Plant.PlantStates.Marchita;
                }
            }
            tiempoCuandoCrecio = TimeManager.Instance.Dia;
        }
        #endregion
    }

    /// <summary>
    /// Estructura que guarda los datos de un tipo de planta
    /// </summary>
    public struct PlantaDatos
    {
        public int N;
        public float tiempoMaduración;
        public Plant.PlantTypes tipo;
        public Color color;

        public PlantaDatos(int n, float tiempoMaduración, Plant.PlantTypes tipo, Color color)
        {
            N = n;
            this.tiempoMaduración = tiempoMaduración;
            this.tipo = tipo;
            this.color = color;
        }
    }

    /// <summary>
    /// Estructura para los frutos dados al cosechar la planta
    /// </summary>
    public struct Cosecha {
        public int cantidadFruto;
        public int cantidadSemilla;
        public TypesGameElement.Fruits tipoDelFruto;
        public TypesGameElement.Seeds tipoDeSemilla;

        public Cosecha(int cantidadFruto, int cantidadSemilla, TypesGameElement.Fruits tipoDelFruto, TypesGameElement.Seeds tipoDeSemilla)
        {
            this.cantidadFruto = cantidadFruto;
            this.cantidadSemilla = cantidadSemilla;
            this.tipoDelFruto = tipoDelFruto;
            this.tipoDeSemilla = tipoDeSemilla;
        }

       
    }

    public interface IFAObserved
    {
        event Action<FAStates> OnFlamasChanged;
    }

    


}


