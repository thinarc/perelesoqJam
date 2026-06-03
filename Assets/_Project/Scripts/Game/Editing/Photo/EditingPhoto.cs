using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Game.Editing.Photo
{
    public class EditingPhoto : MonoBehaviour
    {
        [SerializeField] private RectTransform dustRect;
        [SerializeField] private RectTransform dirtRect;

        [SerializeField, Space(5)] private Sprite rotatePhoto;

        private CanvasGroup _particlesGroup;

        private bool _rotated;

        private void Start()
        {
            _particlesGroup = GetComponentInChildren<CanvasGroup>();
        }

        public void RotatePhoto()
        {
            if (_rotated)
                return;
            
            GetComponent<UIEffects>().photo = true;
            
            _particlesGroup.DOFade(0, 0.2f).SetEase(Ease.InQuad);

            var img = GetComponent<Image>();
            _rotated = true;

            var sequence = DOTween.Sequence();
            sequence.Append(
                transform
                    .DORotate(
                        new Vector3(0f, 90f, 0f),
                        0.15f
                    )
                    .SetEase(Ease.InQuad)
            );

            sequence.AppendCallback(() =>
            {
                img.sprite = rotatePhoto;
            });

            sequence.Append(
                transform
                    .DORotate(
                        new Vector3(0f, 180f, 0f),
                        0.25f
                    )
                    .SetEase(Ease.OutQuad)
            );
        }

        public void PrepareToRotate()
        {
            GetComponent<UIEffects>().photo = false;
            GetComponent<UIEffects>().photoToRotate = true;
        }

        public RectTransform GetDustRect() => dustRect;
        
        public RectTransform GetDirtRect() => dirtRect;
    }
}