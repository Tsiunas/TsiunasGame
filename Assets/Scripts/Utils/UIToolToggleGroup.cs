using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIToolToggleGroup : ToggleGroup {

    // Attributes
    #region Attributes
    public delegate void OnChangeToggleGroup(Toggle active);
    public event OnChangeToggleGroup onChange;
    #endregion	

    // Methods
    #region Methods
    /// <summary>
    /// Obtiene todos los toggles que sean objetos hijos
    /// Añade una implementación a cada evento: onValueChanged
    /// </summary>
    public void GetAllToggles()
    {
        foreach (Transform transformToggle in transform)
        {
            Toggle toggle = transformToggle.gameObject.GetComponent<Toggle>();
            if (toggle != null) {
                toggle.onValueChanged.AddListener((isSelected) =>
                {
                    if (!isSelected)
                    {
                        toggle.interactable = !isSelected;
                        return;
                    }
                    Toggle activeToggle = Active();
                    DoOnChange(activeToggle);
                });
            }
        }
    }
    public Toggle Active()
    {
        return ActiveToggles().FirstOrDefault();
    }

    protected virtual void DoOnChange(Toggle newActive)
    {
        if (onChange != null) onChange(newActive);
    }
    #endregion
}
