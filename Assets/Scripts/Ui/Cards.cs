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
    public Sprite rareBackground;
    public Sprite epicBackground;
    public Sprite legendaryBackground;


    public Image buffSprite;
    public TextMeshProUGUI buffHeadline;
    public TextMeshProUGUI buffDescription;
    public TextMeshProUGUI buffRarity;

    public BuffScriptableObject buff;
    //Use this to check what Events are happening
    BaseEventData m_BaseEvent;

    bool isHighlighted;

    public AudioClip cardReveal;

    public string rarity;

    Image background;


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

    public void SetRarity(string _rarity)
    {
        rarity = _rarity;
        background = GetComponent<Image>();

        if (rarity == "rare")
            background.sprite = rareBackground;
        if (rarity == "epic")
            background.sprite = epicBackground;
        if (rarity == "legendary")
            background.sprite = legendaryBackground;
    }
}
