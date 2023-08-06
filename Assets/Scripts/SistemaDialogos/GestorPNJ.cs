
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tsiunas.SistemaDialogos
{
    public partial class GestorPNJ : Singleton<GestorPNJ>
    {
        /// <summary>
        /// Textos por defecto...
        /// </summary>
        public static readonly string TEXTO_DESDEN = "... (El personaje no quiere hablar, muestra enfado contigo) ...";
        public static readonly string TEXTO_DISGUSTO = "Ah, es usted nuevo... Qué fastidio la gente como usted";
        public static readonly string TEXTO_EMPATICO = "¡Hola de nuevo! ¡Me alegra ver gente con su forma de actuar!";
        public static readonly string TEXTO_AMISTOSO = "¡Qué bueno volver a verte!. !Mantengamos nuestra amistad!";


        public List<PNJDatos> pnjs;
        public PNJDatos pjPPal;
        private List<PNJActor.PNJActorDatos> pnjActoresDatos;

        public List<PNJActor.PNJActorDatos> PnjActoresDatos
        {
            get
            {
                return pnjActoresDatos;
            }

            set
            {
                pnjActoresDatos = value;
            }
        }

        public GestorPNJ()
        {
            uniqueToAllApp = true;
            pnjActoresDatos = new List<PNJActor.PNJActorDatos>();
        }

        private void Awake()
        {
            PersistenceManager.Instance.PerformProfileDataLoading((ProfileData pD) => {
                if (pD.profile_PNJActores != null)
                    PnjActoresDatos = pD.profile_PNJActores;
            });

            CargarPNJs();
            CargarRecursos();
        }

        private void CargarPNJs()
        {
            pnjs = new List<PNJDatos>(Resources.LoadAll<PNJDatos>(""));
            pjPPal = Resources.Load<PNJDatos>("PJ");
        }

        public PNJDatos ObtenerPNJ(string id)
        {
            
            if(string.Compare(id,"PJ") == 0)
            {
                return pjPPal;
            }
            else
            {
                try
                {
                    PNJDatos pnj = pnjs.Find(p => p.id == id);
                    if (pnj == null)
                        throw new TsiunasException("No se encontró el personaje" + id);
                    return pnj;
                }
                catch (TsiunasException e)
                {
                    e.Tratar();                    
                }
            }
            return null;
        }

        public void GuardarDatosPNJActor(PNJActor.PNJActorDatos datos)
        {
            if (pnjActoresDatos.Contains(datos))
            {
                pnjActoresDatos[pnjActoresDatos.FindIndex(d => d.id == datos.id)] = datos;                
            }
            else
            {
                pnjActoresDatos.Add(datos);
            }
        }

        public void GuardarDatosPNJActor(PNJActor datos)
        {
            this.GuardarDatosPNJActor(datos.GetDatos());
        }

        public bool YaContiene(string id)
        {
            return pnjActoresDatos.Any(d => d.id == id);
        }

        

        public PNJActor.PNJActorDatos CargarDatosPNJActor(string id)
        {
            PNJActor.PNJActorDatos rta = pnjActoresDatos.Find(d => d.id == id);
            if (rta == null)
                throw new TsiunasException("Se intentó cargar un PJActor que no estaba guardado. ID intentado:"+ id, modulo: "GestorPNJ", responsable: "Hendrys");
            else
                return rta;

        }


        public PNJActor EncontrarActor(string idPNJ)
        {
            PNJActor[] actores = GameObject.FindObjectsOfType<PNJActor>();
            PNJActor actor = Array.Find<PNJActor>(actores, p => p.id == idPNJ);
            return actor;
        }

        public PNJAnonimo EncontrarAnonimo(string idPNJ)
        {
            PNJAnonimo[] anonimos = GameObject.FindObjectsOfType<PNJAnonimo>();
            PNJAnonimo anonimo = Array.Find<PNJAnonimo>(anonimos, p => p.id == idPNJ);
            return anonimo;
        }


       


    } 

    
}