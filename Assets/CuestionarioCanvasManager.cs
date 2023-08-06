using UnityEngine;

public class CuestionarioCanvasManager : MonoBehaviour
{
	private void Awake()
	{
		gameObject.SetActive(false); 
		DontDestroyOnLoad(gameObject); // Mantener el objeto del CuestionarioCanvas al cambiar de escena
	}
}
