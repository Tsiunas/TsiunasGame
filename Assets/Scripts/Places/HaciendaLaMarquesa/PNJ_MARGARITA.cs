using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tsiunas.Places
{
    public class PNJ_MARGARITA : PNJ_ACTUACION
    {
        public Sprite aparienciaSegundoHito;
        public static readonly string KEY_MARGARITA = "YA_HABLO_CON_MARGARITA";
        public override void Actuar(string arg1, object arg2)
        {
            PlaceFlags.Instance.RaiseFlag(KEY_MARGARITA);
        }

        public void CambiarGraficosSegundoHito()
        {
            SpriteRenderer sr =  GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.sprite = aparienciaSegundoHito;
        }
    }

   
}