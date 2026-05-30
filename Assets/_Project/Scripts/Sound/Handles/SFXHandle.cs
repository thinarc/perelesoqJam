// connect library
using FMOD.Studio;
using FMODUnity;

// address 
namespace _Project.Scripts.Sound.Handles
{
    // class named
    public class SFXHandle
        
        
        // set variables
    {
        private EventInstance _caution;
        private EventInstance _regret;
        private EventInstance _ominous;

        public void Initialize()
            // create a live copy for event to control it
        { 
            _caution = RuntimeManager.CreateInstance("event:/SFX/Caution");
                //_caution.set3DAttributes(RuntimeUtils.To3DAttributes(Vector3.zero));
    
            _regret = RuntimeManager.CreateInstance("event:/SFX/Regret");
                //_regret.set3DAttributes(RuntimeUtils.To3DAttributes(Vector3.zero));
    
            _ominous = RuntimeManager.CreateInstance("event:/SFX/Ominous");
                //_ominous.set3DAttributes(RuntimeUtils.To3DAttributes(Vector3.zero));
        }

        
        // external buttons
        //Just like play&stop
        public void PlayCaution()
        {
            _caution.start();
        }

        public void StopCaution()
        {
            _caution.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        
        public void PlayRegret()
        {
            _regret.start();
        }

        public void StopRegret()
        {
            _regret.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        } 
        public void PlayOminous()
        {
            _ominous.start();
        }

        public void StopOminous()
        {
            _ominous.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
         
        
        // im setting all parametrs here
        //public void SetLowPass(float value)
       // {
         //   _currentAmbient.setParameterByName("LowPass", value);
        //}
        
    }
    
    
}