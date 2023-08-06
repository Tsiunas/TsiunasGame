using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TocarDonJorge : MonoBehaviour {
    private bool unaVez;
    private void OnMouseUp()
    {
        if (!unaVez)
        {
            unaVez = true;
            if (!Util.IsPointerOverUIObject())
                Util.PassToAnotherDialog();
        }
       
    }
}
