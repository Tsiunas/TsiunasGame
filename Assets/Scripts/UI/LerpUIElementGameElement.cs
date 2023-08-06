using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpUIElementGameElement : LerpUIElement {

    public override void Start()
    {
        base.Start();
        OnEndLerp += EndLerp;
    }

    private void OnDestroy()
    {
        OnEndLerp -= EndLerp;
    }

    void EndLerp()
    {
       Destroy(this.gameObject);
    }
}
