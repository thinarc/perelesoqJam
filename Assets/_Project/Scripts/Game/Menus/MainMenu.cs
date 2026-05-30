using _Project.Scripts.Common.EntryPoints;
using _Project.Scripts.Sound;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Game.Menus
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Slider volume;
        
        private CanvasGroup _group;
        
        private void Start()
        {
            _group = GetComponentInChildren<CanvasGroup>();
            _group.DOFade(0, 0);
            _group.interactable = false;
            _group.blocksRaycasts = false;

            volume.value = SoundManager.Instance.Volume;
        }

        public void ShowMenu()
        {
            _group.DOFade(1, 0.4f).SetEase(Ease.InOutSine);
            _group.interactable = true;
            _group.blocksRaycasts = true;
        }

        public void CloseMenu()
        {
            _group.DOFade(0, 0.1f).SetEase(Ease.InOutSine);
            _group.interactable = false;
            _group.blocksRaycasts = false;
            
            GameEntryPoint.Instance.locationSwitcher.StartLocation();
        }
    }
}