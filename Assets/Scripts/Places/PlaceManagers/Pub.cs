using System.Collections;
using System.Collections.Generic;
using Tsiunas.Places;
using UnityEngine;

namespace Tsiunas.Places
{
    public class Pub : PlaceManager
    {


        #region Enums


        #endregion

        #region Atributos y Propiedades


        #endregion

        #region Eventos    


        #endregion

        #region Mensajes Unity
        private new void Start()
        {
            base.Start();
        }


        // Update is called once per frame
        void Update() {

        }
        #endregion

        #region Métodos
        internal override void ConfigurePlace()
        {
            if (PNJ_JUAN.juanSeHaMarchado)
            {
                Destroy(GameObject.FindObjectOfType<PNJ_JUAN>().gameObject);
            }

            if (PNJ_CHICA_BAR.chicaBarSeHaMarchado)
            {
                Destroy(FindObjectOfType<PNJ_CHICA_BAR>().gameObject);
            }


        }

        #endregion
        #region CoRutinas


        #endregion
    }

}