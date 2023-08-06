using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase de utilidad. Borra el archivo de tiempo cuando se presiona un botón en el menú principal
/// Sirve para reiniciar el reloj en dispositivo movil
/// </summary>
public class DebugReloj : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

		
	}

#if DEBUG_ALLOWED
    private void OnGUI()
    {

        if(GUILayout.Button("(DEBUG) Borrar Archivo de tiempo"))
        {
            PersistenceManager.Delete(fileNameToDelete: PersistenceHelp.FileNames.time);
            Debug.Log("Archivo de tiempo borrado. El tiempo iniciarás desde 0");
        }

    }
#endif


}
