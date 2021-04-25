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
        void Start()
        {
            Debug.Log(arrows[0]);
            arrowDex.Add(ArrowScript.ArrowPurpose.SeedBox, arrows[0]);
            arrowDex.Add(ArrowScript.ArrowPurpose.WateringCan, arrows[1]);
            arrowDex.Add(ArrowScript.ArrowPurpose.MoodIndicator, arrows[2]);
            arrowDex.Add(ArrowScript.ArrowPurpose.SwapGarden, arrows[3]);
            Debug.Log(arrows[1]);
            //EnableArrow(ArrowScript.ArrowPurpose.WateringCan);
        }


        // Update is called once per frame
        void Update()
        {

        }
        public void EnableArrow(ArrowScript.ArrowPurpose key)
        {

            arrowDex[key].gameObject.SetActive(true);
            arrowDex[key].SetPurpose(key);
        }
        public void DisableArrow(ArrowScript.ArrowPurpose key)
        {
            arrowDex[key].gameObject.SetActive(false);
        }
    }
}
