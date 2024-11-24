using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorIterator : MonoBehaviour
{
    public LevelManager lm;
    [SerializeField] float waitTime;
    int iteration = 0;
    void Awake()
    {
        StartCoroutine(PeriodicallyGenerate());
    }

    IEnumerator PeriodicallyGenerate()
    {
        while (true)
        {
            lm.generate = true;
            iteration++;
            Debug.Log($"Dungeon generated {iteration} times without overlap");
            yield return new WaitForSecondsRealtime(waitTime);
            foreach (GameObject gO in GameObject.FindGameObjectsWithTag("Barrel"))
                Destroy(gO);
        }
    }
}
