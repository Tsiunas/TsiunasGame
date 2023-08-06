using UnityEngine;
using UnityEngine.EventSystems;
using Tsiunas.SistemaDialogos;
using System;
using UnityEngine.UI;

public class UIDragFruit : UIDragGameElement, ISetCurrentFruit
{

	public Action<GameObject> OnFruitGived = delegate { };
	public Action<GameObject> OnFruitSelled = delegate { };

	public GameObject prohibedSignPrefab;
	private Image prohibedSign;

	private UIFruto _fruto;

	public override void Awake()
	{
		base.Awake();
		_fruto = GetComponent<UIFruto>();
	}

	public override void BeginDrag(PointerEventData eventData)
	{
		base.BeginDrag(eventData);
		GameManager.Instance.currentDragFruit = SetCurrentFruit();
	}

	void FeedbackTsiunar(Action action = null) {
		if (_fruto.fruitType == FruitType.TsiunaFruit)
			if (action != null)
				action();
	}

	public override void OnDrag(PointerEventData eventData)
	{
		base.OnDrag(eventData);
		PNJActor actor = null;

		Component componente = DetectComponentRequired(eventData);
		if (componente == null) {
			if (prohibedSign != null)
				prohibedSign.enabled = false;
			return;
		}

		if (componente is PNJActor) {
			actor = (PNJActor)componente;
		}

		FeedbackTsiunar(() => {
			if (actor != null)
			{
				if (actor.EstadoMachismoActual != PNJDatos.EstadosMachismo.Corresponsable)
				{
					
					if (prohibedSign == null)
					{
						prohibedSign = Instantiate(prohibedSignPrefab).GetComponent<Image>();
						prohibedSign.transform.SetParent(hoverObject.transform, false);
						prohibedSign.transform.position += new Vector3(0, 100, 0);
						prohibedSign.enabled = false;
					}

					if (!actor.SaberPuedeSerTsinuado())
						prohibedSign.enabled = true;
					else
						prohibedSign.enabled = false;
				}
			}
			else
			{
				if (prohibedSign != null)
					prohibedSign.enabled = false;
			}
		});
	}

	public override void EndDrag(PointerEventData eventData)
	{
		base.EndDrag(eventData);

		Component componente = DetectComponentRequired(eventData);
		if (componente == null)
			return;

		if (componente is PNJActor) {
			PNJActor actor = (PNJActor)componente;
			if (GestorPNJ.Instance.EsMamaTule(actor))
				return;
			
			switch (GameManager.Instance.currentDragFruit.fruitType)
			{
				case FruitType.CornFruit:
					SellToMerchant(actor);
					break;
				case FruitType.TsiunaFruit:

					if (actor.EstadoMachismoActual == PNJDatos.EstadosMachismo.Corresponsable) {
						SellToMerchant(actor);
					} else {
						// solo puede ser Tsinuado si no tiene situaciones pendientes
						// Se consulta con la propiedad PuedeSerTsinuado
						if (actor.SaberPuedeSerTsinuado())
						{
							Debug.Log("Tsiunas" + " fue: dado a "+actor.name);						
							TrackerSystem.Instance.SendTrackingData("user", "dragged", "item", "tsiuna|"+actor.name+"|éxito");
							TrackerSystem.Instance.SendTrackingData("user", "decreased", "item", "tsiuna|"+actor.name+"|éxito");		
							GameManager.Instance.currentDragFruit.BeGiven();
							OnFruitGived(actor.gameObject);

							if (GameManager.Instance.GetGameState == GameState.InIntro)
								return;
							GestorPNJ.Instance.TsiunarPNJ(actor);
						}else
						{
							TrackerSystem.Instance.SendTrackingData("user", "dragged", "item", "tsiuna|"+actor.name+"|fallo");
							Debug.Log(GameManager.Instance.GetNamePJ+" dio Tsiunas inccorrectamente a " +actor.name);
						}
					}                   
					break;
			}
		}
	}

	private void SellToMerchant(PNJActor _actor) {
		if (_actor.isMerchant)
		{
			GameManager.SellFruitOrSeed(GameManager.Instance.currentDragFruit, _actor);
			OnFruitSelled(_actor.gameObject);
		}
	}

	public Fruit SetCurrentFruit()
	{
		return GameManager.Instance.GetFruit(GetComponent<UIFruto>().fruitType);
	}

	public override void DropInStomach()
	{
		GameManager.Instance.currentDragFruit.BeEaten();
	}


}
