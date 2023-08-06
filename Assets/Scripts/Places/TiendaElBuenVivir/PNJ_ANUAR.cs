using System.Collections;
using System.Collections.Generic;
using Tsiunas.SistemaDialogos;
using UnityEngine;

namespace Tsiunas.Places
{
    public class PNJ_ANUAR : PNJ_ACTUACION
    {
        
        private new void Start()
        {
            base.Start();
            if (!TiendaElBuenVivir.pendienteHablar)
            {
                Actor.EstablecerSituacionActiva("SIT_INICIAL");
            }
        }

        public static void EstarPendiente()
        {
            TiendaElBuenVivir.pendienteHablar = true;
        }

        public override void Actuar(string arg1, object arg2)
        {
            if (Actor.NivelAmistad < NivelesAmistad.REGULAR)
            {
                if (GameManager.Instance.Money < 15)
                {
                    DesplegadorDialogo.Instance.DesplegarLinea(Actor.datosPNJ, new Speech("No, compa, no tiene suficiente dinero"));
                    Situacion.SituacionActual.noConsumir = true;
                }
                else
                {
                    Situacion.SituacionActual.noConsumir = false;
                    GameManager.Instance.DecreaseMoneyAmount(15);
                    StoreManager.ObtainTool(TypesGameElement.Tools.Watering_Can);
                }
            }
            else
            {
                if (Actor.NivelAmistad > NivelesAmistad.REGULAR)
                {
                    StoreManager.ObtainTool(TypesGameElement.Tools.Watering_Can);
                    SoundManager.PlaySound(SoundManager.SonidosGenerales.RegaloHerramienta);
                }
            }
        }
    }
}