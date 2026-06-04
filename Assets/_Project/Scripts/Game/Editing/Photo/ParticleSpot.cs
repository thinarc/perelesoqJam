using _Project.Scripts.Sound;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Project.Scripts.Game.Editing.Photo
{
    public class ParticleSpot : MonoBehaviour,
        IPointerEnterHandler
    {
        [SerializeField] private EditController.ToolType requiredTool;

        private Image _image;
        private bool _cleaned;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!ToolPickup.ToolInHand) return;
            
            if (_cleaned)
                return;

            if (!Input.GetMouseButton(0))
                return;

            // if (EditController.Instance.currentTool != requiredTool)
            if (EditController.Instance.currentTool != requiredTool)
                return;

            _cleaned = true;
            //нужно проверить какой инструммент в руках => такой звук вкл
            if (EditController.Instance.currentTool == EditController.ToolType.Brush)
                SoundManager.Instance.ActionHandle.PlayBrush();
            else if (EditController.Instance.currentTool == EditController.ToolType.Nag)
                SoundManager.Instance.ActionHandle.PlayNag();

            _image.DOFade(0f, 0.15f)
                .OnComplete(() =>
                {
                    Destroy(gameObject);
                });
        }
    }
}