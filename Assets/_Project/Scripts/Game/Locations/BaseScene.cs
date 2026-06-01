using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace _Project.Scripts.Game.Locations
{
    public abstract class BaseScene : MonoBehaviour
    {
        protected CanvasGroup Group;
        protected Light2D Light;
        
        [SerializeField] private Ease tEase = Ease.Linear;
        [SerializeField] private float tTime = 0.4f;
        
        public Ease Ease => tEase;
        public float Time => tTime;
        
        protected float InitialLight;
        
        protected bool OpenToAnim;

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
        } 
        
        public void Show(Ease ease, float time)
        {
            Group.DOFade(1, time).SetEase(ease);
            OpenToAnim = true;
        } 

        public void InitialHide() => Group.DOFade(0, 0);
        
        public virtual async UniTask Hide(Ease ease, float time)
        {
            Group.DOFade(0, time).SetEase(ease);
            OpenToAnim = true;
            
            await UniTask.Delay(TimeSpan.FromSeconds(tTime / 2));
        }
    }
}