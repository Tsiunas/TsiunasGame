using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem {
	public class DialogueBase {

		// Attributes
		#region Attributes
		private int type;
		private string character;
		private string conversation;
		private bool launchEvent;
		private string [] options;
		private string [] answers_options;
		#endregion

		// Properties
		#region Properties
		public int Type {
			get {
				return type;
			}
			set {
				type = value;
			}
		}

		public string Character {
			get {
				return character;
			}
			set {
				character = value;
			}
		}

		public string Conversation {
			get {
				return conversation;
			}
			set {
				conversation = value;
			}
		}

		public bool LaunchEvent {
			get {
				return this.launchEvent;
			}
			set {
				launchEvent = value;
			}
		}

		public string[] Options {
			get {
				return options;
			}
			set {
				options = value;
			}
		}

		public string[] Answers_options {
			get {
				return answers_options;
			}
			set {
				answers_options = value;
			}
		}
		#endregion

		// Constructor
		#region Constructor
		public DialogueBase (int type, string character, string conversation, bool launchEvent, string[] options, string[] answers_options)
		{
			this.type = type;
			this.character = character;
			this.conversation = conversation;
			this.launchEvent = launchEvent;
			this.options = options;
			this.answers_options = answers_options;
		}

		public DialogueBase ()
		{
			
		}		
		#endregion
	}

	public class DialogueContent : DialogueBase {
		// Constructor
		#region Constructor
		public DialogueContent (int type, string character, string conversation, bool launchEvent, string[] options, string[] answers_options) : base (type, character, conversation, launchEvent, options, answers_options) {	}
		public DialogueContent () { }
		#endregion
	}
}
