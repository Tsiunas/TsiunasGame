using System.Collections;
using System.Collections.Generic;
using Tsiunas.SistemaDialogos;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExplainsMamaTule : MonoBehaviour {

    public void ExplicacionHambreMamaTule() {
        DesplegadorDialogo.Instance.DesplegarTodos(new DesplegadorDialogo.PNJDatosSpeeches(GestorPNJ.Instance.GetMamaTule(),
                                                                                           new Speech("Éste es tu <size=17><b><color=#a52a2aff>Nivel de Hambre</color></b></size>, baja al pasar el tiempo y cada vez que usas una herramienta."),
                                                                                           new Speech("Debes alimentarte comprando comida o arrastrando aquí las semillas o frutos que tengas."),
                                                                                           new Speech("¡Ten cuidado de no desmayarte!")));
    }
    public void ExplicacionIntensidadFAMamaTule()
    {
        DesplegadorDialogo.Instance.DesplegarTodos(new DesplegadorDialogo.PNJDatosSpeeches(GestorPNJ.Instance.GetMamaTule(),
                                                                                           new Speech("Éste circulo representa la <size=15><b><color=#a52a2aff>Intensidad de las Flamas de la Armonía</color></b></size>, baja al pasar el tiempo o cuando no ayudas a fomentar el respeto."),
                                                                                           new Speech("Para subir la intensidad deberás hablar de manera sabia con las personas del pueblo, siempre promoviendo la igualdad y la equidad."),
                                                                                           new Speech("¡Si la intensidad de las Flamas de la Armonía baja demasiado mi espíritu desaparecerá!, no permitas que esto pase.")));
    }
}
