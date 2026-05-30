using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Game.Locations
{
    public class LocationSwitcher : MonoBehaviour
    {
        [SerializeField] private Image[] transitions;

        [SerializeField, Space(5)] private List<LocationData> locations;

        [SerializeField, Space(5)] private CanvasGroup buttonGroup;
        [SerializeField] private Button[] buttons;
        
        private CanvasGroup _group;

        private float _tTime;
        private Ease _tEase;

        private LocationData _currentLoc;

        private void Start()
        {
            _group = GetComponentInChildren<CanvasGroup>();
            
            _tEase = Ease.Linear;
            _tTime = 0.4f;
            
            _group.DOFade(0, 0);
            _group.interactable = false;
            _group.blocksRaycasts = false;
        }

        public void StartLocation()
        {
            _group.DOFade(1, _tTime).SetEase(_tEase);
            _group.interactable = true;
            _group.blocksRaycasts = true;
            
            transitions[0].sprite = locations[0].capture;
            transitions[1].DOFade(0, 0);
            
            _currentLoc = locations[0];
            InitButtons();
        }

        private void InitButtons()
        {
            if (_currentLoc == null) return;
            if (!_currentLoc.directions.HasDirection()) return;
            
            buttonGroup.blocksRaycasts = true;
            buttonGroup.DOFade(1, 0.4f).SetEase(Ease.InOutSine).OnComplete(() => buttonGroup.interactable = true);
            
            buttons.ForEach(b =>
            {
                b.onClick.RemoveAllListeners();
                b.gameObject.SetActive(false);
            });
            
            if (_currentLoc.directions.left != "")
            {
                buttons[0].onClick.AddListener(() => ToLocation(_currentLoc.directions.left, buttons[0]));
                buttons[0].gameObject.SetActive(true);
            }

            if (_currentLoc.directions.right != "")
            {
                buttons[1].onClick.AddListener(() => ToLocation(_currentLoc.directions.right, buttons[1]));
                buttons[1].gameObject.SetActive(true);
            }

            if (_currentLoc.directions.bottom != "")
            {
                buttons[2].onClick.AddListener(() => ToLocation(_currentLoc.directions.bottom, buttons[2]));
                buttons[2].gameObject.SetActive(true);
            }

            if (_currentLoc.directions.up != "")
            {
                buttons[3].onClick.AddListener(() => ToLocation(_currentLoc.directions.up, buttons[3]));
                buttons[3].gameObject.SetActive(true);
            }
        }

        private void CloseButtons(Image butt)
        {
            buttonGroup.blocksRaycasts = false;
            buttonGroup.interactable = false;
            buttonGroup.DOFade(0, 0.4f).SetEase(Ease.InOutSine);
        }

        private void ToLocation(string locName, Button butt)
        {
            CloseButtons(butt.GetComponent<Image>());

            var l = locations.ToList().Find(n => n.name == locName);
            if (l == null) return;
            transitions[0].DOFade(0, _tTime).SetEase(_tEase);
            transitions[1].sprite = l.capture;
            transitions[1].DOFade(1, _tTime).SetEase(_tEase);
            (transitions[0], transitions[1]) = (transitions[1], transitions[0]);

            _currentLoc = l;
            InitButtons();
        }
    }

    [System.Serializable]
    public class LocationData
    {
        public Sprite capture;
        public string name;
        
        [Space(5)] public SwitchDirections directions;
    }

    [System.Serializable]
    public class SwitchDirections
    {
        public string left = "";
        public string right = "";
        public string bottom = "";
        public string up = "";
        
        public bool HasDirection() => left != "" ||  right != "" || bottom != "" || up != "";
    }
}
