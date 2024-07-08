using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif



public class Chest : MonoBehaviour
{
    PlayerInventory pI;
    PlayerLevelling pL;

    [HideInInspector]
    public string chestType;
    [HideInInspector]
    public int chestTypeId;

    public float coinReward, expReward;

    public GameObject weaponReward;


    private void Awake()
    {
        pI = FindObjectOfType<PlayerInventory>();
        pL = FindObjectOfType<PlayerLevelling>();
    }

    public void GiveRewards()
    {
        pL.gainExperience(expReward);
        if (chestType == "weapon")
        {
            pI.weapons.Add(weaponReward);
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
