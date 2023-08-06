using System.Collections;
using System.Collections.Generic;
using Tsiunas.Mechanics;
using UnityEngine;

public class MamaTuleFAStateAnim : MonoBehaviour {

    public SkinnedMeshRenderer skMeshRenderer;
    private IFAObserved fAObserved;
    private Material _material;
	// Use this for initialization
    void Awake () {
        try
        {
            fAObserved = HarmonyFlamesManager.Instance;
            fAObserved.OnFlamasChanged += OnFlamasChanged;
            _material = skMeshRenderer.material;
        }
        catch (TsiunasException e)
        {
            e.Tratar();
        }
    }

    void OnFlamasChanged(FAStates faState)
    {
        SetMamaTuleTexture(faState);
    }

    void SetMamaTuleTexture(FAStates state) {
        if (state == FAStates.Muerte)
            return;
        _material.mainTexture = GetTextureFAStateMamaTule(state.ToString());
    }

    public Texture GetTextureFAStateMamaTule(string nameOfState) {
        return Resources.Load<Texture>("TexturasMamatule/" + nameOfState);
    }

    private void OnDestroy()
    {
        if (fAObserved != null)
            fAObserved.OnFlamasChanged -= OnFlamasChanged;
    }
}
