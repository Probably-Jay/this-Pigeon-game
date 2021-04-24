using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowScript : MonoBehaviour
{
    public bool startPulse;
    public bool startDrag;
    float pulseTimer;
    Image myImage;
    bool pulseTimerForward =true;
    public float pulseSpeed = 0.01f;
    public Animator myAnimator;
    public ArrowPurpose myPurpose;

    public enum ArrowPurpose
    {
        SeedBox,
        ToolBox,
        WateringCan,
        MoodIndicator

    }
    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.ToolBoxOpen, ToolBoxEnd);
        EventsManager.BindEvent(EventsManager.EventType.SeedBagOpen, SeedBagEnd);

        EventsManager.BindEvent(EventsManager.ParameterEventType.OnPerformedTendingAction, WateringCanEnd);

    }
    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.ToolBoxOpen, ToolBoxEnd);
        EventsManager.UnbindEvent(EventsManager.EventType.SeedBagOpen, SeedBagEnd);
        EventsManager.UnbindEvent(EventsManager.ParameterEventType.OnPerformedTendingAction, WateringCanEnd);
    }
    // Start is called before the first frame update
    void Start()
    {
        
        //pulseSpeed = 0.01f;
        myImage = this.GetComponent<Image>();
        myAnimator = this.GetComponent<Animator>();
        //myAnimator.speed = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (startPulse)
        {
            myAnimator.SetBool("doesPulse", true);
            startPulse = false;
        }
        if (startDrag)
        {
            myAnimator.SetBool("doesDrag", true);
            startDrag = false;
        }
    }
    void UpdatePulse()
    {
        pulseTimer++;
        /*
        else
        {
            pulseTimer-=0.2f;
        }
        if (pulseTimer > 50)
        {
            pulseTimerForward = false;
        }
        else if (pulseTimer < 1)
        {
            pulseTimerForward = true;
        }
        */
        Color tempColor = myImage.color;
        tempColor.a = ((Mathf.Sin(pulseTimer * pulseSpeed) + 1) / 4f) + 0.5f;
        Debug.Log(pulseTimer * pulseSpeed);
        Debug.Log(tempColor.a);
        myImage.color = tempColor;
    }
    public void SetPurpose(ArrowPurpose purpose)
    {
        myPurpose = purpose;
        UpdateBools();
    }
    private void UpdateBools()
    {
        if (myPurpose != ArrowPurpose.WateringCan)
        {
            startPulse = true;
        }
        else
        {
            startDrag = true;
        }
    }
    void MoodIndicationEnd()
    {
        if (myPurpose == ArrowPurpose.MoodIndicator)
        {
            Deactivate();
        }
    }
    void ToolBoxEnd()
    {
        if (myPurpose == ArrowPurpose.ToolBox)
        {
            Deactivate();
        }
    }
    void SeedBagEnd()
    {
        if (myPurpose == ArrowPurpose.SeedBox)
        {
            Deactivate();
        }
    }
    void WateringCanEnd(EventsManager.EventParams eventParams)
    {
        
        if ((myPurpose == ArrowPurpose.WateringCan))
        {
            Deactivate();
        }
    }
    void Deactivate()
    {
        this.gameObject.SetActive(false);
    }
}
