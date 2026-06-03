using System;
using _Project.Scripts.Game.Editing;
using _Project.Scripts.Game.Editing.Photo;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

[Flags]
public enum UIEffectType
{
    None = 0,

    HoverScale = 1 << 0,
    PressScale = 1 << 1,

    HoverTilt = 1 << 2,
    HoverMove = 1 << 3,

    Floating = 1 << 4,
    Breathing = 1 << 5,
    Wobble = 1 << 6
}

public class UIEffects : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler
{
    [SerializeField]
    private UIEffectType effects;

    [Header("Scale")]
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float pressScale = 0.9f;
    [SerializeField] private float scaleDuration = 0.15f;

    [Header("Tilt")]
    [SerializeField] private float tiltAmount = 8f;

    [Header("Move")]
    [SerializeField] private float moveAmount = 10f;

    [Header("Tilt & Move")]
    [SerializeField] private float smoothSpeed = 12f;

    [Header("Floating")]
    [SerializeField] private float floatDistance = 10f;
    [SerializeField] private float floatDuration = 1f;

    [Header("Breathing")]
    [SerializeField] private float breathingScale = 1.05f;
    [SerializeField] private float breathingDuration = 1f;

    [Header("Wobble")]
    [SerializeField] private float wobbleAngle = 3f;
    [SerializeField] private float wobbleDuration = 0.08f;

    private RectTransform rect;

    private Vector3 baseScale;
    private Vector3 basePosition;

    private bool hovering;

    private Quaternion targetRotation;
    private Vector3 targetPosition;

    private bool Has(UIEffectType effect)
    {
        return (effects & effect) != 0;
    }

    private void Awake()
    {
        rect = transform as RectTransform;

        baseScale = transform.localScale;
        basePosition = transform.localPosition;

        targetRotation = Quaternion.identity;
        targetPosition = basePosition;
    }

    private void Start()
    {
        if (Has(UIEffectType.Floating))
        {
            transform
                .DOLocalMoveY(basePosition.y + floatDistance, floatDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine)
                .SetUpdate(true);
        }

        if (Has(UIEffectType.Breathing))
        {
            transform
                .DOScale(baseScale * breathingScale, breathingDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine)
                .SetUpdate(true);
        }

        if (Has(UIEffectType.Wobble))
        {
            transform
                .DOLocalRotate(
                    new Vector3(0, 0, wobbleAngle),
                    wobbleDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.Linear)
                .SetUpdate(true);
        }
    }

    private void Update()
    {
        bool useTilt = Has(UIEffectType.HoverTilt);
        bool useMove = Has(UIEffectType.HoverMove);

        if (!useTilt && !useMove)
            return;

        if (!hovering)
        {
            if (useTilt)
            {
                rect.localRotation = Quaternion.Lerp(
                    rect.localRotation,
                    Quaternion.identity,
                    Time.unscaledDeltaTime * smoothSpeed);
            }

            if (useMove)
            {
                rect.localPosition = Vector3.Lerp(
                    rect.localPosition,
                    basePosition,
                    Time.unscaledDeltaTime * smoothSpeed);
            }

            return;
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rect,
            Input.mousePosition,
            null,
            out var point);

        float x = Mathf.Clamp(
            point.x / (rect.rect.width * 0.5f),
            -1f,
            1f);

        float y = Mathf.Clamp(
            point.y / (rect.rect.height * 0.5f),
            -1f,
            1f);

        if (useTilt)
        {
            targetRotation = Quaternion.Euler(
                -y * tiltAmount,
                x * tiltAmount,
                0);

            rect.localRotation = Quaternion.Lerp(
                rect.localRotation,
                targetRotation,
                Time.unscaledDeltaTime * smoothSpeed);
        }

        if (useMove)
        {
            targetPosition = basePosition +
                             new Vector3(
                                 x * moveAmount,
                                 y * moveAmount,
                                 0);

            rect.localPosition = Vector3.Lerp(
                rect.localPosition,
                targetPosition,
                Time.unscaledDeltaTime * smoothSpeed);
        }
    }

    public bool photo;
    public bool photoToRotate;


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ToolPickup.ToolInHand && !photo) return;
        if (photo && !ToolPickup.ToolInHand) return;
        
        hovering = true;

        if (photo)
        {
            SprayTool.InPhotoSpawn = true;
        }

        if (Has(UIEffectType.HoverScale))
        {
            transform.DOKill();

            transform
                .DOScale(baseScale * hoverScale, scaleDuration)
                .SetUpdate(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
        
        if (photo)
        {
            SprayTool.InPhotoSpawn = false;
        }

        if (Has(UIEffectType.HoverScale))
        {
            transform.DOKill();

            transform
                .DOScale(baseScale, scaleDuration)
                .SetUpdate(true);
        }
    }

    public bool yaTool;
    public bool yaInactiveList;
    public bool yaActiveList;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (yaActiveList)
        {
            EditController.Instance.CrossOutLine();
        }
        
        if (yaTool && EditController.Instance.prepareRotate) return;
        if (yaInactiveList) return;
        
        if (photoToRotate && !photo)
        {
            GetComponent<EditingPhoto>().RotatePhoto();
        }
        else
        {
            if (ToolPickup.ToolInHand && !photo) return;
            if (photo && !ToolPickup.ToolInHand) return;
        }
        
        if (!Has(UIEffectType.PressScale))
            return;

        transform
            .DOScale(baseScale * pressScale, 0.05f)
            .SetUpdate(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!Has(UIEffectType.PressScale))
            return;

        float target =
            hovering && Has(UIEffectType.HoverScale)
                ? hoverScale
                : 1f;

        transform
            .DOScale(baseScale * target, 0.1f)
            .SetUpdate(true);
    }

    public void Punch(float strength = 0.15f)
    {
        transform.DOPunchScale(
            Vector3.one * strength,
            0.25f,
            8,
            0.5f)
            .SetUpdate(true);
    }

    public void Shake(float strength = 10f)
    {
        rect.DOShakeAnchorPos(
                0.25f,
                strength,
                20)
            .SetUpdate(true);
    }

    private void OnDestroy()
    {
        DOTween.Kill(transform);
    }
}