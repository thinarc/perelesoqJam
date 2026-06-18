using _Project.Scripts.Common.EntryPoints;
using _Project.Scripts.Sound;
using Cysharp.Threading.Tasks;
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
        
        [SerializeField] private GameObject creditsCol;
        
        public Image[] lightings;
        public GameObject[] header;
        
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
            SoundManager.Instance.AmbientHandle.PlayStreet();
            ChooseVolumeHint();
            
            lightings.ForEach(l => l.DOFade(0, 0).SetUpdate(true));
        }

        private float _quitTime = 2f;

        private void Update()
        {
            if (!_started && _open && !_credits && Input.GetKeyDown(KeyCode.Space)) CloseMenu();
            else if (_credits && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))) CloseCredits();
            
            if (_started && !_open && Input.GetKeyUp(KeyCode.Escape)) ShowMenu(true);
            else if (_started && _open && Input.GetKeyUp(KeyCode.Escape)) CloseMenu(true);

            if (_open && Input.GetKey(KeyCode.LeftArrow)) volume.value -= Time.unscaledDeltaTime * 0.8f;
            else if (_open && Input.GetKey(KeyCode.RightArrow)) volume.value += Time.unscaledDeltaTime * 0.8f;
            
            if (_open && Input.GetKey(KeyCode.Escape)) _quitTime -= Time.unscaledDeltaTime;
            else _quitTime = Mathf.Clamp(_quitTime + Time.unscaledDeltaTime, 1f, 2f);
            
            if (_quitTime < 0f) QuitGame();
        }

        public async void ShowMenu(bool pause = false)
        {
            _group.DOFade(1, 0.4f).SetEase(Ease.InOutSine).SetUpdate(true);
            _group.interactable = true;
            _group.blocksRaycasts = true;
            
            SoundManager.Instance.AmbientHandle.PlayStreet();

            if (!pause)
            {
                lightings.ForEach(l => l.DOFade(1, 1.4f).SetEase(Ease.InOutSine).SetUpdate(true));
            }
            else
            {
                var main = rain.main;
                main.prewarm = true;

                await UniTask.WaitForSeconds(0.2f, true);
            }

            rain.gameObject.SetActive(true);

            _open = true;
            Time.timeScale = 0;

            if (pause)
            {
                creditsButton.gameObject.SetActive(false);
                startText.text = "Вернуться";
            }
        }

        public void CloseMenu(bool pause = false)
        {
            if (!pause) lightings.ForEach(l => l.DOFade(0, 0.4f).SetEase(Ease.InOutSine).SetUpdate(true));
            header.ForEach(h => h.SetActive(false));
            
            if (!pause) _group.DOFade(0, 0.6f).SetEase(Ease.InOutSine).SetUpdate(true);
            else _group.DOFade(0, 0.2f).SetEase(Ease.InOutSine).SetUpdate(true);
            _group.interactable = false;
            _group.blocksRaycasts = false;
            
            SoundManager.Instance.AmbientHandle.StopStreet();
            
            rain.gameObject.SetActive(false);

            if (!pause && !_started)
            {
                SoundManager.Instance.AmbientHandle.GlobalClock();
                GameEntryPoint.Instance.locationSwitcher.StartLocation();
            }
            
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
            _credits = true;
            creditsCol.SetActive(true);
        }

        public void CloseCredits()
        {
            creditsGroup.transform.DOScale(0.8f, 0.2f).SetEase(Ease.InOutSine).SetUpdate(true);
            creditsGroup.DOFade(0, 0.08f).SetEase(Ease.InOutSine).SetUpdate(true);
            creditsGroup.interactable = false;
            creditsGroup.blocksRaycasts = false;
            _credits = false;
            creditsCol.SetActive(false);
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