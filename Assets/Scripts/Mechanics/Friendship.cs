using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIAmistad))]
public class Friendship : MonoBehaviour {

	// Attributes
	#region Attributes
	[SerializeField]
	private int friendship;
	public delegate void OnFriendshipLevelChange(int currentFriendshipLevel);
	public event OnFriendshipLevelChange friendshipLevelChange;
	#endregion

	// Properties
	#region Properties
	public int FriendshipLevel {
		get { return this.friendship; }
		private set { if (value > 4) friendship = 4; else if (value < 0) friendship = 0; else friendship = value; }
	}
	#endregion

	// Methods
	#region Methods
	/// <summary>
	/// Incrementa el valor del nivel de AMISTAD en 1
	/// </summary>
	public void IncreaseFriendshipLevel() {
		this.friendship++;
		if (friendshipLevelChange != null) friendshipLevelChange (this.friendship);
	}

	/// <summary>
	/// Incrementa el valor del nivel de AMISTAD
	/// </summary>
	public void IncreaseFriendshipLevel(int valueToIncrease) {
		this.friendship += valueToIncrease;
		if (friendshipLevelChange != null) friendshipLevelChange (this.friendship);
	}

	/// <summary>
	/// Decrementa el valor del nivel de AMISTAD
	/// </summary>
	public void DecreaseFriendshipLevel(int valueToDecrease) {
		this.friendship -= valueToDecrease;
		if (friendshipLevelChange != null) friendshipLevelChange (this.friendship);
	}

	/// <summary>
	/// Decrementa el valor del nivel de AMISTAD en 1
	/// </summary>
	public void DecreaseFriendshipLevel() {
		this.friendship--;
		if (friendshipLevelChange != null) friendshipLevelChange (this.friendship);
	}
	#endregion
}
