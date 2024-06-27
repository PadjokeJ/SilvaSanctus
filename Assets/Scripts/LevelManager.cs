using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    public AllLevelsSO allRooms;

    List<Vector3> corridorsStartPos = new List<Vector3>();
    List<Vector3> corridorsEndPos = new List<Vector3>();
    List<Vector3> corridorsVector = new List<Vector3>();
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

        List<GameObject> rooms = new List<GameObject>();

        

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
                    allRooms.Levels[Random.Range(0, allRooms.Levels.Length)];

                rooms.Add(Instantiate<GameObject>(randomRoom.roomObject, new Vector3(roomX, roomY), Quaternion.identity));

                roomX += 2f // To give some space between rooms
                    + randomRoom.roomObject.GetComponentInChildren<Tilemap>().size.x;
                roomsHeight = Mathf.Max(
                    roomsHeight,
                    randomRoom.roomObject.GetComponentInChildren<Tilemap>().size.y);

                roomsGenerated++;

                // save right facing door
                if (i != gridSize - 1 || roomsGenerated < roomAmmount)
                    corridorsStartPos.Add(randomRoom.roomObject.GetComponent<ListOfDoors>().doors[1].transform.position + new Vector3(roomX, roomY));
                // save left facing door
                if (i != 0)
                    corridorsEndPos.Add(randomRoom.roomObject.GetComponent<ListOfDoors>().doors[3].transform.position + new Vector3(roomX, roomY));
                

                if (roomsGenerated >= roomAmmount) break;
            }

            for (int iteration = 0; iteration < corridorsStartPos.Count; iteration++)
            {
                Debug.Log(iteration);
                corridorsVector.Add(corridorsEndPos[iteration] - corridorsStartPos[iteration]);
            }

            roomY += roomsHeight
                + 2f; //gives space between rooms

            
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        for (int i = 0; i < corridorsVector.Count; i++)
        {
            Gizmos.DrawLine(corridorsStartPos[i], corridorsStartPos[i] + corridorsVector[i]);
        }
    }

}
