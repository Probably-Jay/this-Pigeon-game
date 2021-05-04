using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolBox : MonoBehaviour
{

    public Transform destinationPos;

   // bool isOpening =false;
  //  bool isOpen = false;
    public float timeToOpen;
   // float startTime;
   // bool isClosing;
    Animator myAnimator;
    Image myImage;

    public bool Open { get; private set; }



    // Start is called before the first frame update
    void Start()
    {
        myImage = this.GetComponent<Image>();
        myAnimator = this.GetComponent<Animator>();

    }
    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.ToolBoxOpen, OpenBox);
        EventsManager.BindEvent(EventsManager.EventType.ToolBoxClose, CloseBox);
    }
    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.ToolBoxOpen, OpenBox);
        EventsManager.UnbindEvent(EventsManager.EventType.ToolBoxClose, CloseBox);
    }
    // Update is called once per frame
    void Update()
    {
        if (!myImage.rectTransform.rect.Contains(myImage.rectTransform.InverseTransformPoint(Input.mousePosition)) && Input.GetMouseButtonDown(0) && myAnimator.GetBool("Open"))
        {
            EventsManager.InvokeEvent(EventsManager.EventType.ToolBoxClose);

        }
        /*
        if (isOpening)
        {
            if (((Time.time - startTime) / timeToOpen) < 1)
            {
                this.transform.position = Vector3.Lerp(startPos, endPos, ((Time.time - startTime) / timeToOpen));
            }
            else
            {
                this.transform.position = endPos;
                isOpening = false;
                isOpen = true;
            }
        }
        if(isClosing)
        {
            if (((Time.time - startTime) / timeToOpen) < 1)
            {
                this.transform.position = Vector3.Lerp(endPos, startPos, ((Time.time - startTime) / timeToOpen));
            }
            else
            {
                this.transform.position = startPos;
                isClosing = false;
                isOpen = false;
            }
        }
        */
    }
    private void OpenBox()
    {
        myAnimator.SetBool("Open",true);
        Open = true;
        /*
        if (!isOpening&&!isOpen)
        {
            isOpening = true;
            startTime = Time.time;
        }
        */
    }
    private void CloseBox()
    {
        myAnimator.SetBool("Open", false);
        Open = false;

        /*
        if (!isClosing && isOpen)
        {
            isClosing = true;
            startTime = Time.time;
        }
        */
    }
}
