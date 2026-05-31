using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace _Project.Scripts.Game.Locations
{
    public abstract class BaseScene : MonoBehaviour
    {
        private CanvasGroup _group;
        private Light2D _light;
        
        [SerializeField] private Ease tEase = Ease.Linear;
        [SerializeField] private float tTime = 0.4f;
        
        public Ease Ease => tEase;
        public float Time => tTime;
        
        private float _initialLight;

        public virtual void Initialize(LocationSwitcher switcher)
        {
            _group = GetComponent<CanvasGroup>();
            _light = GetComponentInChildren<Light2D>();
            
            gameObject.SetActive(true);
            
            _initialLight = _light.intensity;
            _light.intensity = 0;
        }

        public void Show(bool flash = false)
        {
            if (flash) _group.DOFade(1, 0);
            else _group.DOFade(1, tTime).SetEase(tEase);
        }

        public void HideFlash() => _group.DOFade(0, 0);
        
        public async UniTask Hide(Ease ease, float time)
        {
            _group.DOFade(0, time).SetEase(ease);
            await UniTask.Delay(TimeSpan.FromSeconds(tTime / 2));
        }
    }
}