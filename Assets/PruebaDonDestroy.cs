using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PruebaDonDestroy : MonoBehaviour {

    #region Enums
    #endregion

    #region Atributos y propiedades
    public int valor;
	#endregion
	
	#region Eventos
	#endregion
	
	#region Métodos
	#endregion

	#region Mensajes Unity
	// Use this for initialization
	void Start ()
    {
        DontDestroyOnLoad(this);
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            transform.Translate(Vector3.right);
            valor++;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("Escena recargada");
        }

    }
	#endregion
	
	
}
