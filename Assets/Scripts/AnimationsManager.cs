using System;
using System.Collections;
using System.Collections.Generic;
using Tsiunas.SistemaDialogos;
using UnityEngine;

public class AnimationsManager : SimpleSingleton<AnimationsManager>  {

    public const string PARAMETER_ANIM = "estadoAnim";
    public const string TAG_ANIMATOR = "Animator";

    public void AnimarPNJ(string _id, int estadoAnimacion) {
        if (string.IsNullOrEmpty(_id) || _id.Equals("PJ") || _id.Equals("PNJ_MAMTULE"))
            return;

        // Si el PNJ tocado es anónimo, salga del método
        if (GestorPNJ.Instance.EncontrarAnonimo(_id) != null)
            return;

        PNJActor actor = GestorPNJ.Instance.EncontrarActor(_id);
        if (actor == null)
            throw new TsiunasException("Actor con ID: [" + _id + "] no encontrado en escena", true, "INTEGRACION_MODELOS_ANIMADOS", "Eduardo");
        else {
            PNJModelController pnjModel = actor.GetComponent<PNJModelController>();
            if (pnjModel == null)
                throw new TsiunasException("El actor con ID: [" + _id + "] no tiene un componente _PNJModelController_", true, "INTEGRACION_MODELOS_ANIMADOS", "Eduardo");
            else {
                Animator animator = pnjModel.GetAnimator;
                if (animator == null)
                    throw new TsiunasException("El actor con ID: [" + _id + "] no tiene un componente _Animator_ en sus objetos hijos", true, "INTEGRACION_MODELOS_ANIMADOS", "Eduardo");
                else
                    animator.SetInteger(PARAMETER_ANIM, estadoAnimacion);
            }
        }
    }

    public string ObtenerIDPorNombre(string nombre) {
        string resultado = "";
        switch (nombre)
        {
            case "Don Jorge":
                resultado = "PNJ_DON JORGE";
                break;
            case "Yurani":
                resultado = "PNJ_YURANI";
                break;
        }
        return resultado;
    } 
}
