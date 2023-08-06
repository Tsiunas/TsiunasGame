using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileDataController : MonoBehaviour {

    public Button btnPlay;
    public Button btnContinue;
    public Transform containerProfilesUI;
    public GameObject prefabProfileUI;
    public GameObject viewProfiles;
    public Text textAmountProfilesSaved;
	// Use this for initialization
	void Start () {
        SetupUIElements();
        SetupListeners();
	}

    /// <summary>
    /// Establece los listeners de los botones, en el evento OnClick
    /// </summary>
    private void SetupListeners()
    {
        btnPlay.onClick.AddListener(PlayGame);
        btnContinue.onClick.AddListener(ContinueGame);
    }

    void PlayGame() {
        PersistenceManager.Instance.SaveProfileData(PersistenceManager.Instance.ProfileDataDefault);
    }

    void ContinueGame() {
        SetupProfiles();
    }

    /// <summary>
    /// Sirve para dependiendo la cantidad de datos de perfil almacenados, mostrar los Slots respectivos y que se cargue el presionado
    /// </summary>
    private void SetupProfiles()
    {
        // Si se va a almacenar solo un perfil entonces, cuando se presione el botón continuar, cargara los datos del "ProfileData1.gd"
        if (PersistenceManager.MAX_NUMBER_OF_PROFILES == 1)
        {
            Continue(1);
        }
        else
        {
            // si se va a almacenar mas de un perfil, entonces se muestra en un Horizontal Layout Group los botones correspondientes
            viewProfiles.SetActive(true);
            for (int i = 0; i < PersistenceManager.Instance.GetDataProfilesSaved(); i++)
            {
                // Se instancia in ProfileUI (un botón con un texto que indica que perfil es el que se cargara cuando se presione)
                ProfileUI profileUI = Instantiate(prefabProfileUI, containerProfilesUI).GetComponent<ProfileUI>();
                // Se establece el profileID
                profileUI.ProfileID = i + 1;
                // Se establece el texto: ej. Partida 1
                profileUI.SetTextProfile(profileUI.ProfileID);
                // se añade un listener anónimo al evento OnClick
                profileUI.ProfileButton.onClick.AddListener(delegate { Continue(profileUI.ProfileID); });
            }
        }
    }

    public void HideViewProfiles() {
        // Destruye todos los ProfileUI
        foreach (Transform profile in containerProfilesUI)
        {
            Destroy(profile.gameObject);
        }
        viewProfiles.SetActive(false);
    }

    void Continue(int _profileNumber) {
        // Carga los datos de perfil de acuerdo al _profileNumber
        PersistenceManager.Instance.LoadProfileData(_profileNumber);
        Debug.Log("Sesion numero: "+PersistenceManager.Instance.GetProfileSessionId());
        // Va a la escena "Farm" (Luego de pasar por la escena de Loading)
        SceneLoadManager.Instance.CargarEscena("Farm");
        TrackerSystem.Instance.SendTrackingData("user", "pressed", "menu", "boton_continuar_juego|serious-game|éxito");
    }

    /// <summary>
    /// Sirve para configurar los elementos de interfaz
    /// </summary>
    private void SetupUIElements()
    {
        // Si hay almacenados la misma cantidad de datos de perfil que el número máximo de perfiles a crear, entonces: el botón de Jugar se desactiva
        btnPlay.interactable = PersistenceManager.Instance.GetDataProfilesSaved() != PersistenceManager.MAX_NUMBER_OF_PROFILES;
        // Si no hay almacenados ningún dato de perfil, entonces: el botón de Continuar se desactiva
        btnContinue.interactable = PersistenceManager.Instance.GetDataProfilesSaved() != 0;

        // se establece la cantidad de datos de perfil almacenados, en el texto del ícono dentro del botón de Continuar 
        textAmountProfilesSaved.text = PersistenceManager.Instance.GetDataProfilesSaved().ToString();
        // Si la cantidad es 0, el texto (y el fondo: circulo rojo), se oculta
        textAmountProfilesSaved.transform.parent.gameObject.SetActive(btnContinue.IsInteractable());
    }

    void OnDestroy()
    {
        RemoveAllListenerOfButtons();
    }

    void RemoveProfilesListeners () {
        btnPlay.onClick.RemoveListener(PlayGame);
        btnContinue.onClick.RemoveListener(ContinueGame);
    }

    public void DeleteAllFilesProfile() {
        
        PersistenceManager.DeleteAllFilesProfile();
        RemoveProfilesListeners();
        SetupUIElements();
        SetupListeners();
    }

    private void RemoveAllListenerOfButtons()
    {
        // Se remueven todos los listeners de los botones que haya en la escena
        Button[] btns = FindObjectsOfType<Button>();
        if (btns.Length > 0)
        {
            foreach (Button btn in btns)
            {
                btn.onClick.RemoveAllListeners();
            }
        }
    }
}
