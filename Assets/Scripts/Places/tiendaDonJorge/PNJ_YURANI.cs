using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tsiunas.Places
{
    public class PNJ_YURANI : PNJ_ACTUACION
    {
        public override void Actuar(string arg1, object arg2)
        {
            StoreManager.ObtainTool(TypesGameElement.Tools.Axe);
            Actor.NivelAmistad = NivelesAmistad.AMISTAD_ENTABLADA;
        }

        
        #region Enums


        #endregion

        #region Atributos y Propiedades


        #endregion

        #region Eventos    


        #endregion

        #region Mensajes Unity

        // Update is called once per frame
        void Update()
        {

        }
        #endregion

        #region Métodos


        #endregion
        #region CoRutinas


        #endregion
    }
}