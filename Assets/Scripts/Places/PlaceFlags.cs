using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceFlags : SimpleSingleton<PlaceFlags>
{
    Dictionary<string, bool> flags;
    public static readonly string INICIAR_PRIMER_HITO = "INICIAR_PRIMER_HITO";
    public static readonly string YA_EXPLICO_HAMBRE = "YA_EXPLICO_HAMBRE";
    internal static readonly string INICIAR_SEGUNDO_HITO = "INICIAR_SEGUNDO_HITO";
    internal static readonly string PONER_BANDERA = "PONER_BANDERA";
    internal static readonly string SEGUNDO_HITO_FINALIZADO = "SEGUNDO_HITO_FINALIZADO";
    internal static readonly string INICIAR_HITO_AGUA = "INICIAR_HITO_AGUA";
    internal static readonly string YA_HABLO_CON_MARGARITA = "YA_HABLO_CON_MARGARITA";
    public static readonly string YA_EXPLICO_INTENSIDAD_FA = "YA_EXPLICO_INTENSIDAD_FA";

    public Dictionary<string, bool> Flags { get { return flags; }}

    public PlaceFlags()
    {
        flags = new Dictionary<string, bool>();
        PersistenceManager.Instance.PerformProfileDataLoading((pd) =>
        {
            if(pd.profile_flags != null && pd.profile_flags.Count > 0)
                this.flags = new Dictionary<string, bool>(pd.profile_flags);
        });
    }

    public bool IsTrue(string key)
    {
        if (!flags.ContainsKey(key)) return false;
        return flags[key];
    }

    public void AddOrEdit(string key, bool value)
    {
        if (flags.ContainsKey(key)) flags[key] = value;
        else flags.Add(key, value);
    }

    public void RaiseFlag(string key)
    {
        AddOrEdit(key, true);
    }
}
