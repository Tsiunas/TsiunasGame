using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class CuestionarioManager : Singleton<CuestionarioManager>
{
	public Text textoPregunta;
    public Button[] botonesRespuesta;
    public InputField campoRespuesta;
    public Button botonEnviar;
    public GameObject cuestionarioCanvas;

    private string rutaArchivoJSON;
    private List<Pregunta> preguntas;
    private int indicePreguntaActual = 0;

    private static CuestionarioManager instance;

    public static CuestionarioManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("CuestionarioManager");
                instance = go.AddComponent<CuestionarioManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

	private void Start()
	{
		
	}

	private void CargarPreguntasDesdeJSON()
    {
		Debug.Log(rutaArchivoJSON);
        string jsonString = File.ReadAllText(rutaArchivoJSON); // Leer el archivo JSON desde la ruta especificada
        PreguntasData preguntasData = JsonUtility.FromJson<PreguntasData>(jsonString);
        preguntas = preguntasData.preguntas;
    }


	public void MostrarCuestionario(string rutaJSON) // Agregar parámetro para la ruta del archivo JSON
    {
        rutaArchivoJSON = Path.Combine(Application.dataPath, rutaJSON);
        indicePreguntaActual = 0;
        CargarPreguntasDesdeJSON(); // Cargar las preguntas desde el nuevo archivo JSON
        MostrarPregunta();
        cuestionarioCanvas.SetActive(true);
    }


	private void MostrarPregunta()
	{
		if (indicePreguntaActual >= preguntas.Count)
		{
			// Se han mostrado todas las preguntas
			Debug.Log("Fin del cuestionario");
			cuestionarioCanvas.SetActive(false); // Deshabilita el CuestionarioCanvas
			return;
		}

		Pregunta preguntaActual = preguntas[indicePreguntaActual];
		textoPregunta.text = preguntaActual.pregunta;
		if (preguntaActual.esSoloTexto)
			{
				// Pregunta de solo texto
				textoPregunta.text = preguntaActual.pregunta;

				// Desactiva los botones de respuesta
				foreach (Button boton in botonesRespuesta)
				{
					boton.gameObject.SetActive(false);
				}

				// Desactivar botón de enviar y campo de respuesta para preguntas de solo texto
				botonEnviar.gameObject.SetActive(true);
				campoRespuesta.gameObject.SetActive(false);
				botonEnviar.onClick.RemoveAllListeners();
				botonEnviar.onClick.AddListener(EnviarRespuesta);
			}

		else if (preguntaActual.respuestas.Count > 0)
		{
			// Pregunta con opciones de respuesta
			campoRespuesta.gameObject.SetActive(false);

			for (int i = 0; i < preguntaActual.respuestas.Count; i++)
			{
				botonesRespuesta[i].gameObject.SetActive(true);
				botonesRespuesta[i].GetComponentInChildren<Text>().text = preguntaActual.respuestas[i];

				// Eliminar los listeners de eventos anteriores
				botonesRespuesta[i].onClick.RemoveAllListeners();

				// Asociar la función SeleccionarRespuesta al evento OnClick del botón
				int indiceRespuesta = i; // Para evitar problemas con el cierre
				botonesRespuesta[i].onClick.AddListener(() => SeleccionarRespuesta(indiceRespuesta));
			}

			// Desactiva los botones de respuesta adicionales
			for (int i = preguntaActual.respuestas.Count; i < botonesRespuesta.Length; i++)
			{
				botonesRespuesta[i].gameObject.SetActive(false);
			}

			// Desactivar botón de enviar y campo de respuesta para preguntas con opciones
			botonEnviar.gameObject.SetActive(false);
			campoRespuesta.gameObject.SetActive(false);
		}
		else
		{
			// Pregunta de escritura
			campoRespuesta.gameObject.SetActive(true);

			// Desactiva los botones de respuesta
			foreach (Button boton in botonesRespuesta)
			{
				boton.gameObject.SetActive(false);
			}

			// Activar botón de enviar para preguntas abiertas
			botonEnviar.gameObject.SetActive(true);
			botonEnviar.onClick.RemoveAllListeners();
			botonEnviar.onClick.AddListener(EnviarRespuesta);
		}
	}


	public void SeleccionarRespuesta(int indiceRespuesta)
	{
		Pregunta preguntaActual = preguntas[indicePreguntaActual];

		if (indiceRespuesta == preguntaActual.respuestaCorrecta)
		{
			Debug.Log("Respuesta correcta");
		}
		else
		{
			Debug.Log("Respuesta incorrecta");
		}
		if(preguntaActual.respuestas.Count > 0){
			char separador = '\\';
			string[] partes = this.rutaArchivoJSON.Split(separador);
			string ultimoElemento = partes[partes.Length - 1];


			TrackerSystem.Instance.EnviarPreguntaRespuesta(ultimoElemento,preguntaActual.pregunta, preguntaActual.respuestas[indiceRespuesta]);}

		// Pasar a la siguiente pregunta o limpiar el cuestionario al finalizar
		indicePreguntaActual++;
		MostrarPregunta();
	}

	public void EnviarRespuesta()
	{
		Pregunta preguntaActual = preguntas[indicePreguntaActual];
		string respuestaUsuario = campoRespuesta.text;

		Debug.Log("Respuesta del usuario: " + respuestaUsuario);

		// Enviar la pregunta y la respuesta al TrackerSystem
		char separador = '\\';;
			string[] partes = this.rutaArchivoJSON.Split(separador);
			string ultimoElemento = partes[partes.Length - 1];
		TrackerSystem.Instance.EnviarPreguntaRespuesta(ultimoElemento,preguntaActual.pregunta, respuestaUsuario);

		// Pasar a la siguiente pregunta o limpiar el cuestionario al finalizar
		indicePreguntaActual++;
		// Limpiar el campo de respuesta
    	campoRespuesta.text = "";
		MostrarPregunta();
	}
}

[System.Serializable]
public class PreguntasData
{
	public List<Pregunta> preguntas;
}

[System.Serializable]
public class Pregunta
{
	public string pregunta;
	public List<string> respuestas;
	public int respuestaCorrecta;
	public bool esSoloTexto;
}
