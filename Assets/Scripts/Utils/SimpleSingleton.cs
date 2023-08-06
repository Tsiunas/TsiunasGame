using UnityEngine;
/// <summary>
/// Clase Singleton que no requiere de crear un GameObject en la escena
/// </summary>
public class SimpleSingleton<T> where T : SimpleSingleton<T>, new()
{
    private static T _instance;

    private static object _lock = new object();
    

    public static T Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new T();
                    Debug.Log("[SimpleSingleton] Una instancia de " + typeof(T) +" ha sido creada");
                                        
                }

                return _instance;
            }
        }
    }    
}
