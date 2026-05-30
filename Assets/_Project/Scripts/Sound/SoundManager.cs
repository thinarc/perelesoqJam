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
        public SFXHandle SFXHandle { get; private set; } = new();
        public AmbientHandle AmbientHandle { get; private set; } = new();
        public UIHandle UIHandle { get; private set; } = new();
        
    
        // add other Buses
        private Bus _musicBus;
        private Bus _sfxBus;
        private Bus _bgBus;
        private Bus _uiBus;
        private Bus _masterBus;
        
        // storage of buses variables (Music Bus)
        private float _musicVolume;
        
        public float MusicVolume
        {
            get => _musicVolume; // get a current meaning 
            set
            {
                if (value is > 1f or < 0) return;
                _musicVolume = value;
                _musicBus.setVolume(_musicVolume);
                PlayerPrefs.SetFloat("Music", _musicVolume);
            }
        }
        
        // storage of buses variables (SFX Bus) 
        private float _sfxVolume;
        
        public float SFXVolume
        {
            get => _sfxVolume; // get a current meaning 
            set
            {
                if (value is > 1f or < 0) return;
                _sfxVolume = value;
                _sfxBus.setVolume(_sfxVolume);
                PlayerPrefs.SetFloat("SFX", _sfxVolume);
            }
        } 
        
        // storage of buses variables (bg Bus)
        private float _bgVolume;
        
        public float bgVolume
        {
            get => _bgVolume; // get a current meaning 
            set
            {
                if (value is > 1f or < 0) return;
                _bgVolume = value;
                _bgBus.setVolume(_bgVolume);
                PlayerPrefs.SetFloat("bg", _bgVolume);
            }
        }
        
        // storage of buses variables (UI Bus)
        private float _uiVolume;
        
        public float UIVolume
        {
            get => _uiVolume; // get a current meaning 
            set
            {
                if (value is > 1f or < 0) return;
                _uiVolume = value;
                _uiBus.setVolume(_uiVolume);
                PlayerPrefs.SetFloat("UI", _uiVolume);
            }
        } 
        
        // MASTER
        private float _masterVolume;
        public float MasterVolume
        {
            get => _masterVolume;
            set
            {
                if (value is > 1f or < 0) return;
                _masterVolume = value;
                _masterBus.setVolume(_masterVolume);
                PlayerPrefs.SetFloat("Master", _masterVolume);
            }
        }
    
        // all metods
        private void Awake() => Instance = this;

        private void Start()
        {
            Initialize();
        }
        
        
        public void Initialize()
        {
            //Music Bus
            _musicBus = FMODUnity.RuntimeManager.GetBus("bus:/Music");
            MusicVolume = PlayerPrefs.GetFloat("Music", 0.6f);
            //SFX Bus
            _sfxBus = FMODUnity.RuntimeManager.GetBus("bus:/SFX");
            SFXVolume = PlayerPrefs.GetFloat("SFX", 1f);
            //bg Bus 
            _bgBus = FMODUnity.RuntimeManager.GetBus("bus:/bg");
            bgVolume = PlayerPrefs.GetFloat("bg", 1f); 
            //UI Bus
            _uiBus = FMODUnity.RuntimeManager.GetBus("bus:/UI");
            UIVolume = PlayerPrefs.GetFloat("UI", 1f); 
            
            //MASTER
            _masterBus = FMODUnity.RuntimeManager.GetBus("bus:/");
            MasterVolume = PlayerPrefs.GetFloat("Master", 1f);
            
            
            
            MusicHandle.Initialize();
            SFXHandle.Initialize();
            AmbientHandle.Initialize();
            UIHandle.Initialize();
        }
    }
}