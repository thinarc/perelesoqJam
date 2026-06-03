using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Common.HUD.Cursor
{
    public class UICursorController : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private RectTransform cursorRoot;
        [SerializeField] private Image cursorImage;

        [Header("Sprites")]
        [SerializeField] private Sprite defaultIdle;
        [SerializeField] private Sprite defaultClick;
        [SerializeField] private Sprite toolIdle;
        [SerializeField] private Sprite toolClick;

        [Header("Settings")]
        [SerializeField] private float followSpeed = 25f;
        [SerializeField] private float clickScale = 0.85f;
        [SerializeField] private float clickAnimTime = 0.08f;

        private CursorMod mode;
        private CursorState state;

        private Tween scaleTween;

        private void Awake()
        {
            SetState(CursorState.Idle);
#if UNITY_EDITOR
            GetComponent<CanvasGroup>().alpha = 1;
#endif
        }

        private void Update()
        {
            FollowMouse();
        }

        private void FollowMouse()
        {
            Vector2 target = Input.mousePosition;

            cursorRoot.position = Vector2.Lerp(
                cursorRoot.position,
                target,
                followSpeed * Time.unscaledDeltaTime
            );
        }

        public void SetMode(CursorMod newMode)
        {
            mode = newMode;
            UpdateSprite();
        }

        public void SetState(CursorState newState)
        {
            if (state == newState) return;

            state = newState;
            UpdateSprite();
            AnimateClick();
        }

        private void UpdateSprite()
        {
            cursorImage.sprite = (mode, state) switch
            {
                (CursorMod.Default, CursorState.Idle) => defaultIdle,
                (CursorMod.Default, CursorState.Click) => defaultClick,
                (CursorMod.Tool, CursorState.Idle) => toolIdle,
                (CursorMod.Tool, CursorState.Click) => toolClick,
                _ => defaultIdle
            };
        }

        private void AnimateClick()
        {
            scaleTween?.Kill();

            Vector3 targetScale = state == CursorState.Click
                ? Vector3.one * clickScale
                : Vector3.one;
            
            scaleTween = cursorRoot.DOScale(targetScale, clickAnimTime)
                .SetEase(Ease.OutBack);
        }
        
        public enum CursorMod
        {
            Default,
            Tool
        }

        public enum CursorState
        {
            Idle,
            Click
        }
    }
}