using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Tsiunas.Mechanics
{
    public enum TypeInventory
    {
        TOOL,
        FOOD,
        SEED,
        FRUIT
    }

    [System.Serializable]
    public class UIInventory : MonoBehaviour
    {
        public enum Side { LEFT, RIGHT }
        public Transform scrollContent;
        public TypeInventory typeInventory;

        public Button buttonControlPanel;
        private bool isPanelOpen;
        private RectTransform rectArrowButtonControlPanel;
        private RectTransform rectTranformScrollContent;
        public Side side;


        public virtual void Awake()
        {
            switch (typeInventory)
            {
                case TypeInventory.TOOL:
                    GameManager.Instance.tools.OnUpdated += UpdateGameElementsList;
                    GameManager.Instance.tools.OnEmpty += ClosePanel;
                    SetupGameElementsInScrollView(GameManager.Instance.tools.Cast<GameElement>().ToList());
                    break;
                case TypeInventory.FOOD:
                    GameManager.Instance.foods.OnUpdated += UpdateGameElementsList;
                    GameManager.Instance.foods.OnEmpty += ClosePanel;
                    SetupGameElementsInScrollView(GameManager.Instance.foods.Cast<GameElement>().ToList());
                    break;
                case TypeInventory.SEED:
                    GameManager.Instance.seeds.OnUpdated += UpdateGameElementsList;
                    GameManager.Instance.seeds.OnEmpty += ClosePanel;
                    SetupGameElementsInScrollView(GameManager.Instance.seeds.Cast<GameElement>().ToList());
                    break;
                case TypeInventory.FRUIT:
                    GameManager.Instance.fruits.OnUpdated += UpdateGameElementsList;
                    GameManager.Instance.fruits.OnEmpty += ClosePanel;
                    SetupGameElementsInScrollView(GameManager.Instance.fruits.Cast<GameElement>().ToList());
                    break;
            }

            try
            {
                rectTranformScrollContent = scrollContent.GetComponent<RectTransform>();
                rectArrowButtonControlPanel = rectTranformScrollContent.transform.GetChild(0).GetComponentInChildren<RectTransform>();
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error: " + ex.Message);
                throw ex;
            }

            buttonControlPanel.onClick.AddListener(ChangeStatePanel);
        }

        protected void ClosePanel() { if (isPanelOpen) ChangeStatePanel(); }
        private void OpenPanel() { isPanelOpen = false; ChangeStatePanel(); }

        public void ClosePanelFromOtherGO () { ClosePanel(); }
        public void OpenPanelFromOtherGO() { OpenPanel(); }
        

        public virtual void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                switch (typeInventory)
                {
                    case TypeInventory.TOOL:
                        GameManager.Instance.tools.OnUpdated -= UpdateGameElementsList;
                        GameManager.Instance.tools.OnEmpty -= ClosePanel;
                        break;
                    case TypeInventory.FOOD:
                        GameManager.Instance.foods.OnUpdated -= UpdateGameElementsList;
                        GameManager.Instance.foods.OnEmpty -= ClosePanel;
                        break;
                    case TypeInventory.SEED:
                        GameManager.Instance.seeds.OnUpdated -= UpdateGameElementsList;
                        GameManager.Instance.seeds.OnEmpty -= ClosePanel;
                        break;
                    case TypeInventory.FRUIT:
                        GameManager.Instance.fruits.OnUpdated -= UpdateGameElementsList;
                        GameManager.Instance.fruits.OnEmpty -= ClosePanel;
                        break;
                }
            }
            buttonControlPanel.onClick.RemoveAllListeners();
        }

        private void ChangeStatePanel() {
            isPanelOpen = !isPanelOpen;
            StartCoroutine(ControlPanel(isPanelOpen));
        }

        private void Start()
        {
            StartCoroutine(ControlPanel(isPanelOpen));
        }

        public IEnumerator ControlPanel(bool open)
        {
            yield return new WaitForEndOfFrame();
            // Cambia el campo anchoredPosition dependiendo del booleano open que se pasa como parámetro
            float anchoredPosXScrollContent = side == Side.LEFT ? -rectTranformScrollContent.sizeDelta.x : rectTranformScrollContent.sizeDelta.x;
            rectTranformScrollContent.anchoredPosition = !open ? new Vector2(anchoredPosXScrollContent, 0) : Vector2.zero;

            float anchoredPosx = side == Side.LEFT ? rectArrowButtonControlPanel.sizeDelta.x : -rectArrowButtonControlPanel.sizeDelta.x;
            rectArrowButtonControlPanel.anchoredPosition = !open ? new Vector2(anchoredPosx, 0) : Vector2.zero;

            rectArrowButtonControlPanel.localScale = open ? Vector3.one : new Vector3(-1, 1, 1);
        }

        #region SetupGameElementsInScrollView
        void SetupGameElementsInScrollView(List<GameElement> _gameElementList)
        {
            // Lista temporal de elementos de juego
            List<GameElement> tempGameElements = new List<GameElement>();

            if (_gameElementList != null)
            {
                // Se recorren todas los elementos de juego que se tengan en la lista pasada como parámetro
                for (int i = _gameElementList.Count - 1; i >= 0; i--)
                {
                    // Solo se añade a la lista temporal de herramientas 1 por cada tipo de herramienta
                    // ej. puedo tener 5 martillos pero a esta lista temporal solo se añade 1

                    if (_gameElementList[i] is Food)
                    {
                        if (!tempGameElements.Any(gameElementIntoList => ((Food)gameElementIntoList).foodType == ((Food)_gameElementList[i]).foodType))
                            tempGameElements.Add(_gameElementList[i]);
                    }

                    if (_gameElementList[i] is Tool)
                    {
                        if (!tempGameElements.Any(gameElementIntoList => ((Tool)gameElementIntoList).ToolType == ((Tool)_gameElementList[i]).ToolType))
                            tempGameElements.Add(_gameElementList[i]);
                    }

                    if (_gameElementList[i] is Seed)
                    {
                        if (!tempGameElements.Any(gameElementIntoList => ((Seed)gameElementIntoList).SeedType == ((Seed)_gameElementList[i]).SeedType))
                            tempGameElements.Add(_gameElementList[i]);
                    }

                    if (_gameElementList[i] is Fruit)
                    {
                        if (!tempGameElements.Any(gameElementIntoList => ((Fruit)gameElementIntoList).fruitType == ((Fruit)_gameElementList[i]).fruitType))
                            tempGameElements.Add(_gameElementList[i]);
                    }
                }
            }
            else {
                throw new System.Exception("Lista vacia");
            }

            // Se recorre la lista temporal de herramientas
            for (int t = 0; t < tempGameElements.Count; t++)
            {
                CreateUIGO(tempGameElements[t]);
            }
        }
        #endregion

        void UpdateGameElementsList(GameElement gameElementUpdated, bool addedOrRemoved)
        {
            // Si se añadío una herramienta
            if (addedOrRemoved)
            {
                OpenPanel();
                if (!UIGameElementExists(gameElementUpdated))
                    CreateUIGO(gameElementUpdated);
                else
                {
                    UIElementoJuego uiElementoJuego = GetUIGameElement(gameElementUpdated);
                    uiElementoJuego.textAmount.text = GetAmountOfGameElement(gameElementUpdated._name, GameManager.Instance.ReturnListOfGameElements(gameElementUpdated)).ToString();
                    if (gameElementUpdated.GetType().BaseType == typeof(Tool))
                        SetSpriteTool(uiElementoJuego, GameManager.Instance.GetTool((gameElementUpdated as Tool).ToolType));
                }
            }
            // Si se eliminó una herramienta
            else
            {
                if (GetAmountOfGameElement(gameElementUpdated._name, GameManager.Instance.ReturnListOfGameElements(gameElementUpdated)) <= 0)
                {
                    Destroy(GetUIGameElement(gameElementUpdated).gameObject);
                    GetUIGameElement(gameElementUpdated);
                }
                else
                {
                    if (GetUIGameElement(gameElementUpdated) != null)
                    {
                        UIElementoJuego uiElementoJuego = GetUIGameElement(gameElementUpdated);
                        uiElementoJuego.textAmount.text = GetAmountOfGameElement(gameElementUpdated._name, GameManager.Instance.ReturnListOfGameElements(gameElementUpdated)).ToString();
                        if (gameElementUpdated.GetType().BaseType == typeof(Tool))
                            SetSpriteTool(uiElementoJuego, GameManager.Instance.GetTool((gameElementUpdated as Tool).ToolType));
                    }
                    else
                        Debug.LogError("No hay un " + gameElementUpdated._name + "en la lista");
                }
            }
        }

        public bool UIGameElementExists(GameElement _gameElement)
        {
            Transform gameELementFinded = scrollContent.transform.Find(_gameElement._name);
            if (gameELementFinded == null)
                return false;
            else
                return true;
            //return gameELementFinded != null ? true : false;
        }

        public virtual UIElementoJuego SetSpriteTool(UIElementoJuego ui,  GameElement _gameElement) {
            return ui;
        }

        public UIElementoJuego GetUIGameElement(GameElement _gameElement)
        {
            Transform _UIGameELementFinded = scrollContent.transform.Find(_gameElement._name);
            return _UIGameELementFinded != null ? _UIGameELementFinded.GetComponent<UIElementoJuego>() : null;
        }

        public virtual UIElementoJuego CreateUIGO(GameElement _gameElement)
        {
            // Se crea un objeto de UI por cada herramienta
            GameObject go = (GameObject)Instantiate(GetUIPrefab(_gameElement));
            // Se obtiene el componente UITool
            UIElementoJuego uiGameElementCreated = go.GetComponent<UIElementoJuego>();           

            // Se configuran algunos campos con los valores del Objeto Tool
            uiGameElementCreated.name = _gameElement._name;
            //uiGameElementCreated.icon.sprite = Resources.Load<Sprite>(_gameElement.GetType().BaseType.Name + "s/" + _gameElement._name);
            uiGameElementCreated.icon.sprite = TexturesManager.Instance.GetSpriteFromSpriteSheet(_gameElement._name);

            SetTypeGameElement(uiGameElementCreated, _gameElement);
            uiGameElementCreated.textAmount.text = GetAmountOfGameElement(_gameElement._name, GameManager.Instance.ReturnListOfGameElements(_gameElement)).ToString();
            uiGameElementCreated.transform.SetParent(scrollContent);
            uiGameElementCreated.transform.localScale = Vector3.one;
           
            return uiGameElementCreated;
        }

        public GameObject GetUIPrefab(GameElement elementoJuego)
        {
            return Resources.Load<GameObject>("UI/UI" + elementoJuego.GetType().BaseType.Name);
        }
        
        public void SetTypeGameElement(UIElementoJuego elementoJuego, GameElement _gE)
        {
            if (elementoJuego is UIComida)
            {
                ((UIComida)elementoJuego).foodType = _gE is Food ? ((Food)_gE).foodType : FoodType.None;
            }
            if (elementoJuego is UIHerramienta)
            {
                ((UIHerramienta)elementoJuego).toolType = _gE is Tool ? ((Tool)_gE).ToolType : ToolType.None;
            }

            if (elementoJuego is UIFruto)
            {
                ((UIFruto)elementoJuego).fruitType = _gE is Fruit ? ((Fruit)_gE).fruitType : FruitType.None;
            }
            if (elementoJuego is UISemilla)
            {
                ((UISemilla)elementoJuego).seedType = _gE is Seed ? ((Seed)_gE).SeedType : SeedType.None;
            }
        }

        public int GetAmountOfGameElement(string name, List<GameElement> listToLoop)
        {
            int amount = 0;

            for (int i = 0; i < listToLoop.Count; i++)
            {
                if (listToLoop[i]._name == name) amount++;
            }
            return amount;
        }
    }
}

