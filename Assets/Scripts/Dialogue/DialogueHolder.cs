using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class DialogueHolder : MonoBehaviour
{

    [Header("Place INK File Here")]
    [SerializeField] private TextAsset inkJSON;


    public void OnInteract()
    {
        if (!DialogueManager.GetInstance().IsPlaying)
        {
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
        }
    }
}
