using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour {

	// Attributes
	#region Attributes
	public Button btnPlay;
	public Button btnCredits;
	public Button btnQuit;
	#endregion

	// Methods
	#region Methods
	// Use this for initialization
	void Start () {
		
		
		btnPlay.onClick.AddListener (() => {
			SoundManager.PlaySound(this, 0);
			this.Invoke(() => SceneLoadManager.Instance.CargarEscena("Farm"), 0.5f);
			TrackerSystem.Instance.SendTrackingData("user", "pressed", "menu", "boton_iniciar_juego|serious-game|éxito");
			TrackerSystem.Instance.SendTrackingData("user", "joined", "session", "serious-game|serious-game|éxito");
		});

		btnCredits.onClick.AddListener(() => {
			SoundManager.PlaySound(this, 0);
			this.Invoke(() => SceneLoadManager.Instance.CargarEscena("Credits"), 0.5f);			
			TrackerSystem.Instance.SendTrackingData("user", "pressed", "menu", "boton_creditos|serious-game|éxito");
			
		});

		btnQuit.onClick.AddListener (() => {			
			TrackerSystem.Instance.SendTrackingData("user", "pressed", "menu", "boton_abandonar_juego|serious-game|éxito");
			TrackerSystem.Instance.SendTrackingData("user", "quited", "session", "game|serious-game|éxito");
			#if UNITY_ANDROID
			Application.Quit();
			#endif
		});
	}

	void OnDestroy() {
		Button[] btns = GetComponentsInChildren<Button> ();
		if (btns.Length > 0) {
			foreach (Button btn in btns) {
				btn.onClick.RemoveAllListeners ();
			}
		}
	}
	#endregion
}
