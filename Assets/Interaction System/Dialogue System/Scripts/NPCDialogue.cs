using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public AdvancedDialogueSO[] conversation;

    private Transform player;
    private SpriteRenderer speechBubbleRenderer;

    private AdvancedDialogueManager advancedDialogueManager;

    private bool dialogueInitiated;

    // Start is called before the first frame update
    void Start()
    {
        advancedDialogueManager = GameObject.Find("DialogueManager").GetComponent<AdvancedDialogueManager>();
        speechBubbleRenderer = GetComponent<SpriteRenderer>();
        speechBubbleRenderer.enabled = false;
    }

    private void OnTriggerStay(Collider collision)
    {
        if(collision.gameObject.tag == "Player" && !dialogueInitiated)
        {
            //Speech Bubble On
            speechBubbleRenderer.enabled = true;

            //Find the player's transform
            player = collision.gameObject.GetComponent<Transform>();

            advancedDialogueManager.InitiateDialogue(this);
            dialogueInitiated = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //Speech Bubble Off
            speechBubbleRenderer.enabled= false;

            advancedDialogueManager.TurnOffDialogue();
            dialogueInitiated = false;
        }
    }
}