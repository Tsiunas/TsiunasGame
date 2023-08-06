using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tsiunas.Places
{
    public class PNJ_DON_ARCENIO : PNJ_ACTUACION
    {
        
        public override void Actuar(string arg1, object arg2)
        {
            StoreManager.ObtainTool(TypesGameElement.Tools.Machete);
            Actor.NivelAmistad = NivelesAmistad.AMISTAD_ENTABLADA;
        }
    }
}