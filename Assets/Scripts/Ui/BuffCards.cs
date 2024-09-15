using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffCards : MonoBehaviour
{
    public List<BuffScriptableObject> buffsList;

    public Cards[] cards;

    public static BuffCards instance;

    bool active = false;

    void Awake()
    {
        instance = this;

        foreach (Cards cards in cards)
        {
            Debug.Log(cards.gameObject.name);
            cards.gameObject.SetActive(false);
        }
        
    }

    public void OnClick(Cards card)
    {
        Time.timeScale = 1f;
        active = false;

        Health.playerInstance.heal(card.buff.healAmmount);
        Health.playerInstance.health += card.buff.healthAwarded;

        Buffs.damageBuff += card.buff.attackAwarded;
        Buffs.defense += card.buff.defenseAwarded;


        foreach (Cards cards in cards)
        {
            cards.gameObject.SetActive(false);
        }
    }
    public void EnableCards()
    {
        if (!active)
        {
            Time.timeScale = 0f;
            active = true;

            foreach (Cards card in cards)
            {
                card.gameObject.SetActive(true);
                SetCard(card, buffsList[Random.Range(0, buffsList.Count)]);
            }
        }
    }
    void SetCard(Cards card, BuffScriptableObject buff)
    {
        card.buffSprite.sprite = buff.buffSprite;

        card.buffHeadline.text = buff.buffName;
        card.buffDescription.text = buff.buffDescription;

        card.buff = buff;
    }
}
