using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;
using SimpleJSON;

namespace DialogueSystem {
	public class TsiunasSituationsLoader : SituationsReaderFromJSON {

		// Attributes
		#region Attributes
		public delegate void OnLoadSituations(List<SituationContent> listSituations);
		public event OnLoadSituations onLoadSituations;

		public List<SituationContent> _situations;
		#endregion

		// Methods
		#region Methods
		public override void LoadNow (SimpleJSON.JSONNode situationsFile)
		{
			this._situations = new List<SituationContent> ();

			for (int s = 0; s < situationsFile["situations"].Count; s++) {

				_situations.Add(new SituationContent(new DialogueContent[]{}));

				if (_situations [s].Dialogues == null || _situations[s].Dialogues.Length == 0)
					_situations [s].Dialogues = new DialogueContent[situationsFile ["situations"] [s] ["dialogues"].Count];
				
				for (int d = 0; d < situationsFile["situations"][s]["dialogues"].Count; d++) {
					_situations [s].Dialogues [d] = new DialogueContent();

					_situations [s].Dialogues [d].Character = situationsFile ["situations"] [s] ["dialogues"] [d] ["character"];
					_situations [s].Dialogues [d].Conversation = situationsFile ["situations"] [s] ["dialogues"] [d] ["conversation"];
					_situations [s].Dialogues [d].Type = situationsFile ["situations"] [s] ["dialogues"] [d] ["type"].AsInt;
					_situations [s].Dialogues [d].LaunchEvent = situationsFile ["situations"] [s] ["dialogues"] [d] ["launch_event"].AsBool;


					switch (_situations [s].Dialogues [d].Type) {
					case 0:
						_situations [s].Dialogues [d].Options = null;
						_situations [s].Dialogues [d].Answers_options = null;
						break;
					case 1:
						_situations [s].Dialogues [d].Answers_options = null;

						JSONArray _tempOptions = situationsFile ["situations"] [s] ["dialogues"] [d] ["options"].AsArray;
						List<string> _tempListOptions = new List<string>();

						for (int a = 0; a < _tempOptions.Count; a++) {
							_tempListOptions.Add(_tempOptions[a]);
						}

						if (_situations [s].Dialogues [d].Options == null || _situations [s].Dialogues [d].Options.Length == 0)
							_situations [s].Dialogues [d].Options = new string[_tempListOptions.Count];

						for (int o = 0; o < _tempListOptions.Count; o++) {
							_situations [s].Dialogues [d].Options[o] = _tempListOptions [o];
						}
						break;
					case 3:
						_situations [s].Dialogues [d].Options = null;

						JSONArray _tempAnswersOptions = situationsFile ["situations"] [s] ["dialogues"] [d] ["answers_options"].AsArray;
						List<string> _tempListAnswersOptions = new List<string>();

						for (int e = 0; e < _tempAnswersOptions.Count; e++) {
							_tempListAnswersOptions.Add(_tempAnswersOptions[e]);
						}

						if (_situations [s].Dialogues [d].Answers_options == null || _situations [s].Dialogues [d].Answers_options.Length == 0)
							_situations [s].Dialogues [d].Answers_options = new string[_tempListAnswersOptions.Count];

						for (int c = 0; c < _tempListAnswersOptions.Count; c++) {
							_situations [s].Dialogues [d].Answers_options[c] = _tempListAnswersOptions [c];
						}
						break;
					}

				}
			}
			StartCoroutine("coroutineEndLastFrame");
		}

		IEnumerator coroutineEndLastFrame() {

			yield return new WaitForEndOfFrame();

			if (onLoadSituations != null)
				onLoadSituations (this._situations);
		}
		#endregion
	}
}
