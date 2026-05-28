// connect library
using FMOD.Studio;
using FMODUnity;

// address 
namespace _Project.Scripts.Sound.Handles
{
    // class named
    public class MusicHandle
        
        
    // set variables
    {
        private EventInstance _mainTheme;
        private EventInstance _pianoTheme;
        
        // save the link what is current track
        private EventInstance _currentTrack;

        public void Initialize()
        // create a live copy for event to control it
        {
            _mainTheme = RuntimeManager.CreateInstance("event:/Music/Main_Them");
            _pianoTheme = RuntimeManager.CreateInstance("event:/Music/S_Piano_Th");
        }

        
        // external buttons
        public void PlayMainTheme()
        {
            SwitchTo(_mainTheme);
        }
        public void PlayPianoTheme()
        {
            SwitchTo(_pianoTheme);
        }
        
        
        //Lets stop the music
        public void StopMusic()
        {
            _currentTrack.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        public void PauseMusic()
        {
            _currentTrack.setPaused(true);
        }

        public void ResumeMusic()
        {
            _currentTrack.setPaused(false);
        }

        private void SwitchTo(EventInstance newTrack)
        // check: does a current track exist?
        {
            if (_currentTrack.isValid())
                _currentTrack.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _currentTrack = newTrack;
            _currentTrack.start();
        }
        
        // im setting all parametrs here
        public void SetLowPass(float value)
        {
            _currentTrack.setParameterByName("LowPass", value);
        }
        
    }
    
    
}