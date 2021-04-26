using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;

namespace SceneUI
{

    public class SeedBagButtonScript : MonoBehaviour
    {

        private void Update()
        {
            if (GameManager.Instance.Spectating)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }

        public void OpenSeedBag()
        {
            // Xanders edit ~ fixes bug that allows you to plant unlimited plants in the other players garden if
            // the active player has not placed their plant yet and not allow any planting in the other players garden if they have

            if (GameManager.Instance.Spectating)
            {
                return;
            }


            // for planting in active players own garden 

            if (GameManager.Instance.InOwnGarden && GameManager.Instance.LocalPlayer.TurnPoints.HasPointsLeft(TurnPoints.PointType.SelfObjectPlace))
            {
                EventsManager.InvokeEvent(EventsManager.EventType.SeedBagOpen);
            }
            else if (GameManager.Instance.InOwnGarden)
            {
                EventsManager.InvokeEvent(EventsManager.ParameterEventType.NotEnoughPointsForAction, new EventsManager.EventParams() { EnumData = TurnPoints.PointType.SelfObjectPlace });
            }


            // for tha active player planting in the non-active players garden 
            if (!GameManager.Instance.InOwnGarden && GameManager.Instance.LocalPlayer.TurnPoints.HasPointsLeft(TurnPoints.PointType.OtherObjectPlace))
            {
                EventsManager.InvokeEvent(EventsManager.EventType.SeedBagOpen);
            }
            else if (!GameManager.Instance.InOwnGarden)
            {
                EventsManager.InvokeEvent(EventsManager.ParameterEventType.NotEnoughPointsForAction, new EventsManager.EventParams() { EnumData = TurnPoints.PointType.OtherObjectPlace });
            }
            // end of Xanders edit


        }
    }
}