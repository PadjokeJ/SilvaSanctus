using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    private void Update()
    {
        Cursor.visible = false;
        if (Time.timeScale == 0f)
            Cursor.visible = true;
    }
}
