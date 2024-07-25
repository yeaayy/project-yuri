using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdvancedDialogueManager : MonoBehaviour
{
    // The NPC Dialogue we are currently stepping through
    private AdvancedDialogueSO currentConversation;
    private int stepNum;
    private bool dialogueActivated;

    // UI References
    private GameObject dialogueCanvas;
    private TMP_Text actor;
    private Image portrait;
    private TMP_Text dialogueText;

    private string currentSpeaker;
    private Sprite currentPortrait;

    public ActorSO[] actorSO;

    // Button References
    private GameObject[] optionButton;
    private TMP_Text[] optionButtonText;
    private GameObject optionsPanel;

    // Typewriter effect
    [SerializeField]
    private float typingSpeed = 0.02f;
    private Coroutine typeWriterRoutine;
    private bool canContinueText = true;

    void Start()
    {
        // Find Buttons
        optionButton = GameObject.FindGameObjectsWithTag("OptionButton");
        optionsPanel = GameObject.Find("OptionsPanel");

        if (optionsPanel == null)
        {
            Debug.LogError("AdvancedDialogueManager: OptionsPanel is not found in the scene.");
            return;
        }

        optionsPanel.SetActive(false);

        // Find the TMP Text on the Buttons
        optionButtonText = new TMP_Text[optionButton.Length];
        for (int i = 0; i < optionButton.Length; i++)
            optionButtonText[i] = optionButton[i].GetComponentInChildren<TMP_Text>();

        // Turn off the buttons to start
        for (int i = 0; i < optionButton.Length; i++)
            optionButton[i].SetActive(false);

        dialogueCanvas = GameObject.Find("DialogueCanvas");
        if (dialogueCanvas == null)
        {
            Debug.LogError("AdvancedDialogueManager: DialogueCanvas is not found in the scene.");
            return;
        }

        actor = GameObject.Find("ActorText")?.GetComponent<TMP_Text>();
        portrait = GameObject.Find("Portrait")?.GetComponent<Image>();
        dialogueText = GameObject.Find("DialogueText")?.GetComponent<TMP_Text>();

        if (actor == null || portrait == null || dialogueText == null)
        {
            Debug.LogError("AdvancedDialogueManager: One or more UI components are not found in the scene.");
            return;
        }

        dialogueCanvas.SetActive(false);
    }

    void Update()
    {
        if (dialogueActivated && Input.GetButtonDown("Interact") && canContinueText)
        {
            // Cancel dialogue if there are no lines of dialogue remaining
            if (stepNum >= currentConversation.actors.Length)
                TurnOffDialogue();
            else
                PlayDialogue();
        }
    }

    void PlayDialogue()
    {
        // If it's a random NPC
        if (currentConversation.actors[stepNum] == DialogueActors.Random)
            SetActorInfo(false);
        // If it's as recurring character
        else
            SetActorInfo(true);

        // Display Dialogue
        actor.text = currentSpeaker;
        portrait.sprite = currentPortrait;

        // If there is a branch...
        if (currentConversation.actors[stepNum] == DialogueActors.Branch)
        {
            for (int i = 0; i < currentConversation.optionText.Length; i++)
            {
                if (currentConversation.optionText[i] == null)
                    optionButton[i].SetActive(false);
                else
                {
                    optionButtonText[i].text = currentConversation.optionText[i];
                    optionButton[i].SetActive(true);
                }

                // Set the first button to be auto-selected
                optionButton[0].GetComponent<Button>().Select();
            }
        }

        // Keep the routine from running multiple times at the same time
        if (typeWriterRoutine != null)
            StopCoroutine(typeWriterRoutine);

        if (stepNum < currentConversation.dialogue.Length)
            typeWriterRoutine = StartCoroutine(TypeWriterEffect(dialogueText.text = currentConversation.dialogue[stepNum]));
        else
            optionsPanel.SetActive(true);

        dialogueCanvas.SetActive(true);
        stepNum += 1;
    }

    void SetActorInfo(bool recurringCharacter)
    {
        if (recurringCharacter)
        {
            for (int i = 0; i < actorSO.Length; i++)
            {
                if (actorSO[i].name == currentConversation.actors[stepNum].ToString())
                {
                    currentSpeaker = actorSO[i].actorName;
                    currentPortrait = actorSO[i].actorPortrait;
                }
            }
        }
        else
        {
            currentSpeaker = currentConversation.randomActorName;
            currentPortrait = currentConversation.randomActorPortrait;
        }
    }

    public void Option(int optionNum)
    {
        foreach (GameObject button in optionButton)
            button.SetActive(false);

        if (optionNum == 0)
            currentConversation = currentConversation.option0;
        if (optionNum == 1)
            currentConversation = currentConversation.option1;
        if (optionNum == 2)
            currentConversation = currentConversation.option2;
        if (optionNum == 3)
            currentConversation = currentConversation.option3;

        stepNum = 0;
    }

    private IEnumerator TypeWriterEffect(string line)
    {
        dialogueText.text = "";
        canContinueText = false;
        yield return new WaitForSeconds(.5f);
        foreach (char letter in line.ToCharArray())
        {
            if (Input.GetButtonDown("Interact"))
            {
                dialogueText.text = line;
                break;
            }
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        canContinueText = true;
    }

    public void InitiateDialogue(NPCDialogue npcDialogue)
    {
        // The array we are currently stepping through
        currentConversation = npcDialogue.conversation[0];

        dialogueActivated = true;
    }

    public void TurnOffDialogue()
    {
        stepNum = 0;

        dialogueActivated = false;
        optionsPanel.SetActive(false);
        dialogueCanvas.SetActive(false);
    }
}

public enum DialogueActors
{
    NPC1,
    Random,
    Branch
};