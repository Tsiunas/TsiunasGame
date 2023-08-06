
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Tsiunas.SistemaDialogos
{
    
    
    public abstract class PNJ : MonoBehaviour
    {
        public string id;
        [SerializeField]
        public PNJDatos datosPNJ;
        public bool interactuable = true;
        public bool isMerchant;

          
        public void Awake()
        {
            if (datosPNJ == null)
            {
                if(!string.IsNullOrEmpty(id))
                    this.datosPNJ = GestorPNJ.Instance.ObtenerPNJ(id);
                if (this.datosPNJ == null)
                    Debug.LogError("Error cargando el PNJ con id"+id);
            }
            else
            {
                id = datosPNJ.id;
            }
            gameObject.name = id;
        }

        // Se llama a OnMouseDown cuando el usuario presiona el botón del mouse sobre el elemento GUI o colisionador
        private void OnMouseUpAsButton()
        {
            if(interactuable)
                if (this.enabled)
                    if (!Util.IsPointerOverUIObject())
                        Hablar();
        }



        protected abstract void Hablar();
        

    } 
}