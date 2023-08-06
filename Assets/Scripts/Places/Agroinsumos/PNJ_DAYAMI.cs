using System.Collections;
using System.Collections.Generic;
using Tsiunas.Places;
using UnityEngine;

namespace Tsiunas.Places
{
    public class PNJ_DAYAMI : PNJ_ACTUACION
    {
        public override void Actuar(string mensaje, object datos)
        {
            if(mensaje == "DarSemillas")
            {
                StoreManager.ObtainSeed(TypesGameElement.Seeds.Corn, 4);
                SoundManager.PlayExito();
            }
        }
    }
}
