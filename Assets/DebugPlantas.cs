using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPlantas : MonoBehaviour {

	// Use this for initialization
	void Awake ()
    {
        GameManager.Instance.AddTool(new Hoe());
        for (int i = 0; i < 50; i++)
        {
            GameManager.Instance.seeds.Add(new TsiunaSeed());
        }
        

        GameManager.Instance.SetGameState(GameState.InGame);
        TimeManager.Instance.SetDaysPerMinute(1);
        Debug.Log("Hoe agregada")
;    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
