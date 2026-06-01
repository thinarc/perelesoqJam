using System;
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
        
        private Sprite _sky1;
        
        public override void Initialize(LocationSwitcher switcher)
        {
            base.Initialize(switcher);
            _sky1 = picture.sprite;
        }

        public override async void Show(bool flash = false)
        {
            base.Show(flash);

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
            
            await UniTask.Delay(TimeSpan.FromSeconds(time / 2f));
        }
    }
}