using System;
using System.Collections.Generic;
using System.Linq;
using Tsiunas.SistemaDialogos;
using UnityEngine;

namespace Tsiunas.SistemaDialogos
{
    public partial class GestorPNJ : Singleton<GestorPNJ>
    {
        public GameObject spTsiunasOriginal;
        public GameObject flamaArmoniaOriginal;

        /// <summary>
        /// Carga recursos disponibles a los PNJACtores del juego
        /// </summary>
        private void CargarRecursos()
        {
            spTsiunasOriginal = Resources.Load<GameObject>("ParticulasTsiunas");
            flamaArmoniaOriginal = Resources.Load<GameObject>("FlamaArmonia");
        }
    }
}
