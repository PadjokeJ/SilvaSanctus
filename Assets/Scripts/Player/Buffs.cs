using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Buffs
{
    public static float damageBuff;
    public static float healthPercent;
    public static float defense;
    
    public static void ResetBuffs()
    {
        damageBuff = 0f;
        healthPercent = 1f;
        defense = 0f;
    }

    public static void HealPlayer(float ammount)
    {
        Health.playerInstance.heal(ammount);
    }
}
