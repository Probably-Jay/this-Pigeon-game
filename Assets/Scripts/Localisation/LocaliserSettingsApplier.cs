using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Localisation
{
    public class LocaliserSettingsApplier : MonoBehaviour
    {
        //public Language language;
        private void Awake()
        {
            if (!PlayerPrefs.HasKey(Localiser.LangagePrefsKey))
            {
                Localiser.SetLanguage(Language.English);
                return;
            }

            Localiser.SetLanguage((Language)PlayerPrefs.GetInt(Localiser.LangagePrefsKey));

        }
    }
}