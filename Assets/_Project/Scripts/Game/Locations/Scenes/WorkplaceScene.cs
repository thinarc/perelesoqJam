using _Project.Scripts.Sound;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Game.Locations.Scenes
{
    public class WorkplaceScene : BaseScene
    {
        [SerializeField, Space(5)] private Image picture;
        [SerializeField] private Sprite darkplace;
        
        public override void Initialize(LocationSwitcher switcher)
        {
            base.Initialize(switcher);
            switcher.LampBroken += OnLampBroken;
        }

        private async void OnLampBroken()
        {
            // тут скрипт что лампа сначала мигает потом гаснет
            // это нужно чтобы анимация стакалась со звуком
            // sound of broken lamp
            SoundManager.Instance.SFXHandle.PlayLightOut();
            
            // phase 1 - light
            await UniTask.Delay(2000);
            
            // phase 2 - fast blinking
            for (int i = 0; i < 3; i++)
            {
                Light.intensity = InitialLight;
                await UniTask.Delay(80);
                Light.intensity = 0;
                await UniTask.Delay(60);
            }
            
            // space between blinking
            Light.intensity = InitialLight;
            await UniTask.Delay(1300);
            
            //phase 3 light goes out
            for (int i = 0; i < 2; i++)
            {
                Light.intensity = InitialLight;
                await UniTask.Delay(60);
                Light.intensity = 0;
                await UniTask.Delay(80);
            }

            Light.intensity = 0;
            await UniTask.Delay(600);
            Light.intensity = InitialLight;
            await UniTask.Delay(40);
            Light.intensity = 0;

            picture.sprite = darkplace;
        }
    }
}