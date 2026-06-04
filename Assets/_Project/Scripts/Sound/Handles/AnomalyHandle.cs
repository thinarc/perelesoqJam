// connect library
using FMOD.Studio;
using FMODUnity;

// address 
namespace _Project.Scripts.Sound.Handles
{
    // class named
    public class AnomalyHandle
        
        
    // set variables
    {
        private EventInstance _whispered;
        
        
        // save the link what is current track
        private EventInstance _currentTrack;

        public void Initialize()
        // create a live copy for event to control it
        {
            _whispered = RuntimeManager.CreateInstance("event:/Anomaly/Whispered");
        }

        
        
        
    }
    
    
}