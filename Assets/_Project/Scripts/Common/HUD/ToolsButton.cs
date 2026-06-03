using _Project.Scripts.Common.HUD.Cursor;
using _Project.Scripts.Curtain;
using _Project.Scripts.Game.Editing;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Scripts.Common.HUD
{
    public class ToolsButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private bool _placed;
        private bool _hold;

        public bool holded = true;
        public bool yaTool;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (yaTool && EditController.Instance.prepareRotate) return;
            if (!DraggableCurtain.Instance.Open) return;
            _placed = true;
            CursorModeSwitcher.Instance.SetToolMode();
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (yaTool && EditController.Instance.prepareRotate) return;
            if (!DraggableCurtain.Instance.Open) return;
            CursorModeSwitcher.Instance.SetToolMode();
            _hold = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _placed = false;
            if (!_placed && !_hold) CursorModeSwitcher.Instance.SetDefaultMode();
            else if (!holded) CursorModeSwitcher.Instance.SetDefaultMode();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _hold = false;
            if (!_placed && !_hold) CursorModeSwitcher.Instance.SetDefaultMode();
        }
    }
}
