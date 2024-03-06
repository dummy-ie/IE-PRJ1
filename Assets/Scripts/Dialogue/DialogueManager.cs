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

    private bool submitPressed;

    void Start()
    {
        dialoguePanel.SetActive(false);
        dialogueIsPlaying = false;
    }

    void Update()
    {
        if (!dialogueIsPlaying)
            return;


        if (GetSubmitPressed())
        {
            ContinueStory();
        }
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


    public void SubmitPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Pressed Stuff");
            submitPressed = true;
        }
        else if (context.canceled)
        {
            submitPressed = false;
        }
    }

    public bool GetSubmitPressed()
    {
        bool result = submitPressed;
        submitPressed = false;
        return result;
    }

    public void RegisterSubmitPressed()
    {
        submitPressed = false;
    }

}
