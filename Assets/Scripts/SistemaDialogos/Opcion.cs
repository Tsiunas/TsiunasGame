
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Tsiunas.SistemaDialogos
{
    [Serializable]
    public class Opcion : IContenedorPadre<Opcion, Consecuencia>
    {

        private LineaDialogoOpciones owner;
        public Opcion(LineaDialogoOpciones owner)
        {
            consecuencias = new List<Consecuencia>();
            this.owner = owner;

        }

        public string sentencia;

        public List<Consecuencia> consecuencias;

        string IContenedorPadre<Opcion, Consecuencia>.NombreContenedorPadre { get { return "Opcion"; } }

        string IContenedorPadre<Opcion, Consecuencia>.NombreContenedorPadrePlural { get { return "Opciones"; } }

        string IContenedorPadre<Opcion, Consecuencia>.NombreHijos { get { return "Consecuencia"; } }

        string IContenedorPadre<Opcion, Consecuencia>.NombreHijosPlural { get { return "Consecuencias"; } }

        string IContenedorPadre<Opcion, Consecuencia>.PrefijoHijos { get { return "CON_"; } }

        public void Ejecutar()
        {
            foreach (Consecuencia c in consecuencias)
            {
                c.Ejecutar();
            }

        }

        public int GetCantidadHijos()
        {
            return consecuencias != null ? consecuencias.Count : 0;

        }

        List<Consecuencia> IContenedorPadre<Opcion, Consecuencia>.GetListaHijos()
        {
            return consecuencias;
        }

        string IContenedorPadre<Opcion, Consecuencia>.IdHijo(int i)
        {
            return consecuencias != null && i < consecuencias.Count ? consecuencias[i].GetID() : "NOID";
        }
    }
}