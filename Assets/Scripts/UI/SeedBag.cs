using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedBag : MonoBehaviour
{
    public Image myImage;
    // Start is called before the first frame update
    void OnEnable()
    {
        myImage = this.GetComponent<Image>();
        EventsManager.BindEvent(EventsManager.EventType.SeedBagClose, CloseBag);
        EventsManager.BindEvent(EventsManager.EventType.PlantingBegin, CloseBag);
    }
    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.SeedBagClose, CloseBag);
        EventsManager.UnbindEvent(EventsManager.EventType.PlantingBegin, CloseBag);

    }

    // Update is called once per frame
    void Update()
    {
        if (!myImage.rectTransform.rect.Contains(myImage.rectTransform.InverseTransformPoint(Input.mousePosition))&&Input.GetMouseButtonDown(0))
            {
            EventsManager.InvokeEvent(EventsManager.EventType.SeedBagClose);
            Debug.Log("clicked outside of seed bag, closing");
            
        }
    }
    void CloseBag()
    {
        this.gameObject.SetActive(false);
    }
}
