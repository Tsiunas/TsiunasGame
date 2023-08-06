using UnityEngine;
/// <summary>
/// Clase de ayuda para definir constantes importantes en el manejo de la persistencia
/// Si se desea cambiar algún valor recurrir a esta clase.
/// </summary>
public static class PersistenceHelp
{
    /// <summary>
    /// Nombres de los archivos donde se guardaran aspectos importantes del juego
    /// </summary>
    public static class FileNames { public static readonly string time = "Time.gd"; }

    /// <summary>
    /// El prefijo del nombre de los archivos para guardar el perfil de juego (slot)
    /// </summary>
    public static readonly string PROFILE_DATA_FILE_NAME = "ProfileData";
    /// <summary>
    /// La extensión del archivo para guardar el perfil de juego (slot)
    /// </summary>
    public static readonly string PROFILE_DATA_FILE_EXTENSION = ".gd";

    /// <summary>
    /// Retorna la ruta del directorio dependiendo de la plataforma
    /// </summary>
    /// <returns>The profile data directory path.</returns>
    public static string GetProfile_Data_Directory_Path()
    {
#if UNITY_IPHONE || UNITY_ANDROID
        return Application.persistentDataPath + "/Profiles/";
#else
        return Application.dataPath + "/Profiles/";
#endif
    }
}