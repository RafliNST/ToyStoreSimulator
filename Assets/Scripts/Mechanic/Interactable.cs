using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Interactable Settings")]
    [SerializeField]
    Sprite iconToShow;
    [SerializeField]
    string actionIdentifier;

    public virtual void Action()
    {
        
    }

    public void ShowIcon()
    {
        InteractableController.Instance.onInteractableDetected?.Invoke(actionIdentifier, iconToShow);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ShowIcon();
        Debug.Log($"Identifier Name: {actionIdentifier}");
    }
}
