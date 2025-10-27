using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InteractableController : Singleton<InteractableController>
{
    [HideInInspector]
    public UnityEvent<string, Sprite> onInteractableDetected;
    [HideInInspector]
    public UnityEvent<string> onInteractableExit, onInteractableSelected;
    private Dictionary<string, int> interactableIdentityRecord;

    [SerializeField]
    GameObject iconDisplayerPrefab;
    [SerializeField]
    Vector3 iconOffset;
    [SerializeField, Range(1,7)]
    int maxIconShowed;
    [SerializeField, Range(.1f, 1f)]
    float iconTransparency;

    private List<SpriteRenderer> spriteRenderers;
    private List<GameObject> spriteRenderersGO;
    
    int _currentIconIdx = 0, activeIconCount = -1;
    Color colorTranspareancy;
    Vector3 initialPos;

    int currentIconIdx
    {
        get
        {
            return Mathf.Clamp(_currentIconIdx, 0, maxIconShowed);
        }
        set
        {
            spriteRenderers[currentIconIdx].color = colorTranspareancy;
            
            _currentIconIdx = Mathf.Clamp(value, 0, activeIconCount);
            spriteRenderers[currentIconIdx].color = Color.white;
        }
    }

    public override void OnInitialize()
    {
        interactableIdentityRecord = new Dictionary<string, int>();
        spriteRenderers = new List<SpriteRenderer>();
        spriteRenderersGO = new List<GameObject>();
        onInteractableExit = new UnityEvent<string>();
        onInteractableSelected = new UnityEvent<string>();

        onInteractableDetected.AddListener(DisplayIcon);
        onInteractableExit.AddListener(RemoveIcon);

        initialPos = transform.localPosition;
    }

    private void Start()
    {
        for (int i = 0; i < maxIconShowed; i++)
        {
            GameObject rendererGO = Instantiate(iconDisplayerPrefab, transform, false);
            rendererGO.transform.localPosition = i * iconOffset;
            SpriteRenderer renderer = rendererGO.GetComponent<SpriteRenderer>();

            spriteRenderersGO.Add(rendererGO);
            spriteRenderers.Add(renderer);
            renderer.color = new Color(1, 1, 1, iconTransparency);
            rendererGO.SetActive(false);
        }

        colorTranspareancy = new Color(1, 1, 1, iconTransparency);
        GameInput.Instance.onPlayerChangeMenu.performed += OnMenuChanged;
        GameInput.Instance.onPlayerInteract.performed += OnMenuSelected;
    }

    private void OnDisable()
    {
        GameInput.Instance.onPlayerChangeMenu.performed -= OnMenuChanged;
        GameInput.Instance.onPlayerInteract.performed -= OnMenuSelected;
    }

    private void DisplayIcon(string actionIdentifier, Sprite icon)
    {
        if (activeIconCount < maxIconShowed-1)
            activeIconCount++;

        spriteRenderersGO[activeIconCount].SetActive(true);
        spriteRenderers[activeIconCount].sprite = icon;

        interactableIdentityRecord.TryAdd(actionIdentifier, activeIconCount);

        spriteRenderers[activeIconCount].color = (activeIconCount == currentIconIdx) ?
            Color.white : colorTranspareancy;

        GameInput.Instance.onPlayerInteract.Enable();
    }

    private void RemoveIcon(string actionIdentifier)
    {
        if (interactableIdentityRecord.TryGetValue(actionIdentifier, out int rendererIdx))
        {
            spriteRenderers[rendererIdx].color = colorTranspareancy;
            spriteRenderersGO[rendererIdx].SetActive(false);
            interactableIdentityRecord.Remove(actionIdentifier);
        }

        activeIconCount--;
        currentIconIdx = currentIconIdx;
        transform.localPosition = new Vector3((-currentIconIdx * iconOffset).x, initialPos.y);   
        
        if (activeIconCount < 0)
            GameInput.Instance.onPlayerInteract.Disable();
        
        ReDisplayIcons();
    }

    private void ReDisplayIcons()
    {
        for (int i = 0; i < maxIconShowed; i++)
        {
            spriteRenderers[i].color = (i == currentIconIdx) ?
                    Color.white : colorTranspareancy;

            spriteRenderersGO[i].SetActive(false);
        }

        List<string> actionIdentifiers = interactableIdentityRecord.Keys.ToList();

        for (int i = 0; i < actionIdentifiers.Count; i++)
        {
            if (interactableIdentityRecord.TryGetValue(actionIdentifiers[i], out int idx))
            {
                spriteRenderersGO[i].SetActive(true);
                spriteRenderers[i].sprite = spriteRenderers[idx].sprite;

                spriteRenderers[i].color = (i == currentIconIdx) ?
                    Color.white : colorTranspareancy;
            }
        }
    }

    private void OnMenuChanged(InputAction.CallbackContext ctx)
    {
        int dir = (int)GameInput.Instance.ChangeMenu().x;

        if (currentIconIdx + dir <= activeIconCount && currentIconIdx + dir >= 0)
        {
            currentIconIdx += dir;
            transform.localPosition = new Vector3((-currentIconIdx * iconOffset).x, initialPos.y);
        }
    }

    private void OnMenuSelected(InputAction.CallbackContext ctx)
    {
        var keys = interactableIdentityRecord.Keys.ToList();
        Debug.Log($"Terpanggil, Identifier: {keys[currentIconIdx]}");
        onInteractableSelected?.Invoke(keys[currentIconIdx]);
    }
}
