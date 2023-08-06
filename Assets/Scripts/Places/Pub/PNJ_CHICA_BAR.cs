using UnityEngine;
using System.Collections;
using Tsiunas.SistemaDialogos;

namespace Tsiunas.Places {

    [RequireComponent(typeof(Animator))]
    public class PNJ_CHICA_BAR : PNJ_ACTUACION
    {
        public static bool chicaBarSeHaMarchado = false;
        private bool paso = false;

        public void ActivarFlagPrimeraAnimacion() {
            paso = true;
            PNJActor.AnimarHablar(Actor.id);
        }

        public override void Actuar(string arg1, object arg2)
        {
            Animator animator = GetComponent<Animator>();
            animator.enabled = true;
            animator.Play(EscogerAnimacion(this.paso));

            PNJActor.AnimarCaminar(Actor.id);
            chicaBarSeHaMarchado = true;
        }

        string EscogerAnimacion (bool _paso) {
            return _paso ? "Irse" : "Pasar";
        } 
    }
}
