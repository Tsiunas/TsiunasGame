using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Tsiunas.Mechanics;
using Tsiunas.SistemaDialogos;
using UnityEngine.SceneManagement;
using UnityEngine;

/// <summary>
/// Esta clase se encarga de manejar la persistencia del juego
/// </summary>
public class PersistenceManager : Singleton<PersistenceManager>
{
    // Attributes
    #region Attributes
    static string fileName;
    public ProfileData profileData;
    /// <summary>
    /// Número máximo de perfiles a guardar
    /// </summary>
    public const int MAX_NUMBER_OF_PROFILES = 3;
    /// <summary>
    /// Número de perfil que ha sido actualmente cargado
    /// </summary>
    /// <value>The currently loaded profile number.</value>
    public int CurrentlyLoadedProfileNumber { get; private set; }
    /// <summary>
    /// Datos de perfil por defecto (se usa para iniciarlizar)
    /// </summary>
    private ProfileData profileDataDefault = new ProfileData();

    private string currentScene = "MainMenu";
    #endregion

    // Properties
    #region Properties
    public ProfileData ProfileDataDefault
    {
        get { return profileDataDefault; }
        set { profileDataDefault = value; }
    }
    #endregion

    #region Constructor
    protected PersistenceManager()
    {
        base.uniqueToAllApp = true;
    }
    #endregion

    // Methods
    #region Methods
    public override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != currentScene && GameManager.Instance.GetGameState != GameState.InIntro)
            currentScene = scene.name;
    }

    private void Awake()
    {
        //profileData = profileDataDefault;
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
        }
    }

	public int GetProfileSessionId()
	{
		return profileData.profile_sessionid;
	}
	public void SetProfileSessionId(int profileSessionId)
	{
		profileData.profile_sessionid = profileSessionId;
	}

    public int GetProfileUserId()
	{
		return profileData.profile_userid;
	}
	public void SetProfileUserId(int profile_userid)
	{
		profileData.profile_userid = profile_userid;
	}


    /// <summary>
    /// Retorna la cantidad de datos de perfil guardados (dentro del directorio especificado)
    /// este valor debe ser igual o menor al valor de MAX_NUMBER_OF_PROFILES
    /// </summary>
    /// <returns>la cantidad de daros de perfil de juego guardados</returns>
    public int GetDataProfilesSaved()
    {
        VerifyDirectory();
        return Directory.GetFiles(PersistenceHelp.GetProfile_Data_Directory_Path()).Length;
    }

    /// <summary>
    /// Verifica si el directorio donde estaran almacenados los datos de perfil de juego existe
    /// </summary>
    void VerifyDirectory()
    {
        // si el directorio no existe lo crea
        if (!Directory.Exists(PersistenceHelp.GetProfile_Data_Directory_Path()))
            Directory.CreateDirectory(PersistenceHelp.GetProfile_Data_Directory_Path());
    }

    /// <summary>
    /// Retorna la ruta completa para identificar los datos de perfil de juego guardados
    /// </summary>
    /// <returns>ruta completa de los daros de perfil de juego</returns>
    /// <param name="profileNumber">el número de perfil a obtener, va de 1 a menor o igual a MAX_NUMBER_OF_PROFILES</param>
    public string GetProfileDataFilePath(int profileNumber)
    {
        // Verifica que profileNumber sea mayor a 0
        if (profileNumber < 1)
            throw new TsiunasException("profileNumber debe ser mas grande que 1; profileNumber es: " + profileNumber, true, "PERSISTENCIA_PERFILES", "Eduardo Andrade");

        VerifyDirectory();
        // Regresa la ruta completa para identificar los datos de perfil de juego guardados
        // ej. /Users/Eduardo/Library/Application Support/Freyja Dev/Tsiunas/profiles/profileData1.gd
        return PersistenceHelp.GetProfile_Data_Directory_Path() + PersistenceHelp.PROFILE_DATA_FILE_NAME + profileNumber.ToString() + PersistenceHelp.PROFILE_DATA_FILE_EXTENSION;
    }

    /// <summary>
    /// Sirve para guardar los datos de perfil de juego actual
    /// </summary>
    /// <param name="_profileDataToSave">Datos de perfil de juego a guardar</param>
    public void SaveProfileData(ProfileData _profileDataToSave)
    {


        // Se realiza comprobación de los daros de perfil de juego guardado
        // si no existen se crean, si no se sobreescriben
        if (CurrentlyLoadedProfileNumber <= 0)
        {
            for (int i = 1; i <= MAX_NUMBER_OF_PROFILES; i++)
            {
                if (!File.Exists(GetProfileDataFilePath(i)))
                {
                    CurrentlyLoadedProfileNumber = i;
                    break;
                }
            }
        }
        if (CurrentlyLoadedProfileNumber <= 0) CurrentlyLoadedProfileNumber = 1;
        

        if (CurrentlyLoadedProfileNumber > 0)
        {
            profileData = Save(GetProfileDataFilePath(CurrentlyLoadedProfileNumber), _profileDataToSave) ? _profileDataToSave : null;
        }
    }

    /// <summary>
    /// Sirve para cargar los datos de perfil de juego guardados
    /// </summary>
    /// <param name="_profileNumber">el número de perfil a obtener, va de 1 a menor o igual a MAX_NUMBER_OF_PROFILES</param>
    public void LoadProfileData(int _profileNumber)
    {
        if (_profileNumber == CurrentlyLoadedProfileNumber)
            return;

        this.profileData = Load<ProfileData>(GetProfileDataFilePath(_profileNumber));
        CurrentlyLoadedProfileNumber = _profileNumber;
    }

    /// <summary>
    /// Regresa los datos guardados en un archivo, dentro de una ruta específica 
    /// </summary>
    /// <returns>los datos deserializados.</returns>
    /// <param name="fileNameToLoad">Nombre del archivo que se creará</param>
    public T Load<T>(string fileNameToLoad) where T : class
    {
        if (File.Exists(fileNameToLoad))
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = File.Open(fileNameToLoad, FileMode.Open);
                T t = formatter.Deserialize(stream) as T;
                stream.Close();
                return t;
            }
            catch (TsiunasException e)
            {
                Debug.Log(e.Message);
                e.Tratar();
            }
        }
        return default(T);
    }

    /// <summary>
    /// Guarda datos en un archivo, dentro de una ruta específica
    /// </summary>
    /// <param name="filePathToSave">Nombre del archivo a guardar.</param>
    /// <param name="dataToSave">datos a guardar</param>
    public bool Save<T>(string filePathToSave, T dataToSave) where T : class
    {
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = File.Open(filePathToSave, FileMode.OpenOrCreate);
            formatter.Serialize(stream, dataToSave);
            stream.Close();
            Debug.Log("PersistenceManager - Datos de perfil guardados con éxito");
            return true;
        }
        catch (TsiunasException ex)
        {
            ex.Tratar();
            return false;
        }
    }

    public static void Delete(string fileNameToDelete)
    {
#if UNITY_IPHONE || UNITY_ANDROID
        fileName = Application.persistentDataPath + "/" + fileNameToDelete;
#else
        fileName = Application.dataPath + "/" + fileNameToDelete;
#endif
        File.Delete(fileName);
    }

    /// <summary>
    /// Elimina un archivo de perfil guardado en el directorio de persistencia
    /// </summary>
    /// <param name="profileNumber">número de perfil a borrar.</param>
    public void DeleteFileProfile(int profileNumber) {
        if (Directory.Exists(PersistenceHelp.GetProfile_Data_Directory_Path()))
        {
            string fileProfileSavedPath = GetProfileDataFilePath(profileNumber);
            if (File.Exists(fileProfileSavedPath))
            {
                File.Delete(fileProfileSavedPath);
            }
        }
    }

    public static void DeleteAllFilesProfile() {
        if (Directory.Exists(PersistenceHelp.GetProfile_Data_Directory_Path()))
        {
            var files = Directory.GetFiles(PersistenceHelp.GetProfile_Data_Directory_Path());

            if (files.Length == 0)
            {
                Debug.Log("No hay archivos en la ruta: " + PersistenceHelp.GetProfile_Data_Directory_Path() + "que eliminar");
                return;
            }

            for (int i = 0; i < files.Length; i++)
            {
                File.Delete(files[i]);
            }

            Debug.Log(files.Length + (files.Length == 1 ? " archivo" : " archivos") + " en la ruta: " + PersistenceHelp.GetProfile_Data_Directory_Path() + (files.Length == 1 ? " fue eliminado" : " fueron eliminados") + " correctamente");
        }
    }
    #endregion

    public void GuardarPerfil()
    {
        Debug.Log("PersistenceManager - Guardando Perfil");

        profileData = ProfileData.Crear();
        if (profileData == null)
            return;
        SaveProfileData(profileData);
    }


    public void PerformProfileDataLoading(Action<ProfileData> action) {
        if (this.profileData != null)
            if (action != null)
            action(this.profileData);
    }
}

[Serializable]
public class ProfileData
{
    [NonSerialized]
    public bool isDefaultData;
    
    public ObservedList<Tool> profile_Tools;
    
    public ObservedList<Seed> profile_Seeds;
    
    public ObservedList<Fruit> profile_Fruits;
    
    public ObservedList<Food> profile_Foods;
    
    public GameState profile_GameState;
    
    public string profile_namePJ;

    public int profile_userid;

    public int profile_sessionid;
    
    public int profile_Money;
    
    public int profile_HarmonyFlames;
    
    public int profile_IntensityHarmonyFlames;
    
    public List<ToolTarget.ToolTargetToSave> profile_FarmState;
    
    public GameTime profile_GameTime;
    
    public int profile_Hungry;
    
    public List<PNJActor.PNJActorDatos> profile_PNJActores;

    public Dictionary<string, bool> profile_flags;
    //TODO: Guardar PlaceFlags

    /*
    public ProfileData(bool isDefaultData) {
        this.isDefaultData = isDefaultData;
        this.profile_Tools = GameManager.Instance.tools;
        this.profile_Seeds = GameManager.Instance.seeds;
        this.profile_Fruits = GameManager.Instance.fruits;
        this.profile_Foods = GameManager.Instance.foods;
        this.profile_GameState = GameManager.Instance.GetGameState;
        this.profile_namePJ = GameManager.Instance.GetNamePJ;
        this.profile_Money = GameManager.Instance.Money;
        this.profile_HarmonyFlames = HarmonyFlamesManager.Instance.HarmonyFlames;
        this.profile_IntensityHarmonyFlames = HarmonyFlamesManager.Instance.IntensityHarmonyFlames;
        this.profile_FarmState = PersistentFarm.Load();
        this.profile_GameTime = TimeManager.Instance.gameTime;
        this.profile_Hungry = HungerManager.Instance.Hungry;
        this.profile_PNJActores = GestorPNJ.Instance.PnjActoresDatos;
    }*/

    public ProfileData()
    {
        this.isDefaultData = true;
        this.profile_Tools = new ObservedList<Tool>();
        this.profile_Seeds = new ObservedList<Seed>();
        this.profile_Fruits = new ObservedList<Fruit>();
        this.profile_Foods = new ObservedList<Food>();
        this.profile_GameState = GameState.InIntro;
        this.profile_namePJ = "";
        this.profile_userid = -1;
        this.profile_sessionid = -1;
        this.profile_Money = 20;
        this.profile_HarmonyFlames = 0;
        this.profile_IntensityHarmonyFlames = 100;
        this.profile_FarmState = new List<ToolTarget.ToolTargetToSave>();
        this.profile_GameTime = new GameTime(0, 0, 0, 0);
        this.profile_Hungry = 100;
        this.profile_PNJActores = new List<PNJActor.PNJActorDatos>();
        
    }


    


    internal static ProfileData Crear()
    {
        ProfileData pd = new ProfileData();
        
        GameManager.Instance.tools.ForEach((x) => pd.profile_Tools.Add(x));
        GameManager.Instance.foods.ForEach((x) => pd.profile_Foods.Add(x));
        GameManager.Instance.seeds.ForEach((x) => pd.profile_Seeds.Add(x));
        GameManager.Instance.fruits.ForEach((x) => pd.profile_Fruits.Add(x));

        pd.isDefaultData = false;

        pd.profile_GameState = GameManager.Instance.GetGameState;
        pd.profile_namePJ = GameManager.Instance.GetNamePJ;
        pd.profile_Money = GameManager.Instance.Money;
        pd.profile_HarmonyFlames = HarmonyFlamesManager.Instance.HarmonyFlames;
        pd.profile_IntensityHarmonyFlames = HarmonyFlamesManager.Instance.IntensityHarmonyFlames;
        pd.profile_FarmState = PersistentFarm.tooltargetsGuardados;
        pd.profile_GameTime = TimeManager.Instance.GetGameTime();
        pd.profile_sessionid=PersistenceManager.Instance.GetProfileSessionId ();
        pd.profile_userid=PersistenceManager.Instance.GetProfileUserId ();
        pd.profile_Hungry = HungerManager.Instance.Hungry;
        pd.profile_PNJActores = GestorPNJ.Instance.PnjActoresDatos;
        pd.profile_flags = PlaceFlags.Instance.Flags;


        return pd;
    }
}