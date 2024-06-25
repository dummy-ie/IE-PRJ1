using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : Singleton<DialogueManager>
{

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private Story currentStory;

    private bool dialogueIsPlaying;
    public bool IsPlaying
    {
        get { return dialogueIsPlaying; }
    }

    private InputAction _submitAction => InputReader.Instance.InputActions.Gameplay.Submit;

    void Start()
    {
        dialoguePanel.SetActive(false);
        dialogueIsPlaying = false;
    }

    void Update()
    {
        if (_submitAction.WasPressedThisFrame())
        {
            ContinueStory();
        }


        if (!dialogueIsPlaying)
            return;
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
        
    }

    public IEnumerator ExitDialogueMode()
    {

        yield return new WaitForSeconds(.2f);
         
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    public void ContinueStory()
    {

        if (currentStory.canContinue)
            dialogueText.text = currentStory.Continue();
        else
            StartCoroutine(ExitDialogueMode());
    }
}
