using System;
using System.Collections.Generic;
using _Project.Scripts.Curtain;
using _Project.Scripts.Game.Editing;
using _Project.Scripts.Game.Locations.Scenes;
using _Project.Scripts.Game.Menus;
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
            _hpAnomaly = 5f;

            ano.pastSprite = ano.target.sprite;
            if (ano.appliedPic.Length == 1) ano.target.sprite = ano.appliedPic[0];
            else if (ano.appliedPic.Length == 2)
            {
                GetComponent<StreetScene>().SetupSkyBlink(ano.appliedPic[0], ano.appliedPic[1]);
            }

            while (true)
            {
                await UniTask.Yield();
                if (checkForAnomaly && _hpAnomaly > 0 && !DraggableCurtain.Instance.OneOpen)
                    _hpAnomaly -= Time.deltaTime;
                if (_hpAnomaly <= 0 && !DraggableCurtain.Instance.OneOpen && _currentAnomaly != null) HideAnomaly();
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
            _currentAnomaly = null;
        }
    }

    [Serializable]
    public class AnomalyObject
    {
        public Image target;
        public Sprite[] appliedPic;
        [HideInInspector] public Sprite pastSprite;
    }
}