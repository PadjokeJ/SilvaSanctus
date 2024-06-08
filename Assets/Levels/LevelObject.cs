using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Default room")]
public class LevelObject : ScriptableObject
{
    public bool loreRoom;
    public float difficulty;
    public int level;
    public int roomID;
    public float rarity;
    public string roomName;
    public int roomTypeID;
    public string roomType;

    public Vector2Int roomSize;
    public GameObject roomObject;

    public List<GameObject> roomDoors;
    public List<Vector2Int> roomDoorsDirection;
    public bool isStartingRoom = false;
    public int exitCount = 4;
    public Vector2 roomExit;
    public Vector2Int roomExitDirection;

    
}
/*
#if UNITY_EDITOR
[CustomEditor(typeof(LevelObject))]
public class LOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var lO = (LevelObject)target;

        lO.roomName = EditorGUILayout.TextField(lO.roomName);
        lO.roomID = EditorGUILayout.IntField(lO.roomID);
        EditorGUILayout.Space();

        lO.level = EditorGUILayout.IntField("Floor Level", Mathf.Clamp(lO.level, 0, 99999));
        EditorGUILayout.Space();

        lO.roomObject = (GameObject)EditorGUILayout.ObjectField("Room Object", lO.roomObject, typeof(GameObject), true);
        EditorGUILayout.Space();

        lO.loreRoom = GUILayout.Toggle(lO.loreRoom, "Is lore room?");

        if (lO.loreRoom == false)
        {
            lO.difficulty = EditorGUILayout.Slider("Difficulty", lO.difficulty, 0f, 100f);
            lO.rarity = EditorGUILayout.Slider("Rarity", lO.rarity, 0f, 100f);
            string[] options = new string[]
            {
                "Combat", "Scenery", "Shop", "Story", "Shrine"
            };
            lO.roomTypeID = EditorGUILayout.Popup("Room type", lO.roomTypeID, options);
        }
        EditorGUILayout.Space();

        lO.roomSize = EditorGUILayout.Vector2IntField("Room size", lO.roomSize);
        EditorGUILayout.Space();

        lO.isStartingRoom = GUILayout.Toggle(lO.isStartingRoom, "Is starting room?");
        if (lO.isStartingRoom)
            lO.roomExit = EditorGUILayout.Vector2Field("Entrance position", lO.roomExit);
        EditorGUILayout.Space();

        int oldCount = lO.exitCount;
        lO.exitCount = EditorGUILayout.IntField("Number of exits", Mathf.Clamp(lO.exitCount, 0, 99999));
        for (int i = 0; i < Mathf.Clamp(lO.exitCount-oldCount, 0, 99999); i++)
        {
            //lO.roomDoors.Add(new GameObject());
            lO.roomDoorsDirection.Add(new Vector2Int(0, 0));
        }
        for (int i = 0; i< lO.exitCount; i++)
        {
            //lO.roomDoors[i] = (GameObject)EditorGUILayout.ObjectField("Exit Object", lO.roomDoors[i], typeof(GameObject), true);
            lO.roomDoorsDirection[i] = EditorGUILayout.Vector2IntField("Door Direction", lO.roomDoorsDirection[i]);
        }
    }
}
#endif
*/