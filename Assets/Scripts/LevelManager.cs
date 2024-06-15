using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    public AllLevelsSO allRooms;
    void Awake()
    {
        GenerateLevel(20, new Vector2(100f, 100f));

    }
    // Update is called once per frame
    void Update()
    {
        

    }
    void GenerateLevel(int roomAmmount, Vector2 levelSize)
    {
        GenerateLevel(roomAmmount, levelSize, 0, 0);
    }
    void GenerateLevel(int roomAmmount, Vector2 levelSize, float loreRoomProbability, int maxloreRoomAmmount)
    {
        //place rooms in a grid

        //calculate grid dimensions (prioritise squares)
        int gridSize = Mathf.FloorToInt(Mathf.Sqrt(roomAmmount));

        int roomsGenerated = 0;
        float roomY = 0;

        LevelObject randomRoom;
        while(roomsGenerated < roomAmmount)
        {
            float roomX = 0;
            float roomsHeight = 0;
            for(int i = 0; i < gridSize; i++)
            {
                //pick a random room
                randomRoom =
                    allRooms.Levels[Random.Range(0, allRooms.Levels.Length - 1)];
                Instantiate<GameObject>(randomRoom.roomObject, new Vector3(roomX, roomY), Quaternion.identity);

                roomX += 2f // To give some space between rooms
                    + randomRoom.roomObject.GetComponentInChildren<Tilemap>().size.x;
                roomsHeight = Mathf.Max(
                    roomsHeight,
                    randomRoom.roomObject.GetComponentInChildren<Tilemap>().size.y);

                roomsGenerated++;

                if (roomsGenerated >= roomAmmount) break;
            }

            roomY += roomsHeight
                + 2f; //gives space between rooms

            
        }
    }
    
}
