using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WindowOpener : MonoBehaviour
{
    public GameObject seedBag;
    public GameObject trowelConfirm;
    // Start is called before the first frame update
    void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.triedToRemoveOwnObject, OpenTrowelConfirm);
        EventsManager.BindEvent(EventsManager.EventType.SeedBagOpen, OpenSeedBag);
    }
    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.triedToRemoveOwnObject, OpenTrowelConfirm);
        EventsManager.UnbindEvent(EventsManager.EventType.SeedBagOpen, OpenSeedBag);
    }


    void OpenSeedBag()
    {
        seedBag.SetActive(true);
    }
    void OpenTrowelConfirm()
    {
        trowelConfirm.SetActive(true);
    }
}
