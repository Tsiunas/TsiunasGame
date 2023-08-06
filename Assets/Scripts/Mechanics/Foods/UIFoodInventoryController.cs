using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public class UIFoodInventoryController : MonoBehaviour {
    public GameObject prefabFood;
    public Transform scrollContent;
    private RectTransform rectArrowButtonManipulateFoodPanel;
    private RectTransform rectTranformScrollContent;
    public Button btnManipulateToolPanel;
    private bool foodPanelOpen;
    private ObservedList<UIFood> uiFoods = new ObservedList<UIFood>();

    private void Awake()
    {
        rectTranformScrollContent = scrollContent.GetComponent<RectTransform>();
        rectArrowButtonManipulateFoodPanel = btnManipulateToolPanel.transform.GetChild(0).GetComponentInChildren<RectTransform>();
        // Añade una función anónima para manipular el estado del panel de herramientas (Abierto / Cerrado)
        btnManipulateToolPanel.onClick.AddListener(delegate {
            foodPanelOpen = !foodPanelOpen;
            StartCoroutine(ManipulateToolPanel(foodPanelOpen));
        });

        GameManager.Instance.foods.OnUpdated += UpdateFoodList;
    }

    void UpdateFoodList(Food foodUpdatedIntoList, bool addOrRemove)
    {
        // Si se añadío una herramienta
        if (addOrRemove)
        {
            
            if (!UIFoodExists(foodUpdatedIntoList))
                uiFoods.Add(CreateNewUITool(foodUpdatedIntoList));
            else
                GetUIFood(foodUpdatedIntoList).textAmount.text = GetAmountOfFoods(foodUpdatedIntoList.GetName, GameManager.Instance.foods).ToString();
        }
        // Si se eliminó una herramienta
        else
        {
            

            if (GetAmountOfFoods(foodUpdatedIntoList.GetName, GameManager.Instance.foods) <= 0)
            {
                Destroy(GetUIFood(foodUpdatedIntoList).gameObject);
                uiFoods.Remove(GetUIFood(foodUpdatedIntoList));
            }
            else
            {
                if (GetUIFood(foodUpdatedIntoList) != null)
                    GetUIFood(foodUpdatedIntoList).textAmount.text = GetAmountOfFoods(foodUpdatedIntoList.GetName, GameManager.Instance.foods).ToString();
                else
                    Debug.LogError("No hay un " + foodUpdatedIntoList.GetName + "en la lista");
            }
        }
    }

    public UIFood CreateNewUITool(Food _food)
    {
        // Se crea un objeto de UI por cada herramienta
        GameObject food = (GameObject)Instantiate(prefabFood);
        // Se obtiene el componente UITool
        UIFood newFood = food.GetComponent<UIFood>();

        // Se configuran algunos campos con los valores del Objeto Tool
        newFood.name = _food.GetName;
        newFood.iconFood.sprite = Resources.Load<Sprite>("Foods/" + _food.GetName);
        newFood.foodType = _food.foodType;
        newFood.textAmount.text = GetAmountOfFoods(_food.GetName, GameManager.Instance.foods).ToString();
        newFood.transform.SetParent(scrollContent);
        newFood.transform.localScale = Vector3.one;
        return newFood;
    }



    public bool UIFoodExists(Food uiFood)
    {
        Transform foodFinded = scrollContent.transform.Find(uiFood.GetName);
        return foodFinded != null ? true : false;
    }

    public UIFood GetUIFood(Food tool)
    {
        Transform foodFinded = scrollContent.transform.Find(tool.GetName);
        return foodFinded != null ? foodFinded.GetComponent<UIFood>() : null;
    }

    private void Start()
    {
        SetupFoodsInScrollView();
        StartCoroutine(ManipulateToolPanel(foodPanelOpen));
    }

    /// <summary>
    /// Sirve para manipular el panel de herramientas (Abrir / Cerrar)
    /// </summary>
    /// <param name="open">Si se setea a <c>true</c> el panel se abrira.</param>
    public IEnumerator ManipulateToolPanel(bool open)
    {
        yield return new WaitForEndOfFrame();
        // Cambia el campo anchoredPosition dependiendo del booleano open que se pasa como parámetro
        rectTranformScrollContent.anchoredPosition = !open ? new Vector2(-rectTranformScrollContent.sizeDelta.x, 0) : Vector2.zero;
        rectArrowButtonManipulateFoodPanel.localScale = open ? new Vector3(-1, 1, 1) : Vector3.one;
    }

    // Methods
    #region Methods
    /// <summary>
    /// Configura las comidas a mostrar en el inventario de la UI, dependiendo de las comidas que se tengan
    /// </summary>
    void SetupFoodsInScrollView()
    {
        // Lista temporal de herramientas
        List<Food> tempFoods = new List<Food>();

        // Se recorren todas las herramientas que se tengan
        for (int i = 0; i < GameManager.Instance.foods.Count; i++)
        {
            if (!tempFoods.Any(foodIntoList => foodIntoList.foodType == GameManager.Instance.foods[i].foodType))
            {
                // Solo se añade a la lista temporal de herramientas 1 por cada tipo de herramienta
                // ej. puedo tener 5 martillos pero a esta lista temporal solo se añade 1
                tempFoods.Add(GameManager.Instance.foods[i]);
            }
        }

        // Se recorre la lista temporal de herramientas
        for (int t = 0; t < tempFoods.Count; t++)
        {
            CreateUIFood(tempFoods[t]);
        }

        tempFoods = new List<Food>();
    }

    public UIFood CreateUIFood(Food _food)
    {
        // Se crea un objeto de UI por cada herramienta
        GameObject food = (GameObject)Instantiate(prefabFood);
        // Se obtiene el componente UITool
        UIFood newFood = food.GetComponent<UIFood>();

        // Se configuran algunos campos con los valores del Objeto Tool
        newFood.name = _food.GetName;
        newFood.iconFood.sprite = Resources.Load<Sprite>("Foods/" + _food.GetName);
        newFood.foodType = _food.foodType;
        newFood.textAmount.text = GetAmountOfFoods(_food.GetName, GameManager.Instance.foods).ToString();
        newFood.transform.SetParent(scrollContent);
        newFood.transform.localScale = Vector3.one;

        return newFood;
    }

    /// <summary>
    /// Obtiene la cantidad de herramientas pertenecientes a un mismo tipo
    /// </summary>
    /// <returns>La cantidad de herramientas</returns>
    /// <param name="name">Tipo de la herramienta de la cual se quiere saber su cantidad</param>
    /// <param name="listToLoop">Lista de las herramientas en la que se pretende hacer la busqueda</param>
    public int GetAmountOfFoods(string name, List<Food> listToLoop)
    {
        int amount = 0;

        for (int i = 0; i < listToLoop.Count; i++)
        {
            if (listToLoop[i].GetName == name) amount++;
        }
        return amount;
    }
    #endregion
}
