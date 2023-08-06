using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// 
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// </summary>
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance;

    private static object _lock = new object();
    protected bool uniqueToAllApp = false;

    public static T Instance
    {
        get
        {

            if (applicationIsQuitting)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                    "' already destroyed on application quit." +
                    " Won't create again - returning null.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError("[Singleton] Something went really wrong " +
                            " - there should never be more than 1 singleton!" +
                            " Reopening the scene might fix it.");
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = "(singleton) " + typeof(T).ToString();

                        //AÑADIDO: Esto hace que el objeto sea unico para toda la aplicacion
                        //Por defecto es unico solo para la escena
                        if (_instance.uniqueToAllApp == true)
                            _instance.SetToBeUniqueOnAllApp();

                        Debug.Log("[Singleton] An instance of " + typeof(T) +
                            " is needed in the scene, so '" + singleton +
                            "' was created with DontDestroyOnLoad.");
                    }
                    else
                    {
                        Debug.Log("[Singleton] Using instance already created: " +
                            _instance.gameObject.name);
                        
                    }
                }

                return _instance;
            }
        }
    }

    protected void Start()
    {
        if (_instance != null)
        {
            if (uniqueToAllApp)
                SetToBeUniqueOnAllApp();
        }
        if (FindObjectsOfType(typeof(T)).Length > 1)
        {
            Destroy(this.gameObject);
        }
    }




    private static bool applicationIsQuitting = false;
    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it have been destroyed, 
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    public void OnDestroy()
    {
        if (uniqueToAllApp)
            applicationIsQuitting = true;
    }

    private void OnApplicationQuit()
    {
        applicationIsQuitting = true;
    }

    /// <summary>
    /// Este método lo agrego aqúí par que el DontDestroyOnLoad sea opcional. 
    /// Si se llama este método entonces el objeto es unico para toda la aplicación
    /// Si nunca se llama este método, el objeto es único solo para la escena
    /// </summary>
    private void SetToBeUniqueOnAllApp()
    {
        if (uniqueToAllApp)
            if (_instance != null)
                DontDestroyOnLoad(_instance.gameObject);
            else
                throw new SingletonException("No se puede llamar al método SetToBeUniqueOnAllApp porque la instancia no ha sido inicializada");
        else
            throw new SingletonException("El objeto singleton no ha sido marcado para NO eliminarse porque la variable uniqueToAllApp está marcada como false. Marquela como true en su clase Singleton para poder usar este método.");

    }

    // brinda el lugar donde realizar la llamada de eventos cada que se cargue una escena
    // así al iniciar x escena se disparan los eventos y quien los implemente será notificado
    // ej. actualizar al cargar la escena la GUI de Dinero
    #region Llamado de eventos
    void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    public virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode) { }
    void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }
    #endregion
}


public class SingletonException : System.Exception
{
    public SingletonException(string message):base(message)
    {
        
    }

}


