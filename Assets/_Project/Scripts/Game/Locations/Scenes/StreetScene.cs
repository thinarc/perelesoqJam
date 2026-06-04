using _Project.Scripts.Sound;
using System;
using _Project.Scripts.Curtain;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Game.Locations.Scenes
{
    public class StreetScene : BaseScene
    {
        [SerializeField, Space(5)] private Image picture;
        [SerializeField] private Sprite sky2;

        [SerializeField, Space(5)] private int skyDelay = 100;
        [SerializeField] private DraggableCurtain curtain;
        
        private Sprite _sky1;

        private Sprite[] _oldSkies;

        public void SetupSkyBlink(Sprite sk1, Sprite sk2)
        {
            _oldSkies = new[] {_sky1, sky2};
            _sky1 = sk1;
            sky2 = sk2;
        }

        public void ReturnOldSky()
        {
            _sky1 = _oldSkies[0];
            sky2 = _oldSkies[1];
        }
        
        public override void Initialize(LocationSwitcher switcher)
        {
            base.Initialize(switcher);
            _sky1 = picture.sprite;
        }

        public override async void Show(bool flash = false)
        {
            if (!yaWindow) curtain.Show(Ease, Time / 1.6f, true);
            
            base.Show(flash);
            // bring down music here
            SoundManager.Instance.MusicHandle.SetLowPass(0.2f);
            SoundManager.Instance.MusicHandle.SetVolume(0.38f);
            
            // Play streets sound
            SoundManager.Instance.AmbientHandle.PlayStreet();

            while (true)
            {
                await UniTask.Delay(skyDelay);
                picture.sprite = sky2;
                await UniTask.Delay(skyDelay);
                picture.sprite = _sky1;
                
                if (!OpenToAnim) break;
            }
        }
        
        public override async UniTask Hide(Ease ease, float time)
        {
            Group.DOFade(0, time).SetEase(ease);
            OpenToAnim = true;
            // Take back music parameters
            SoundManager.Instance.MusicHandle.SetLowPass(1f);
            SoundManager.Instance.MusicHandle.SetVolume(1f);
            SoundManager.Instance.AmbientHandle.StopStreet();
            
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            
            curtain.Hide(ease, time, true);
            
            await UniTask.Delay(TimeSpan.FromSeconds(time / 1.6f));
        }
        
        public async UniTask Hide(Ease ease, float time, bool inWindow = false)
        {
            Group.DOFade(0, time).SetEase(ease);
            OpenToAnim = true;
            // Take back music parameters
            SoundManager.Instance.MusicHandle.SetLowPass(1f);
            SoundManager.Instance.MusicHandle.SetVolume(1f);
            SoundManager.Instance.AmbientHandle.StopStreet();
            
            if (!inWindow) await UniTask.Delay(TimeSpan.FromSeconds(0.07f));
            
            curtain.Hide(ease, time, true, inWindow);
            
            await UniTask.Delay(TimeSpan.FromSeconds(time / 1.7f));
        }
    }
}