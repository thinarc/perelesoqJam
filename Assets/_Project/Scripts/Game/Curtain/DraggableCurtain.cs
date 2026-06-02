using _Project.Scripts.Game.Curtain;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

namespace _Project.Scripts.Curtain
{
    public class DraggableCurtain : MonoBehaviour
    {
        public static DraggableCurtain Instance { get; private set; }

        [SerializeField] private CurtainSide left;
        [SerializeField] private CurtainSide right;

        [SerializeField, Space(5)] private RectTransform sizer;
        [SerializeField] private Light2D globalLight;

        private CanvasGroup _group;
        
        public bool Open => left.Open && right.Open;

        private void Awake()
        {
            Instance = this;
            
            _group = GetComponent<CanvasGroup>();
            _group.DOFade(0, 0);
            _group.blocksRaycasts = false;
        }

        private void Update()
        {
            var leftAdd = left.Value / 10f;
            var rightAdd = right.Value / 10f;

            var targetIntensity = 0.7f + leftAdd * 1.5f + rightAdd * 1.5f;

            globalLight.intensity = Mathf.Lerp(
                globalLight.intensity,
                targetIntensity,
                Time.deltaTime * 5f
            );
        }
        
        public void Show(Ease ease, float time, bool toStreet = false)
        {
            time /= 1.3f;
            
            _group.DOFade(1, time).SetEase(ease);
            _group.blocksRaycasts = true;
            
            if (toStreet)
            {
                left.interactable = false;
                right.interactable = false;
                sizer.DOScale(2.6f, 0.01f).SetEase(Ease.Linear);
            }
            else
            {
                left.interactable = true;
                right.interactable = true;
            }
        }

        public async void Hide(Ease ease, float time, bool fromStreet = false)
        {
            time /= 1.3f;
            
            _group.blocksRaycasts = false;
            await _group.DOFade(0, time).SetEase(ease).AsyncWaitForCompletion();
            
            if (fromStreet)
            {
                left.interactable = true;
                right.interactable = true;
                sizer.DOScale(1, 0.01f).SetEase(Ease.Linear);
            }
        }

        public void OpenBoth(Ease ease, float time)
        {
            left.AnimateTo(1f, ease, time);
            right.AnimateTo(1f, ease, time);
        }

        public void CloseBoth(Ease ease, float time)
        {
            left.AnimateTo(0f, ease, time);
            right.AnimateTo(0f, ease, time);
        }
    }
}