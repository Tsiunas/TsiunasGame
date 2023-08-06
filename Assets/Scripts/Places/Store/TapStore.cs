using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;

public class TapStore : Tap {

	// Attributes
	#region Attributes
	public List<string> tags;
    
	#endregion

	// Methods
	#region Methods
	public override T TapGameobject<T> (T parameter)
	{
		if (parameter.GetType () == typeof(GameObject)) {
			GameObject go = parameter as GameObject;
			#region Tag: Hoe
			// Tag: Hoe
			if (go.tag.Equals (tags[0])) {
				Util.PassToAnotherDialog();
				go.GetComponent<Collider>().enabled = false;
			}

			if (go.tag.Equals (tags[1]))
            {
                // Toca a Yurani
                //Desactiva la mano indicadora
                
				DialogueSystemController dialogSC = GameObject.FindObjectOfType<DialogueSystemController>();
				dialogSC.PresentSituation(dialogSC.indexPresentSituation);
				go.GetComponent<Collider>().enabled = false;
			}
			#endregion
		}
		return default(T);
	}
	#endregion
}
