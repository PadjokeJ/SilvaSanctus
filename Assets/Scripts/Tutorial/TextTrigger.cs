using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTrigger : MonoBehaviour
{
    public string textToShow;
    private void OnTriggerEnter2D(Collider2D colldier)
    {
        TutorialText.instance.PlayText(textToShow);
    }
}
