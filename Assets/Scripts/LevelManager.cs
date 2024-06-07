using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelManager : MonoBehaviour
{
    LevelObject currentRoom;
    public AllLevelsSO allRooms;
    List<LevelObject> roomIndex;
    List<LevelObject> loreRooms;
    List<LevelObject> normalRooms;
    List<LevelObject> genRooms;
    List<Vector2> roomPos;
    Vector2 StartRoomExitPos;
    Vector2Int StartRoomExitDir;
    List<Vector2Int> Dirs;

    int hasGenerated = 0;
    void Awake()
    {
        roomIndex = new List<LevelObject>(allRooms.Levels);
        IndexRooms(out loreRooms, out normalRooms);
        
        StartRoomExitPos = this.transform.position;
        StartRoomExitDir = new Vector2Int(0, 1);

        Dirs = new List<Vector2Int>();
        Dirs.Add(new Vector2Int(0, 1));
        Dirs.Add(new Vector2Int(-1, 0));
        Dirs.Add(new Vector2Int(0, -1));
        Dirs.Add(new Vector2Int(-1, 0));
        GenerateLevel(10, new Vector2(50, 50));
    }

    // Update is called once per frame
    void Update()
    {
        

    }
    public void GenerateLevel(int roomAmmount, Vector2 levelSize)
    {
        GenerateLevel(roomAmmount, levelSize, 0, 0);
    }
    public void GenerateLevel(int roomAmmount, Vector2 levelSize, float loreRoomProbability, int maxloreRoomAmmount)
    {
        List<Vector2> exits;
        List<Vector2> newExits;
        List<Vector2> tempExits;
        tempExits = new List<Vector2>();
        exits = new List<Vector2>();
        newExits = new List<Vector2>();
        exits.Add(StartRoomExitPos);
        Vector2 entryCoords;

        List<Vector2Int> exitDir;
        List<Vector2Int> newExitDir;
        List<Vector2Int> tempDir;
        tempDir = new List<Vector2Int>();
        exitDir = new List<Vector2Int>();
        newExitDir = new List<Vector2Int>();
        exitDir.Add(StartRoomExitDir);
        int entryDirToPick;
        

        int room;
        LevelObject roomObject;
        List<GameObject> roomObjectDoors;
        roomObjectDoors = new List<GameObject>();

        int crashPrevention = 0;
        List<Vector2> roomsGenerated = new List<Vector2>();
        List<Vector2> sizesGenerated = new List<Vector2>();

        for (int i = 0; i < roomAmmount; i++)
        {
            crashPrevention++;
            newExits.Clear();
            newExitDir.Clear();
            tempExits.Clear();
            tempDir.Clear();
            int j = 0;
            foreach (Vector2 exit in exits)
            {
                room = Random.Range(0, roomIndex.Count);
                roomObject = roomIndex[room];
                roomObjectDoors = IndexDoors(roomObject.roomObject);

                Vector2Int tempVector = exitDir[j] * -1;
                entryDirToPick = roomObject.roomDoorsDirection.IndexOf(tempVector);
                entryCoords = roomObjectDoors[entryDirToPick].transform.position;

                GameObject level = Instantiate<GameObject>(roomObject.roomObject, exit - entryCoords, Quaternion.identity);
                level.name = "Combat" + roomsGenerated.ToString();

                
                tempExits = new List<Vector2>(GameObjectToVector2(roomObjectDoors));
                tempExits.RemoveAt(tempExits.IndexOf(entryCoords));
                for(int k = 0; k < tempExits.Count; k++)
                {
                    tempExits[k] = tempExits[k] + exit - entryCoords;
                    Debug.Log(tempExits[k]);
                }
                tempDir = new List<Vector2Int>(roomObject.roomDoorsDirection);
                if (tempDir.IndexOf(roomObject.roomDoorsDirection[entryDirToPick]) != -1)
                tempDir.RemoveAt(tempDir.IndexOf(roomObject.roomDoorsDirection[entryDirToPick]));
                

                newExits.AddRange(tempExits);
                newExitDir.AddRange(tempDir);

                roomsGenerated.Add(level.transform.position);
                sizesGenerated.Add(roomObject.roomSize);

                if (roomsGenerated.Count >= roomAmmount) break;
                
            }

            if (roomsGenerated.Count >= roomAmmount) break;
            exits = new List<Vector2>(newExits);
            exitDir = new List<Vector2Int>(newExitDir);
            Debug.Log(exits.Count);

            if(crashPrevention > 500)
            {
                Debug.LogError("Error generating dungeon!!!!!!");
                break;
            }
        }
    }
    void IndexRooms(out List<LevelObject> lR, out List<LevelObject> nR)
    {
        lR = new List<LevelObject>();
        nR = new List<LevelObject>();
        foreach (LevelObject r in roomIndex)
        {
            print(r);
            if (r.loreRoom)
            { 
                lR.Add(r); 
            }
            else
            {
                nR.Add(r);
            }
        }
    }
    int Warp(int num, int min, int max)
    {
        if (num < min) num += min;
        if (num > max) num -= max;
        return num;
    }

    List<Vector2> GameObjectToVector2(List<GameObject> input)
    {
        List<Vector2> newList;
        newList = new List<Vector2>();

        int i = 0;
        foreach(GameObject obj in input)
        {
            newList.Add(new Vector2(obj.transform.position.x, obj.transform.position.y));
            i++;
        }
        return newList;
    }
    List<GameObject> IndexDoors(GameObject input)
    {
        List<GameObject> listOfChildren;
        listOfChildren = new List<GameObject>();

        foreach(Transform child in input.transform)
        {
            if (child.gameObject.CompareTag("Door")) listOfChildren.Add(child.gameObject);
        }

        return listOfChildren;
    }
}
