using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;

public class TapAgroinsumos : Tap {

	// Attributes
	#region Attributes
	public string[] tags;
	#endregion

	// Methods
	#region Methods
	public override T TapGameobject<T> (T parameter)
	{
		if (parameter.GetType () == typeof(GameObject)) {
			GameObject go = parameter as GameObject;
			#region Tag: Dayami
			// Tag: Dayami
			if (go.tag.Equals (tags[0])) {
				// Toca a Dayami
				DialogueSystemController dialogSC = GameObject.FindObjectOfType<DialogueSystemController>();
				dialogSC.PresentSituation(dialogSC.indexPresentSituation);
				go.GetComponent<Collider>().enabled = false;
			}

			// Tag: Hoe
			if (go.tag.Equals (tags[1])) {
				// Toca el Arado
				Util.PassToAnotherDialog();
				go.GetComponent<Collider>().enabled = false;
			}
			#endregion
		}

		return default(T);
	}
	#endregion
}
