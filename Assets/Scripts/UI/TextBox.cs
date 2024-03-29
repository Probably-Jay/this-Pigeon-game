﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

//created Zap 28/03
public class TextBox : MonoBehaviour
{
    private List<string> thingsToSay = new List<string> { };
    [SerializeField] private TMP_Text myText;
    private int listFocus;
    public GameObject backButton;
    public GameObject forwardButton;
    [SerializeField] private Text forwardButtonText;
    // Start is called before the first frame update
    void OnEnable()
    {
        //forwardButtonText = forwardButton.GetComponentInChildren<Text>();
        //myText=this.GetComponentInChildren<Text>();
        EventsManager.BindEvent(EventsManager.EventType.DialogueSkip, SkipToEnd);
        EventsManager.BindEvent(EventsManager.EventType.DialogueNext, NextWords);
        EventsManager.BindEvent(EventsManager.EventType.DialoguePrevious, PreviousWords);
        EventsManager.BindEvent(EventsManager.EventType.EnterPlayingState, Close);
        EventsManager.BindEvent(EventsManager.EventType.EnterSpectatingState, Close);
        UpdateButtons();
    }



    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.DialogueSkip, SkipToEnd);
        EventsManager.UnbindEvent(EventsManager.EventType.DialogueNext, NextWords);
        EventsManager.UnbindEvent(EventsManager.EventType.DialoguePrevious, PreviousWords);
        EventsManager.UnbindEvent(EventsManager.EventType.EnterPlayingState, Close);
        EventsManager.UnbindEvent(EventsManager.EventType.EnterSpectatingState, Close);
    }
 

    // Update is called once per frame
    void Update()
    {
        if (thingsToSay.Count > 0)
        {
            myText.text = thingsToSay[listFocus];
        }
        
    }
    public void Say(string content)
    {
        thingsToSay.Add(content);
        myText.gameObject.SetActive(true);
        UpdateButtons();

        EventsManager.InvokeEvent(EventsManager.EventType.OnDialogueOpen);
    }
    void SkipToEnd()
    {
        listFocus = thingsToSay.Count - 1;
        NextWords();
    }

    void NextWords()
    {
        if (listFocus + 1 >= thingsToSay.Count)
        {
            myText.gameObject.SetActive(false);
            thingsToSay.Clear();
            listFocus = 0;
            this.gameObject.SetActive(false);
            EventsManager.InvokeEvent(EventsManager.EventType.OnDialogueClose);
        }
        else
        {
            listFocus++;
            UpdateButtons();
        }
    }
    void PreviousWords()
    {
        if (listFocus > 0)
        {
            listFocus--;
            UpdateButtons();
        }
    }
    void UpdateButtons()
    {
        if (listFocus != 0)
        {       
            backButton.SetActive(true);
        }
        else
        {
            backButton.SetActive(false);
        }

        if (listFocus == thingsToSay.Count - 1)
        {
            forwardButtonText.text = ("Close");
        }
        else
        {
            forwardButtonText.text = ("Okay");
        }
    }
    private void Close()
    {
        SkipToEnd();
    }
}
