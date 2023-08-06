using System.Collections;
using System.Collections.Generic;
using Tsiunas.Places;
using UnityEngine;

namespace Tsiunas.Places

{
    public class Bakery : PlaceManager
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
        void Update()
        {

        }
        #endregion

        #region Métodos
        internal override void ConfigurePlace()
        {
            //Configurar a Juan
            PNJ_JUAN juan = GameObject.FindObjectOfType<PNJ_JUAN>();
            if (!PNJ_JUAN.juanSeHaMarchado)
            {
                Destroy(juan.gameObject);
            }
            else
            {
                juan.Actor.isMerchant = true;
                juan.ActivarSitCompra();
            }
        }

        #endregion
        #region CoRutinas


        #endregion
    }
}