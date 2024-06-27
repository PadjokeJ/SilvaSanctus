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
        GenerateLevel(25, new Vector2(100f, 100f), 5f);

    }
    // Update is called once per frame
    void Update()
    {
        

    }
    void GenerateLevel(int roomAmmount, Vector2 levelSize, float spacing)
    {
        GenerateLevel(roomAmmount, levelSize, spacing, 0f, 0);
    }
    void GenerateLevel(int roomAmmount, Vector2 levelSize, float spacing, float loreRoomProbability, int maxloreRoomAmmount)
    {
        //place rooms in a grid

        List<GameObject> rooms = new List<GameObject>();

        

        //calculate grid dimensions (prioritise squares)
        int gridSize = Mathf.FloorToInt(Mathf.Sqrt(roomAmmount));

        int roomsGenerated = 0;
        float roomY = 0;

        int row = 0, height = 0;

        LevelObject randomRoom;
        GameObject generatedRoom;
        while(roomsGenerated < roomAmmount)
        {
            float roomX = 0;
            float roomsHeight = 0;
            row = 0;
            for(int i = 0; i < gridSize; i++)
            {
                //pick a random room
                randomRoom =
                    allRooms.Levels[Random.Range(0, allRooms.Levels.Length)];


                generatedRoom = Instantiate<GameObject>(randomRoom.roomObject, new Vector3(roomX, roomY), Quaternion.identity);
                rooms.Add(generatedRoom);

                roomX += spacing // To give some space between rooms
                    + randomRoom.roomObject.GetComponentInChildren<Tilemap>().size.x;
                roomsHeight = Mathf.Max(
                    roomsHeight,
                    randomRoom.roomObject.GetComponentInChildren<Tilemap>().size.y);

                roomsGenerated++;

                // find the door index in the list
                int leftIndex = randomRoom.roomDoorsDirection.IndexOf(new Vector2Int(-1, 0));
                int rightIndex = randomRoom.roomDoorsDirection.IndexOf(new Vector2Int(1, 0));

                if (row != 0)
                    corridorsEndPos.Add(generatedRoom.GetComponent<ListOfDoors>().doors[leftIndex].transform.position);
                if (row < gridSize - 1)
                    corridorsStartPos.Add(generatedRoom.GetComponent<ListOfDoors>().doors[rightIndex].transform.position);

                if (roomsGenerated >= roomAmmount) break;
                row++;
            }
            height++;

            

            roomY += roomsHeight
                + spacing; //gives space between rooms

            
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        for (int i = 0; i < corridorsStartPos.Count; i++)
        {
            Gizmos.DrawLine(corridorsStartPos[i], corridorsEndPos[i]);
        }
    }


}
