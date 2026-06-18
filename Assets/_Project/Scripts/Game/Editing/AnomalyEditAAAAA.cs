using _Project.Scripts.Game.Locations;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using _Project.Scripts.Sound;

namespace _Project.Scripts.Game.Editing
{
    public class AnomalyEditAAAAA : MonoBehaviour
    {
        private static readonly int Brightness = Shader.PropertyToID("_Brightness");
        private static readonly int FullAlphaDissolveFade = Shader.PropertyToID("_FullAlphaDissolveFade");
        private static readonly int GaussianBlurFade = Shader.PropertyToID("_GaussianBlurFade");
        public static AnomalyEditAAAAA Instance { get; private set; }
        private void Awake() => Instance = this;

        [SerializeField] private Canvas generalCanvas;
        [SerializeField] private Camera visionCam;

        [SerializeField, Space(5)] private TextMeshProUGUI spaceText;
        [SerializeField] private int steps = 3;

        private int _step;
        private float _timeBack;

        private CanvasGroup _group;
        private Camera _baseCam;

        private RawImage _vision;
        
        private float _brightness;
        private float _alphaDissolve;
        private float _gaussianBlur;

        private bool _control;

        private void Start()
        {
            _group = GetComponentInChildren<CanvasGroup>();
            _baseCam = generalCanvas.worldCamera;
            
            _vision = GetComponentInChildren<RawImage>();
            
            _brightness = _vision.material.GetFloat(Brightness);
            _alphaDissolve = _vision.material.GetFloat(FullAlphaDissolveFade);
            _gaussianBlur = _vision.material.GetFloat(GaussianBlurFade);
            
            _vision.material.SetFloat(Brightness, 1);
            _vision.material.SetFloat(FullAlphaDissolveFade, 1);
            _vision.material.SetFloat(GaussianBlurFade, 0);

            visionCam.gameObject.SetActive(false);
            _group.alpha = 0f;
            _group.blocksRaycasts = false;
            _group.interactable = false;
        }

        private bool _catchedInDoor;
        public GameObject toDisableTemp;
        
        private void Update()
        {
            if (!_control && !_catchedInDoor && LocationSwitcher.Instance._currentLoc.name == "door")
            {
                CatchControl();
                _catchedInDoor = true;
            }
        }

        private void OnDisable()
        {
            _vision.material.SetFloat(Brightness, _brightness);
            _vision.material.SetFloat(FullAlphaDissolveFade, _alphaDissolve);
            _vision.material.SetFloat(GaussianBlurFade, _gaussianBlur);
        }

        public async void CatchControl()
        {
            _control = true;
            
            generalCanvas.worldCamera = visionCam;
            visionCam.gameObject.SetActive(true);
            DisableView();
            
            //ВКЛЮЧАЮ ЗВУК СЦЕНЫ 
            SoundManager.Instance.SFXHandle.PlayCaution();
            SoundManager.Instance.AmbientHandle.PlayDeaf();
            SoundManager.Instance.MusicHandle.SetVolume(0f);

            _group.alpha = 1f;
            _group.blocksRaycasts = true;
            _group.interactable = true;

            _step = 0;
            _timeBack = 0;

            while (true)
            {
                var brightness = Mathf.MoveTowards(_vision.material.GetFloat(Brightness), _brightness, Time.deltaTime * 2f);
                var alphaDissolve = Mathf.MoveTowards(_vision.material.GetFloat(FullAlphaDissolveFade), _alphaDissolve, Time.deltaTime * 2f);
                var gaussianBlur = Mathf.MoveTowards(_vision.material.GetFloat(GaussianBlurFade), _gaussianBlur, Time.deltaTime * 2f);

                _vision.material.SetFloat(Brightness, brightness);
                _vision.material.SetFloat(FullAlphaDissolveFade, alphaDissolve);
                _vision.material.SetFloat(GaussianBlurFade, gaussianBlur);
                
                await UniTask.Yield();
                if (Mathf.Approximately(brightness, _brightness) && Mathf.Approximately(alphaDissolve, _alphaDissolve) && Mathf.Approximately(gaussianBlur, _gaussianBlur))
                    break;
            }

            while (true)
            {
                var brightness = _vision.material.GetFloat(Brightness);
                var alphaDissolve = _vision.material.GetFloat(FullAlphaDissolveFade);
                var gaussianBlur = _vision.material.GetFloat(GaussianBlurFade);

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    brightness = Mathf.MoveTowards(brightness, 1, (1 - _brightness) / steps);
                    alphaDissolve = Mathf.MoveTowards(alphaDissolve, 1, (1 - _alphaDissolve) / steps);
                    gaussianBlur = Mathf.MoveTowards(gaussianBlur, 0, _gaussianBlur / steps);
                    _step++;
                    
                    _vision.material.SetFloat(Brightness, brightness);
                    _vision.material.SetFloat(FullAlphaDissolveFade, alphaDissolve);
                    _vision.material.SetFloat(GaussianBlurFade, gaussianBlur);
                    
                    PunchText();
                    
                    async void PunchText()
                    {
                        await spaceText.transform.DOScale(0.85f, 0.08f).SetEase(Ease.OutQuad).AsyncWaitForCompletion();
                        spaceText.text = "Пробел";
                        await spaceText.transform.DOScale(1f, 0.12f).SetEase(Ease.OutBack).AsyncWaitForCompletion();
                        spaceText.text = "[Пробел]";
                    }
                }
                else if (_timeBack <= 0)
                {
                    brightness = Mathf.MoveTowards(brightness, _brightness, (1 - _brightness) / steps);
                    alphaDissolve = Mathf.MoveTowards(alphaDissolve, _alphaDissolve, (1 - _alphaDissolve) / steps);
                    gaussianBlur = Mathf.MoveTowards(gaussianBlur, _gaussianBlur, _gaussianBlur / steps);
                    _step--;
                    _timeBack = 0.4f;
                    
                    _vision.material.SetFloat(Brightness, brightness);
                    _vision.material.SetFloat(FullAlphaDissolveFade, alphaDissolve);
                    _vision.material.SetFloat(GaussianBlurFade, gaussianBlur);
                }
                
                // ReSharper disable once PossibleLossOfFraction
                spaceText.DOFade(1 - (_step / steps), 0.1f);
                _timeBack -= Time.deltaTime;
                
                await UniTask.Yield();
                if (Mathf.Approximately(brightness, 1) && Mathf.Approximately(alphaDissolve, 1) && Mathf.Approximately(gaussianBlur, 0))
                    break;
            }
            
            spaceText.DOFade(0, 0.3f).SetEase(Ease.InQuad);
            await UniTask.Delay(300);
            ReturnControl();

            async void DisableView()
            {
                await UniTask.Delay(400);
                
                await UniTask.NextFrame();
                visionCam.gameObject.SetActive(false);
            }
        }

        public bool waitReturn;
        
        private void ReturnControl()
        {
            generalCanvas.worldCamera = _baseCam;
            visionCam.gameObject.SetActive(false);
            
            // Возвращаю норм звук
            SoundManager.Instance.AmbientHandle.StopDeaf();
            SoundManager.Instance.MusicHandle.PlayBgCello();
            SoundManager.Instance.MusicHandle.SetVolume(1f);
            
            _group.DOFade(0, 0.3f).SetEase(Ease.InQuad).OnComplete(() => spaceText.DOFade(1, 0));
            _group.blocksRaycasts = false;
            _group.interactable = false;

            waitReturn = false;
            _control = false;

            if (_catchedInDoor) toDisableTemp.SetActive(false);
        }
    }
}