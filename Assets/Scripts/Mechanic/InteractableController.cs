using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableController : Singleton<InteractableController>
{
    [HideInInspector]
    public UnityEvent<string, Sprite> onInteractableDetected;
    private Dictionary<string, Action> interactableActions;

    [SerializeField]
    GameObject iconDisplayerPrefab;
    [SerializeField]
    Vector3 iconOffset;
    [SerializeField, Range(1,7)]
    int maxIconShowed;
    [SerializeField]
    float iconTransparency;

    private List<SpriteRenderer> spriteRenderers;
    private List<GameObject> spriteRenderersGO;
    int currentIconIdx = -1;

    private void Start()
    {
        for (int i = 0; i < maxIconShowed; i++)
        {
            GameObject rendererGO = Instantiate(iconDisplayerPrefab, Vector3.zero + (i * iconOffset),
                Quaternion.identity, transform);
            SpriteRenderer renderer = rendererGO.GetComponent<SpriteRenderer>();

            spriteRenderersGO.Add(rendererGO);
            spriteRenderers.Add(renderer);
            renderer.color = new Color(1, 1, 1, iconTransparency);
            rendererGO.SetActive(false);
        }
    }

    public override void OnInitialize()
    {
        interactableActions = new Dictionary<string, Action>();
        spriteRenderers = new List<SpriteRenderer>();
        spriteRenderersGO = new List<GameObject>();

        onInteractableDetected.AddListener(DisplayIcon);
    }

    private void DisplayIcon(string actionIdentifier, Sprite icon)
    {
        if (currentIconIdx < maxIconShowed-1)
            currentIconIdx++;

        //transform.localPosition = Vector3.right * (currentIconIdx-1);
        spriteRenderers[currentIconIdx].color = Color.white;
        spriteRenderersGO[currentIconIdx].SetActive(true);

    }
}
