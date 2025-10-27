using UnityEngine;

public class AnimationInteraction : InteractableBase
{
    Animator animator;
    [SerializeField]
    string triggerName;

    int triggerHash;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        triggerHash = Animator.StringToHash(triggerName);
    }

    public override void Interact()
    {
        animator.SetTrigger(triggerHash);
    }
}
