using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Use the Selectable class as a base class to access the IsHighlighted method
public class Cards : Selectable
{
    public Image buffSprite;
    public TextMeshProUGUI buffHeadline;
    public TextMeshProUGUI buffDescription;

    public BuffScriptableObject buff;
    //Use this to check what Events are happening
    BaseEventData m_BaseEvent;

    bool isHighlighted;

    public AudioClip cardReveal;

    void Update()
    {
        if (IsHighlighted() && isHighlighted != IsHighlighted())
            OnHighlight();
        isHighlighted = IsHighlighted();
    }

    void OnHighlight()
    {
        Debug.Log("highlited this!!!");
        AudioManager.instance.PlayAudio(cardReveal, transform.position, 2f, 0.2f);
    }
}
