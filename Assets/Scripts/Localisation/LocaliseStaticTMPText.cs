using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Localisation
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocaliseStaticTMPText : MonoBehaviour
    {
        TMP_Text textComponent;
        [SerializeField] TextID key;

        // Start is called before the first frame update
        void Start()
        {
            textComponent = GetComponent<TMP_Text>();
            var text = Localiser.GetText(key);
            textComponent.text = text;
        }
    }
}

