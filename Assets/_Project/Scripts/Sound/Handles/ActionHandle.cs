using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace _Project.Scripts.Sound.Handles
{
    public class ActionHandle
    {
        private EventInstance _openCurtain;
        private float _lastPlayTime;
        private const float StopDelay = 0.15f; // Время сглаживания «песка»
        private bool _isPlayRequested;

        public void Initialize()
        {
            _openCurtain = RuntimeManager.CreateInstance("event:/SFX/Action/OpenCurtain");
        }

        public void PlayCurtain()
        {
            _openCurtain.getPlaybackState(out var state);
            
            // Если звук вообще не играет — стартуем его
            if (state == PLAYBACK_STATE.STOPPED)
            {
                _openCurtain.start();
            }
            
            // Запоминаем время последнего движения мыши
            _lastPlayTime = Time.time;

            if (!_isPlayRequested)
            {
                _isPlayRequested = true;
                // Запускаем внутренний таймер затухания, если его еще нет
                StopAfterDelay();
            }
        }

        private async void StopAfterDelay()
        {
            // Пока мышь двигается и время с последнего OnDrag меньше чем StopDelay — ждем
            while (Time.time - _lastPlayTime < StopDelay)
            {
                // Ждем один кадр Unity
                await System.Threading.Tasks.Task.Yield();
            }

            // Время вышло, мышь остановилась — глушим звук шторы
            _openCurtain.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _isPlayRequested = false;
        }

        // Этот метод вызывается из OnPointerUp и на краях шторы для мгновенной остановки
        public void StopCurtainImmediate()
        {
            _lastPlayTime = 0f; // Сбрасываем таймер
            _openCurtain.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}