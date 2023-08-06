using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ComportamientoCargaAsincronaEscena : MonoBehaviour {

    public Image imagenCarga;
    SceneLoadManager sceneLoad;

    private void Start()
    {
        sceneLoad = SceneLoadManager.Instance;
        StartCoroutine(CargarAsincronamente(sceneLoad.nombreEscenaAIr));
    }

    IEnumerator CargarAsincronamente(string nombre) {
        AsyncOperation operacion = SceneManager.LoadSceneAsync(nombre);

        while (!operacion.isDone)
        {
            float progreso = Mathf.Clamp01(operacion.progress / .9f);
            imagenCarga.fillAmount = progreso;
            yield return null;
        }
    }
}

public class SceneLoadManager : SimpleSingleton<SceneLoadManager> {
    public string nombreEscenaAIr;
    public void CargarEscena(string nombreEscena, float retrasoParaCargarEscena = 0)
    {
		TrackerSystem.Instance.SendTrackingData("user", "leave", "zone",SceneManager.GetActiveScene().name+ "|serious-game|éxito");
		TrackerSystem.Instance.SendTrackingData("user", "accessed", "zone",nombreEscena+ "|serious-game|éxito");
        nombreEscenaAIr = nombreEscena;
        StaticCoroutine.DoCoroutine(CargarEscenaConRetraso(retrasoParaCargarEscena));
    }

    IEnumerator CargarEscenaConRetraso(float retraso) {
		
        yield return new WaitForSeconds(retraso);
        SceneManager.LoadScene("Loading");
    }
}
