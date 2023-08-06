using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameElement {
    // Attributes
    #region Attributes
    private string name;
    [SerializeField]
    private int durability;
    public delegate void OnFinishedDurability(GameElement thisGameElement);
    public event OnFinishedDurability finishedDurability;
    private int filledDurability;
    #endregion

    // Properties
    #region Properties
    public string _name
    {   get { return name; }
        set { name = value; }
    }

    public int _durability
    {
        get { return durability; }
        set { durability = value; }
    }

    public int _filledDurability {
        get { return filledDurability; }
        set { filledDurability = value; }
    }
    #endregion

    public void FinishedDurability() {
        if (finishedDurability != null)
            finishedDurability(this);
    }

    /// <summary>
    /// Se usa para ir restando la durabilidad de ésta herramienta
    /// </summary>
    public void DecreaseDurability()
    {
        // Resta en 1 la durabilidad
        if (_durability > 0)
            _durability--;
        
        if (_durability == 0)
            FinishedDurability();
    }
}
