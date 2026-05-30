// foler with our handels
using _Project.Scripts.Sound.Handles;

// a library cenntections
using FMOD.Studio;
using UnityEngine;

// file pathhhhh
namespace _Project.Scripts.Sound
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }
        
        // add other handles
        public MusicHandle MusicHandle { get; private set; } = new();
        
        // add other Buses
        private Bus _volumeBus;
        
        // storage of buses variables (bg Bus)
        private float _volume;
        public float Volume

        {
            get => _volume; // get a current meaning 
            set
            {
                if (value is > 1f or < 0) return;
                _volume = value;
                _volumeBus.setVolume(_volume);
                PlayerPrefs.SetFloat("volume", _volume);
            }
        }
        // storage of buses variables (Other Bus) 
        
        
    
        // all metods
        private void Awake() => Instance = this;

        private void Start()
        {
            Initialize();
        }
        
        
        public void Initialize()
        {
            _volumeBus = FMODUnity.RuntimeManager.GetBus("bus:/");
            Volume = PlayerPrefs.GetFloat("volume", 0.6f);
            
            MusicHandle.Initialize();
        }
    }
}