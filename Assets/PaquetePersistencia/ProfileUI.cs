using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileUI : MonoBehaviour {

    private Button profileButton;
    public Text textNameProfile;
    private const string NAME_PROFILE = "Partida";
    [SerializeField]
    private int profileID;

    public Button ProfileButton
    {
        get { return profileButton; }
        set { profileButton = value; }
    }

    public int ProfileID
    {
        get { return profileID; }
        set { profileID = value; }
    }

    // Use this for initialization
    void Awake () {
        this.profileButton = GetComponent<Button>();
	}

    public void SetTextProfile(int profileNumber) {
        this.textNameProfile.text = NAME_PROFILE + " " + profileNumber; 
    }
}
