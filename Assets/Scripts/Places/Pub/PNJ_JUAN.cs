using System;
using System.Collections;
using System.Collections.Generic;
using Tsiunas.SistemaDialogos;
using UnityEngine;

namespace Tsiunas.Places
{

   

    [RequireComponent(typeof(PNJActor))]
    [RequireComponent(typeof(Animator))]
    public class PNJ_JUAN : PNJ_ACTUACION
    {

        #region Enums


        #endregion

        #region Atributos y Propiedades
        public static bool juanSeHaMarchado = false;

        #endregion

        #region Eventos    


        #endregion

        #region Mensajes Unity    
        
	
	    // Update is called once per frame
	    void Update () {
		
	    }
        #endregion

        #region Métodos


        #endregion
        #region CoRutinas


        #endregion

        public override void Actuar(string mensaje, object origen)
        {
            Animator a = GetComponent<Animator>();
            a.enabled = true;
            a.Play("Irse");

            PNJActor.AnimarCaminar(Actor.id);
            PNJActor anibal = GestorPNJ.Instance.EncontrarActor("PNJ_ANIBAL");
            if(anibal != null)
            {
                anibal.EliminarSituacionActiva();
            }


            juanSeHaMarchado = true;
        }

        internal void ActivarSitCompra()
        {
            Actor.EstablecerSituacionActiva("SIT_COMPRA");
        }

       

    }


    [RequireComponent(typeof(PNJActor))]
    public abstract class PNJ_ACTUACION : MonoBehaviour, IActor
    {
        protected void Start()
        {         
            this.GetComponent<PNJActor>().OnActuar += Actuar;
        }

        public abstract void Actuar(string mensaje, object datos);

        protected internal PNJActor Actor
        {
            get
            {
                return GetComponent<PNJActor>();
            }
        }
    }
}
