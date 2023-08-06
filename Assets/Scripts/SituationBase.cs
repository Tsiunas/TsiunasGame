using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;

namespace DialogueSystem {
	public class SituationBase {
		// Attributes
		#region Attributes
		private DialogueContent [] dialogues;
		#endregion

		// Properties
		#region Properties
		public DialogueContent[] Dialogues {
			get {
				return dialogues;
			}
			set {
				dialogues = value;
			}
		}
		#endregion

		// Constructor
		#region Constructor
		public SituationBase (DialogueContent[] dialogues)
		{
			this.dialogues = dialogues;
		}		
		#endregion
	}

	public class SituationContent : SituationBase {
		// Constructor
		#region Constructor
		public SituationContent (DialogueContent[] dialogues) : base (dialogues) {	}
		#endregion
	}
}
