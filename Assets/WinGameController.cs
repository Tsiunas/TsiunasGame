using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGameController : MonoBehaviour {

    void Awake () {
        HarmonyFlamesManager.Instance.OnJuegoGanado += Instance_OnJuegoGanado;
	}

    void Instance_OnJuegoGanado()
    {
        
    }

    private void OnDestroy()
    {
        if (HarmonyFlamesManager.Instance != null)
            HarmonyFlamesManager.Instance.OnJuegoGanado -= Instance_OnJuegoGanado;
    }
}
