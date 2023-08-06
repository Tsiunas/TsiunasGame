
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tsiunas.SistemaDialogos;

namespace Tsiunas.SistemaDialogos
{
    public class LineaDialogoSimple : LineaDialogo
    {

        public LineaDialogoSimple() : base()
        {
            speeches = new List<Speech>();
        }

        public LineaDialogoSimple(string text) : this()
        {
            speeches.Add(new Speech(text));
        }
        public Action onTerminada;

        
        public List<Speech> speeches;
        private PNJDatos pnjDatos;

        IEnumerator<Speech> e;
        public override void Desplegar()
        {
            if (speeches == null || speeches.Count == 0)
                throw new TsiunasException("Linea de Dialogo sin Speeches");
            string pjActor = idPNJ;
            if(idPNJ != "PJ")
                if (!idPNJ.StartsWith("PNJ"))
                    pjActor = owner.GetIdPNJActual();               
            
            pnjDatos = GestorPNJ.Instance.ObtenerPNJ(pjActor);



            e = speeches.GetEnumerator();
            
            ReproducirSiguienteSpeech();
        }

        private void DesplegadorCerrado()
        {
            DesplegadorDialogo.Instance.OnCerrado -= DesplegadorCerrado;
            ReproducirSiguienteSpeech();
            
        }

        private void ReproducirSiguienteSpeech()
        {
            if (e.MoveNext())
            {
                Speech s = e.Current;
                DesplegadorDialogo.Instance.OnCerrado += DesplegadorCerrado;
                DesplegadorDialogo.Instance.DesplegarLinea(pnjDatos, s);

                PNJActor.AnimarHablar(pnjDatos.id);

            }
            else
            {
                PNJActor.AnimarIdle(pnjDatos.id);
                owner.ReproducirSiguienteLinea();

            }
        }

        internal string GetLastSpeechText()
        {
            if (speeches != null && speeches.Count > 0)
                return speeches.Last().texto;
            return null;
        }

        public void Terminar()
        {
            // TODO implement here

        }

    }
}