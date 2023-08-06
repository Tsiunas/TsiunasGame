
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tsiunas.SistemaDialogos
{
    [Serializable]
    public class Speech
    {        
        public Speech(string texto, AudioClip clip)
        {
            this.texto = texto;
            this.audio = clip;
        }

        public Speech(string texto):this(texto,null)
        {
            
        }

        public Speech()
        {
        }

        public AudioClip audio;

        public string texto;


    }
    [Serializable]
    public class Speeches
    {
        public Speech[] speeches;
    }
}