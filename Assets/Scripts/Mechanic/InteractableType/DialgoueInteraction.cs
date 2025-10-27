using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DialgoueInteraction : InteractableBase
{
    [SerializeField]
    List<DialogueNode> dialogueNodes;

    bool alreadyDisplayed = false;

    public override void Interact()
    {
        if (alreadyDisplayed) return;
        DialogueSystem.Instance.
            SetDialogueList(dialogueNodes).
            StartDialogue();
        alreadyDisplayed = true;
    }    
}
