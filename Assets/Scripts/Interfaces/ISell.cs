using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISell {
    int SellPrice { get; set; }
    GameElement Sell();
}
