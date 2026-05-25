using _Project.Scripts.Sound.Handles;
using FMOD.Studio;
using UnityEngine;

namespace _Project.Scripts.Sound
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        public SampleHandle SampleHandle { get; private set; } = new();

        private Bus _bgBus;
        private Bus _sfxBus;
        
        private float _bgVolume;

        public float BgVolume
        {
            get => _bgVolume;
            set
            {
                if (value is > 1f or < 0) return;
                _bgVolume = value;
                _bgBus.setVolume(_bgVolume);
                PlayerPrefs.SetFloat("bg", _bgVolume);
            }
        }
        
        private float _sfxVolume;
        
        public float SfxVolume
        {
            get => _sfxVolume;
            set
            {
                if (value is > 1f or < 0) return;
                _sfxVolume = value;
                _sfxBus.setVolume(_sfxVolume);
                PlayerPrefs.SetFloat("sfx", _sfxVolume);
            }
        }

        private void Awake() => Instance = this;

        public void Initialize()
        {
            // _bgBus = FMODUnity.RuntimeManager.GetBus("bus:/bg");
            // _sfxBus = FMODUnity.RuntimeManager.GetBus("bus:/sfx");
            //
            // BgVolume = PlayerPrefs.GetFloat("bg", 1f);
            // SfxVolume = PlayerPrefs.GetFloat("sfx", 0.75f);

            SampleHandle.Initialize();
        }
    }
}