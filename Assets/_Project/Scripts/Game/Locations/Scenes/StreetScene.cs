using _Project.Scripts.Sound;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.Game.Locations.Scenes
{
    public class StreetScene : BaseScene
    {
        public override void Show(bool flash = false)
        {
            base.Show(flash);
            SoundManager.Instance.MusicHandle.SetLowPass(0.0f);
            SoundManager.Instance.MusicHandle.SetVolume(0.4f);
        }

        public async override UniTask Hide(Ease ease, float time)
        {
            await base.Hide(ease, time);
            SoundManager.Instance.MusicHandle.SetLowPass(1f);
            SoundManager.Instance.MusicHandle.SetVolume(1f);
        } 
    }
}