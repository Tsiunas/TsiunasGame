using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;


public class TrackerSystem : Singleton<TrackerSystem> {

	private int tracks = 0;
    private int id_session=-1;
    private int id_user=-1;
	private string url = "localhost";

	// Use this for initialization
	void Start () {
		LoadURLFromTextFile();
	}
    private void LoadURLFromTextFile()
    {
        string filePath = Path.Combine(Application.dataPath, "ruta.txt");

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length >= 1)
            {
                url = "http://"+lines[0]+":4000/api/";
            }
        }
    }

	
	// Update is called once per frame
	void Update () {
		
	}
	 IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(uri))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                
            }
            else
            {
                // Show results as text
            }
        }
    }
     IEnumerator PostRequest( string json,string endPoint) {
        // Crear una instancia de WWWForm y agregar los datos a enviar en la solicitud POST
        WWWForm form = new WWWForm();
        form.AddField("json", json);
        // Crear una instancia de la clase WWW para enviar la solicitud POST
        WWW www = new WWW(url+endPoint, form);

        // Esperar a que se complete la solicitud
        yield return www;

        // Verificar si hubo algún error en la solicitud
        if (www.error == null) {
            // La solicitud fue exitosa
            Debug.Log("POST request successful");
            if (id_session==-1)
            {
                 
            string[] responseParts = www.text.Replace("\"", "").Split('|');         
            try
            {PersistenceManager.Instance.SetProfileSessionId(int.Parse(responseParts[0]));
            PersistenceManager.Instance.SetProfileUserId(int.Parse(responseParts[1]));
                
            }
            catch (System.Exception)
            {
               
            }
            
            }
            
           

        } else {
            // Ocurrió un error en la solicitud
            Debug.Log("POST request error: " + www.error);
        }
    }
public void EnviarPreguntaRespuesta(string nombre,string pregunta, string respuesta)
{
    try
    {
        id_session = PersistenceManager.Instance.GetProfileSessionId();
        id_user = PersistenceManager.Instance.GetProfileUserId();
    }
    catch (System.Exception)
    {
    }

    // Crear una cadena de texto que contenga los parámetros requeridos junto con el timestamp
    string preguntaRespuestaData = string.Format(@"{{""pregunta"":""{0}"",""respuesta"":""{1}"",""id_user"":""{2}"",""id_session"":""{3}"",""name"":""{4}""}}", pregunta, respuesta, id_user, id_session,nombre);

    // Llamar a la función PostRequest con la cadena de texto creada y el endpoint 'preguntas' como parámetros
    StartCoroutine(PostRequest(preguntaRespuestaData, "preguntas"));
}

	public void SendTrackingData(string actor, string verb, string objeto, string resultado)
	{
        try
        {
            id_session = PersistenceManager.Instance.GetProfileSessionId ();
            id_user = PersistenceManager.Instance.GetProfileUserId ();
        }
        catch (System.Exception)
        {
        }
		

		// Crear una cadena de texto que contenga los parámetros requeridos junto con el timestamp
		string trackingData = string.Format(@"{{""actor"":""{0}"",""verb"":""{1}"",""objeto"":""{2}"",""resultado"":""{3}"",""id_user"":""{4}"",""id_session"":""{5}""}}", actor, verb, objeto, resultado,id_user,id_session);


		// Llamar a la función PostRequest con la cadena de texto creada como parámetro
		StartCoroutine(PostRequest(trackingData,"traces"));
	}

}

