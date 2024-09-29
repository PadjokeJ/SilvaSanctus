using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Buff")]
public class BuffScriptableObject : ScriptableObject
{
    public Sprite buffSprite;
    public string buffName;
    public string buffDescription;

    public float defenseAwarded;
    public float attackAwarded;
    public float healAmmount;
    public float healthAwarded;
    public float healthPercent;

    public string rarity;
}
