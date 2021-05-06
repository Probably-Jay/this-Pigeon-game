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
            Localiser.SetLanguage(language);
        }
    }
}