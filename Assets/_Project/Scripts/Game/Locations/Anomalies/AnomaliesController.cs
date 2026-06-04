using System;
using System.Collections.Generic;
using _Project.Scripts.Curtain;
using _Project.Scripts.Game.Editing;
using _Project.Scripts.Game.Locations.Scenes;
using _Project.Scripts.Game.Menus;
using _Project.Scripts.Sound; // Подключаем пространство имен звукового менеджера
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace _Project.Scripts.Game.Locations.Anomalies
{
    public class AnomaliesController : MonoBehaviour
    {
        public static AnomaliesController Instance { get; private set; }
        
        [SerializeField] private AnomalyObject[] anomalies;
        private List<AnomalyObject> _anomalies;
        
        private AnomalyObject _currentAnomaly;
        public bool checkForAnomaly;
        public float _hpAnomaly;

        public int hp = 5;

        private void Awake() => Instance = this;

        private void Start()
        {
            _anomalies = new List<AnomalyObject>(anomalies);
            EditController.Instance.YaStartAnomaliyaStreet += CallRandomAnomaly;
            EditController.Instance.ProgressUpdate += HideAnomaly;
        }

        [Button]
        public async void CallRandomAnomaly()
        {
            var ano = _anomalies[UnityEngine.Random.Range(0, _anomalies.Count)];
            _currentAnomaly = ano;
            _anomalies.Remove(ano);
            checkForAnomaly = false;
            _hpAnomaly = ano.hpAno;

            ano.pastSprite = ano.target.sprite;
            if (ano.appliedPic.Length == 1) ano.target.sprite = ano.appliedPic[0];
            else if (ano.appliedPic.Length == 2)
            {
                GetComponent<StreetScene>().SetupSkyBlink(ano.appliedPic[0], ano.appliedPic[1]);
            }

            // --- ЗДЕСЬ ПРОПИСЫВАЕМ ЗАПУСК СЛУЧАЙНЫХ ЗВУКОВ ---
            if (ano.soundMethodName == "Footsteps") SoundManager.Instance.AnomalyHandle.Footsteps();
            else if (ano.soundMethodName == "Whispered") SoundManager.Instance.AnomalyHandle.Whispered();
            else if (ano.soundMethodName == "Phone") SoundManager.Instance.AnomalyHandle.Phone();
            else if (ano.soundMethodName == "Scream") SoundManager.Instance.AnomalyHandle.Scream();
            else if (ano.soundMethodName == "Ominous") SoundManager.Instance.SFXHandle.PlayOminous();
            else if (ano.soundMethodName == "Caution") SoundManager.Instance.SFXHandle.PlayCaution();
            else if (ano.soundMethodName == "Regret") SoundManager.Instance.SFXHandle.PlayRegret();

            while (true)
            {
                await UniTask.Yield();
                if (checkForAnomaly && _hpAnomaly > 0 && !DraggableCurtain.Instance.OneOpen)
                    _hpAnomaly -= Time.deltaTime;
                if (_hpAnomaly <= 0 && !DraggableCurtain.Instance.OneOpen && _currentAnomaly != null && LocationSwitcher.Instance._currentLoc.name != "window" && LocationSwitcher.Instance._currentLoc.name != "street")
                    HideAnomaly();
                if (_currentAnomaly == null) break;
            }
        }

        [Button]
        public void HideAnomaly()
        {
            if (_currentAnomaly == null) return;
            if (!checkForAnomaly)
            {
                print("you dont seen anomaly");
                hp--;
            }
            else if (_hpAnomaly > 0)
            {
                print("you dont def anomaly");
                hp--;
            }
            else print("save from anomaly");
            
            if (hp <= 0) GameEnd.Instance.DefeatGame();
            
            if (_currentAnomaly.appliedPic.Length == 1) _currentAnomaly.target.sprite = _currentAnomaly.pastSprite;
            else if (_currentAnomaly.appliedPic.Length == 2) GetComponent<StreetScene>().ReturnOldSky();

            // --- ЗДЕСЬ ОТКЛЮЧАЕМ ПОТОКОВЫЕ ЗВУКИ АНОМАЛИЙ ПРИ ИХ ИСЧЕЗНОВЕНИИ ---
            SoundManager.Instance.AnomalyHandle.GoOut();          // Гасит шаги
            SoundManager.Instance.AnomalyHandle.StopMumbling();   // Гасит шёпот
            
            SoundManager.Instance.MusicHandle.PlayRandomBackgroundMusic(); 

            _currentAnomaly = null;
        }
    }

    [Serializable]
    public class AnomalyObject
    {
        public Image target;
        public Sprite[] appliedPic;
        [HideInInspector] public Sprite pastSprite;
        public float hpAno;
        [Tooltip("Впиши сюда: Regret, Ominous, Caution, Footsteps, Scream, Whispered, Phone")]
        public string soundMethodName;
    }
}