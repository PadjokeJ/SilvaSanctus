using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

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

        Health playerHealth = Health.playerInstance;

        playerHealth.maxHealth += card.buff.healthAwarded;
        playerHealth.health *= (card.buff.healthPercent + 1);
        playerHealth.health += playerHealth.maxHealth * card.buff.maxHealthPercent;
        playerHealth.heal(card.buff.healAmmount);

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
            int index = 0;
            List<BuffScriptableObject> legBuffs = new List<BuffScriptableObject>();
            List<BuffScriptableObject> epBuffs = new List<BuffScriptableObject>();
            List<BuffScriptableObject> raBuffs = new List<BuffScriptableObject>();
            legBuffs = legendaryBuffs.OrderBy(x => Random.value).ToList();
            epBuffs = epicBuffs.OrderBy(x => Random.value).ToList();
            raBuffs = rareBuffs.OrderBy(x => Random.value).ToList();


            foreach (Cards card in cards)
            {
                card.gameObject.SetActive(true);
                if (Random.Range(0f, 1f) < 0.1f)
                    SetCard(card, legBuffs[index], "legendary");
                else if (Random.Range(0f, 1f) < 0.3f)
                    SetCard(card, epBuffs[index], "epic");
                else
                    SetCard(card, raBuffs[index], "rare");
                index++;
            }
        }
    }
    void SetCard(Cards card, BuffScriptableObject buff, string rarity)
    {
        Debug.Log($"Current Language : {LocalizationSettings.SelectedLocale.name}");
        if (LocalizationSettings.SelectedLocale.name.StartsWith("English"))
        {
            card.buffHeadline.text = buff.buffName;
            card.buffDescription.text = buff.buffDescription;
        }
        if (LocalizationSettings.SelectedLocale.name.StartsWith("French"))
        {
            card.buffHeadline.text = buff.buffNameFr;
            card.buffDescription.text = buff.buffDescriptionFr;
        }


        card.buffSprite.sprite = buff.buffSprite;
        card.buff = buff;

        card.SetRarity(rarity);
    }
}
