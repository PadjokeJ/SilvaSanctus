using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public string ObjectType;
    void Awake()
    {
        ObjectType = gameObject.name;

        DontDestroyOnLoad(this.gameObject);

        foreach (DontDestroy dontDestroyObject in FindObjectsOfType<DontDestroy>())
        {
            if (dontDestroyObject.gameObject != this.gameObject && dontDestroyObject.ObjectType == ObjectType)
            {
                Destroy(this.gameObject);
                break;
            } 
        }
    }

}
