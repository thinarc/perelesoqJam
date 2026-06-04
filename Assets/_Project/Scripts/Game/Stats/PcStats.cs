using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.Game.Stats
{
    public class PcStats : MonoBehaviour
    {
        public static PcStats Instance { get; private set; }
        private void Awake() => Instance = this;
        
        [SerializeField] private TextMeshProUGUI progressText;
        [SerializeField] private TextMeshProUGUI timeText;
        
        private CanvasGroup _group;

        private void Start()
        {
            _group = GetComponent<CanvasGroup>();
        }
    }
}