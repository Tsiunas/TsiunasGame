
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tsiunas.SistemaDialogos
{
    public abstract class LineaDialogo : UnityEngine.ScriptableObject, IWithID
    {
        [NonSerialized]
        protected Intervencion owner;
        public string idPNJ;

        public enum TiposLineaDialogo
        {
            Simple,
            ConOpciones            
        }


        public LineaDialogo()
        {

        }

        public void SetOwner(Intervencion i)
        {
            this.owner = i;
        }

        public Intervencion GetOwner()
        {
            return owner;
        }


        public abstract void Desplegar();

        #region Implementación IWithID
        public void SetID(string id)
        {
            this.idPNJ = id;
        }
        public string GetID()
        {
            return idPNJ;
        }
        #endregion
    }
}