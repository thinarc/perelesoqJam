using _Project.Scripts.Sound;
using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Curtain;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Game.Locations
{
    public class LocationSwitcher : MonoBehaviour
    {
        public static LocationSwitcher Instance { get; private set; }
        
        [SerializeField, Space(5)] private List<LocationData> locations;

        [SerializeField, Space(5)] private CanvasGroup buttonGroup;
        [SerializeField] private Button[] buttons;
        
        private CanvasGroup _group;

        [HideInInspector] public LocationData _currentLoc;
        private bool _initButtons;

        public event Action LampBroken;
        public event Action DoorAnomaly;

        private void Awake() => Instance = this;

        private void Start()
        {
            _group = GetComponentInChildren<CanvasGroup>();
            
            _group.DOFade(0, 0);
            _group.interactable = false;
            _group.blocksRaycasts = false;
            
            locations.ForEach(l =>
            {
                l.scene.Initialize(this);
                var group = l.scene.GetComponent<CanvasGroup>();
                group.blocksRaycasts = false;
                group.interactable = false;
            });
            InitButtons();
        }

        private void Update()
        {
            buttons[3].interactable = DraggableCurtain.Instance.Open;
            buttons[4].interactable = DraggableCurtain.Instance.Open;
        }

        public void StartLocation()
        {
            _group.DOFade(1, 0.4f).SetEase(Ease.Linear);
            _group.interactable = true;
            _group.blocksRaycasts = true;
            
            locations.ForEach(l => l.scene.InitialHide());
            locations[0].scene.Show(true);
            locations[0].scene.UpdateLight();
            var group = locations[0].scene.GetComponent<CanvasGroup>();
            group.blocksRaycasts = true;
            group.interactable = true;
            
            _currentLoc = locations[0];
            InitButtons();
            
            // await UniTask.Delay(2000);
            // LampBroken?.Invoke();
        }

        private void InitButtons()
        {
            buttons.ForEach(b =>
            {
                b.onClick.RemoveAllListeners();
                b.gameObject.SetActive(false);
            });
            
            if (_currentLoc == null) return;
            
            buttonGroup.blocksRaycasts = true;
            buttonGroup.DOFade(1, 0.4f).SetEase(Ease.InOutSine).OnComplete(async () =>
            {
                buttonGroup.interactable = true;
                _initButtons = true;
                
                while (true)
                {
                    if (_currentLoc.directions.left != "" && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))) ToLocation(_currentLoc.directions.left);

                    if (_currentLoc.directions.right != "" && (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))) ToLocation(_currentLoc.directions.right);

                    if (_currentLoc.directions.bottom != "" && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))) ToLocation(_currentLoc.directions.bottom);

                    if (DraggableCurtain.Instance.Open && _currentLoc.directions.up != "" && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
                        ToLocation(_currentLoc.directions.up);
                    
                    if (DraggableCurtain.Instance.Open && _currentLoc.directions.things != "" && (Input.GetKeyDown(KeyCode.E)))
                        ToLocation(_currentLoc.directions.things);
                
                    await UniTask.Yield();
                    if (!_initButtons) break;
                }
            });
            
            if (_currentLoc.directions.left != "")
            {
                buttons[0].onClick.AddListener(() => ToLocation(_currentLoc.directions.left));
                buttons[0].gameObject.SetActive(true);
            }

            if (_currentLoc.directions.right != "")
            {
                buttons[1].onClick.AddListener(() => ToLocation(_currentLoc.directions.right));
                buttons[1].gameObject.SetActive(true);
            }

            if (_currentLoc.directions.bottom != "")
            {
                buttons[2].onClick.AddListener(() => ToLocation(_currentLoc.directions.bottom));
                buttons[2].gameObject.SetActive(true);
            }

            if (_currentLoc.directions.up != "")
            {
                buttons[3].onClick.AddListener(() => ToLocation(_currentLoc.directions.up));
                buttons[3].gameObject.SetActive(true);
            }

            if (_currentLoc.directions.things != "")
            {
                buttons[4].onClick.AddListener(() => ToLocation(_currentLoc.directions.things));
                buttons[4].gameObject.SetActive(true);
            }
        }

        private void CloseButtons()
        {
            buttonGroup.blocksRaycasts = false;
            buttonGroup.interactable = false;
            buttonGroup.DOFade(0, 0.4f).SetEase(Ease.InOutSine);
            _initButtons = false;
        }

        private async void ToLocation(string locName)
        {
            // add a sound here
            SoundManager.Instance.UIHandle.PlayScroll();

            CloseButtons();

            var l = locations.ToList().Find(n => n.name == locName);
            if (l == null) return;
            
            _currentLoc.scene.UpdateLight(true);
            await _currentLoc.scene.Hide(l.scene.Ease, l.scene.Time);
            l.scene.Show();
            l.scene.UpdateLight();
            var group = l.scene.GetComponent<CanvasGroup>();
            group.blocksRaycasts = true;
            group.interactable = true;

            _currentLoc = l;
            InitButtons();
        }
    }

    [Serializable]
    public class LocationData
    {
        public BaseScene scene;
        public string name;
        
        [Space(5)] public SwitchDirections directions;
    }

    [Serializable]
    public class SwitchDirections
    {
        public string left = "";
        public string right = "";
        public string bottom = "";
        public string up = "";
        public string things = "";
    }
}
