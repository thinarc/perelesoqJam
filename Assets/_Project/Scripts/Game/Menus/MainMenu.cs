using _Project.Scripts.Common.EntryPoints;
using _Project.Scripts.Sound;
using DG.Tweening;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Game.Menus
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Slider volume;
        [SerializeField] private Image[] volumeHints = new Image[4];

        [SerializeField, Space(5)] private CanvasGroup creditsGroup;
        [SerializeField] private Button creditsButton;
        
        [SerializeField, Space(5)] private TextMeshProUGUI startText;
        [SerializeField] private ParticleSystem rain;
        
        [SerializeField, Space(5)] private TextMeshProUGUI headerText;
        
        private CanvasGroup _group;
        private bool _open;
        private bool _started;
        private bool _credits;
        
        private void Start()
        {
            _group = GetComponentInChildren<CanvasGroup>();
            _group.DOFade(0, 0);
            _group.interactable = false;
            _group.blocksRaycasts = false;

            creditsGroup.transform.DOScale(0.8f, 0);
            creditsGroup.DOFade(0, 0);
            creditsGroup.interactable = false;
            creditsGroup.blocksRaycasts = false;

            volume.value = SoundManager.Instance.MasterVolume;
            ChooseVolumeHint();
        }

        private float _quitTime = 2f;

        private void Update()
        {
            if (!_started && _open && !_credits && Input.GetKeyDown(KeyCode.Space)) CloseMenu();
            else if (_credits && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))) CloseCredits();
            
            if (_started && !_open && Input.GetKeyDown(KeyCode.Escape)) ShowMenu(true);
            else if (_started && _open && Input.GetKeyDown(KeyCode.Escape)) CloseMenu(true);

            if (_open && Input.GetKey(KeyCode.LeftArrow)) volume.value -= Time.unscaledDeltaTime * 0.8f;
            else if (_open && Input.GetKey(KeyCode.RightArrow)) volume.value += Time.unscaledDeltaTime * 0.8f;
            
            if (_open && Input.GetKey(KeyCode.Escape)) _quitTime -= Time.unscaledDeltaTime;
            else _quitTime = Mathf.Clamp(_quitTime + Time.unscaledDeltaTime, 1f, 2f);
            
            if (_quitTime < 0f) QuitGame();
        }

        public void ShowMenu(bool pause = false)
        {
            _group.DOFade(1, 0.4f).SetEase(Ease.InOutSine).SetUpdate(true).OnComplete(() => rain.gameObject.SetActive(true));
            _group.interactable = true;
            _group.blocksRaycasts = true;
            
            _open = true;
            Time.timeScale = 0;

            if (pause)
            {
                creditsButton.gameObject.SetActive(false);
                startText.text = "Вернуться";
                headerText.text = "Пауза";
            }
        }

        public void CloseMenu(bool pause = false)
        {
            _group.DOFade(0, 0.2f).SetEase(Ease.InOutSine).SetUpdate(true);
            _group.interactable = false;
            _group.blocksRaycasts = false;
            
            rain.gameObject.SetActive(false);
            
            if (!pause && !_started) GameEntryPoint.Instance.locationSwitcher.StartLocation();
            else headerText.text = "";
            
            Time.timeScale = 1;
            _open = false;
            _started = true;
        }
        
        public void ShowCredits()
        {
            creditsGroup.transform.DOScale(1, 0.1f).SetEase(Ease.InOutSine).SetUpdate(true);
            creditsGroup.DOFade(1, 0.1f).SetEase(Ease.InOutSine).SetUpdate(true);
            creditsGroup.interactable = true;
            creditsGroup.blocksRaycasts = true;
            headerText.text = "Мы";
            _credits = true;
        }

        public void CloseCredits()
        {
            creditsGroup.transform.DOScale(0.8f, 0.2f).SetEase(Ease.InOutSine).SetUpdate(true);
            creditsGroup.DOFade(0, 0.08f).SetEase(Ease.InOutSine).SetUpdate(true);
            creditsGroup.interactable = false;
            creditsGroup.blocksRaycasts = false;
            headerText.text = "Напротив";
            _credits = false;
        }

        public void OnVolumeChange()
        {
            SoundManager.Instance.MasterVolume = volume.value;
            ChooseVolumeHint();
        }
        
        public void OnVolume()
        {
            if (_pastVolume < 0.2f) _pastVolume = 0.2f;
            SoundManager.Instance.MasterVolume = _pastVolume;
            ChooseVolumeHint();
            
            volume.value = SoundManager.Instance.MasterVolume;
        }

        private float _pastVolume;

        public void OffVolume()
        {
            _pastVolume = SoundManager.Instance.MasterVolume;
            SoundManager.Instance.MasterVolume = 0f;
            ChooseVolumeHint();
            
            volume.value = SoundManager.Instance.MasterVolume;
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