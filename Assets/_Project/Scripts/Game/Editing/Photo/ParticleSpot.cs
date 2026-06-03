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

            _image.DOFade(0f, 0.15f)
                .OnComplete(() =>
                {
                    Destroy(gameObject);
                });
        }
    }
}