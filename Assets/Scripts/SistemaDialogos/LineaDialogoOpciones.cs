
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tsiunas.SistemaDialogos
{
    public class LineaDialogoOpciones : LineaDialogo
    {

        public LineaDialogoOpciones()
        {
            opciones = new List<Opcion>();
        }

        public List<Opcion> opciones;
        public string pregunta;

        public override void Desplegar()
        {
            DesplegadorDialogo.Instance.OnClicOpcion += EjecutarOpcion;
            DesplegadorDialogo.Instance.OnCerrado += DesplegadorCerrado;
            //Obtien las sentencias de la lista de opciones
            string[] sentencias = opciones.Select<Opcion, string>(o => o.sentencia).ToArray<string>();
            DesplegadorDialogo.Instance.DesplegarLinea(pregunta, sentencias);
        }

        private void DesplegadorCerrado()
        {
            DesplegadorDialogo.Instance.OnCerrado -= DesplegadorCerrado;
            owner.ReproducirSiguienteLinea();
        }

        private void EjecutarOpcion(int o)
        {
            DesplegadorDialogo.Instance.OnClicOpcion -= EjecutarOpcion;
            opciones[o].Ejecutar();
            
        }
    }
}