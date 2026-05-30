// connect library
using FMOD.Studio;
using FMODUnity;

// address 
namespace _Project.Scripts.Sound.Handles
{
    // class named
    public class UIHandle
        
        
        // set variables
    {
        private EventInstance _scroll;

        public void Initialize()
            // create a live copy for event to control it
        {
            _scroll = RuntimeManager.CreateInstance("event:/UI/Scroll");
        }

        
        // external buttons
        //Just like play
        public void PlayScroll()
        {
            _scroll.start();
        }
        
        
    }
    
    
}