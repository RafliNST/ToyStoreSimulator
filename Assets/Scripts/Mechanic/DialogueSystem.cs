using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.InputSystem;
using System.Linq;

public class DialogueSystem : Singleton<DialogueSystem>
{
    private bool IsDialogueShowed
    {
        get => dialogueCanvas.enabled;
        set => dialogueCanvas.enabled = value;
    }

    #region Dialogue System Setup
    [SerializeField]
    Canvas dialogueCanvas;

    [SerializeField, Range(.05f, .5f)]
    float textSpeed;

    [SerializeField]
    public List<DialogueNode> dialogueList;

    [SerializeField]
    TMPro.TextMeshProUGUI speakerName, speakerDialogue;
    [SerializeField]
    SpriteRenderer speakerSprite;
    #endregion

    int currentDialogueIdx = 0;
    bool isTyping = false;

    public override void OnInitialize()
    {

    }

    private void Start()
    {
        GameInput.Instance.onPlayerInteract.performed += OnNextDialgoue;

        IsDialogueShowed = true;
    }

    private void Update()
    {

    }

    private void OnDisable()
    {
        GameInput.Instance.onPlayerInteract.performed -= OnNextDialgoue;
    }

    public void StartDialogue()
    {
        if (dialogueList == null || dialogueList.Count == 0)
        {
            Debug.LogWarning("Isi Kosong");
            return;
        }

        IsDialogueShowed = true;
        currentDialogueIdx = 0;

    }

    private void DisplayCurrentLine()
    {
        DialogueNode dialogueNode = dialogueList[currentDialogueIdx];

        speakerName.text = dialogueNode.speakerName;
        if (speakerSprite != null && dialogueNode.speakerSprite != null)
        {
            speakerSprite.sprite = dialogueNode.speakerSprite;
        }

        speakerName.text = dialogueNode.speakerName;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(dialogueNode.speakerDialogue));
    }

    private void OnNextDialgoue(InputAction.CallbackContext ctx)
    {
        if (!IsDialogueShowed || isTyping)
        {
            return;
        }

        currentDialogueIdx++;

        if (currentDialogueIdx >= dialogueList.Count)
        {
            EndDialogue();
            return;
        }

        DisplayCurrentLine();
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        speakerDialogue.text = "";

        foreach(char c in sentence)
        {
            speakerDialogue.text += c;

            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
    }

    private void EndDialogue()
    {
        IsDialogueShowed = false;

        dialogueList.Clear();
        currentDialogueIdx = 0;
        StopAllCoroutines();
    }

    public void SetDialogueList(List<DialogueNode> dialogueNodes)
    {
        dialogueList = dialogueNodes;
        currentDialogueIdx = 0;
    }
}
