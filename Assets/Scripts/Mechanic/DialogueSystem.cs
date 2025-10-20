using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.InputSystem;
using System.Linq;

public class DialogueSystem : Singleton<DialogueSystem>
{
    #region Dialogue System Setup
    [SerializeField, Range(.1f, 5f)]
    float textSpeed;

    [SerializeField]
    public List<DialogueNode> dialogueList;

    [SerializeField]
    TMPro.TextMeshProUGUI speakerName, speakerDialogue;
    [SerializeField]
    SpriteRenderer speakerSprite;
    #endregion

    bool notInDialogue = true;

    public override void OnInitialize()
    {
        
    }

    private void Start()
    {
        //GameInput.Instance.onPlayerMove.performed += ShowDialogue;
        GameInput.Instance.onPlayerInteract.performed += ShowDialogue;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (notInDialogue)
            {
                //GameInput.Instance.onPlayerInteract.performed += ShowDialogue;
                Debug.Log("Masuk Ke If");
            }
            Debug.Log("Space Terdeteksi");
        }
    }

    private void OnDisable()
    {
        GameInput.Instance.onPlayerInteract.performed -= ShowDialogue;
    }

    public void ChangeDialogue()
    {
        speakerName.text = dialogueList.First().speakerName;
        speakerDialogue.text = dialogueList.First().speakerDialogue;
        dialogueList.RemoveAt(0);
    }

    public void ShowDialogue(InputAction.CallbackContext ctx)
    {
        Debug.Log("Show Dialgoue Invoked");
        if (dialogueList.Count > 0)
        {
            //notInDialogue = false;
            Debug.Log("List > 0");
            ChangeDialogue();
        }   
        //else
        //{
        //    GameInput.Instance.onPlayerInteract.performed -= ShowDialogue;
        //}
    }
}
