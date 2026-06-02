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
        }
        
        public override async void UpdateLight(bool close = false)
        {
            if (!close) await UniTask.Delay(100);
            base.UpdateLight(close);
        }
        
        public override async void Show(bool flash = false)
        {
            base.Show(flash);

            streetScene.gameObject.SetActive(true);

            await UniTask.Delay(80);

            curtain.Show(Ease, Time / 1.21f);

            await UniTask.Delay(120);

            streetScene.yaWindow = true;
            streetScene.Show(Ease, Time * 1.02f);
        }
        
        public override async UniTask Hide(Ease ease, float time)
        {
            // 1. ускоряем таймлайн в одном месте
            float streetTime = time * 0.6f;
            float curtainTime = time * 0.7f;

            // 2. начинаем гасить улицу (без ожидания curtain)
            var streetTask = ((StreetScene)streetScene).Hide(ease, streetTime / 1.72f, true);
            streetScene.yaWindow = false;

            // 3. параллельно можно начинать закрытие curtain с небольшой задержкой
            await UniTask.Delay(TimeSpan.FromSeconds(time * 0.16f));
            curtain.Hide(ease, curtainTime * 1.12f);

            // 4. ждём завершения улицы (главный слой)
            await streetTask;

            // 5. финально выключаем сцену
            await base.Hide(ease, time * 0.62f);
        }
    }
}