using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    public AllLevelsSO allRooms;
    public bool generate;
    public bool destroy;

    List<GameObject> rooms = new List<GameObject>();

    List<Vector3> corridorsRightPos = new List<Vector3>();
    List<Vector3> corridorsLeftPos = new List<Vector3>();
    List<Vector3> corridorsUpPos = new List<Vector3>();
    List<Vector3> corridorsDownPos = new List<Vector3>();
    List<Vector3> corridorsVector = new List<Vector3>();

    GameObject tilemapObject;

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
        float timeToGenerate = Time.realtimeSinceStartup;
        //place rooms in a grid
        DestroyDungeon();

        //calculate grid dimensions (prioritise squares)
        int gridSize = Mathf.FloorToInt(Mathf.Sqrt(roomAmmount));

        int roomsGenerated = 0;
        float roomY = 0;

        int row = 0, height = 0;
        int maxHeight;
        maxHeight = Mathf.FloorToInt(roomAmmount / gridSize);

        LevelObject randomRoom;
        GameObject generatedRoom = new GameObject();
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
                    corridorsLeftPos.Add(generatedRoom.GetComponent<ListOfDoors>().doors[leftIndex].transform.position);
                if (row < gridSize - 1)
                    corridorsRightPos.Add(generatedRoom.GetComponent<ListOfDoors>().doors[rightIndex].transform.position);


                int downIndex = randomRoom.roomDoorsDirection.IndexOf(new Vector2Int(0, -1));
                int upIndex = randomRoom.roomDoorsDirection.IndexOf(new Vector2Int(0, 1));

                if (height != 0)
                    corridorsDownPos.Add(generatedRoom.GetComponent<ListOfDoors>().doors[downIndex].transform.position);
                if (height != maxHeight)
                    corridorsUpPos.Add(generatedRoom.GetComponent<ListOfDoors>().doors[upIndex].transform.position);


                if (roomsGenerated >= roomAmmount) break;
                row++;
            }
            height++;

            

            roomY += roomsHeight
                + spacing; //gives space between rooms

            
        }


        //time to generate corridors!

        tilemapObject = GenerateCorridors(generatedRoom);

        timeToGenerate = Time.realtimeSinceStartup - timeToGenerate;
        Debug.Log("Dungeon took " + timeToGenerate.ToString() + " seconds to generate");

    }

    GameObject GenerateCorridors(GameObject baseTilemapRoom)
    {
        GameObject obj = new GameObject();
        obj.AddComponent<Tilemap>();
        obj.AddComponent<TilemapRenderer>();
        obj.AddComponent<Grid>();
        obj.AddComponent<TilemapCollider2D>();

        obj = Instantiate<GameObject>(obj, Vector3.zero, Quaternion.identity);
        obj.name = "Corridor tilemap";

        Tile wallTile = new Tile();
        Tilemap tilemap = baseTilemapRoom.GetComponentInChildren<Tilemap>();


        // get the type of tile to use for walls
        foreach (var position in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.HasTile(position))
            {
                wallTile = tilemap.GetTile<Tile>(position);
                break;
            }
        }

        tilemap = obj.GetComponent<Tilemap>();

        // break up corridors
        Vector3 startPos, endPos, midPos1, midPos2;
        

        for (int i = 0; i < corridorsRightPos.Count; i++)
        {
            endPos = corridorsLeftPos[i];
            startPos = corridorsRightPos[i];
            midPos1 = (endPos - startPos) / 2 + startPos;
            midPos1 = new Vector3(midPos1.x, startPos.y);

            midPos2 = (startPos - endPos) / 2 + endPos;
            midPos2 = new Vector3(midPos2.x, endPos.y);

            SetTiles(wallTile, tilemap, startPos, midPos1);
            SetTiles(wallTile, tilemap, midPos1, midPos2);
            SetTiles(wallTile, tilemap, midPos2, endPos);
        }
        

        return tilemapObject;

    }
    void SetTiles(Tile wallTile, Tilemap tilemap, Vector3 startPos, Vector3 endPos)
    {
        Vector3 segment;
        segment = endPos - startPos;

        Vector3 displacement;
        displacement = new Vector3(segment.y, segment.x);
        displacement.Normalize();

        for (int j = 0; j < Mathf.CeilToInt(segment.magnitude); j++)
        {
            tilemap.SetTile(Vector3Int.FloorToInt(startPos + segment.normalized * j + displacement), wallTile);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        for (int i = 0; i < corridorsRightPos.Count; i++)
        {
            Vector3 inBetweenRight;
            inBetweenRight = (corridorsRightPos[i] - corridorsLeftPos[i]) / 2 + corridorsLeftPos[i];
            inBetweenRight = new Vector3(inBetweenRight.x, corridorsLeftPos[i].y);
            Gizmos.DrawLine(corridorsLeftPos[i], inBetweenRight);

            Vector3 inBetweenLeft;
            inBetweenLeft = (corridorsLeftPos[i] - corridorsRightPos[i]) / 2 + corridorsRightPos[i];
            inBetweenLeft = new Vector3(inBetweenLeft.x, corridorsRightPos[i].y);
            Gizmos.DrawLine(corridorsRightPos[i], inBetweenLeft);

            Gizmos.DrawLine(inBetweenRight, inBetweenLeft);
            
        }
        for (int i = 0; i < corridorsDownPos.Count; i++)
        {
            Vector3 inBetweenUp;
            inBetweenUp = (corridorsUpPos[i] - corridorsDownPos[i]) / 2 + corridorsDownPos[i];
            inBetweenUp = new Vector3(corridorsDownPos[i].x, inBetweenUp.y);
            Gizmos.DrawLine(corridorsDownPos[i], inBetweenUp);

            Vector3 inBetweenDown;
            inBetweenDown = (corridorsDownPos[i] - corridorsUpPos[i]) / 2 + corridorsUpPos[i];
            inBetweenDown = new Vector3(corridorsUpPos[i].x, inBetweenDown.y);
            Gizmos.DrawLine(corridorsUpPos[i], inBetweenDown);

            Gizmos.DrawLine(inBetweenUp, inBetweenDown);
        }
    }

    void DestroyDungeon()
    {
        foreach(GameObject room in rooms)
        {
            Destroy(room);
        }
        rooms.Clear();
        corridorsRightPos.Clear();
        corridorsLeftPos.Clear();
        corridorsUpPos.Clear();
        corridorsDownPos.Clear();

        if(tilemapObject != null)
            tilemapObject.GetComponentInChildren<Tilemap>().ClearAllTiles();

        Destroy(tilemapObject);
    }

    [ExecuteInEditMode]

    private void OnValidate()
    {
        if(generate)
        {
            GenerateLevel(25, new Vector2(100f, 100f), 5f);
        }
        if (destroy)
        {
            DestroyDungeon();
        }
        generate = false;
        destroy = false;
    }


}
