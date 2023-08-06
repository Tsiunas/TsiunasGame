
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Tsiunas.SistemaDialogos
{
    [Serializable]

    public class Intervencion : IContenedorPadre<Intervencion, LineaDialogo>, IWithID
    {
        #region Atributos Y Propiedades
        public enum TiposIntervenciones {Normal, ActuarAlFinal}
        public string id;
        public List<LineaDialogo> lineas;
        [NonSerialized]
        private Situacion owner;
        public bool actuarAlFinalizar = false;
        public string actuacion;
        public bool encolaIntervencionAlTerminar;
        public string idIntervencionAEncolarAlTerminar;
        
        #endregion

        #region Constructores
        public Intervencion()
        {
            lineas = new List<LineaDialogo>();
        }
        #endregion

        #region Métodos

        IEnumerator<LineaDialogo> e;
        public void Iniciar()
        {
            if (lineas == null || lineas.Count == 0)
                throw new TsiunasException("Intervencion sin lineas");
            e = lineas.GetEnumerator();
            ReproducirSiguienteLinea();


        }

        internal string GetIdPNJActual()
        {
            return GetOwner().IdPNJOwner;
        }

        public void ReproducirSiguienteLinea()
        {
            string anteriorPregunta = "";
            if (e.Current != null && e.Current is LineaDialogoSimple)
            {
                LineaDialogoSimple lds = e.Current as LineaDialogoSimple;
                anteriorPregunta = lds.GetLastSpeechText();
            }

            if (e.MoveNext())
            {
                LineaDialogo ld = e.Current;
                ld.SetOwner(this);
                if(ld is LineaDialogoOpciones)
                {
                    (ld as LineaDialogoOpciones).pregunta = anteriorPregunta;
                }

                ld.Desplegar();
            }
            else
            {
                //Se acabaron las líneas
                //Se pone la situación a nulo
                owner.IntervencionTerminada();
            }
        }

        public void SetOwner(Situacion s)
        {
            this.owner = s;
        }

        public Situacion GetOwner()
        {
            return owner;
        }
        #endregion

        #region implementacipn IcontenedorPadre
        public string NombreContenedorPadre { get { return "Intervencion"; } }

        public string NombreContenedorPadrePlural { get { return "Intervenciones"; } }

        public string NombreHijos { get { return "Linea de Dialogo"; } }

        public string NombreHijosPlural { get { return "Lineas de Dialogo"; } }

        public string PrefijoHijos { get { return "LDD_"; } }




        public int GetCantidadHijos()
        {
            return lineas.Count;
        }

        public List<LineaDialogo> GetListaHijos()
        {
            return lineas;
        }

        public string IdHijo(int i)
        {
            return lineas[i].GetID();

        }
        #endregion

        #region implementación IWithID
        public void SetID(string id)
        {
            this.id = id;
        }

        public string GetID()
        {
            return id;
        }
        #endregion

    }
}