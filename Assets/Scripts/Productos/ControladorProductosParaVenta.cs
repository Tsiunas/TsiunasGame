using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tsiunas.Mechanics;
using UnityEngine;
using UnityEngine.UI;

public class ControladorProductosParaVenta : MonoBehaviour {

    public List<ProductoDatos> productos;
    public List<ProductoUI> productosUI;
    public GameObject prefabProductoUI;
    private GridLayoutGroup grid;
    public GameObject prefabPopupConfirmacionVenta;
    private PopupConfirmacionVenta popup;
    public Canvas contenedorPopup;
    public int probabilidadProductoOro;
    public int cantidadComidasEnLugar = 3;
    private List<ProductoDatos> _comidas;

    private void Awake()
    {
        try { grid = FindObjectOfType<GridLayoutGroup>(); }
        catch (System.Exception ex) { throw ex; }
    }

    public bool ObtenerProbabilidadOro()
    {
        return UnityEngine.Random.value <= (probabilidadProductoOro / 100f) ? true : false;
    }

    /// <summary>
    /// Carga los datos de las comidas (ScriptabelObjects desde los Assets)
    /// </summary>
    /// <param name="callback">Ejecuta una función una vez han sido cargados todos los datos de las comidas</param>
    private void CargarComidasDatos(Action callback = null) {
        _comidas = new List<ProductoDatos>(Resources.LoadAll<ProductoDatos>("Comidas"));
        if (callback != null)
            callback();
    }


    /// <summary>
    /// Regresa una lista de comidas aleatorias (en base a todas las comidas referenciadas desde los scriptableObjects)
    /// </summary>
    /// <returns>Una lista de comidas aleatorias, la cantidad es pasada como parámetro</returns>
    /// <param name="cantidadAObtener">Cantidad AO btener.</param>
    private List<ProductoDatos> ObtenerComidaAlAzar(int cantidadAObtener) {
        var comidasTemporal = _comidas.ToList();
        var listaADevolver = new List<ProductoDatos>(cantidadAObtener);
        var numerosAleatorios = new List<int>();

        int l = 0;
        while(l < cantidadAObtener) {
            listaADevolver.Add(comidasTemporal[ObtenerNumeroAleatorio(0, _comidas.Count, numerosAleatorios)]);
            l++;
        }

        return listaADevolver;
    }

    private int ObtenerNumeroAleatorio(int minimo, int maximo, List<int> numeroYaUsados)
    {
        int numeroAleatorio = UnityEngine.Random.Range(minimo, maximo);
        while (numeroYaUsados.Contains(numeroAleatorio)) { numeroAleatorio = UnityEngine.Random.Range(minimo, maximo); }
        numeroYaUsados.Add(numeroAleatorio);
        return numeroAleatorio;
    }

    private void Start()
    {
        // Una vez cargados todos los datos de las comidas, se crean al azar y se agregan a los productos (estos se encargan de crear los objetos de UI)
        CargarComidasDatos(() => { AgregarComidas(); });

        foreach (ProductoDatos prodcutoActual in productos)
        {
            productosUI.Add(ConfigurarDatosProductoUI(CrearProductoUI(), prodcutoActual));
        }

        ProductosUIAgregados();
    }

    public virtual void ProductosUIAgregados() { }

    public virtual void AgregarComidas() {
        ObtenerComidaAlAzar(cantidadComidasEnLugar).ForEach((ProductoDatos comidaActual) => { productos.Add(comidaActual); });
    }

    //AgregarProductoAlEstante( producto)

    public ProductoUI CrearProductoUI () {
        GameObject productoCreado = Instantiate(prefabProductoUI, grid.transform);
        return productoCreado.GetComponent<ProductoUI>();
    }

    public ProductoUI ConfigurarDatosProductoUI(ProductoUI productoUI, ProductoDatos datos) {
        productoUI.EstablecerImagen(datos.esProductoDeOro ? datos.imagenOro : datos.imagen);
        productoUI.EstalecerTextoPrecio(datos.esProductoDeOro ? datos.precio * 2 : datos.precio);
        productoUI.EstablecerTipoProducto(datos);
        productoUI.EstalecerTextoNombreProducto(datos.nombre);
        productoUI.EsProductoDeOro = datos.esProductoDeOro;
        productoUI.name = datos.name;

        productoUI.GetComponent<Button>().onClick.AddListener(delegate {
            TocarProducto(productoUI, datos);
        });
        return productoUI;
    }

    public virtual void TocarProducto(ProductoUI ui, ProductoDatos datos)
    {
        CrearYConfigurarPopupConfirmacionVenta(ui, datos);
    }

    public void CrearYConfigurarPopupConfirmacionVenta(ProductoUI ui, ProductoDatos datos) {
        GameObject popupCreado = Instantiate(prefabPopupConfirmacionVenta, contenedorPopup.transform);
        popup = popupCreado.GetComponent<PopupConfirmacionVenta>();
        popup.imagenProducto.sprite = ui.imagenProducto.sprite;
        popup.textoConfirmacionVenta.text = "¿Desea comprar <color=#a52a2aff>" + ui.Nombre + "</color> por <color=#808000ff>" + ui.Precio + "</color> " + (ui.Precio == 1 ? "moneda" : "monedas") + " de oro?";

        popup.botonSI.onClick.AddListener(delegate {
            switch (ui.tipoProducto)
            {
                case ProductoDatos.Tipo.FRUTO:

                    (StoreManager.BuyFruitCorn() ? new Action(
                        // Si se pudo realizar la compra (Si se tenía dinero suficiente)
                        () =>
                        {
                            popup.DestroyPopup();
                            ui.Vender();
                        }) :
                       // Si NO se pudo realizar la compra - ¡Feedback, dinero disponible actualmente!
                       () => popup.EstablecerTextoDineroDisponible(GameManager.Instance.Money)
                   )();
                    
                    break;
                case ProductoDatos.Tipo.HERRAMIENTA:
                    
                    (StoreManager.BuyTool(RegresarValorEnumDadoElNombre<TypesGameElement.Tools>(ui.name), ui.EsProductoDeOro) ? new Action(
                        // Si se pudo realizar la compra (Si se tenía dinero suficiente)
                        () =>
                        {
                            popup.DestroyPopup();
                            
                            ui.Vender(delegate {
                                bool esDeOro = ObtenerProbabilidadOro();
                                ui.EsProductoDeOro = esDeOro;
                                ui.RestablecerProductoUI(esDeOro ? datos.imagenOro : datos.imagen, esDeOro ? datos.precio * 2 : datos.precio);
                            });
                        }) :
                        // Si NO se pudo realizar la compra - ¡Feedback, dinero disponible actualmente!
                        () => popup.EstablecerTextoDineroDisponible(GameManager.Instance.Money)
                    )();
                    break;
                case ProductoDatos.Tipo.COMIDA:
                    
                    (StoreManager.BuyFood(RegresarValorEnumDadoElNombre<TypesGameElement.Foods>(ui.name)) ? new Action(
                        // Si se pudo realizar la compra (Si se tenía dinero suficiente)
                        () =>
                        {
                            popup.DestroyPopup();
                            ui.Vender();
                            //Comerse la comida automáticamente
                            HungerManager.EatFood(GameManager.Instance.GetFood(RegresarValorEnumDadoElNombre<FoodType>(ui.name)));
                            EatFoodFeedback(ui);

                        }) :
                        // Si NO se pudo realizar la compra - ¡Feedback, dinero disponible actualmente!
                       () => popup.EstablecerTextoDineroDisponible(GameManager.Instance.Money)
                   )();
                    
                    break;
            }
        });

    }

    private void EatFoodFeedback(ProductoUI ui)
    {
        try
        {
            UIHungerController hungerController = FindObjectOfType<UIHungerController>();

            if (hungerController == null)
                throw new TsiunasException("Componente UIHungerController no encontrado", true, "COMER_COMIDA", "Eduardo");
            else
            {
                StaticCoroutine.DoCoroutine(Util.CreateUILerpGameElement(ui.gameObject,
                                                                         hungerController.gameObject,
                                                                         TexturesManager.Instance.GetSpriteFromSpriteSheet(ui.name)));
            }
        }
        catch (TsiunasException ex)
        {
            ex.Tratar();
        }
    }

    private T RegresarValorEnumDadoElNombre<T>(string strTipo)
    {
        T constanteEnum = (T)System.Enum.Parse(typeof(T), strTipo);
        return constanteEnum;
    }



}
