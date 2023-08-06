
using System;
using UnityEngine;

namespace Tsiunas.Mechanics
{
    public class TexturesManager : Singleton<TexturesManager>
    {
        private Sprite[] allTextureFromSpriteSheet;
        internal bool allTexturesHaveBeenLoaded;

        protected TexturesManager() {
            uniqueToAllApp = true;
        }

        private void OnEnable()
        {
            LoadAllTextures(() => { allTexturesHaveBeenLoaded = true; });
        }

        internal Sprite GetSpriteFromSpriteSheet(string searchCriteria) {
            Sprite spriteObtained = Array.Find(allTextureFromSpriteSheet, (Sprite obj) => obj.name.Equals(searchCriteria));
            if (spriteObtained == null)
                throw new ArgumentException("La textura: " + searchCriteria + " requerida no ha sido encontrada");
            return spriteObtained;
        }

        private void LoadAllTextures(Action callback) {
            allTextureFromSpriteSheet = Resources.LoadAll<Sprite>("ResourcesMechanics");
            callback();
        }
    }
}
