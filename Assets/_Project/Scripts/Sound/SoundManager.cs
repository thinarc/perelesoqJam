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
        public ActionHandle ActionHandle { get; private set; } = new();
        public AnomalyHandle AnomalyHandle { get; private set; } = new();

        // add other Buses
        private Bus _musicBus;
        private Bus _sfxBus;
        private Bus _sfxActionBus;
        private Bus _sfxEventBus;
        private Bus _bgBus;
        private Bus _uiBus;
        private Bus _toolsBus;
        private Bus _anomalyBus;
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
        
        // SFXAction
        private float _sfxActionVolume;

        public float SFXActionVolume
        {
            get => _sfxActionVolume;
            set
            {
                if (value is > 1f or < 0) return;
                _sfxActionVolume = value;
                _sfxActionBus.setVolume(_sfxActionVolume);
                PlayerPrefs.SetFloat("SFXAction", _sfxActionVolume);
            }
        }
        
        // SFXEvent
        private float _sfxEventVolume;

        public float SFXEventVolume
        {
            get => _sfxEventVolume;
            set
            {
                if (value is > 1f or < 0) return;
                _sfxEventVolume = value;
                _sfxEventBus.setVolume(_sfxEventVolume);
                PlayerPrefs.SetFloat("SFXEvent", _sfxEventVolume);
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
        
        // Tools BUS
        private float _toolsVolume;

        public float ToolsVolume
        {
            get => _toolsVolume;
            set
            {
                if (value is > 1f or < 0) return;
                _toolsVolume = value;
                _toolsBus.setVolume(_toolsVolume);
                PlayerPrefs.SetFloat("ToolsVolume", _toolsVolume);
            }
        }
        
        // AnomalyBus
        private float _anomalyVolume;

        public float AnomalyVolume
        {
            get => _anomalyVolume;
            set
            {
                if (value is > 1f or < 0) return;
                _anomalyVolume = value;
                _anomalyBus.setVolume(_anomalyVolume);
                PlayerPrefs.SetFloat("AnomalyVolume", _anomalyVolume);
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

        private void Update()
        {
            ActionHandle.Tick();
        }
        
        public void Initialize()
        {
            //Music Bus
            _musicBus = FMODUnity.RuntimeManager.GetBus("bus:/Music");
            MusicVolume = PlayerPrefs.GetFloat("Music", 0.6f);
            
            //SFX Bus
            _sfxBus = FMODUnity.RuntimeManager.GetBus("bus:/SFX");
            _sfxActionBus = FMODUnity.RuntimeManager.GetBus("bus:/SFX/SFXAction");
            _sfxEventBus = FMODUnity.RuntimeManager.GetBus("bus:/SFX/SFXEvent");
            
            SFXVolume = PlayerPrefs.GetFloat("SFX", 1f);
            SFXActionVolume = PlayerPrefs.GetFloat("SFXAction", 1f);
            SFXEventVolume = PlayerPrefs.GetFloat("SFXEvent", 1f);
            
            //bg Bus 
            _bgBus = FMODUnity.RuntimeManager.GetBus("bus:/bg");
            bgVolume = PlayerPrefs.GetFloat("bg", 1f); 
            
            //UI Bus
            _uiBus = FMODUnity.RuntimeManager.GetBus("bus:/UI");
            UIVolume = PlayerPrefs.GetFloat("UI", 1f); 
            
            //ToolsBus
            _toolsBus = FMODUnity.RuntimeManager.GetBus("bus:/Tools");
            ToolsVolume = PlayerPrefs.GetFloat("Tools", 1f); 
            
            //Anomaly Bus
            _anomalyBus = FMODUnity.RuntimeManager.GetBus("bus:/Anomaly");
            AnomalyVolume = PlayerPrefs.GetFloat("Anomaly", 1f); 
            
            //MASTER
            _masterBus = FMODUnity.RuntimeManager.GetBus("bus:/");
            MasterVolume = PlayerPrefs.GetFloat("Master", 0.6f);
            
            
            // I set ALL HANDLE here
            MusicHandle.Initialize();
            SFXHandle.Initialize();
            AmbientHandle.Initialize();
            UIHandle.Initialize();
            ActionHandle.Initialize();
            AnomalyHandle.Initialize();
            
            // I set background music there
            //MusicHandle.PlayPianoTheme();
            
            
            
            // сбросить на нормальные значения после инициализации
            MusicHandle.SetLowPass(1f);
            MusicHandle.SetVolume(1f);

        }
    }
}