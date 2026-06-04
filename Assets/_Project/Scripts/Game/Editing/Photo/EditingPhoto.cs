using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Game.Editing.Photo
{
    public class EditingPhoto : MonoBehaviour
    {
        [SerializeField] private RectTransform dustRect;
        [SerializeField] private RectTransform dirtRect;
        [SerializeField] private RectTransform shineRect;

        [SerializeField, Space(5)] private Sprite rotatePhoto;

        [Space(5)] public bool acceptable = true;

        private CanvasGroup _particlesGroup;

        private bool _rotated;

        public int numberPhoto;

        private Sprite _origPhoto;

        private void Start()
        {
            _particlesGroup = GetComponentInChildren<CanvasGroup>();
            _particlesGroup.DOFade(0, 0);
        }

        public async void StartEditing()
        {
            _rotated = false;
            _particlesGroup.DOFade(0, 0);
            GetComponent<UIEffects>().photo = true;
            GetComponent<UIEffects>().photoToRotate = false;
            for (var i = 0; i < shineRect.childCount; i++)
            {
                Destroy(shineRect.GetChild(i).gameObject);
            }
            if (_origPhoto) GetComponent<Image>().sprite = _origPhoto;
            await UniTask.Delay(200);
            _particlesGroup.DOFade(1, 0.7f).SetEase(Ease.OutBack);
        }

        public void RotatePhoto()
        {
            if (_rotated)
                return;

            EditController.Instance.waitChoose = true;
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
                _origPhoto = img.sprite;
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
            
            EditController.Instance.ShowButtons();
        }

        public void PrepareToRotate(out bool waitChoose)
        {
            waitChoose = false;
            if (!this) return;
            GetComponent<UIEffects>().photo = false;
            GetComponent<UIEffects>().photoToRotate = true;
        }

        public RectTransform GetDustRect() => dustRect;
        
        public RectTransform GetDirtRect() => dirtRect;
        
        public RectTransform GetShineRect() => shineRect;
    }
}