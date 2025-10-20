using System;
using UnityEngine;

[Serializable]
public class DialogueNode
{
    public Sprite speakerSprite;
    public string speakerName;
    [TextArea]
    public string speakerDialogue;
}
