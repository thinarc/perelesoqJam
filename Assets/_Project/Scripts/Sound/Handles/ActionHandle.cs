// connect library
using FMOD.Studio;
using FMODUnity;

// address 
namespace _Project.Scripts.Sound.Handles
{
    // class named
    public class ActionHandle
        
        
        // set variables
    {
        private EventInstance _openCurtain;

        public void Initialize()
            // create a live copy for event to control it
        {
            _openCurtain = RuntimeManager.CreateInstance("event:/SFX/Action/OpenCurtain");
        }

        
        // external buttons
        //Just like play
        public void PlayCurtain()
        {
            _openCurtain.getPlaybackState(out var state);
            if (state == PLAYBACK_STATE.PLAYING) return;
            _openCurtain.start();
        }

        public void StopCurtain()
        {
            _openCurtain.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        
        
        
        
    }
    
    
}