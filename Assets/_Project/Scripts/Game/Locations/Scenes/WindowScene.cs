using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace _Project.Scripts.Game.Locations.Scenes
{
    public class WindowScene : BaseScene
    {
        [SerializeField, Space(5)] private BaseScene streetScene;
        
        public override void Initialize(LocationSwitcher switcher)
        {
            Group = GetComponent<CanvasGroup>();
            Light = streetScene.GetComponentInChildren<Light2D>();
            
            gameObject.SetActive(true);
            
            InitialLight = Light.intensity;
            Light.intensity = 0;
        }
        
        public override void Show(bool flash = false)
        {
            streetScene.Show(Ease, Time);
            base.Show(flash);
        }
        
        public override async UniTask Hide(Ease ease, float time)
        {
            await streetScene.Hide(ease, time / 2);
            await base.Hide(ease, time);
        }
    }
}