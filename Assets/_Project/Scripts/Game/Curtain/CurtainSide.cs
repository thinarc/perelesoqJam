using _Project.Scripts.Sound;
using _Project.Scripts.Common.HUD.Cursor;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Scripts.Game.Curtain
{
    public class CurtainSide : MonoBehaviour,
        IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Side side;
        [SerializeField] private float sensitivity = 0.0025f;
        
        private RectTransform _rect;
        
        public bool dragging;
        public bool placed;
        private float _value;
        
        public float Value => _value;

        [SerializeField, Space(5)] private CurtainSide otherSide;
        public bool Dragging => dragging || otherSide.dragging;
        public bool Placed => placed || otherSide.placed;
        
        private const float MinScale = 0.4f;
        public enum Side { Left, Right }

        public bool Open => _value > 0.1f;

        public bool interactable;

        private void Start() => _rect = GetComponent<RectTransform>();
        
        private void Apply()
        {
            var scale = _rect.localScale;
            scale.x = Mathf.Lerp(1f, MinScale, _value);
            _rect.localScale = scale;
        }

        private void Set(float value)
        {
            _value = Mathf.Clamp01(value);
            Apply();
        }

        public void AnimateTo(float target, Ease ease, float time)
        {
            DOTween.To(
                    () => _value,
                    x =>
                    {
                        _value = x;
                        Apply();
                    },
                    target,
                    time)
                .SetEase(ease)
                .SetUpdate(true);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            placed = true;
            CursorModeSwitcher.Instance.SetToolMode();
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            dragging = true;
            CursorModeSwitcher.Instance.SetToolMode();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            placed = false;
            if (!Dragging) CursorModeSwitcher.Instance.SetDefaultMode();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            dragging = false;
            // Игрок отпустил мышь — гасим звук мгновенно
            SoundManager.Instance.ActionHandle.StopCurtainImmediate(); 
            if (!Dragging && !Placed) CursorModeSwitcher.Instance.SetDefaultMode();
        }
       
        public void OnDrag(PointerEventData eventData)
        {
            if (!dragging || !interactable) return;

            var delta = eventData.delta.x * sensitivity;

            if (side == Side.Left)
                Set(_value - delta);
            else
                Set(_value + delta);

            // Если уперлись в края шторы — звук выключаем сразу
            if (_value <= 0f || _value >= 1f)
            {
                SoundManager.Instance.ActionHandle.StopCurtainImmediate();
            }
            else
            {
                // Мышь двигается внутри допустимых границ — пинаем метод проигрывания
                SoundManager.Instance.ActionHandle.PlayCurtain();
            }
        }
    }
}