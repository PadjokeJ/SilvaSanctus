using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public class LevelScreenshotter : MonoBehaviour
{
    Camera ssCam;
    public bool takeScreenShot;
    static LevelScreenshotter instance;
    int files;

    [SerializeField] AllLevelsSO allRooms;

    RenderTexture rT;

    void Awake()
    {
        ssCam = GetComponent<Camera>();
        instance = this;

        rT = ssCam.targetTexture;

        StartCoroutine(SpawnAndScreenshotLevels());
    }

    IEnumerator SpawnAndScreenshotLevels()
    {
        foreach (LevelObject room in allRooms.Levels)
        {
            GameObject obj = Instantiate<GameObject>(room.roomObject);

            Tilemap tm = obj.GetComponentInChildren<Tilemap>();
            Vector3Int size = tm.size;
            ssCam.orthographicSize = size.y / 2f;
            rT.width = size.x * 64;
            rT.height = size.y * 64;

            Vector3 temp = size;

            transform.position = tm.origin + temp * 0.5f + new Vector3Int(0, 0, -10);

            yield return new WaitForEndOfFrame();

            takeScreenShot = true;

            yield return new WaitForSeconds(0.1f);

            Destroy(obj);
        }
    }


    void OnEnable()
    {
        RenderPipelineManager.endCameraRendering += RenderPipelineManager_endCameraRendering;
    }
    void OnDisable()
    {
        RenderPipelineManager.endCameraRendering -= RenderPipelineManager_endCameraRendering;
    }
    void RenderPipelineManager_endCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        OnPostRender();
    }
    void OnPostRender()
    {

        if (takeScreenShot == true)
        {
            string savePath = Application.persistentDataPath + $"/Room{files + 1}.png";
            files++;
            Debug.Log("Getting ready to save" + savePath);
            takeScreenShot = false;


            //take screenshot
            Debug.Log("Rendering image " + savePath);
            Texture2D result = new Texture2D(rT.width, rT.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, rT.width, rT.height);
            result.ReadPixels(rect, 0, 0);
            Debug.Log(savePath + "has been rendered");


            //save screenshot
            Debug.Log("Attempting to save" + savePath);
            byte[] byteArray = result.EncodeToPNG();

            System.IO.File.WriteAllBytes(savePath, byteArray);
            Debug.Log("Saved " + savePath);

            RenderTexture.ReleaseTemporary(rT);
            //ssCam.targetTexture = null;

        }
    }

    void TakeScreenshot(int width, int height)
    {
        takeScreenShot = true;
        ssCam.targetTexture = RenderTexture.GetTemporary(width, height, 16);

        Debug.Log("a");
    }

    private void Update()
    {
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.S))
        {
            TakeScreenshot(160, 160);
        }
        if (ssCam.orthographicSize + Input.mouseScrollDelta.y > 0)
        {
            ssCam.orthographicSize += Input.mouseScrollDelta.y;
        }
    }
    public static void SaveImage(int width, int height)
    {
        instance.TakeScreenshot(width, height);
    }
}
