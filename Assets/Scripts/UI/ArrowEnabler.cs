using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Tutorial
{
    public class ArrowEnabler : MonoBehaviour
    {
        public ArrowScript[] arrows = new ArrowScript[4];
        public Dictionary<ArrowScript.ArrowPurpose, ArrowScript> arrowDex = new Dictionary<ArrowScript.ArrowPurpose, ArrowScript>() { };
        // Start is called before the first frame update
        void Awake()
        {
            Debug.Log(arrows[0]);
            arrowDex.Add(ArrowScript.ArrowPurpose.SeedBox, arrows[0]);
            Debug.Log(arrowDex[ArrowScript.ArrowPurpose.SeedBox]);
            arrowDex.Add(ArrowScript.ArrowPurpose.WateringCan, arrows[1]);
            arrowDex.Add(ArrowScript.ArrowPurpose.MoodIndicator, arrows[2]);
            arrowDex.Add(ArrowScript.ArrowPurpose.SwapGarden, arrows[3]);
            arrowDex.Add(ArrowScript.ArrowPurpose.ToolBox, arrows[4]);
            Debug.Log(arrows[1]);
            //EnableArrow(ArrowScript.ArrowPurpose.WateringCan);
        }


        // Update is called once per frame
        void Update()
        {

        }
        public void EnableArrow(ArrowScript.ArrowPurpose key)
        {
            Debug.Log(key);
            arrowDex[key].gameObject.SetActive(true);
            arrowDex[key].SetPurpose(key);
        }
        public void DisableArrow(ArrowScript.ArrowPurpose key)
        {
            arrowDex[key].gameObject.SetActive(false);
        }
        public void BindArrowEnabling(ArrowScript.ArrowPurpose purpose, EventsManager.EventType eventType)
        {
            EventsManager.BindEvent(eventType, () => EnableArrow(purpose));
        }
    }
}
