using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using Tsiunas.Mechanics;

public class UIGameManager : MonoBehaviour
{
	public Slider sliderHungerLevel;
	public Slider slider;
	// Use this for initialization
	void Start ()
	{
		HungerManager.Instance.OnHungerLevelChange += eventHungerLevelChange;
		slider.onValueChanged.AddListener (delegate(float arg0) {
			
		});

		
		//unityGameManager.Instance.AddTool (ToolType.Hoe);

	}

	void eventHungerLevelChange (int currentHungerLevel)
	{
		float normalizedFloat = (float)currentHungerLevel / 100f;
		sliderHungerLevel.value = normalizedFloat;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.R)) {
            HungerManager.Instance.DecreaseHungerLevel (1);
		}
	}
}

