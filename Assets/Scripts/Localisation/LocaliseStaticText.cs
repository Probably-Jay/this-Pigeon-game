using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Localisation
{
    [RequireComponent(typeof(Text))]
    public class LocaliseStaticText : MonoBehaviour
    {

        Text textComponent;
        [SerializeField] TextID key;

        // Start is called before the first frame update
        void Start()
        {
            textComponent = GetComponent<Text>();
            var text = Localiser.GetText(key);
            textComponent.text = text;
        }


    }
}