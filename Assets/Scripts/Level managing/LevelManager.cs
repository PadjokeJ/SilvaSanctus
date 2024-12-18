using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    public AllLevelsSO allRooms;
    public bool generate;
    public bool destroy;

    public RuleTile wallTile, floorTile;

    List<GameObject> rooms = new List<GameObject>();

    List<Vector3> corridorsRightPos = new List<Vector3>();
    List<Vector3> corridorsLeftPos = new List<Vector3>();
    List<Vector3> corridorsUpPos = new List<Vector3>();
    List<Vector3> corridorsDownPos = new List<Vector3>();
    List<Vector3> deadEnds = new List<Vector3>();
    List<Vector2Int> doors = new List<Vector2Int>();

    GameObject tilemapObject;

    public int roomsAmmount = 25;
    public float spacingBetweenRooms = 10;

    public GameObject startRoom;
    GameObject spawnedStart;
    public GameObject endRoom;
    GameObject spawnedEnd;

    Transition transition;

    public GameObject bossFightRoomPrefab;

    public Vector3 BossRoomoffset;

    public Material lightMaterial;

    Tilemap floorTilemap;
    GameObject bossRoom;

    void Awake()
    {
        transition = FindAnyObjectByType<Transition>();
        if (transition != null)
            transition.GetComponent<Image>().color = Color.black;

        StartCoroutine(GenerateLevel(roomsAmmount, new Vector2(100f, 100f), spacingBetweenRooms));
        Buffs.ResetBuffs();
    }

    private void FixedUpdate()
    {
        if (generate)
        {
            StartCoroutine(GenerateLevel(roomsAmmount, new Vector2(100f, 100f), spacingBetweenRooms));
            generate = false;
        }
    }
    IEnumerator GenerateLevel(int roomAmmount, Vector2 levelSize, float spacing)
    {
        StartCoroutine(GenerateLevel(roomAmmount, levelSize, spacing, 0f, 0));
        yield return new WaitForEndOfFrame();
    }
    IEnumerator GenerateLevel(int roomAmmount, Vector2 levelSize, float spacing, float loreRoomProbability, int maxloreRoomAmmount)
    {
        yield return new WaitForEndOfFrame();
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
        GameObject dupeObj = generatedRoom;
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
                else
                {
                    deadEnds.Add(generatedRoom.GetComponent<ListOfDoors>().doors[leftIndex].transform.position);
                    doors.Add(new Vector2Int(-1, 0));
                }
                if (row < gridSize - 1)
                    corridorsRightPos.Add(generatedRoom.GetComponent<ListOfDoors>().doors[rightIndex].transform.position);
                else
                {
                    deadEnds.Add(generatedRoom.GetComponent<ListOfDoors>().doors[rightIndex].transform.position);
                    doors.Add(new Vector2Int(1, 0));
                }


                int downIndex = randomRoom.roomDoorsDirection.IndexOf(new Vector2Int(0, -1));
                int upIndex = randomRoom.roomDoorsDirection.IndexOf(new Vector2Int(0, 1));

                if (height != 0)
                    corridorsDownPos.Add(generatedRoom.GetComponent<ListOfDoors>().doors[downIndex].transform.position);
                else
                {
                    deadEnds.Add(generatedRoom.GetComponent<ListOfDoors>().doors[downIndex].transform.position);
                    doors.Add(new Vector2Int(0, -1));
                }
                if (height != maxHeight - 1)
                    corridorsUpPos.Add(generatedRoom.GetComponent<ListOfDoors>().doors[upIndex].transform.position);
                else
                {
                    deadEnds.Add(generatedRoom.GetComponent<ListOfDoors>().doors[upIndex].transform.position);
                    doors.Add(new Vector2Int(0, 1));
                }

                if (roomsGenerated >= roomAmmount) break;
                row++;
            }
            height++;

            

            roomY += roomsHeight * 1.2f + spacing; //gives space between rooms
            roomY = Mathf.CeilToInt(roomY);

            
        }
        Destroy(dupeObj);

        //time to generate corridors!
        Debug.Log("Rooms took " + (Time.realtimeSinceStartup - timeToGenerate).ToString() + " seconds to generate");

        int randomSpawn = Random.Range(0, 2);

        timeToGenerate = Time.realtimeSinceStartup;

        tilemapObject = GenerateCorridors(generatedRoom, randomSpawn);

        Debug.Log("Corridors took " + (Time.realtimeSinceStartup - timeToGenerate).ToString() + " seconds to generate");

        Vector3 startRoomPos;
        Vector2Int startRoomDir;

        Tilemap tilemap = tilemapObject.GetComponent<Tilemap>();

        int otherIndex;
        int index;

        if (randomSpawn == 0)
        {
            index = 1;
            otherIndex = 0;
            startRoomPos = deadEnds[0];
            startRoomDir = new Vector2Int(-1, 0);
        }
        else
        {
            index = 0;
            otherIndex = 1;
            startRoomPos = deadEnds[1];
            startRoomDir = new Vector2Int(0, -1);
        }
        

        spawnedStart = Instantiate<GameObject>(startRoom, Vector3.zero, Quaternion.identity);
        
        spawnedStart.transform.position = startRoomPos - spawnedStart.GetComponent<ListOfDoors>().doors[index].transform.position;

        Vector3 otherStartDoor;
        otherStartDoor = spawnedStart.GetComponent<ListOfDoors>().doors[otherIndex].transform.position;

        Vector3Int startTileOffset;
        startTileOffset = new Vector3Int(startRoomDir.x, startRoomDir.y);

        BlockEntrance(otherStartDoor, wallTile, tilemap);


        Vector3 endRoomPos;
        Vector2Int endRoomDir;

        int lastIndex = deadEnds.Count - 1;
        
        index = 1;
        otherIndex = 0;
        endRoomPos = deadEnds[lastIndex];
        endRoomDir = new Vector2Int(1, 0);


        bossRoom = Instantiate<GameObject>(bossFightRoomPrefab, Vector3.zero, Quaternion.identity);

        bossRoom.transform.position = endRoomPos - BossRoomoffset;

        /*else
        {
            index = 0;
            otherIndex = 1;
            endRoomPos = deadEnds[lastIndex];
            endRoomDir = new Vector2Int(0, 1);
        }*/

        /*spawnedEnd = Instantiate<GameObject>(endRoom, Vector3.zero, Quaternion.identity);

        spawnedEnd.transform.position = endRoomPos - spawnedEnd.GetComponent<ListOfDoors>().doors[index].transform.position;

        Vector3 otherEndDoor;
        otherEndDoor = spawnedEnd.GetComponent<ListOfDoors>().doors[otherIndex].transform.position;

        Vector3Int endTileOffset;
        endTileOffset = new Vector3Int(endRoomDir.x, endRoomDir.y);

        BlockEntrance(otherEndDoor, wallTile, tilemap);*/

        yield return new WaitForSecondsRealtime(0.5f);
        if (transition != null)
            transition.FadeToWhite();
        PlayerLevelling.InitDeltaLevel();
    }

    GameObject GenerateCorridors(GameObject baseTilemapRoom, int randomSpawn)
    {
        GameObject obj = new GameObject();
        Tilemap tilemap = obj.AddComponent<Tilemap>();
        TilemapRenderer tmRenderer = obj.AddComponent<TilemapRenderer>();
        tmRenderer.material = lightMaterial;
        obj.AddComponent<Grid>();
        

        obj.name = "Corridor tilemap";

        GameObject floorObj = new GameObject();
        floorTilemap = floorObj.AddComponent<Tilemap>();

        TilemapRenderer ftmRenderer = floorObj.AddComponent<TilemapRenderer>();
        ftmRenderer.sortingOrder = -2;
        ftmRenderer.material = lightMaterial;

        floorObj.AddComponent<Grid>();

        floorObj.name = "Corridor Floor tilemap";

        

        // break up corridors
        Vector3 startPos, endPos, midPos1, midPos2;
        

        for (int i = 0; i < corridorsRightPos.Count; i++)
        {
            endPos = corridorsLeftPos[i];
            startPos = corridorsRightPos[i];
            midPos1 = Vector3Int.FloorToInt((endPos - startPos) / 2) + startPos;
            midPos1 = new Vector3(midPos1.x, startPos.y);

            midPos2 = Vector3Int.FloorToInt((startPos - endPos) / 2) + endPos;
            midPos2 = new Vector3(midPos2.x, endPos.y);

            SetSquareTiles(wallTile, tilemap, startPos, midPos1 + new Vector3(3, 0), 6);
            SetSquareTiles(wallTile, tilemap, midPos1, midPos2, 6);
            SetSquareTiles(wallTile, tilemap, midPos2 - new Vector3(3, 0), endPos, 6);

            SetSquareTiles(floorTile, floorTilemap, startPos, midPos1 + new Vector3(3, 0), 6);
            SetSquareTiles(floorTile, floorTilemap, midPos1, midPos2, 6);
            SetSquareTiles(floorTile, floorTilemap, midPos2 - new Vector3(3, 0), endPos, 6);

            SetSquareTiles(null, tilemap, startPos, midPos1 + new Vector3(1, 0), 2);
            SetSquareTiles(null, tilemap, midPos1, midPos2, 2);
            SetSquareTiles(null, tilemap, midPos2 - new Vector3(1, 0), endPos, 2);
        }

        for (int i = 0; i < corridorsDownPos.Count; i++)
        {
            endPos = corridorsDownPos[i];
            startPos = corridorsUpPos[i];

            midPos1 = Vector3Int.FloorToInt((endPos - startPos) / 2) + startPos;
            midPos1 = new Vector3(startPos.x, midPos1.y);

            midPos2 = Vector3Int.FloorToInt((startPos - endPos) / 2) + endPos;
            midPos2 = new Vector3(endPos.x, midPos2.y);

            Vector3 hor1 = midPos1;
            Vector3 hor2 = midPos2;
            if (endPos.x > startPos.x)
            {
                hor1 = midPos2;
                hor2 = midPos1;
            }

            SetSquareTiles(wallTile, tilemap, startPos, midPos1 + new Vector3(0, 3), 6);
            SetSquareTiles(wallTile, tilemap, hor1 + new Vector3(1, -1), hor2 - new Vector3(3, 1), 6);
            SetSquareTiles(wallTile, tilemap, midPos2 - new Vector3(0, 3), endPos, 6);

            SetSquareTiles(floorTile, floorTilemap, startPos, midPos1 + new Vector3(0, 3), 6);
            SetSquareTiles(floorTile, floorTilemap, hor1 + new Vector3(1, -1), hor2 - new Vector3(3, 1), 6);
            SetSquareTiles(floorTile, floorTilemap, midPos2 - new Vector3(0, 3), endPos, 6);

            SetSquareTiles(null, tilemap, startPos - new Vector3(0, 1), midPos1 + new Vector3(0, 1), 2);
            SetSquareTiles(null, tilemap, hor1 - new Vector3(0, 1), hor2 - new Vector3(2, 1), 2);
            SetSquareTiles(null, tilemap, midPos2 - new Vector3(0, 1), endPos + new Vector3(0, 1), 2);

        }

        int iteration = 0;
        int maxIteration = deadEnds.Count - 1;

        bool canGen = true;
        foreach(Vector3 position in deadEnds)
        {
            canGen = iteration != randomSpawn && maxIteration - iteration != 0; // checks if the iteration is equal to the door that will get blocked  

            if (canGen) // checks if the deadend is horizontal
            {
                BlockEntrance(position, wallTile, tilemap);
            }
            iteration++;
        }

        obj.AddComponent<TilemapCollider2D>();

        return obj;

    }

    void BlockEntrance(Vector3 position, RuleTile tile, Tilemap tilemap)
    {
        tilemap.SetTile(Vector3Int.CeilToInt(position) + new Vector3Int(0, 0), tile);
        tilemap.SetTile(Vector3Int.CeilToInt(position) + new Vector3Int(0, -1), tile);
        tilemap.SetTile(Vector3Int.CeilToInt(position) + new Vector3Int(-1, 0), tile);
        tilemap.SetTile(Vector3Int.CeilToInt(position) + new Vector3Int(-1, -1), tile);
    }    

    void SetSquareTiles(RuleTile wallTile, Tilemap tm, Vector3 startPos, Vector3 endpos, int thickness)
    {
        for(int i = 0; i < thickness; i++)
            SetTiles(wallTile, tm, startPos, endpos, i - Mathf.FloorToInt(thickness/2));
    }
    void SetTiles(RuleTile wallTile, Tilemap tilemap, Vector3 startPos, Vector3 endPos, int displaceMult)
    {
        Vector3 segment;
        segment = endPos - startPos;

        Vector3 displacement;
        displacement = new Vector3(segment.normalized.y * displaceMult, segment.normalized.x * displaceMult);


        for (int j = 0; j < Mathf.CeilToInt(segment.magnitude); j++)
        {
            tilemap.SetTile(Vector3Int.FloorToInt(displacement + startPos + segment.normalized * j), wallTile);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        foreach(GameObject room in rooms)
        {
            Gizmos.DrawWireSphere(room.transform.position, 5);
        }
        Gizmos.color = Color.green;
        foreach (GameObject room in rooms)
        {
            Tilemap mainTilemap = room.GetComponentInChildren<Tilemap>();
            List<Vector3> v3s = new List<Vector3>();            
            
            Gizmos.DrawWireSphere(room.transform.position + mainTilemap.origin, 1);
            Gizmos.DrawWireSphere(room.transform.position + mainTilemap.origin + mainTilemap.size, 1);
            Gizmos.DrawWireSphere(room.transform.position + mainTilemap.origin + new Vector3(mainTilemap.size.x, 0), 1);
            Gizmos.DrawWireSphere(room.transform.position + mainTilemap.origin + new Vector3(0, mainTilemap.size.y), 1);
        }
        Gizmos.color = Color.white;
        for (int i = 0; i < corridorsRightPos.Count; i++)
        {
            Vector3 inBetweenRight;
            inBetweenRight = Vector3Int.FloorToInt((corridorsRightPos[i] - corridorsLeftPos[i]) / 2) + corridorsLeftPos[i];
            inBetweenRight = new Vector3(inBetweenRight.x, corridorsLeftPos[i].y);
            Gizmos.DrawLine(corridorsLeftPos[i], inBetweenRight);

            Vector3 inBetweenLeft;
            inBetweenLeft = Vector3Int.FloorToInt((corridorsLeftPos[i] - corridorsRightPos[i]) / 2) + corridorsRightPos[i];
            inBetweenLeft = new Vector3(inBetweenLeft.x, corridorsRightPos[i].y);
            Gizmos.DrawLine(corridorsRightPos[i], inBetweenLeft);

            Gizmos.DrawLine(inBetweenRight, inBetweenLeft);
            
        }
        for (int i = 0; i < corridorsDownPos.Count; i++)
        {
            Vector3 inBetweenUp;
            inBetweenUp = Vector3Int.FloorToInt((corridorsUpPos[i] - corridorsDownPos[i]) / 2) + corridorsDownPos[i];
            inBetweenUp = new Vector3(corridorsDownPos[i].x, inBetweenUp.y);
            Gizmos.DrawLine(corridorsDownPos[i], inBetweenUp);

            Vector3 inBetweenDown;
            inBetweenDown = Vector3Int.FloorToInt((corridorsDownPos[i] - corridorsUpPos[i]) / 2) + corridorsUpPos[i];
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
        deadEnds.Clear();

        if(tilemapObject != null)
            tilemapObject.GetComponentInChildren<Tilemap>().ClearAllTiles();
        if (floorTilemap != null)
        {
            floorTilemap.ClearAllTiles();
            Destroy(floorTilemap);
        }

        if (bossRoom != null)
            Destroy(bossRoom);

        Destroy(tilemapObject);
        Destroy(spawnedStart);
        Destroy(spawnedEnd);
    }

    [ExecuteInEditMode]

    private void OnValidate()
    {
        if(generate)
        {
            StartCoroutine(GenerateLevel(roomsAmmount, new Vector2(100f, 100f), spacingBetweenRooms));
        }
        if (destroy)
        {
            DestroyDungeon();
        }
        generate = false;
        destroy = false;
    }


}
