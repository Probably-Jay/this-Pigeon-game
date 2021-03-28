using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolBox : MonoBehaviour
{
    Vector3 startPos;
    public Transform destinationPos;
    Vector3 endPos;
    bool isOpening =false;
    bool isOpen = false;
    public float timeToOpen;
    float startTime;
    bool isClosing;
    Animator myAnimator;
    Image myImage;
    
    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;
        endPos = destinationPos.position;
    }
    private void OnEnable()
    {
        myImage = this.GetComponent<Image>();
        myAnimator = this.GetComponent<Animator>();
        EventsManager.BindEvent(EventsManager.EventType.ToolBoxOpen, OpenBox);
        EventsManager.BindEvent(EventsManager.EventType.ToolBoxClose, CloseBox);
    }
    // Update is called once per frame
    void Update()
    {
        if (!myImage.rectTransform.rect.Contains(myImage.rectTransform.InverseTransformPoint(Input.mousePosition)) && Input.GetMouseButtonDown(0))
        {
            EventsManager.InvokeEvent(EventsManager.EventType.SeedBagClose);
            Debug.Log("clicked outside of toolbox, closing");

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
        /*
        if (!isClosing && isOpen)
        {
            isClosing = true;
            startTime = Time.time;
        }
        */
    }
}
