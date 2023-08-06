using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPurchase {
    int PurchasePrice { get; set; }
    GameElement Purchase();

}
