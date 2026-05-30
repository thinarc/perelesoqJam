using _Project.Scripts.Common.EntryPoints;
using _Project.Scripts.Sound;
using DG.Tweening;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Game.Menus
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Slider volume;
        [SerializeField] private Image[] volumeHints = new Image[4]; 
        
        private CanvasGroup _group;
        
        private void Start()
        {
            _group = GetComponentInChildren<CanvasGroup>();
            _group.DOFade(0, 0);
            _group.interactable = false;
            _group.blocksRaycasts = false;

            volume.value = SoundManager.Instance.Volume;
            ChooseVolumeHint();
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

        public void OnVolumeChange()
        {
            SoundManager.Instance.Volume = volume.value;
            ChooseVolumeHint();
        }

        private void ChooseVolumeHint()
        {
            volumeHints.ForEach(h => h.gameObject.SetActive(false));
            if (volume.value < 0.05f) volumeHints[0].gameObject.SetActive(true);
            else if (volume.value < 0.4f) volumeHints[1].gameObject.SetActive(true);
            else if (volume.value < 0.9f) volumeHints[2].gameObject.SetActive(true);
            else if (volume.value <= 1f) volumeHints[3].gameObject.SetActive(true);
        }

        public void QuitGame()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}