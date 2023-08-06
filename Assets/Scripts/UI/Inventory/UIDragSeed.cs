using System;
using Tsiunas.SistemaDialogos;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragSeed : UIDragGameElement, ISetCurrentSeed
{
    public override void BeginDrag(PointerEventData eventData)
    {
        base.BeginDrag(eventData);
        GameManager.Instance.currentDragSeed = SetCurrentSeed();
    }

    public override void EndDrag(PointerEventData eventData)
    {
        base.EndDrag(eventData);
        Component componente = DetectComponentRequired(eventData);
        if (componente == null)
            return;

        if (componente is PNJActor) {
            PNJActor actor = (PNJActor)componente;
            if (actor.isMerchant) {
                GameManager.SellFruitOrSeed(GameManager.Instance.currentDragSeed, actor);
            }

            // TODO: pendiente definir que pasa cuando se dá una semilla de un PNJActor que no es comerciantes
            /*
             * Al dar los frutos de Tsiunas se Tsinua un PNJActor, al dar las semillas (de maíz o Tsiunas) ¿que pasa?
            */
        }

        if (componente is ToolTarget) {
            ToolTarget target = (ToolTarget)componente;
            if (target.targetType == TargetType.GROUND)
            {
                if (!target.IsPlanted && target.IsPlowed)
                {
                    target.IsPlanted = true;
                    target.ChangeType(TargetType.PLANT);

                    /* Se hace la llamada al método de sembrar y se pasa como parámetro
                       el objeto tocado en la escena: El TileGround */
                    GameManager.Instance.currentDragSeed.BeSowed(target.gameObject);
                }
            }
        }
    }

    public override void DropInStomach()
    {
        GameManager.Instance.currentDragSeed.BeEaten();
    }

    public Seed SetCurrentSeed()
    {
        return GameManager.Instance.GetSeed(GetComponent<UISemilla>().seedType);
    }
}
