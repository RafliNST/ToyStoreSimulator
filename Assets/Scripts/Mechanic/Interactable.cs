using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Interactable Settings")]
    [SerializeField]
    Sprite iconToShow;
    [SerializeField]
    string actionIdentifier;

    DialgoueInteraction dialgoueInteraction;
    SceneInteraction sceneInteraction;

    public virtual void InteractableAction(string identifier)
    {
        if (identifier == actionIdentifier)
        {
            if (dialgoueInteraction != null)
            {
                dialgoueInteraction.Interact();
            }

            if (sceneInteraction != null)
            {
                sceneInteraction.Interact();
            }
        }
    }

    private void Start()
    {
        InteractableController.Instance.onInteractableSelected.AddListener(InteractableAction);

        dialgoueInteraction = GetComponent<DialgoueInteraction>();
        sceneInteraction = GetComponent<SceneInteraction>();
    }

    public void ShowIcon()
    {
        InteractableController.Instance.onInteractableDetected?.Invoke(actionIdentifier, iconToShow);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ShowIcon();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        InteractableController.Instance.onInteractableExit?.Invoke(actionIdentifier);
    }
}

public abstract class InteractableBase : MonoBehaviour
{
    public abstract void Interact();
}
