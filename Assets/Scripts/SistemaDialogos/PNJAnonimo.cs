
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tsiunas.SistemaDialogos
{
    public class PNJAnonimo : PNJ
    {
        private SpriteRenderer _spriteR;
        private void Start()
        {
            _spriteR = GetComponent<SpriteRenderer>();
            if (_spriteR == null)
                throw new TsiunasException("No se encontrado componente _SpriteRenderer_", true, "INTEGRACION_PERSONAJES_ANONIMOS", "Eduardo");
            else
                AsignarSprite();
        }

        private void AsignarSprite()
        {
            _spriteR.sprite = ObtenerSprite();
            RestablecerCollider();
        }

        void RestablecerCollider() {
            Destroy(GetComponent<PolygonCollider2D>());
            this.gameObject.AddComponent<PolygonCollider2D>();
        }

        Sprite ObtenerSprite()
        {
            Sprite sp = ExtrasManager.Instance.sprites.Find(sprite => sprite.name.Equals(id));
            if (sp == null)
                throw new TsiunasException("Sprite con nombre: " + id + " no encontrado", true, "INTEGRACION_PERSONAJES_ANONIMOS", "Eduardo");
            else
                return sp;
        }

        protected override void Hablar()
        {
            //Hablar usando el speech del PNJ Datos
            TrackerSystem.Instance.SendTrackingData("user", "interacted", "character",this.name+"|"+this.name+"|éxito");
            DesplegadorDialogo.Instance.DesplegarLinea(this.datosPNJ, this.datosPNJ.speechPorDefecto);
        }


    }

    public class ExtrasManager : Singleton<ExtrasManager>
    {
        public List<Sprite> sprites;
        private void Awake() { sprites = new List<Sprite>(Resources.LoadAll<Sprite>("Extras")); }
        public ExtrasManager() { base.uniqueToAllApp = true; }
    }
}