using System;
using _Project.Scripts.Curtain;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace _Project.Scripts.Game.Locations.Scenes
{
    public class WindowScene : BaseScene
    {
        [SerializeField, Space(5)] private BaseScene streetScene;
        [SerializeField] private DraggableCurtain curtain;
        
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
            curtain.Show(Ease, Time);
            base.Show(flash);
        }
        
        public override async UniTask Hide(Ease ease, float time)
        {
            await streetScene.Hide(ease, time / 2);
            curtain.Hide(ease, time);
            await base.Hide(ease, time);
        }
    }
}