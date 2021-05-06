using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Localisation
{
    public class LocaliserSettingsApplier : MonoBehaviour
    {
        public Language language;
        private void Awake()
        {
            if( Localiser.CurrentLanguage == (Language)(-1))
                Localiser.SetLanguage(language);
        }
    }
}