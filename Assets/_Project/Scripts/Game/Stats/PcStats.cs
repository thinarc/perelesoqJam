using _Project.Scripts.Game.Editing;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.Game.Stats
{
    public class PcStats : MonoBehaviour
    {
        public static PcStats Instance { get; private set; }

        [SerializeField] private TextMeshProUGUI progressText;
        [SerializeField] private TextMeshProUGUI timeText;

        private CanvasGroup _group;

        private bool _trainingCompleted;
        private float _timer;

        private void Awake() => Instance = this;

        private void Start()
        {
            _group = GetComponent<CanvasGroup>();
            UpdateTimeText(23, 0);
        }

        private void Update()
        {
            progressText.text = EditController.Instance.currentProgress + "/10 фото";
            
            _timer += Time.deltaTime;

            if (!_trainingCompleted)
            {
                var progress = Mathf.Clamp01(_timer / 60f);
                var minutes = Mathf.FloorToInt(progress * 59f);
                UpdateTimeText(23, minutes);
            }
            else
            {
                var progress = Mathf.Clamp01(_timer / 360f);
                var totalMinutes = progress * 360f;
                var hours = Mathf.FloorToInt(totalMinutes / 60f);
                var minutes = Mathf.FloorToInt(totalMinutes % 60f);

                UpdateTimeText(hours, minutes);
            }
        }

        public void CompleteTraining()
        {
            _trainingCompleted = true;
            _timer = 0f;
            UpdateTimeText(0, 0);
        }
        
        private int _lastHour = -1;
        private float _nextUpdateTime;

        private void UpdateTimeText(int hours, int minutes)
        {
            if (hours != _lastHour)
            {
                _lastHour = hours;
                _nextUpdateTime = Time.time + Random.Range(2f, 10f);

                timeText.text = $"{hours}:{minutes:00}";
                return;
            }

            if (Time.time >= _nextUpdateTime)
            {
                _nextUpdateTime = Time.time + Random.Range(2f, 10f);

                timeText.text = $"{hours}:{minutes:00}";
            }
        }
    }
}