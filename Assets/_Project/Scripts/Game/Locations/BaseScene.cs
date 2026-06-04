using System;
using _Project.Scripts.Curtain;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

namespace _Project.Scripts.Game.Locations
{
    public abstract class BaseScene : MonoBehaviour
    {
        protected CanvasGroup Group;
        protected Light2D Light;
        
        [SerializeField] private Ease tEase = Ease.Linear;
        [SerializeField] private float tTime = 0.4f;

        [SerializeField, Space(5)] private Image thingsButton;
        
        public Ease Ease => tEase;
        public float Time => tTime;
        
        protected float InitialLight;
        
        protected bool OpenToAnim;
        
        public bool yaWindow;

        public virtual void Initialize(LocationSwitcher switcher)
        {
            Group = GetComponent<CanvasGroup>();
            Light = GetComponentInChildren<Light2D>();
            
            gameObject.SetActive(true);
            
            InitialLight = Light.intensity;
            Light.intensity = 0;
        }

        public virtual void Show(bool flash = false)
        {
            if (flash) Group.DOFade(1, 0);
            else Group.DOFade(1, tTime).SetEase(tEase);
            OpenToAnim = true;
            thingsButton?.DOFade(0, 0).OnComplete(() => thingsButton?.DOFade(1, tTime).SetEase(tEase));
        } 
        
        public void Show(Ease ease, float time)
        {
            Group.DOFade(1, time).SetEase(ease);
            OpenToAnim = true;
        }

        private bool _lightToClose;

        [SerializeField, Space(5)] private bool streetLight;

        public static bool start;

        public virtual async void UpdateLight(bool close = false)
        {
            if (!start)
            {
                start = true;
                return;
            }
            _lightToClose = close;

            if (!close && streetLight)
            {
                Light.intensity = 0;
                await UniTask.Delay(80);
            }
            
            while (true)
            {
                var target = InitialLight;
                var ev = 12f;

                if (close || (!streetLight && !DraggableCurtain.Instance.Open))
                {
                    target = 0;
                    ev = 20f;
                }

                if (streetLight) ev *= 0.2f;
                
                Light.intensity = Mathf.MoveTowards(Light.intensity, target, UnityEngine.Time.deltaTime * ev);
                await UniTask.Yield();
                if (Mathf.Approximately(Light.intensity, target) || _lightToClose != close) break;
            }
        }

        public void InitialHide() => Group.DOFade(0, 0);
        
        public virtual async UniTask Hide(Ease ease, float time)
        {
            Group.DOFade(0, time).SetEase(ease);
            OpenToAnim = true;
            thingsButton?.DOFade(0, time).SetEase(ease);
            await UniTask.Delay(TimeSpan.FromSeconds(tTime / 2));
        }
    }
}