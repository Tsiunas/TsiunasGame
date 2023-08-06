using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System;
using Tsiunas.Mechanics;
using Tsiunas.SistemaDialogos;
using RedBlueGames.Tools.TextTyper;

// Struct
#region Struct
struct EstructuraTextoTecleado
{
    public Coroutine corutinaETT;
    public string textoETT;
}
#endregion

public abstract class ControladorGloboDialogo : MonoBehaviour, IPointerDownHandler
{
    private int vecesGloboDialogoTocado;
    private const string TAG_GLOBO_DIALOGO = "GloboDialogo";
    private const string PARAMETRO_ESTADO_DIALOGO_SALE = "DialogoSale";

    private EstructuraTextoTecleado estructuraTextoTecleado;
    public Text letreroNombrePNJ, letreroGloboDialogo;
    public Image imagenAvatar;
    private Animator animador;
    public TextTyper componenteTextTyper;

    private AudioSource fuenteAudioTextoTecleado;

    private void Awake()
    {
        try
        {
            animador = GetComponent<Animator>();
            fuenteAudioTextoTecleado = GetComponent<AudioSource>();
        }
        catch (System.Exception ex)
        {
            throw ex;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AumentarVecesGloboDialogoTocado();
        if (eventData.pointerEnter != null)
        if (eventData.pointerEnter.tag.Equals(TAG_GLOBO_DIALOGO)) if (vecesGloboDialogoTocado == 1) /*PresentarTextoCompleto(this.letreroGloboDialogo);*/ componenteTextTyper.Skip();  else SegundoToqueSobreGloboDialogo();
    }

    /// <summary>
    /// Aumenta en 1, la variable vecesGloboDialogoTocado
    /// esta variable indica las veces que el globo de diálogo es tocado
    /// </summary>
    void AumentarVecesGloboDialogoTocado()
    {
        vecesGloboDialogoTocado++;
    }

    /// <summary>
    /// Detiene la coroutina que muestra el texto de díalogo letra por letra y asigana el texto completo al instante
    /// </summary>
    /// <param name="uiTexto">UI del texto a mostrar completo</param>
    public void PresentarTextoCompleto(Text uiTexto)
    {
        StopCoroutine(estructuraTextoTecleado.corutinaETT);
        uiTexto.text = estructuraTextoTecleado.textoETT;
    }

    /// <summary>
    /// Reinicia a 0 el valor de la variable vecesGloboDialogoTocado
    /// En clases derivadas se podrá reemplazar su implementación o añadir una nueva
    /// </summary>
    public virtual void SegundoToqueSobreGloboDialogo()
    {
        vecesGloboDialogoTocado = 0;
    }

    public void ActivarAnimacionGloboDialogoSale()
    {
        this.animador.SetBool(PARAMETRO_ESTADO_DIALOGO_SALE, true);
    }

    /// <summary>
    /// Este método es llamado agregando un evento (en la ventana de Animación), cuando termina la animación de salida del globo de diálogo
    /// Destruye este GameObject
    /// </summary>
    public void EventoAnimacionDialogoSaleTerminada()
    {
        DesplegadorDialogo.Instance.CerrarDialogo();
        Destroy(this.gameObject);        
    }

    /// <summary>
    /// Método abstracto para establecer el comportamiento del Globo de diálogo según su tipo (si tiene lineas simples o con opción)
    /// </summary>
    public abstract void EstablecerComportamientoSegunTipo (string [] sentencias = null, Action callback = null);

    /// <summary>
    /// Establece el nombre para la UI del personaje interlocutor
    /// </summary>
    /// <param name="_nombrePNJ">nombre del PNJ a establecer</param>
    public void EstablecerNombrePNJ(string _nombrePNJ = "PNJ")
    {
        string nombre = _nombrePNJ.Equals("PNJ") ? GameManager.Instance.GetNamePJ : _nombrePNJ;
        this.letreroNombrePNJ.text = nombre;
    }


    /// <summary>
    /// Establece el texto para la UI de la conversación
    /// </summary>
    /// <param name="_texto">texto a establecer</param>
    public virtual void EstablecerLineaDialogo(string _texto, bool opciones)
    {
        estructuraTextoTecleado.textoETT = _texto;
        if (!opciones)
            componenteTextTyper.TypeText(_texto, 0.05f);
        else
            this.letreroGloboDialogo.text = _texto;
        // estructuraTextoTecleado.corutinaETT = StartCoroutine(TeclearTexto(_texto, this.letreroGloboDialogo, 0.05f, AumentarVecesGloboDialogoTocado));
    }

    /// <summary>
    /// Muestra el texto asignado letra por letra con un intervalo de tiempo entre cada una
    /// </summary>
    /// <param name="textoCompleto">texto a establecer</param>
    /// <param name="letretoDialogo">UI de conversación</param>
    /// <param name="intervaloPorLetra">intervalo de tiempo entre cada letra presentada</param>
    IEnumerator TeclearTexto(string textoCompleto, Text letretoDialogo, float intervaloPorLetra, Action callback = null)
    {
        //foreach (char letra in textoCompleto)
        //{
        //    letretoDialogo.text += letra;
        //    //fuenteAudioTextoTecleado.PlayOneShot(fuenteAudioTextoTecleado.clip);
        //    yield return new WaitForSeconds(intervaloPorLetra);
        //}
        componenteTextTyper.TypeText(textoCompleto, 0.05f);
        // Al terminar de mostrar letra por letra hace la llamada a un 'callback'
        if (callback != null) callback();
        return null;
    }

    /// <summary>
    /// Establece el sprite para la UI del personaje interlocutor
    /// </summary>
    /// <param name="spriteAvatar">sprite de Avatar del PNJ</param>
    public void EstablecerImagenAvatar(Sprite spriteAvatar = null)
    {
        this.imagenAvatar.sprite = spriteAvatar ?? Resources.Load<Sprite>("AvatarPJ");
    }
}
