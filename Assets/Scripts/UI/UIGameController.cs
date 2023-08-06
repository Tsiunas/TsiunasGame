using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DialogueSystem;

public class UIGameController : MonoBehaviour {

	// Attributes
	#region Attributes
	public Image containerTsiuna;
	public DialogueSystemController dialogueSC;
	private int indexEvent;
	public Transform[] spawnersTsiuna;
	public GameObject prefabTsiuna;
	public GameObject prefabHand;
	public GameObject imagePrefab;
	private bool wasCreated;
	GameObject tsiunaDragging;
	private int amountTsiunas;
	public Text textAmountTsiunas;
	public GameObject prefabSeedTsiuna;
	public GameObject prefabSeedTsiunaHoe;
	public GameObject prefabHandHelp;
	public GameObject door;
	private List<GameObject> tsiunas = new List<GameObject>();

	private GameObject[] fertileTiles;
	public Image arrow;
	public List<Image> uiHarmonyFlames = new List<Image>(9); 
	#endregion

	// Methods
	#region Methods
	// Use this for initialization
	void Start () {
		NotificationCenter.DefaultCenter().AddObserver(this, "EventLaunched");
		NotificationCenter.DefaultCenter().AddObserver(this, "DragTsiuna");
		NotificationCenter.DefaultCenter().AddObserver(this, "AddTsiuna");
		NotificationCenter.DefaultCenter().AddObserver(this, "SeedTsiuna");



		fertileTiles = GameObject.FindGameObjectsWithTag ("fertile");

		if (GameManager.Instance.WasObtainedTool (ToolType.Hoe)) {
			// Instanciar los retoños
			ReappearSproutTsiunas ();
		} else {
			Camera.main.GetComponent<LerpCamera> ().CreateLerpCamera (new Vector2 (0, 8), 0.5f);
		}

		SetupHarmonyFlames (6);

	}

	void SetupHarmonyFlames(int flames) {
		for (int i = uiHarmonyFlames.Count; i > 0; i--) {
			uiHarmonyFlames [i - 1].gameObject.SetActive (i > flames ? false : true);
		}
	}

	/// <summary>
	/// Sirve para cargar en escenas las plantas de Tsiunas sembradas
	/// </summary>
	public void ReappearSproutTsiunas() {
		for (int i = 0; i < fertileTiles.Length; i++) {
			Sprout sprout = Instantiate (prefabSeedTsiunaHoe, fertileTiles[i].transform, false).GetComponent<Sprout>();
			sprout.ChangeSpriteState (GameManager.Instance.BoughtInStore);
		}
	}

	IEnumerator CoroutinePassToAnotherDialog(float _delay) {
		yield return new WaitForSecondsRealtime (_delay);
		Util.PassToAnotherDialog ();
	}


	/// <summary>
	/// Desvanece las Flamas de la Armonía que han sido restadas en base a decisiones tomadas
	/// Hace aparecer las Flamas de la Armonía que han sido sumadas en base a decisiones tomadas
	/// </summary>
	/// <param name="forward">si esta seteado como <c>true</c> restar flamas</param>
	IEnumerator FadeHarmonyFlame (bool forward) {

		if (forward) {
			for (int i = uiHarmonyFlames.Count; i > 0; i--) {
			
                if (i > HarmonyFlamesManager.Instance.HarmonyFlames) {
					Debug.Log ("Flama " + i.ToString () + " _ Ocultar");
					uiHarmonyFlames [i - 1].GetComponent<Animator> ().SetBool ("FlameFallDown", true);
					yield return new WaitForSecondsRealtime (0.5f);
				} else {
			
					uiHarmonyFlames [i - 1].enabled = true;
					Debug.Log ("Flama " + i.ToString () + " _ Dejar visible");
				}
			
			}
		} else {
			
			for (int i = 0; i < uiHarmonyFlames.Count; i++) {

				Debug.Log ("Flama " + uiHarmonyFlames[i].name);
				Debug.Log ("Flama " + i.ToString () + " _ activo: " + uiHarmonyFlames [i].gameObject.activeSelf);
				if (!uiHarmonyFlames [i].gameObject.activeInHierarchy) {
					
					uiHarmonyFlames [i ].gameObject.SetActive(true);
					uiHarmonyFlames [i ].GetComponent<Animator> ().SetBool ("FlameAppear", true);
					yield return new WaitForSecondsRealtime (0.5f);

				} else {


					uiHarmonyFlames [i ].enabled = true;
					Debug.Log ("Flama " + i.ToString () + " _ Dejar visible");
				}

			}
		}
	}

	/// <summary>
	/// Este método es llamado cada que se toca el botón de omitir texto o tocando 2 veces en el globo de texto de un objeto DialogueUIController cuyo atributo *launchEvent*
	/// está seteado en true
	/// </summary>
	/// <param name="notification">Notification.</param>
	public void EventLaunched (Notification notification) {
		indexEvent++;
		// se lleva el conteo de un entero para ejecutar la lógica correspondiente

		Debug.Log ("Index Event: " + indexEvent.ToString());
		if (indexEvent == 1) {
			// Se condiciona si ya fue comprado el azadon y si se hizo la compra en la tienda de Don Jorge
			if (!GameManager.Instance.BoughtInStore && !GameManager.Instance.WasObtainedTool (ToolType.Hoe)) {
				// Se realiza interpolación de la cámara para observar a MamaTule
				Camera.main.GetComponent<LerpCamera> ().CreateLerpCamera (new Vector2 (0, 0), 0.5f);
				Camera.main.GetComponent<LerpCamera> ().lerpFinished += delegate() {
					SpawnTsiunas ();
				};
			} 
			else {
				arrow.gameObject.SetActive(true);
				StartCoroutine (CoroutinePassToAnotherDialog(2.0f));
				StartCoroutine (FadeHarmonyFlame(GameManager.Instance.BoughtInStore));
			}
		}

		if (indexEvent == 2) {

			if (!GameManager.Instance.BoughtInStore && !GameManager.Instance.WasObtainedTool (ToolType.Hoe)) {
				EnableDragTsiunas (true);
			} else {
				GameObject.FindObjectOfType<CameraHandler> ().CanPanCamera = true;
				arrow.gameObject.SetActive(false);
			}
		}
		if (indexEvent == 3) {
			Util.PassToAnotherDialog ();
			door.SetActive (true);
			door.GetComponent<Collider> ().enabled = false;
		}

		if (indexEvent == 4) {
			door.GetComponent<Collider> ().enabled = true;
			GameObject.FindObjectOfType<CameraHandler>().CanPanCamera = true;
			Util.PassToAnotherDialog ();
		}


	}

	public void AddTsiuna (Notification notification) {
		amountTsiunas++;
		textAmountTsiunas.text = amountTsiunas.ToString ();

		if (amountTsiunas >= 5) {
			//EnableDragTsiunas (true);
			Util.PassToAnotherDialog();
		}
	}

	public void SeedTsiuna (Notification notification) {
		amountTsiunas--;
		textAmountTsiunas.text = amountTsiunas.ToString ();

		if (amountTsiunas >= 2) {
			
			if ((GameObject)notification.data != null) {
				GameObject fertileTile = (GameObject)notification.data;
				Instantiate (prefabSeedTsiuna, fertileTile.transform, false);	
			}				
		}

		if (amountTsiunas == 2) {
			Util.PassToAnotherDialog ();
			EnableDragTsiunas (false);
		}
	}

	public void EnableDragTsiunas (bool enable) {
		foreach (Transform tsiuna in containerTsiuna.transform) {
			tsiuna.gameObject.GetComponent<UIDragBehaviourTsiunas>().enabled = enable;
		}
	}


	public void DragTsiuna (Notification notification) {
		object[] data = (object[])notification.data;
		Vector3 pos = (Vector3)data[0];
		tsiunaDragging = Instantiate (imagePrefab, pos, Quaternion.identity);
		tsiunaDragging.GetComponent<RectTransform>().SetParent(containerTsiuna.transform);
	}

	/// <summary>
	/// Instancia los frutos de Tsiunas
	/// </summary>
	public void SpawnTsiunas() {
		for (int i = 0; i < spawnersTsiuna.Length; i++) {
			GameObject tsiuna = (GameObject)Instantiate (prefabTsiuna, spawnersTsiuna [i].position, spawnersTsiuna[i].rotation);
			tsiuna.transform.parent = spawnersTsiuna [i].transform;
			tsiunas.Add (tsiuna);
		}
	}
	#endregion
}
