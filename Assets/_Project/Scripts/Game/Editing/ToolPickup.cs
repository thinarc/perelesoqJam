using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Scripts.Game.Editing
{
    public class ToolPickup : MonoBehaviour,
        IPointerDownHandler
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private EditController.ToolType toolType;

        private RectTransform _rect;
        private CanvasGroup _group;

        private bool _picked;

        public static bool ToolInHand;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
            _group = GetComponent<CanvasGroup>();

            ToolInHand = false;
        }

        private void Update()
        {
            if (!_picked)
                return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                Input.mousePosition,
                canvas.worldCamera,
                out var localPoint
            );

            _rect.localPosition = localPoint;

            if (Input.GetMouseButtonUp(0))
            {
                ToolInHand = false;
                Drop();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            EditController.Instance.сurrentTool = toolType;
            ToolInHand = true;
            PickUp();
        }

        private void PickUp()
        {
            transform.SetAsLastSibling();
            
            _picked = true;
            _group.blocksRaycasts = false;
        }

        private void Drop()
        {
            _picked = false;
            _group.blocksRaycasts = true;
            EditController.Instance.сurrentTool = EditController.ToolType.None;
        }
    }
}