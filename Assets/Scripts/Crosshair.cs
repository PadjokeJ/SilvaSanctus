using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    private void Update()
    {
        
        if (Time.timeScale == 1f)
            Cursor.visible = false;
        else
            Cursor.visible = true;
    }
}
