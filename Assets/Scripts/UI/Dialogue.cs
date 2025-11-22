using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;
using System;
public class Dialogue : MonoBehaviour, InputSystem_Actions.IUIActions
{
    private InputSystem_Actions inputActions;

    public TextMeshProUGUI dialogueText;
    public string[] IntroDialoguelines;
    public string[] FinalWinDialoguelines;
    public string[] FinalLooseDialoguelines;
    private string[] currentLines;
    public float textSpeed;

    private bool canType = true;
    private bool canBeSkipped = true;
    private int index;

    public static bool hasIntroHappened = false;

    public static event Action OnIntroDialogueEnded;
    public static event Action OnEndDialogueEnded;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.UI.SetCallbacks(this);

        gameObject.SetActive(false);

        BaseMenu.OnPauseStateChanged += HandlePauseStateChanged;
        BaseMenu.OnRestartChosen += RestartDialogueState;
        Player.OnIntroDialogueTriggered += StartIntroDialogue;
        Player.OnEndDialogueTriggered += StartEndDialogue;
    }
    private void OnEnable() => inputActions.UI.Enable();
    private void OnDisable() => inputActions.UI.Disable();

    private void StartDialogue(string[] lines)
    {
        index = 0;
        gameObject.SetActive(true);
        StartCoroutine(TypeLine(lines));
    }
    private IEnumerator TypeLine(string[] lines)
    {
        foreach(char c in lines[index])
        {
            if (canType)
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(textSpeed);
            }
        }
    }
    private void NextLine(string[] lines)
    {
        if (index < lines.Length - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeLine(lines));
        }
        else
        {
            if (!hasIntroHappened)
            {
                hasIntroHappened = true;
                OnIntroDialogueEnded.Invoke();
            }
            else OnEndDialogueEnded.Invoke();

            gameObject.SetActive(false);
        }
    }
    private void SkipLine(string[] lines)
    {
        if (dialogueText.text == lines[index]) NextLine(lines);
        else
        {
            StopAllCoroutines();
            dialogueText.text = lines[index];
        }
    }

    // Not needed here
    public void OnPause(InputAction.CallbackContext context) { }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (canBeSkipped && canType && context.performed)
        {
            canBeSkipped = false;
            SkipLine(currentLines);
        }
        else if (context.canceled) { canBeSkipped = true; }
    }

    // ----- DELEGATES -----

    private void HandlePauseStateChanged(bool isPaused)
    {
        if (isPaused) canType = false;
        else canType = true;
    }
    private void RestartDialogueState()
    {
        index = 0;
        hasIntroHappened = false;
    }
    private void StartIntroDialogue()
    {
        dialogueText.text = string.Empty;
        currentLines = IntroDialoguelines;
        StartDialogue(currentLines);
    }
    private void StartEndDialogue()
    {
        dialogueText.text = string.Empty;
        if (Player.didWin) currentLines = FinalWinDialoguelines;
        else currentLines = FinalLooseDialoguelines;
        StartDialogue(currentLines);
    }
}