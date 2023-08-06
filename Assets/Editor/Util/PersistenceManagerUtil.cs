using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PersistenceManagerUtil : ScriptableObject {

    [MenuItem("Tsiunas/Persistencia/Borrar archivos de perfil")]
    static void DoIt()
    {
        PersistenceManager.DeleteAllFilesProfile();
    }
}
