using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif



public class Chest : MonoBehaviour
{
    PlayerInventory pI;

    [HideInInspector]
    public string chestType;
    [HideInInspector]
    public int chestTypeId;

    public float coinReward, expReward;

    public GameObject weaponReward;


    private void Awake()
    {
        pI = FindObjectOfType<PlayerInventory>();
    }

    public void GiveRewards()
    {
        PlayerLevelling.GainExperience(expReward);
        if (chestType == "weapon")
        {
            pI.AddWeapon(weaponReward);
            PlayerLevelling.GainExperience(expReward);
        }
        if (chestType == "coins")
        {
            
        }
        Destroy(this.gameObject);
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(Chest))]
public class ChestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var ChestScript = (Chest)target;

        string[] options = new string[]
        {
            "weapon", "coins"
        };
        ChestScript.chestTypeId = EditorGUILayout.Popup("ChestType", ChestScript.chestTypeId, options);
        ChestScript.chestType = options[ChestScript.chestTypeId];

        base.OnInspectorGUI();
    }
}
#endif
