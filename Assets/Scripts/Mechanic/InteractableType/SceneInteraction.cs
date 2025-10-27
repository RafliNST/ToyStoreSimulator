using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInteraction : InteractableBase
{
    [SerializeField]
    string sceneName;

    public override void Interact()
    {
        SceneManager.LoadScene(sceneName);
    }
}
