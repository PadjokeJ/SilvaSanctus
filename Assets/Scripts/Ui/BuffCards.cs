using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffCards : MonoBehaviour
{
    public List<BuffScriptableObject> rareBuffs;
    public List<BuffScriptableObject> epicBuffs;
    public List<BuffScriptableObject> legendaryBuffs;

    List<BuffScriptableObject> buffsList = new List<BuffScriptableObject>();

    public Cards[] cards;

    public static BuffCards instance;

    bool active = false;

    public AudioClip selectAudio;

    void Awake()
    {
        instance = this;

        foreach (Cards cards in cards)
        {
            cards.gameObject.SetActive(false);
        }
        
    }

    public void OnClick(Cards card)
    {
        Time.timeScale = 1f;
        active = false;

        Health.playerInstance.maxHealth += card.buff.healthAwarded;
        Health.playerInstance.health += (card.buff.healthPercent) * Health.playerInstance.maxHealth;
        Health.playerInstance.heal(card.buff.healAmmount);

        Buffs.damageBuff += card.buff.attackAwarded;
        Buffs.defense += card.buff.defenseAwarded;


        foreach (Cards cards in cards)
        {
            cards.gameObject.SetActive(false);
        }

        AudioManager.instance.PlayAudio(selectAudio, transform.position, 2f, 0.1f);
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
                if (Random.Range(0f, 1f) < 0.1f)
                    SetCard(card, legendaryBuffs[Random.Range(0, legendaryBuffs.Count)], "legendary");
                else if (Random.Range(0f, 1f) < 0.3f)
                    SetCard(card, epicBuffs[Random.Range(0, epicBuffs.Count)], "epic");
                else
                    SetCard(card, rareBuffs[Random.Range(0, rareBuffs.Count)], "rare");
            }
        }
    }
    void SetCard(Cards card, BuffScriptableObject buff, string rarity)
    {
        card.buffSprite.sprite = buff.buffSprite;

        card.buffHeadline.text = buff.buffName;
        card.buffDescription.text = buff.buffDescription;

        card.buff = buff;

        card.SetRarity(rarity);
    }
}
