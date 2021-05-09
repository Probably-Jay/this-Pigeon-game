using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace SceneUI
{
    [RequireComponent(typeof(TMP_Text))]
    public class GardenNameSignController : MonoBehaviour
    {
        TMP_Text text;
        [SerializeField] Player.PlayerEnum gardenIn;
        // Start is called before the first frame update
        void Awake()
        {
            text = GetComponent<TMP_Text>();
        }
        private void OnEnable()
        {
            EventsManager.BindEvent(EventsManager.EventType.GameLoaded, SetText);
        }

        private void OnDisable()
        {
            EventsManager.UnbindEvent(EventsManager.EventType.GameLoaded, SetText);
        }

        private void SetText()
        {
          
            if(gardenIn == GameCore.GameManager.Instance.LocalPlayerEnumID)
            {
                text.text = Localisation.Localiser.GetText(Localisation.TextID.GameScene_GardenNamePannel_MyGarden);
            }
            else
            {
                text.text = Localisation.Localiser.GetText(Localisation.TextID.GameScene_GardenNamePannel_CompanionsGarden);
            }

        }
    }
}