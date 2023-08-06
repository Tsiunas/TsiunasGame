using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tsiunas.Mechanics;
using UnityEngine.UI;
using System;

[System.Serializable]
public class UIToolInventory : UIInventory {

    private UIToolToggleGroup toggleGroup;
    public Image activeTool;
    public Text textNameActiveTool;
    private List<Toggle> toggles = new List<Toggle>();
    public Slider durabilityBar;
    public Text percentText;

    public List<Toggle> Toggles
    {   get { return toggles; }
        set { toggles = value; }
    }

    public override void Awake()
    {
        toggleGroup = scrollContent.gameObject.AddComponent<UIToolToggleGroup>();
        toggleGroup.allowSwitchOff = true;

        base.Awake();
        toggleGroup.onChange += EventOnChangeToggleActive;


        
        GameManager.Instance.tools.OnUpdated += OnUpdatedToolList;
        

        GameManager.Instance.tools.ForEach(x => x.OnDurabilityChange += (int currentDurability) => {
            SetDurabilityValueBar(currentDurability, x._filledDurability);
        });
    }

    
    private void SetDurabilityValueBar(int currentDurability, int filledDurability) {
        float percent = ((float)currentDurability / filledDurability);
        durabilityBar.value = percent;
        float percentTxt = ((float)percent * 100);
        percentText.text = percentTxt <= 0 ? "" : percentTxt.ToString() + "%";
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        toggleGroup.onChange -= EventOnChangeToggleActive;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.tools.OnUpdated -= OnUpdatedToolList;
            GameManager.Instance.tools.ForEach(x => x.ClearEventOnDurabilityChange());
        }
    }

    void OnUpdatedToolList(Tool arg1, bool arg2)
    {
        // El Sprite actual de la herramienta actual se quita
        activeTool.sprite = null;
        // Se quita referencia a la herramienta actual activa (se deselleciona la herramienta que estaba activada)
        GameManager.Instance.currentToolActive = null;
        // El texto del nombre en español de cada herramienta se deja vacío 
        textNameActiveTool.text = "";
        percentText.text = "";

        // se desactivan todos los toggles que contiene el toggle Group
        toggleGroup.SetAllTogglesOff();

        if (!arg2)
        {
            
            if (GetUIGameElement(arg1) != null) {
                toggles.Remove(GetUIGameElement(arg1).GetComponent<Toggle>());
            }

            SoundManager.PlayHerramientaAcabada();
            OpenPanelFromOtherGO();
        }
    }

    public override UIElementoJuego CreateUIGO(GameElement _gameElement)
    {
        // Como los objetos creados son a su vez Toggles se agregan a un Toogle Group

        UIElementoJuego ui = base.CreateUIGO(_gameElement);
        ui.icon.sprite = TexturesManager.Instance.GetSpriteFromSpriteSheet((_gameElement as Tool).Gold ? _gameElement._name + "_Gold" : _gameElement._name);

        ui.GetComponent<Toggle>().group = toggleGroup;

        Toggle toggle = ui.GetComponent<Toggle>();

        toggleGroup.GetAllToggles();
        toggles.Add(toggle);

        return ui;
    }

    public override UIElementoJuego SetSpriteTool(UIElementoJuego ui, GameElement _gameElement)
    {
        UIElementoJuego _ui = base.SetSpriteTool(ui, _gameElement);
        if (_ui != null)
            _ui.icon.sprite = TexturesManager.Instance.GetSpriteFromSpriteSheet((_gameElement as Tool).Gold ? _gameElement._name + "_Gold" : _gameElement._name);
        return _ui;
    }

    void EventOnChangeToggleActive(Toggle active)
    {
        activeTool.type = Image.Type.Simple;
        activeTool.preserveAspect = true;
        // El sprite de la herramienta actual se establece en base al sprite de la herramienta seleccionada
        activeTool.sprite = active.GetComponent<UIHerramienta>().icon.sprite;
        // Se almacena la herrmienta actual activa
        GameManager.Instance.currentToolActive = GameManager.Instance.GetTool(active.GetComponent<UIHerramienta>().toolType);
        // Se establece el nombre de la herramienta activa actualmente (EN ESPAÑOL)
       
        textNameActiveTool.text = SetTextOFCurrentActiveTool(active.GetComponent<UIHerramienta>().toolType);

        SetDurabilityValueBar(GameManager.Instance.currentToolActive.Durability, GameManager.Instance.currentToolActive._filledDurability);

        foreach (Transform item in toggleGroup.transform)
        {
            // El toggle de la herramienta seleccionada se ocultará (los demmás no)
            Toggle t = item.GetComponent<Toggle>();
            if (t != null)
                t.interactable = (t != active);
        }

        ClosePanel();
    }

    string SetTextOFCurrentActiveTool(ToolType type)
    {
        string strToReturn = "";
         TrackerSystem.Instance.SendTrackingData("user", "selected", "tool", type+"|user|éxito");
        switch (type)
        {
            case ToolType.Axe:
                strToReturn = ToolHelp.SpanishNames.Axe;
                break;
            case ToolType.Hammer:
                strToReturn = ToolHelp.SpanishNames.Hammer;
                break;
            case ToolType.Hoe:
                strToReturn = ToolHelp.SpanishNames.Hoe;
                break;
            case ToolType.Machete:
                strToReturn = ToolHelp.SpanishNames.Machete;
                break;
            case ToolType.Watering_Can:
                strToReturn = ToolHelp.SpanishNames.Watering_can;
                break;
        }
        return strToReturn;
    }
}
