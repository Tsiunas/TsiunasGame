using UnityEngine.EventSystems;

public class UIDragFood : UIDragGameElement, ISetCurrentFood
{
    public override void BeginDrag(PointerEventData eventData)
    {
        base.BeginDrag(eventData);
        GameManager.Instance.currentDragFood = SetCurrentFood();
    }

    public override void EndDrag(PointerEventData eventData)
    {
        base.EndDrag(eventData);
    }

    public Food SetCurrentFood()
    {
        return GameManager.Instance.GetFood(GetComponent<UIComida>().foodType);
    }

    public override void DropInStomach()
    {
        GameManager.Instance.currentDragFood.BeEaten();
    }
}
