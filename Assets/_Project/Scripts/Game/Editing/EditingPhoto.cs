using UnityEngine;

namespace _Project.Scripts.Game.Editing
{
    public class EditingPhoto : MonoBehaviour
    {
        [SerializeField] private RectTransform dustRect;
        [SerializeField] private RectTransform dirtRect;

        private CanvasGroup _particlesGroup;

        private void Start()
        {
            _particlesGroup = GetComponentInChildren<CanvasGroup>();
        }

        public RectTransform GetDustRect() => dustRect;
        
        public RectTransform GetDirtRect() => dirtRect;
    }
}