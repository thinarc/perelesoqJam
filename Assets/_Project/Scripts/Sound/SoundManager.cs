// foler with our handels
using _Project.Scripts.Sound.Handles;

// a library cenntections
using FMOD.Studio;
using UnityEngine;

// file path
namespace _Project.Scripts.Sound
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }
        
        // add other handles
        public MusicHandle MusicHandle { get; private set; } = new();
        
        // add other Buses
        private Bus _bg;
        
        // storage of buses variables (Bg Bus)
        private float _musicVolume;
        public float MusicVolume

        {
            get => _musicVolume; // get a current meaning 
            set
            {
                if (value is > 1f or < 0) return;
                _musicVolume = value;
                _allMusicBus.setVolume(_musicVolume);
                PlayerPrefs.SetFloat("Bg", _musicVolume);
            }
        }
        // storage of buses variables (Other Bus) 
        
        
    
        // all metods
        private void Awake() => Instance = this;

        public void Initialize()
        {
            _allMusicBus = FMODUnity.RuntimeManager.GetBus("bus:/Bg");
            // _sfxBus = FMODUnity.RuntimeManager.GetBus("bus:/sfx");
            //
            MusicVolume = PlayerPrefs.GetFloat("Bg", 1f);
            // SfxVolume = PlayerPrefs.GetFloat("sfx", 0.75f);
            
            MusicHandle.Initialize();
        }
    }
}