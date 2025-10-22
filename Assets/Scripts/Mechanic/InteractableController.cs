using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InteractableController : Singleton<InteractableController>
{
    [HideInInspector]
    public UnityEvent<string, Sprite> onInteractableDetected;
    [HideInInspector]
    public UnityEvent onInteractableExit;
    private Dictionary<string, Action> interactableActions;

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
        interactableActions = new Dictionary<string, Action>();
        spriteRenderers = new List<SpriteRenderer>();
        spriteRenderersGO = new List<GameObject>();
        onInteractableExit = new UnityEvent();

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
    }

    private void OnDisable()
    {
        GameInput.Instance.onPlayerChangeMenu.performed -= OnMenuChanged;
    }

    private void DisplayIcon(string actionIdentifier, Sprite icon)
    {
        if (activeIconCount < maxIconShowed-1)
            activeIconCount++;


        spriteRenderersGO[activeIconCount].SetActive(true);
        spriteRenderers[activeIconCount].sprite = icon;

        spriteRenderers[activeIconCount].color = (activeIconCount == currentIconIdx) ?
            Color.white : colorTranspareancy;
    }

    private void RemoveIcon()
    {
        spriteRenderers[activeIconCount].color = colorTranspareancy;
        spriteRenderersGO[activeIconCount].SetActive(false);

        activeIconCount--;
        currentIconIdx = currentIconIdx;

        transform.localPosition = new Vector3((-currentIconIdx * iconOffset).x, initialPos.y);
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
}
