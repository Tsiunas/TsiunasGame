using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Tsiunas.SistemaDialogos;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueSystemController : MonoBehaviour
    {
        // Attributes
#region Attributes
        private IEnumerator[] coroutineDialogues;

        public GameObject[] speechBubbles;

        public TsiunasSituationsLoader tSituationsLoader;

        private List<SituationContent> _situations;

        private int idAnswerOption;

        private string namePlayer;

        public DialogueUIController currentDialogue;

        public int indexPresentSituation;

        public bool touchToInit;

        public ConsequenceDecision consequenceDecision;
#endregion


        // Methods
#region Methods
        // Use this for initialization
        void Start()
        {
            // Registra el método NotificationBetweenDialogueUI para recibir notificaciones
            NotificationCenter
                .DefaultCenter()
                .AddObserver(this, "NotificationBetweenDialogueUI");
            tSituationsLoader.onLoadSituations += eventOnLoadSituations;
        }

        /// <summary>
        /// Recibe notificación desde otro objeto e interpreta la información pasada según el siguiente criterio:
        /// * Data received:pjDatos.nombre
        ///	* 0 = int
        ///	* 1 = string
        ///	* 2 = float
        //	* 3 = bool
        /// </summary>
        /// <param name="notification">Notification.</param>
        public void NotificationBetweenDialogueUI(Notification notification)
        {
            object[] tales = (object[]) notification.data;
            if (notification.data == null) return;

            switch ((int) tales[0])
            {
                case 0:
                    idAnswerOption = (int) tales[1];
                    Debug
                        .Log("NotificationBetweenDialogueUI: " +
                        idAnswerOption.ToString());
                    break;
                case 1:
                    namePlayer = (string) tales[1];
                    Debug.Log("NotificationBetweenDialogueUI: " + namePlayer);
                    PNJDatos pjDatos = (PNJDatos)(Resources.Load("PJ"));
                    pjDatos.nombre = namePlayer;
                    GameManager.Instance.SetNamePJ = pjDatos.nombre;

                    break;
            }

            if (consequenceDecision != null)
                consequenceDecision.ImplementConsequence(idAnswerOption);
        }

        void eventOnLoadSituations(List<SituationContent> listSituations)
        {
            _situations = listSituations;

            if (!touchToInit)
            {
                if (
                    !GameManager.Instance.WasObtainedTool(ToolType.Hoe) &&
                    GameManager.Instance.GetGameState == GameState.InIntro
                )
                {
                    PresentSituation (indexPresentSituation);
                }
                else
                {
                    if (GameManager.Instance.GetGameState == GameState.InIntro)
                    {
                        // Cargar estado de MamaTule dependiendo de la decisión que tomó en la tienda de Don Jorge
                        Debug
                            .Log("Cargar estado de MamaTule dependiendo de la decisión que tomó en la tienda de Don Jorge");

                        // Si compró el azadón el la tienda de Don Jorge cambiar el idAnswer
                        idAnswerOption =
                            GameManager.Instance.BoughtInStore ? 0 : 1;

                        PresentSituation(3);
                    }
                    else
                    {
                        if (Camera.main.GetComponent<Animator>() != null)
                        {
                            Camera.main.GetComponent<Animator>().enabled =
                                false;
                        }

                        if (Camera.main.GetComponent<CameraHandler>() != null)
                        {
                            Camera
                                .main
                                .GetComponent<CameraHandler>()
                                .CanPanCamera = true;
                        }
                    }
                }
            }
        }

        public void PresentSituation(int indexSituation)
        {
            int amountDialogues = _situations[indexSituation].Dialogues.Length;
            coroutineDialogues = new IEnumerator[amountDialogues];
            for (int i = 0; i < amountDialogues; i++)
            {
                coroutineDialogues[i] = PresentDialogue(indexSituation, i);
            }

            StartCoroutine(Sequence(coroutineDialogues));
        }

        /// <summary>
        /// Ejecuta varias corutinas, una tras de otra en secuencia
        /// </summary>
        /// <param name="sequence">secuencia de corrutinas a ejecutar</param>
        public static IEnumerator Sequence(params IEnumerator[] sequence)
        {
            for (int i = 0; i < sequence.Length; ++i)
            {
                while (sequence[i].MoveNext()) yield return sequence[i].Current;
            }
        }

        /// <summary>
        /// Devuelve el prefab a instanciar dependiendo del valor del tipo del objeto json
        /// </summary>
        /// <returns>Prefab a instanciar</returns>
        /// <param name="_type">Tipo del objeto JSON</param>
        GameObject GetSpeechBubble(int _type, string nameTalker)
        {
            GameObject original = null;
            switch (_type)
            {
                case 0:
                    original =
                        nameTalker == "Tu"
                            ? speechBubbles[3]
                            : speechBubbles[0];
                    break;
                case 1:
                    original = speechBubbles[1];
                    break;
                case 2:
                    original = speechBubbles[2];
                    break;
                case 3:
                    original =
                        nameTalker == "Tu"
                            ? speechBubbles[3]
                            : speechBubbles[0];
                    break;
            }
            return original;
        }

        public string LimpiarString(string str)
        {
            // Expresión regular que busca todos los caracteres que no son alfanuméricos
            Regex regex = new Regex("[^a-zA-Z0-9 ]");

            // Reemplaza todos los caracteres que no son alfanuméricos con una cadena vacía
            string strLimpio = regex.Replace(str, "");

            return strLimpio;
        }

        IEnumerator PresentDialogue(int indexSituation, int indexCoroutine)
        {
            int intType =
                this._situations[indexSituation].Dialogues[indexCoroutine].Type;
            GameObject original =
                GetSpeechBubble(intType,
                this
                    ._situations[indexSituation]
                    .Dialogues[indexCoroutine]
                    .Character);

            GameObject goInstantiated = (GameObject) Instantiate(original);
            goInstantiated.name = "Dialogue_" + indexCoroutine.ToString();
            goInstantiated.transform.SetParent(this.gameObject.transform);
            RectTransform rectT = goInstantiated.GetComponent<RectTransform>();

            //rectT.offsetMin = Vector2.zero;
            //rectT.offsetMax = Vector2.zero;
            rectT.localScale = Vector2.one;

            string character =
                this
                    ._situations[indexSituation]
                    .Dialogues[indexCoroutine]
                    .Character;
            PNJActor
                .AnimarHablar(AnimationsManager
                    .Instance
                    .ObtenerIDPorNombre(character));

            currentDialogue =
                goInstantiated.GetComponent<DialogueUIController>();
            currentDialogue.SetCharacterDialogue (character);
            string textObtained =
                intType != 3
                    ? this
                        ._situations[indexSituation]
                        .Dialogues[indexCoroutine]
                        .Conversation
                    : this
                        ._situations[indexSituation]
                        .Dialogues[indexCoroutine]
                        .Answers_options[idAnswerOption];
            string textToSet =
                textObtained.Replace("PJ", GameManager.Instance.GetNamePJ);
            currentDialogue.SetConversationDialogue (textToSet, intType);
            currentDialogue
                .SetImageCharacter(this
                    ._situations[indexSituation]
                    .Dialogues[indexCoroutine]
                    .Character);
            currentDialogue
                .SetLauncEvent(this
                    ._situations[indexSituation]
                    .Dialogues[indexCoroutine]
                    .LaunchEvent);
            currentDialogue
                .SetBehaviourType(this
                    ._situations[indexSituation]
                    .Dialogues[indexCoroutine]
                    .Options);
            Debug.LogWarning("Dialogue: " + indexCoroutine.ToString());

            string textToSend = LimpiarString(textToSet);
            TrackerSystem
                .Instance
                .SendTrackingData("game", "launch", "dialog-tree", textToSend+"|user|éxito");
            yield return new WaitUntil(() =>
                        goInstantiated.GetComponent<Condition>().meetCondition);

            Destroy (goInstantiated);
        }
#endregion
    }

    public abstract class ConsequenceDecision : MonoBehaviour
    {
        public abstract T ImplementConsequence<T>(T parameter);
    }
}
