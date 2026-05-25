using FMOD.Studio;
using FMODUnity;

namespace _Project.Scripts.Sound.Handles
{
    public class SampleHandle
    {
        private EventInstance _clip;
        
        public void Initialize() { }

        public void CommandExample()
        {
            _clip = RuntimeManager.CreateInstance("event:/Path/ToEvent");
            _clip.start();
            _clip.setParameterByName("clipParameter_Int", 0);
            
            _clip.getParameterByName("clipParameter_Int", out var val);
            _clip.getTimelinePosition(out var pos);
            _clip.getDescription(out var desc);
            desc.getLength(out var length);
            
            _clip.setPaused(true);
            _clip.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}